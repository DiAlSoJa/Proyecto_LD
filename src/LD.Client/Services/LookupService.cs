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

    }
}
