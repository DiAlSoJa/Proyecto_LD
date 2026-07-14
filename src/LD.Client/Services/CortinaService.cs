using LD.Contracts.DTOs.Security;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class CortinaService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public CortinaService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<CortinaDto>>> GetCortinas(int? warehouseId = null)
    {
        var endpoint = _apiEndpoints.Cortina_GetAll;
        if (warehouseId.HasValue)
            endpoint += $"?warehouseId={warehouseId.Value}";

        return await _api.GetAsync<ApiResponseDto<List<CortinaDto>>>(endpoint);
    }

    public async Task<ApiResponseDto<CortinaRequest>> GetCortinaById(int cortinaId)
        => await _api.GetAsync<ApiResponseDto<CortinaRequest>>(_apiEndpoints.Cortina_GetById.Replace("{id}", cortinaId.ToString()));

    public async Task<ApiResponseDto<string>> CreateCortina(CortinaRequest request)
        => await _api.PostAsync<CortinaRequest, ApiResponseDto<string>>(_apiEndpoints.Cortina_Create, request);

    public async Task<ApiResponseDto<string>> UpdateCortina(int cortinaId, CortinaRequest request)
        => await _api.PutAsync<CortinaRequest, ApiResponseDto<string>>(_apiEndpoints.Cortina_Update.Replace("{id}", cortinaId.ToString()), request);

    public async Task<ApiResponseDto<string>> DeleteCortina(int cortinaId)
        => await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.Cortina_Delete.Replace("{id}", cortinaId.ToString()));
}
