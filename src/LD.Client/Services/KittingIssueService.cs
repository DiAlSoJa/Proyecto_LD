using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class KittingIssueService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public KittingIssueService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<KittingIssueRequest>> GetKittingIssueById(int kittingIssueDetailId)
    {
        return await _api.GetAsync<ApiResponseDto<KittingIssueRequest>>(
            _apiEndpoints.KittingIssue_GetById.Replace("{kittingIssueDetailId}", kittingIssueDetailId.ToString()));
    }

    public async Task<ApiResponseDto<List<KittingIssueDetailDto>>> GetKittingIssues()
    {
        return await _api.GetAsync<ApiResponseDto<List<KittingIssueDetailDto>>>(_apiEndpoints.KittingIssue_GetAll);
    }

    public async Task<ApiResponseDto<List<KittingIssueDetailDto>>> GetKittingIssuesByKittingDetailId(int kittingDetailId)
    {
        return await _api.GetAsync<ApiResponseDto<List<KittingIssueDetailDto>>>(
            _apiEndpoints.KittingIssue_GetByKittingDetailId.Replace("{kittingDetailId}", kittingDetailId.ToString()));
    }

    public async Task<ApiResponseDto<string>> CreateKittingIssue(KittingIssueRequest request)
    {
        return await _api.PostAsync<KittingIssueRequest, ApiResponseDto<string>>(_apiEndpoints.KittingIssue_Create, request);
    }

    public async Task<ApiResponseDto<string>> UpdateKittingIssue(int kittingIssueDetailId, KittingIssueRequest request)
    {
        return await _api.PutAsync<KittingIssueRequest, ApiResponseDto<string>>(
            _apiEndpoints.KittingIssue_Update.Replace("{kittingIssueDetailId}", kittingIssueDetailId.ToString()),
            request);
    }

    public async Task<ApiResponseDto<string>> DeleteKittingIssue(int kittingIssueDetailId)
    {
        return await _api.DeleteAsync<ApiResponseDto<string>>(
            _apiEndpoints.KittingIssue_Delete.Replace("{kittingIssueDetailId}", kittingIssueDetailId.ToString()));
    }
}
