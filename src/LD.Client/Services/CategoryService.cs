using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.Category;
using LD.Contracts.Client;
using LD.Contracts.DTOs;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class CategoryService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public CategoryService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }
        public async Task<ApiResponseDto<CategoryRequest>> GetCategoryById(string categoryId)
        {
            return await _api.GetAsync<ApiResponseDto<CategoryRequest>>(_apiEndpoints.Category_GetById.Replace("{categoryId}", categoryId));
        }

        public async Task<ApiResponseDto<List<CategoryDto>>> GetCategory()
        {
            return await _api.GetAsync<ApiResponseDto<List<CategoryDto>>>(_apiEndpoints.Category_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateCategory(CategoryRequest request)
        {
            return await _api.PostAsync<CategoryRequest, ApiResponseDto<string>>(_apiEndpoints.Category_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateCategory(string categoryId, CategoryRequest request)
        {
            return await _api.PutAsync<CategoryRequest, ApiResponseDto<string>>(_apiEndpoints.Category_Update.Replace("{categoryId}", categoryId), request);
        }

        public async Task<ApiResponseDto<List<DropDownDto>>> GetClientLookup()
          => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Client);

        public async Task<ApiResponseDto<List<DropDownDto>>> GetProjecttLookup()
          => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Project);
    }
}
