using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Client
{
    public class AuthService
    {
        public readonly ApiService _api;
        public readonly ApiEndpoints _endpoints;

        public AuthService(ApiEndpoints endpoints, ApiService api)
        {
            _api = api;
            _endpoints = endpoints;
        }

        public async Task<ApiResponseDto<UserDto?>> GetMeAsync()
        {
            //_api.SetBearerToken(UserSession.AccessToken ?? "");
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
