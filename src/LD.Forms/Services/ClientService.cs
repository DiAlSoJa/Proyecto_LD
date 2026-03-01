
using LD.Contracts.Client;
using LD.Contracts.Requests.Client;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;
using System.Net;

namespace LD.Forms.Services
{
    public class ClientService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public ClientService(ApiEndpoints apiEndpoints)
        {
            _api = new ApiService();
            _api.SetBearerToken(UserSession.AccessToken??"");
            _apiEndpoints = apiEndpoints;
        }


        public async Task<ApiResponseDto<ClientRequest>> GetClientById(int clientId)
        {
            return await _api.GetAsync<ApiResponseDto<ClientRequest>>(_apiEndpoints.Client_GetById.Replace("{id}", clientId.ToString()));
        }

        public async Task<ApiResponseDto<List<ClientDto>>> GetClients()
        {
            return await _api.GetAsync<ApiResponseDto<List<ClientDto>>>(_apiEndpoints.Client_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateClient(ClientRequest request)
        {
            return await _api.PostAsync<ClientRequest,ApiResponseDto<string>>(_apiEndpoints.Client_Create,request);
        }

        public async Task<ApiResponseDto<string>> UpdateClient(int clientId, ClientRequest request)
        {
            return await _api.PutAsync<ClientRequest, ApiResponseDto<string>>(_apiEndpoints.Client_Update.Replace("{id}", clientId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveClient(int clientId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.Client_Delete.Replace("{id}", clientId.ToString()));
        }
    }
}
