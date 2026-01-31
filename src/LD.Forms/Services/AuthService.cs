
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Services
{
    public class AuthService
    {
        public readonly ApiService _api;
        public AuthService()
        {
            _api = new ApiService();
        }

        public async Task<AuthResponse> LoginAsync(string user, string password)
        {
            return await _api.PostAsync<LoginRequest, AuthResponse>(
                ApiEndpoints.Auth.Login,
                new LoginRequest
                {
                    Username = user,
                    Password = password
                });
        }
    }
}
