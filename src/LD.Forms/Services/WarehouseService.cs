using LD.Contracts.Project;
using LD.Contracts.Requests;
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
        public async Task<ApiResponseDto<WarehouseDto>> GetWarehouseById(int warehouseId)
        {
            return await _api.GetAsync<ApiResponseDto<WarehouseDto>>(ApiEndpoints.Warehouse.GetById.Replace("{id}", warehouseId.ToString()));
        }

        public async Task<ApiResponseDto<List<WarehouseDto>>> GetWarehouses()
        {
            return await _api.GetAsync<ApiResponseDto<List<WarehouseDto>>>(ApiEndpoints.Warehouse.GetAll);
        }



        public async Task<ApiResponseDto<string>> CreateWarehouse(WarehouseRequest request)
        {
            return await _api.PostAsync<WarehouseRequest, ApiResponseDto<string>>(ApiEndpoints.Warehouse.Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateWarehouse(int warehouseId, WarehouseRequest request)
        {
            return await _api.PutAsync<WarehouseRequest, ApiResponseDto<string>>(ApiEndpoints.Warehouse.Update.Replace("{id}", warehouseId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveWarehouse(int warehouseId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(ApiEndpoints.Warehouse.Delete.Replace("{id}", warehouseId.ToString()));
        }
    }
}
