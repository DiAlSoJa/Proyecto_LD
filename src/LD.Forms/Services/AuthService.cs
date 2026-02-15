
using LD.Contracts.User;
using LD.Forms.Classes;
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

        public async Task<ApiResponseDto<UserDto?>> GetMeAsync()
        {
            _api.SetBearerToken(UserSession.AccessToken ?? "");
            return await _api.GetAsync<ApiResponseDto<UserDto?>>(ApiEndpoints.Auth.GetMe);
        }

        public async Task<ApiResponseDto<string>> LoginAsync(string user, string password)
        {
            return await _api.PostAsync<LoginRequest, ApiResponseDto<string>>(
                ApiEndpoints.Auth.Login,
                new LoginRequest
                {
                    Username = user,
                    Password = password
                });
        }
    }
}
