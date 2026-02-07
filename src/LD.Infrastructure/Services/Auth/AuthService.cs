using Azure.Core;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        public async Task<AuthResponse> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return AuthResponse.Fail("Credenciales inválidas");

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _signInManager
                .CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                return AuthResponse.Fail("Credenciales inválidas");

            var token = _jwtTokenService.GenerateToken(user.Id.ToString(), user.Email!, roles);

            return AuthResponse.Ok(token);
        }

        public async Task<AuthResponse> Register(string email, string password)
        {
            var userExists = await _userManager.FindByEmailAsync(email);
            if (userExists != null)
                return AuthResponse.Fail("El usuario ya existe");

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return AuthResponse.Fail(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            return AuthResponse.Ok(null);
        }
    }
}
