using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.Client;
using LD.Contracts.Currency;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class CurrencyService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public CurrencyService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }
        public async Task<ApiResponseDto<CurrencyRequest>> GetCurrencyById(string currencyId)
        {
            return await _api.GetAsync<ApiResponseDto<CurrencyRequest>>(_apiEndpoints.Currency_GetById.Replace("{currencyId}", currencyId));
        }

        public async Task<ApiResponseDto<List<CurrencyDto>>> GetCurrency()
        {
            return await _api.GetAsync<ApiResponseDto<List<CurrencyDto>>>(_apiEndpoints.Currency_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateCurrency(CurrencyRequest request)
        {
            return await _api.PostAsync<CurrencyRequest, ApiResponseDto<string>>(_apiEndpoints.Currency_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateCurrency(string currencyId, CurrencyRequest request)
        {
            return await _api.PutAsync<CurrencyRequest, ApiResponseDto<string>>(_apiEndpoints.Currency_Update.Replace("{currencyId}", currencyId), request);
        }
    }
}
