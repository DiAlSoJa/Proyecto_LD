using LD.Contracts.InventarioCiclico;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class CyclicInventoryService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public CyclicInventoryService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<CyclicInventoryDto>>> GetCyclicInventories(
        DateTime? desde = null,
        DateTime? hasta = null,
        string? estatus = null)
    {
        var query = new List<string>();

        if (desde.HasValue)
        {
            query.Add($"desde={Uri.EscapeDataString(desde.Value.ToString("yyyy-MM-dd"))}");
        }

        if (hasta.HasValue)
        {
            query.Add($"hasta={Uri.EscapeDataString(hasta.Value.ToString("yyyy-MM-dd"))}");
        }

        if (!string.IsNullOrWhiteSpace(estatus))
        {
            query.Add($"estatus={Uri.EscapeDataString(estatus)}");
        }

        var endpoint = query.Count == 0
            ? _apiEndpoints.CyclicInventory_GetAll
            : $"{_apiEndpoints.CyclicInventory_GetAll}?{string.Join("&", query)}";

        return await _api.GetAsync<ApiResponseDto<List<CyclicInventoryDto>>>(endpoint);
    }

    public async Task<ApiResponseDto<InventarioCiclicoRequest>> GetCyclicInventoryById(int cyclicInventoryId)
    {
        return await _api.GetAsync<ApiResponseDto<InventarioCiclicoRequest>>(
            _apiEndpoints.CyclicInventory_GetById.Replace("{cyclicInventoryId}", cyclicInventoryId.ToString()));
    }

    public async Task<ApiResponseDto<string>> CreateCyclicInventory(InventarioCiclicoRequest request)
    {
        return await _api.PostAsync<InventarioCiclicoRequest, ApiResponseDto<string>>(
            _apiEndpoints.CyclicInventory_Create,
            request);
    }

    public async Task<ApiResponseDto<string>> UpdateCyclicInventory(int cyclicInventoryId, InventarioCiclicoRequest request)
    {
        return await _api.PutAsync<InventarioCiclicoRequest, ApiResponseDto<string>>(
            _apiEndpoints.CyclicInventory_Update.Replace("{cyclicInventoryId}", cyclicInventoryId.ToString()),
            request);
    }
}
