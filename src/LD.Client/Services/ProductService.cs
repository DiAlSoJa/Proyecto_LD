using LD.Contracts.Client;
using LD.Contracts.DTOs;
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
            return await _api.GetAsync<ApiResponseDto<List<ProductDto?>>>(_apiEndpoints.Product_GetAll);
        }

        

        public async Task<ApiResponseDto<List<ProductAutocompleteDto>>> GetProductByClientId(int clientId, int projectId)
            => await _api.GetAsync<ApiResponseDto<List<ProductAutocompleteDto>>>(
                $"{_apiEndpoints.Product_GetAll}/{clientId}/{projectId}");

        public async Task<ApiResponseDto<ProductRequest>> GetItemById(int itemId)
        {
            return await _api.GetAsync<ApiResponseDto<ProductRequest>>(_apiEndpoints.Product_GetById.Replace("{id}", itemId.ToString()));
        }

        public async Task<ApiResponseDto<string>> CreateItem(ProductRequest request)
        {
            return await _api.PostAsync<ProductRequest, ApiResponseDto<string>>(_apiEndpoints.Product_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateItem(int itemId, ProductRequest request)
        {
            return await _api.PutAsync<ProductRequest, ApiResponseDto<string>>(_apiEndpoints.Product_Update.Replace("{id}", itemId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveItem(int itemId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.Product_Delete.Replace("{id}", itemId.ToString()));
        }
    }
}
