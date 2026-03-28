using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.Client;
using LD.Contracts.Dimensioner;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.Units;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class DimensionerService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public DimensionerService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }
        public async Task<ApiResponseDto<DimensionerRequest>> GetDimensionerById(string dimensionerId)
        {
            return await _api.GetAsync<ApiResponseDto<DimensionerRequest>>(_apiEndpoints.Dimensioner_GetById.Replace("{dimensionerId}", dimensionerId));
        }

        public async Task<ApiResponseDto<List<DimensionerDto>>> GetDimensioners()
        {
            return await _api.GetAsync<ApiResponseDto<List<DimensionerDto>>>(_apiEndpoints.Dimensioner_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateDimensioner(DimensionerRequest request)
        {
            return await _api.PostAsync<DimensionerRequest, ApiResponseDto<string>>(_apiEndpoints.Dimensioner_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateDimensioner(string dimensionerId, DimensionerRequest request)
        {
            return await _api.PutAsync<DimensionerRequest, ApiResponseDto<string>>(_apiEndpoints.Dimensioner_Update.Replace("{dimensionerId}", dimensionerId), request);
        }
    }
}
