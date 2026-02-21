
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
        public readonly ApiEndpoints _endpoints;

        public AuthService(ApiEndpoints endpoints)
        {
            _api = new ApiService();
            _endpoints = endpoints;
        }

        public async Task<ApiResponseDto<UserDto?>> GetMeAsync()
        {
            _api.SetBearerToken(UserSession.AccessToken ?? "");
            return await _api.GetAsync<ApiResponseDto<UserDto?>>(_endpoints.GetMe);
        }

        public async Task<ApiResponseDto<string>> LoginAsync(string user, string password)
        {
            return await _api.PostAsync<LoginRequest, ApiResponseDto<string>>(
               _endpoints.Login,
                new LoginRequest
                {
                    Username = user,
                    Password = password
                });
        }
    }
}
