using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class KittingDetailService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public KittingDetailService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<KittingDetailRequest>> GetKittingDetailById(int kittingDetailId)
    {
        return await _api.GetAsync<ApiResponseDto<KittingDetailRequest>>(
            _apiEndpoints.KittingDetail_GetById.Replace("{kittingDetailId}", kittingDetailId.ToString()));
    }

    public async Task<ApiResponseDto<List<KittingDetailDto>>> GetKittingDetails()
    {
        return await _api.GetAsync<ApiResponseDto<List<KittingDetailDto>>>(_apiEndpoints.KittingDetail_GetAll);
    }

    public async Task<ApiResponseDto<List<KittingDetailDto>>> GetKittingDetailsByKittingId(int kittingId)
    {
        return await _api.GetAsync<ApiResponseDto<List<KittingDetailDto>>>(
            _apiEndpoints.KittingDetail_GetByKittingId.Replace("{kittingId}", kittingId.ToString()));
    }

    public async Task<ApiResponseDto<string>> CreateKittingDetail(KittingDetailRequest request)
    {
        return await _api.PostAsync<KittingDetailRequest, ApiResponseDto<string>>(_apiEndpoints.KittingDetail_Create, request);
    }

    public async Task<ApiResponseDto<string>> UpdateKittingDetail(int kittingDetailId, KittingDetailRequest request)
    {
        return await _api.PutAsync<KittingDetailRequest, ApiResponseDto<string>>(
            _apiEndpoints.KittingDetail_Update.Replace("{kittingDetailId}", kittingDetailId.ToString()),
            request);
    }

    public async Task<ApiResponseDto<string>> DeleteKittingDetail(int kittingDetailId)
    {
        return await _api.DeleteAsync<ApiResponseDto<string>>(
            _apiEndpoints.KittingDetail_Delete.Replace("{kittingDetailId}", kittingDetailId.ToString()));
    }
}
