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
    public class AsnService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public AsnService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<ApiResponseDto<AsnRequest>> GetAsnById(int asnId)
        {
            return await _api.GetAsync<ApiResponseDto<AsnRequest>>(_apiEndpoints.Asn_GetById.Replace("{asnId}", asnId.ToString()));
        }

        public async Task<ApiResponseDto<List<AsnDto>>> GetAsn()
        {
            return await _api.GetAsync<ApiResponseDto<List<AsnDto>>>(_apiEndpoints.Asn_GetAll);
        }


        public async Task<ApiResponseDto<string>> CreateAsn(AsnRequest request)
        {
            return await _api.PostAsync<AsnRequest, ApiResponseDto<string>>(_apiEndpoints.Asn_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateAsn(int asnId, AsnRequest request)
        {
            return await _api.PutAsync<AsnRequest, ApiResponseDto<string>>(_apiEndpoints.Asn_Update.Replace("{asnId}", asnId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ConfirmAsn(int asnId)
        {
            return await _api.PostAsync<object, ApiResponseDto<string>>(
                _apiEndpoints.Asn_Confirm.Replace("{asnId}", asnId.ToString()),
                new { });
        }

        public async Task<ApiResponseDto<string>> CancelAsn(int asnId)
        {
            return await _api.PostAsync<object, ApiResponseDto<string>>(
                _apiEndpoints.Asn_Cancel.Replace("{asnId}", asnId.ToString()),
                new { });
        }
    }
}
