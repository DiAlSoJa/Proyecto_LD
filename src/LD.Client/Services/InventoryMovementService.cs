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

    public async Task<ApiResponseDto<List<InventoryMovementDto>>> GetInventoryMovements(int? standardId = null)
    {
        var endpoint = _apiEndpoints.InventoryMovement_GetAll;
        if (standardId.HasValue)
            endpoint += $"?standardId={standardId.Value}";

        return await _api.GetAsync<ApiResponseDto<List<InventoryMovementDto>>>(
            endpoint);
    }

    public async Task<ApiResponseDto<List<InventoryMovementDto>>> GetInventoryMovementsByStandardIdCode(string standardIdCode)
    {
        var endpoint = _apiEndpoints.InventoryMovement_GetAll;
        if (!string.IsNullOrWhiteSpace(standardIdCode))
            endpoint += $"?standardIdCode={Uri.EscapeDataString(standardIdCode.Trim())}";

        return await _api.GetAsync<ApiResponseDto<List<InventoryMovementDto>>>(
            endpoint);
    }
}
