using LD.Contracts.Client;
using LD.Contracts.DTOs;
using LD.Contracts.Requests.Client;
using LD.Contracts.Responses;
using LD.Forms.Classes;
using LD.Forms.Configuration;
using System.Net;

namespace LD.Forms.Services
{
    public class LookupService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public LookupService(ApiService apiService, ApiEndpoints apiEndpoints)
        {
            _api = apiService;
            _apiEndpoints = apiEndpoints;
            _api.SetBearerToken(UserSession.AccessToken ?? "");
        }


        public async Task<ApiResponseDto<LookupsDto>> GetLookups()
        {
            _api.SetBearerToken(UserSession.AccessToken??"");
            return await _api.GetAsync<ApiResponseDto<LookupsDto>>(_apiEndpoints.Lookup_GetAll);
        }
        public async Task<ApiResponseDto<DropDownDto>> GetWarehouseLookup()
            => await _api.GetAsync<ApiResponseDto<DropDownDto>>(_apiEndpoints.Lookup_Warehouse);
        public async Task<ApiResponseDto<DropDownDto>> GetClientLookup()
           => await _api.GetAsync<ApiResponseDto<DropDownDto>>(_apiEndpoints.Lookup_Client);
        public async Task<ApiResponseDto<DropDownDto>> GetLocationLookup()
           => await _api.GetAsync<ApiResponseDto<DropDownDto>>(_apiEndpoints.Lookup_Location);

    }
}
