using LD.Contracts.DTOs.KittingFolioCapture;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class KittingFolioCaptureService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public KittingFolioCaptureService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<KittingFolioCaptureDto>>> GetKittingFolioCaptures(int? clientId = null, int? projectId = null)
    {
        var endpoint = BuildQueryEndpoint(clientId, projectId);
        return await _api.GetAsync<ApiResponseDto<List<KittingFolioCaptureDto>>>(endpoint);
    }

    public async Task<ApiResponseDto<KittingFolioCaptureDto>> GetKittingFolioCaptureById(int captureId)
    {
        return await _api.GetAsync<ApiResponseDto<KittingFolioCaptureDto>>(
            _apiEndpoints.KittingFolioCapture_GetById.Replace("{captureId}", captureId.ToString()));
    }

    public async Task<ApiResponseDto<KittingFolioCapturePreviewDto>> PreviewKittingFromFolioFile(GenerateKittingFolioCaptureRequest request)
    {
        return await _api.PostAsync<GenerateKittingFolioCaptureRequest, ApiResponseDto<KittingFolioCapturePreviewDto>>(
            _apiEndpoints.KittingFolioCapture_Preview,
            request);
    }

    public async Task<ApiResponseDto<string>> GenerateKittingFromFolioFile(GenerateKittingFolioCaptureRequest request)
    {
        return await _api.PostAsync<GenerateKittingFolioCaptureRequest, ApiResponseDto<string>>(
            _apiEndpoints.KittingFolioCapture_Generate,
            request);
    }

    private string BuildQueryEndpoint(int? clientId, int? projectId)
    {
        var query = new List<string>();
        if (clientId.HasValue && clientId.Value > 0)
            query.Add($"clientId={clientId.Value}");

        if (projectId.HasValue && projectId.Value > 0)
            query.Add($"projectId={projectId.Value}");

        return query.Count == 0
            ? _apiEndpoints.KittingFolioCapture_GetAll
            : $"{_apiEndpoints.KittingFolioCapture_GetAll}?{string.Join("&", query)}";
    }
}
