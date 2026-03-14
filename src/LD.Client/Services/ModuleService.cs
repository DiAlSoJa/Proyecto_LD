using LD.Contracts.Client;
using LD.Contracts.DTOs.Auth;
using LD.Contracts.Requests.Client;
using LD.Contracts.Responses;
using LD.Forms.Configuration;
using System.Net;

namespace LD.Client.Services
{
    public class ModuleService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public ModuleService(ApiService api,ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }


        public async Task<ApiResponseDto<List<ModuleAuthorizationDto>>> GetModules()
        {
            return await _api.GetAsync<ApiResponseDto<List<ModuleAuthorizationDto>>>(_apiEndpoints.Module_GetAll);
        }

      
    }
}
