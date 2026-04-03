using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.ASN;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class AsnDetailService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public AsnDetailService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<ApiResponseDto<AsnDetailRequest>> GetAsnDetailById(int asnDetailId)
        {
            return await _api.GetAsync<ApiResponseDto<AsnDetailRequest>>(_apiEndpoints.AsnDetail_GetById.Replace("{asnId}", asnDetailId.ToString()));
        }

        public async Task<ApiResponseDto<List<AsnDetailDto>>> GetAsnDetails()
        {
            return await _api.GetAsync<ApiResponseDto<List<AsnDetailDto>>>(_apiEndpoints.AsnDetail_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateAsnDetail(AsnDetailRequest request)
        {
            return await _api.PostAsync<AsnDetailRequest, ApiResponseDto<string>>(_apiEndpoints.AsnDetail_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateAsnDetail(int asnDetailId, AsnDetailRequest request)
        {
            return await _api.PutAsync<AsnDetailRequest, ApiResponseDto<string>>(_apiEndpoints.AsnDetail_Update.Replace("{asnId}", asnDetailId.ToString()), request);
        }
    }
}
