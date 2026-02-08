
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Services
{
    public class ClientService
    {
        public readonly ApiService _api;
        public ClientService()
        {
            _api = new ApiService();
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
