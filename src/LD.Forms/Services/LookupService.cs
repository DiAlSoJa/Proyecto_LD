
using LD.Contracts.Client;
using LD.Contracts.DTOs;
using LD.Contracts.Requests.Client;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;
using System.Net;

namespace LD.Forms.Services
{
    public class LookupService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public LookupService(ApiEndpoints apiEndpoints)
        {
            _api = new ApiService();
            _apiEndpoints = apiEndpoints;
        }


        public async Task<ApiResponseDto<LookupsDto>> GetLookups()
        {
            _api.SetBearerToken(UserSession.AccessToken??"");
            return await _api.GetAsync<ApiResponseDto<LookupsDto>>(_apiEndpoints.Lookup_GetAll);
        }

    
    }
}
