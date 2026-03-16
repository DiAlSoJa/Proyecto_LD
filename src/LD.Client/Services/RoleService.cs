using LD.Contracts.Client;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.Contracts.Requests.Client;
using LD.Contracts.Responses;
using LD.Forms.Configuration;
using System.Net;

namespace LD.Client.Services
{
    public class RoleService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public RoleService(ApiService api,ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }


        public async Task<ApiResponseDto<RoleRequest>> GetRolById(int rolId)
        {
            return await _api.GetAsync<ApiResponseDto<RoleRequest>>(_apiEndpoints.Role_GetById.Replace("{id}", rolId.ToString()));
        }

        public async Task<ApiResponseDto<List<RoleDto>>> GetRols()
        {
            return await _api.GetAsync<ApiResponseDto<List<RoleDto>>>(_apiEndpoints.Role_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateRol(RoleRequest request)
        {
            return await _api.PostAsync<RoleRequest, ApiResponseDto<string>>(_apiEndpoints.Role_Create,request);
        }

        public async Task<ApiResponseDto<string>> UpdateRol(int rolId, RoleRequest request)
        {
            return await _api.PutAsync<RoleRequest, ApiResponseDto<string>>(_apiEndpoints.Role_Update.Replace("{id}", rolId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveRol(int rolId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.Role_Delete.Replace("{id}", rolId.ToString()));
        }
    }
}
