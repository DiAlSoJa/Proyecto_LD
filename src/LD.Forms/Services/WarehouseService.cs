using LD.Contracts.Warehouse;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;

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

        public async Task<ApiResponseDto<List<WarehouseDto>>> GetWarehouses()
        {
            return await _api.GetAsync<ApiResponseDto<List<WarehouseDto>>>(ApiEndpoints.Warehouse.GetAll);
        }
    }
}
