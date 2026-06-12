using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.Client;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class InventaryStatusService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public InventaryStatusService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }
        public async Task<ApiResponseDto<InventaryStatusRequest>> GetStatusById(string statusId)
        {
            return await _api.GetAsync<ApiResponseDto<InventaryStatusRequest>>(_apiEndpoints.InventaryStatus_GetById.Replace("{statusId}", statusId));
        }

        public async Task<ApiResponseDto<List<InventaryStatusDto>>> GetInventaryStatus(int? clientId = null, int? projectId = null)
        {
            var query = new List<string>();

            if (clientId.HasValue)
            {
                query.Add($"clientId={clientId.Value}");
            }

            if (projectId.HasValue)
            {
                query.Add($"projectId={projectId.Value}");
            }

            var endpoint = _apiEndpoints.InventaryStatus_GetAll;
            if (query.Count > 0)
            {
                endpoint = $"{endpoint}?{string.Join("&", query)}";
            }

            return await _api.GetAsync<ApiResponseDto<List<InventaryStatusDto>>>(endpoint);
        }

        public async Task<ApiResponseDto<string>> CreateInventaryStatus(InventaryStatusRequest request)
        {
            return await _api.PostAsync<InventaryStatusRequest, ApiResponseDto<string>>(_apiEndpoints.InventaryStatus_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateInventaryStatus(string statusId, InventaryStatusRequest request)
        {
            return await _api.PutAsync<InventaryStatusRequest, ApiResponseDto<string>>(_apiEndpoints.InventaryStatus_Update.Replace("{statusId}", statusId), request);
        }
    }
}
