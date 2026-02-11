
using LD.Contracts;
using LD.Contracts.Client;
using LD.Forms.Classes;
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
            _api.SetBearerToken(UserSession.AccessToken??"");
        }

        public async Task<ApiResponseDto<List<ClientDto>>> GetClients()
        {
            return await _api.GetAsync<ApiResponseDto<List<ClientDto>>>(ApiEndpoints.Client.GetAll);
        }
    }
}
