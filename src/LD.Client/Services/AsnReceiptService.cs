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
    public class AsnReceiptService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public AsnReceiptService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<ApiResponseDto<AsnReceiptRequest>> GetAsnReceiptById(int asnReceiptId)
        {
            return await _api.GetAsync<ApiResponseDto<AsnReceiptRequest>>(_apiEndpoints.AsnReceipt_GetById.Replace("{asnId}", asnReceiptId.ToString()));
        }

        public async Task<ApiResponseDto<List<AsnReceiptDetailDto>>> GetAsnReceipts()
        {
            return await _api.GetAsync<ApiResponseDto<List<AsnReceiptDetailDto>>>(_apiEndpoints.AsnReceipt_GetAll);
        }

        public async Task<ApiResponseDto<List<AsnReceiptDetailDto>>> GetAsnReceiptsByAsnDetailId(int asnDetailId)
        {
            return await _api.GetAsync<ApiResponseDto<List<AsnReceiptDetailDto>>>(
                _apiEndpoints.AsnReceipt_GetByAsnDetailId.Replace("{asnDetailId}", asnDetailId.ToString()));
        }

        public async Task<ApiResponseDto<string>> CreateAsnReceipt(AsnReceiptRequest request)
        {
            return await _api.PostAsync<AsnReceiptRequest, ApiResponseDto<string>>(_apiEndpoints.AsnReceipt_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateAsnReceipt(int asnReceiptId, AsnReceiptRequest request)
        {
            return await _api.PutAsync<AsnReceiptRequest, ApiResponseDto<string>>(_apiEndpoints.AsnReceipt_Update.Replace("{asnId}", asnReceiptId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> DeleteAsnReceipt(int asnReceiptId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.AsnReceipt_Delete.Replace("{asnId}", asnReceiptId.ToString()));
        }
    }
}
