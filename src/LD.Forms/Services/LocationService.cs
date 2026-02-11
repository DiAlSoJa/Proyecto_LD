
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
    public class LocationService
    {
        public readonly ApiService _api;
        public LocationService()
        {
            _api = new ApiService();
            _api.SetBearerToken(UserSession.AccessToken??"");
        }

        public async Task<ApiResponseDto<List<ClientDto>>> GetLocations()
        {
            return await _api.GetAsync<ApiResponseDto<List<ClientDto>>>(ApiEndpoints.Location.GetAll);
        }
    }
}
