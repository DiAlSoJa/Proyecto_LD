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

    public async Task<ApiResponseDto<string>> ChangeLocation(int standardId, string ubicacionDestino)
    {
        return await ChangeLocation(new[] { standardId }, ubicacionDestino);
    }

    public async Task<ApiResponseDto<string>> ChangeLocation(IEnumerable<int> standardIds, string ubicacionDestino)
    {
        var request = new ChangeInventoryLocationRequest
        {
            StandardIds = (standardIds ?? Enumerable.Empty<int>())
                .Where(x => x > 0)
                .Distinct()
                .ToList(),
            UbicacionDestino = ubicacionDestino
        };

        return await _api.PostAsync<ChangeInventoryLocationRequest, ApiResponseDto<string>>(
            _apiEndpoints.AvailableInventory_ChangeLocation,
            request);
    }

    public async Task<ApiResponseDto<string>> ChangeStatus(int standardId, string statusDestino)
    {
        return await ChangeStatus(new[] { standardId }, statusDestino);
    }

    public async Task<ApiResponseDto<string>> ChangeStatus(IEnumerable<int> standardIds, string statusDestino)
    {
        var request = new ChangeInventoryStatusRequest
        {
            StandardIds = (standardIds ?? Enumerable.Empty<int>())
                .Where(x => x > 0)
                .Distinct()
                .ToList(),
            StatusDestino = statusDestino
        };

        return await _api.PostAsync<ChangeInventoryStatusRequest, ApiResponseDto<string>>(
            _apiEndpoints.AvailableInventory_ChangeStatus,
            request);
    }

    public async Task<ApiResponseDto<string>> ChangeWarehouse(IEnumerable<int> standardIds, int warehouseId, string ubicacionDestino)
    {
        var request = new ChangeInventoryWarehouseRequest
        {
            StandardIds = (standardIds ?? Enumerable.Empty<int>())
                .Where(x => x > 0)
                .Distinct()
                .ToList(),
            WarehouseId = warehouseId,
            UbicacionDestino = ubicacionDestino
        };

        return await _api.PostAsync<ChangeInventoryWarehouseRequest, ApiResponseDto<string>>(
            _apiEndpoints.AvailableInventory_ChangeWarehouse,
            request);
    }
}
