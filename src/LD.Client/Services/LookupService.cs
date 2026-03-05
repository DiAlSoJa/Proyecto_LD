using LD.Contracts.Client;
using LD.Contracts.DTOs;
using LD.Contracts.Requests.Client;
using LD.Contracts.Responses;
using System.Net;

namespace LD.Client
{
    public class LookupService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public LookupService(ApiEndpoints apiEndpoints, ApiService api)
        {
            _api = api;
            //_api.SetBearerToken(UserSession.AccessToken??"");
            _apiEndpoints = apiEndpoints;
        }


        public async Task<ApiResponseDto<LookupsDto>> GetLookups()
        {
            return await _api.GetAsync<ApiResponseDto<LookupsDto>>(_apiEndpoints.Lookup_GetAll);
        }

    
    }
}
