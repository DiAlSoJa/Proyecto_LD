using LD.Contracts.Client;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.User;
using LD.Forms.Configuration;
using System.Net;

namespace LD.Client.Services
{
    public class UserService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public UserService(ApiService apiService, ApiEndpoints apiEndpoints)
        {
            _api = apiService;
            _apiEndpoints = apiEndpoints;
        }


        public async Task<ApiResponseDto<UserDto>> GetUserById(string userId)
        {
            return await _api.GetAsync<ApiResponseDto<UserDto>>(_apiEndpoints.User_GetById.Replace("{id}", userId));
        }

        public async Task<ApiResponseDto<List<UserDto>>> GetUsers()
        {
            return await _api.GetAsync<ApiResponseDto<List<UserDto>>>(_apiEndpoints.User_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateUser(UserRequest request)
        {
            return await _api.PostAsync<UserRequest, ApiResponseDto<string>>(_apiEndpoints.User_Create,request);
        }

        public async Task<ApiResponseDto<string>> UpdateUser(string userId, UserRequest request)
        {
            return await _api.PutAsync<UserRequest, ApiResponseDto<string>>(_apiEndpoints.User_Update.Replace("{id}", userId), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveUser(string userId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.User_Delete.Replace("{id}", userId));
        }
    }
}
