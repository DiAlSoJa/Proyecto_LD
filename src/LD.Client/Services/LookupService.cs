using LD.Contracts.Client;
using LD.Contracts.DTOs;
using LD.Contracts.Requests.Client;
using LD.Contracts.Responses;
using LD.Forms.Configuration;
using System.Net;

namespace LD.Client.Services
{
    public class LookupService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public LookupService(ApiService apiService, ApiEndpoints apiEndpoints)
        {
            _api = apiService;
            _apiEndpoints = apiEndpoints;
        }


        public async Task<ApiResponseDto<LookupsDto>> GetLookups()
        {
            return await _api.GetAsync<ApiResponseDto<LookupsDto>>(_apiEndpoints.Lookup_GetAll);
        }
        public async Task<ApiResponseDto<List<DropDownDto>>> GetWarehouseLookup()
            => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Warehouse);
        public async Task<ApiResponseDto<List<DropDownDto>>> GetClientLookup()
           => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Client);
        public async Task<ApiResponseDto<List<DropDownDto>>> GetLocationLookup()
           => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Location);
        public async Task<ApiResponseDto<List<DropDownDto>>> GetLocationWarehouseLookup(int warehouseId)
           => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>($"{_apiEndpoints.Lookup_LocationWarehouse}/{warehouseId}");
        public async Task<ApiResponseDto<List<DropDownDto>>> GetRoleLookup()
          => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Role);
        public async Task<ApiResponseDto<List<DropDownDto>>> GetProjectLookup()
          => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Project);
        public async Task<ApiResponseDto<List<DropDownDto>>> GetSystemFieldLookup()
         => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_SystemField);
        public async Task<ApiResponseDto<List<DropDownDto>>> GetUnitLookup()
          => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Unit);
        public async Task<ApiResponseDto<List<DropDownDto>>> GetDimensionerLookup()
           => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Dimensioner);
        public async Task<ApiResponseDto<List<DropDownDto>>> GetScanTypeLookup()
           => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_ScanType);
        public async Task<ApiResponseDto<List<DropDownDto>>> GetScanSaveTypeLookup()
           => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_ScanSaveType);

        public async Task<ApiResponseDto<List<DropDownDto>>> GetProjectClientLookup(int clientId)
                => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>($"{_apiEndpoints.Lookup_ProjectClient}/{clientId}");

        public async Task<ApiResponseDto<List<DropDownDto>>> GetCategoryClientLookup(int clientId, int projectId)
                => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>($"{_apiEndpoints.Lookup_Category}/{clientId}/{projectId}");
        public async Task<ApiResponseDto<List<DropDownDto>>> GetFamilyClientLookup(int clientId, int projectId)
               => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>($"{_apiEndpoints.Lookup_Family}/{clientId}/{projectId}");




    }
}
