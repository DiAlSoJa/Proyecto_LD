

using LD.Contracts.Client;
using LD.Contracts.Item;
using LD.Contracts.Requests;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;

namespace LD.Forms.Services
{
    public class ItemService
    {
        public readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public ItemService(ApiEndpoints apiEndpoints)
        {
            _api = new ApiService();
            _api.SetBearerToken(UserSession.AccessToken ?? "");
            _apiEndpoints = apiEndpoints;
        }

        public async Task<ApiResponseDto<List<ItemDto?>>> GetItems()
        {
            return await _api.GetAsync<ApiResponseDto<List<ItemDto?>>>(_apiEndpoints.Item_GetAll);
        }

        public async Task<ApiResponseDto<ItemDto>> GetItemById(int itemId)
        {
            return await _api.GetAsync<ApiResponseDto<ItemDto>>(_apiEndpoints.Item_GetById.Replace("{id}", itemId.ToString()));
        }

        public async Task<ApiResponseDto<string>> CreateItem(ItemRequest request)
        {
            return await _api.PostAsync<ItemRequest, ApiResponseDto<string>>(_apiEndpoints.Item_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateClient(int itemId, ItemRequest request)
        {
            return await _api.PutAsync<ItemRequest, ApiResponseDto<string>>(_apiEndpoints.Item_Update.Replace("{id}", itemId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveClient(int itemId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.Item_Delete.Replace("{id}", itemId.ToString()));
        }
    }
}
