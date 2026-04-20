using LD.Contracts.InventoryMovement;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class InventoryMovementService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public InventoryMovementService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<InventoryMovementDto>>> GetInventoryMovements()
    {
        return await _api.GetAsync<ApiResponseDto<List<InventoryMovementDto>>>(
            _apiEndpoints.InventoryMovement_GetAll);
    }
}
