using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.User;
using LD.Forms.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Client.Services
{
    public class AuthService
    {
        public readonly ApiService _api;
        public readonly ApiEndpoints _endpoints;

        public AuthService(ApiService api,ApiEndpoints endpoints)
        {
            _api = api;
            _endpoints = endpoints;
        }

        public async Task<ApiResponseDto<UserDto?>> GetMeAsync()
        {
            return await _api.GetAsync<ApiResponseDto<UserDto?>>(_endpoints.GetMe);
        }

        public async Task<ApiResponseDto<LoginResponse>> LoginAsync(string user, string password)
        {
            var result =await _api.PostAsync<LoginRequest, ApiResponseDto<LoginResponse>>(
               _endpoints.Login,
                new LoginRequest
                {
                    Username = user,
                    Password = password
                });
            if(result.IsSuccess) _api.SetBearerToken(result.Data?.Accesstoken!);
            return result;
        }
    }
}
