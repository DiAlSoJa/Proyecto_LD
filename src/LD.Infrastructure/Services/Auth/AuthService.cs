using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Contracts.Responses;
using LD.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly LdProyectDbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService,
            LdProyectDbContext context)
        {
            _userManager    = userManager;
            _signInManager  = signInManager;
            _jwtTokenService = jwtTokenService;
            _context        = context;
        }

        public async Task<LoginResponse?> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new Exception("Credenciales inválidas");

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (!result.Succeeded)
                throw new Exception("Credenciales inválidas");

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken  = _jwtTokenService.GenerateToken(user.Id.ToString(), user.UserName ?? "", roles);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // Revocar tokens previos del usuario para evitar acumulación
            var existingTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == user.Id && !rt.IsRevoked)
                .ToListAsync();

            foreach (var old in existingTokens)
                old.IsRevoked = true;

            _context.RefreshTokens.Add(new RefreshToken
            {
                Token      = refreshToken,
                UserId     = user.Id,
                Expiration = DateTime.UtcNow.AddDays(7),
                CreatedAt  = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Accesstoken  = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponse?> RefreshToken(string refreshToken)
        {
            var stored = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (stored is null || stored.Expiration < DateTime.UtcNow)
                return null;

            var user = await _userManager.FindByIdAsync(stored.UserId);
            if (user is null || !user.IsActive)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var newAccessToken  = _jwtTokenService.GenerateToken(user.Id.ToString(), user.UserName ?? "", roles);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            // Rotación: invalidar el token usado y emitir uno nuevo
            stored.IsRevoked       = true;
            stored.ReplacedByToken = newRefreshToken;

            _context.RefreshTokens.Add(new RefreshToken
            {
                Token      = newRefreshToken,
                UserId     = user.Id,
                Expiration = DateTime.UtcNow.AddDays(7),
                CreatedAt  = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Accesstoken  = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
