using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class KittingService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public KittingService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<KittingRequest>> GetKittingById(int kittingId)
    {
        return await _api.GetAsync<ApiResponseDto<KittingRequest>>(
            _apiEndpoints.Kitting_GetById.Replace("{kittingId}", kittingId.ToString()));
    }

    public async Task<ApiResponseDto<List<KittingDto>>> GetKittings()
    {
        return await _api.GetAsync<ApiResponseDto<List<KittingDto>>>(_apiEndpoints.Kitting_GetAll);
    }

    public async Task<ApiResponseDto<List<KittingDto>>> GetKittingsByClient(int clientId, int projectId)
    {
        return await _api.GetAsync<ApiResponseDto<List<KittingDto>>>(
            _apiEndpoints.Kitting_GetByClient
                .Replace("{clientId}", clientId.ToString())
                .Replace("{projectId}", projectId.ToString()));
    }

    public async Task<ApiResponseDto<string>> CreateKitting(KittingRequest request)
    {
        return await _api.PostAsync<KittingRequest, ApiResponseDto<string>>(_apiEndpoints.Kitting_Create, request);
    }

    public async Task<ApiResponseDto<string>> UpdateKitting(int kittingId, KittingRequest request)
    {
        return await _api.PutAsync<KittingRequest, ApiResponseDto<string>>(
            _apiEndpoints.Kitting_Update.Replace("{kittingId}", kittingId.ToString()),
            request);
    }

    public async Task<ApiResponseDto<string>> ConfirmKitting(int kittingId)
    {
        return await _api.PostAsync<object, ApiResponseDto<string>>(
            _apiEndpoints.Kitting_Confirm.Replace("{kittingId}", kittingId.ToString()),
            new { });
    }

    public async Task<ApiResponseDto<string>> CancelKitting(int kittingId)
    {
        return await _api.PostAsync<object, ApiResponseDto<string>>(
            _apiEndpoints.Kitting_Cancel.Replace("{kittingId}", kittingId.ToString()),
            new { });
    }

    public async Task<ApiResponseDto<string>> LocateKitting(int kittingId)
    {
        return await _api.PostAsync<object, ApiResponseDto<string>>(
            _apiEndpoints.Kitting_Locate.Replace("{kittingId}", kittingId.ToString()),
            new { });
    }
}
