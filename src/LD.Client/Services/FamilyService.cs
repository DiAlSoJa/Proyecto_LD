using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.Client;
using LD.Contracts.DTOs;
using LD.Contracts.DTOs.Family;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class FamilyService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public FamilyService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }
        public async Task<ApiResponseDto<FamilyRequest>> GetFamilyById(int familyId)
        {
            return await _api.GetAsync<ApiResponseDto<FamilyRequest>>(_apiEndpoints.Family_GetById.Replace("{familyId}", familyId.ToString()));
        }

        public async Task<ApiResponseDto<List<FamilyDto>>> GetFamily()
        {
            return await _api.GetAsync<ApiResponseDto<List<FamilyDto>>>(_apiEndpoints.Family_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateFamily(FamilyRequest request)
        {
            return await _api.PostAsync<FamilyRequest, ApiResponseDto<string>>(_apiEndpoints.Family_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateFamily(int familyId, FamilyRequest request)
        {
            return await _api.PutAsync<FamilyRequest, ApiResponseDto<string>>(_apiEndpoints.Family_Update.Replace("{familyId}", familyId.ToString()), request);
        }

        public async Task<ApiResponseDto<List<DropDownDto>>> GetClientLookup()
          => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Client);

        public async Task<ApiResponseDto<List<DropDownDto>>> GetProjecttLookup()
          => await _api.GetAsync<ApiResponseDto<List<DropDownDto>>>(_apiEndpoints.Lookup_Project);
    }
}
