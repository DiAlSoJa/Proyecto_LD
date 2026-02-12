
using LD.Contracts.Client;
using LD.Contracts.Requests;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;

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

        public async Task<ApiResponseDto<List<ClientDto>>> GetClients()
        {
            return await _api.GetAsync<ApiResponseDto<List<ClientDto>>>(ApiEndpoints.Client.GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateClient(ClientRequest request)
        {
            return await _api.PostAsync<ClientRequest,ApiResponseDto<string>>(ApiEndpoints.Client.Create,request);
        }
    }
}
