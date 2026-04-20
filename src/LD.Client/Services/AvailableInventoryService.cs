using LD.Contracts.AvailableInventory;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class AvailableInventoryService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public AvailableInventoryService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<AvailableInventoryDto>>> GetAvailableInventories()
    {
        return await _api.GetAsync<ApiResponseDto<List<AvailableInventoryDto>>>(
            _apiEndpoints.AvailableInventory_GetAll);
    }
}
