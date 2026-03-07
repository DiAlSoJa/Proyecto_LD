using Azure.Core;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<LoginResponse?> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new Exception("Credenciales inválidas");

            var result = await _signInManager.PasswordSignInAsync(
                user,
                password,
                false,
                false
            );

            if (!result.Succeeded)
                throw new Exception("Credenciales inválidas");

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtTokenService.GenerateToken(
                user.Id.ToString(),
                user.UserName??"",
                roles
            );

            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            var refreshEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Expiration = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            //_context.RefreshTokens.Add(refreshEntity);

            //await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Accesstoken = accessToken,
                RefreshToken = refreshToken
            };
        }

    }
}
