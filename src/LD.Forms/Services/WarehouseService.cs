
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Services
{
    public class WarehouseService
    {
        public readonly ApiService _api;
        public WarehouseService()
        {
            _api = new ApiService();
            _api.SetBearerToken(UserSession.AccessToken??"");
        }

        public async Task<ApiResponseDto<List<ClientDto>>> GetWarehouses()
        {
            return await _api.GetAsync<ApiResponseDto<List<ClientDto>>>(ApiEndpoints.Warehouse.GetAll);
        }
    }
}
