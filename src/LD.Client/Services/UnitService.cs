using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.Client;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.Units;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class UnitService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public UnitService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }
        public async Task<ApiResponseDto<UnitRequest>> GetUnitById(string unitId)
        {
            return await _api.GetAsync<ApiResponseDto<UnitRequest>>(_apiEndpoints.Unit_GetById.Replace("{unitIdS}", unitId));
        }

        public async Task<ApiResponseDto<List<UnitDto>>> GetUnits()
        {
            return await _api.GetAsync<ApiResponseDto<List<UnitDto>>>(_apiEndpoints.Unit_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateUnit(UnitRequest request)
        {
            return await _api.PostAsync<UnitRequest, ApiResponseDto<string>>(_apiEndpoints.Unit_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateUnit(string unitId, UnitRequest request)
        {
            return await _api.PutAsync<UnitRequest, ApiResponseDto<string>>(_apiEndpoints.Unit_Update.Replace("{unitIdS}", unitId), request);
        }
    }
}
