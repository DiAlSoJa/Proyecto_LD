using LD.Contracts.Client;
using LD.Contracts.Product;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class ProductService
    {
        public readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public ProductService(ApiService apiService,ApiEndpoints apiEndpoints)
        {
            _api = apiService;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<ApiResponseDto<List<ProductDto?>>> GetItems()
        {
            return await _api.GetAsync<ApiResponseDto<List<ProductDto?>>>(_apiEndpoints.Item_GetAll);
        }

        public async Task<ApiResponseDto<ProductDto>> GetItemById(int itemId)
        {
            return await _api.GetAsync<ApiResponseDto<ProductDto>>(_apiEndpoints.Item_GetById.Replace("{id}", itemId.ToString()));
        }

        public async Task<ApiResponseDto<string>> CreateItem(ItemRequest request)
        {
            return await _api.PostAsync<ItemRequest, ApiResponseDto<string>>(_apiEndpoints.Item_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateItem(int itemId, ItemRequest request)
        {
            return await _api.PutAsync<ItemRequest, ApiResponseDto<string>>(_apiEndpoints.Item_Update.Replace("{id}", itemId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveItem(int itemId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.Item_Delete.Replace("{id}", itemId.ToString()));
        }
    }
}
