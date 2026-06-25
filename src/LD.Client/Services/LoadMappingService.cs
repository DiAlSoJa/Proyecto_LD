using LD.Contracts.DTOs.LoadMapping;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class LoadMappingService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public LoadMappingService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<LoadMappingDto>>> GetLoadMappings()
        => await _api.GetAsync<ApiResponseDto<List<LoadMappingDto>>>(_apiEndpoints.LoadMapping_GetAll);

    public async Task<ApiResponseDto<List<LoadMappingAvailableOrderDto>>> GetLoadingOrders()
        => await _api.GetAsync<ApiResponseDto<List<LoadMappingAvailableOrderDto>>>(_apiEndpoints.LoadMapping_GetLoadingOrders);

    public async Task<ApiResponseDto<string>> CreateLoadMapping(CreateLoadMappingRequest request)
        => await _api.PostAsync<CreateLoadMappingRequest, ApiResponseDto<string>>(_apiEndpoints.LoadMapping_Create, request);

    public async Task<ApiResponseDto<List<LoadMappingScanDto>>> GetLoadMappingScans(int loadMappingId)
        => await _api.GetAsync<ApiResponseDto<List<LoadMappingScanDto>>>(
            _apiEndpoints.LoadMapping_GetScans.Replace("{loadMappingId}", loadMappingId.ToString()));

    public async Task<ApiResponseDto<LoadMappingScanDto>> CreateLoadMappingScan(int loadMappingId, CreateLoadMappingScanRequest request)
        => await _api.PostAsync<CreateLoadMappingScanRequest, ApiResponseDto<LoadMappingScanDto>>(
            _apiEndpoints.LoadMapping_CreateScan.Replace("{loadMappingId}", loadMappingId.ToString()),
            request);

    public async Task<ApiResponseDto<string>> DeleteLoadMappingScan(int loadMappingId, int scanId)
        => await _api.DeleteAsync<ApiResponseDto<string>>(
            _apiEndpoints.LoadMapping_DeleteScan
                .Replace("{loadMappingId}", loadMappingId.ToString())
                .Replace("{scanId}", scanId.ToString()));
}
