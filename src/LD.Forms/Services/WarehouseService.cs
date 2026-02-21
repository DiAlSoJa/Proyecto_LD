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
        private readonly ApiEndpoints _apiEndpoints;
        public WarehouseService(ApiEndpoints apiEndpoints)
        {
            _api = new ApiService();
            _api.SetBearerToken(UserSession.AccessToken ?? "");
            _apiEndpoints = apiEndpoints;
        }
        public async Task<ApiResponseDto<WarehouseDto>> GetWarehouseById(int warehouseId)
        {
            return await _api.GetAsync<ApiResponseDto<WarehouseDto>>(_apiEndpoints.Warehouse_GetById.Replace("{id}", warehouseId.ToString()));
        }

        public async Task<ApiResponseDto<List<WarehouseDto>>> GetWarehouses()
        {
            return await _api.GetAsync<ApiResponseDto<List<WarehouseDto>>>(_apiEndpoints.Warehouse_GetAll);
        }



        public async Task<ApiResponseDto<string>> CreateWarehouse(WarehouseRequest request)
        {
            return await _api.PostAsync<WarehouseRequest, ApiResponseDto<string>>(_apiEndpoints.Warehouse_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateWarehouse(int warehouseId, WarehouseRequest request)
        {
            return await _api.PutAsync<WarehouseRequest, ApiResponseDto<string>>(_apiEndpoints.Warehouse_Update.Replace("{id}", warehouseId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveWarehouse(int warehouseId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.Warehouse_Delete.Replace("{id}", warehouseId.ToString()));
        }
    }
}
