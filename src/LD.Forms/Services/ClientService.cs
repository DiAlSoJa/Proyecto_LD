
using LD.Contracts.Client;
using LD.Contracts.Requests;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;
using System.Net;

namespace LD.Forms.Services
{
    public class ClientService
    {
        public readonly ApiService _api;
        public ClientService()
        {
            _api = new ApiService();
            _api.SetBearerToken(UserSession.AccessToken??"");
        }


        public async Task<ApiResponseDto<ClientDto>> GetClientById(int clientId)
        {
            return await _api.GetAsync<ApiResponseDto<ClientDto>>(ApiEndpoints.Client.GetById.Replace("{id}", clientId.ToString()));
        }

        public async Task<ApiResponseDto<List<ClientDto>>> GetClients()
        {
            return await _api.GetAsync<ApiResponseDto<List<ClientDto>>>(ApiEndpoints.Client.GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateClient(ClientRequest request)
        {
            return await _api.PostAsync<ClientRequest,ApiResponseDto<string>>(ApiEndpoints.Client.Create,request);
        }

        public async Task<ApiResponseDto<string>> UpdateClient(int clientId, ClientRequest request)
        {
            return await _api.PutAsync<ClientRequest, ApiResponseDto<string>>(ApiEndpoints.Client.Update.Replace("{id}", clientId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveClient(int clientId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(ApiEndpoints.Client.Delete.Replace("{id}", clientId.ToString()));
        }
    }
}
