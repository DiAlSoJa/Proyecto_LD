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
        string? estatus = null,
        string? auditorUserId = null)
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

        if (!string.IsNullOrWhiteSpace(auditorUserId))
        {
            query.Add($"auditorUserId={Uri.EscapeDataString(auditorUserId)}");
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

    public async Task<ApiResponseDto<List<CyclicInventoryScanDto>>> GetCyclicInventoryScans(
        int cyclicInventoryId,
        int cyclicInventoryDetailId)
    {
        var endpoint = _apiEndpoints.CyclicInventory_GetScans
            .Replace("{cyclicInventoryId}", cyclicInventoryId.ToString())
            .Replace("{cyclicInventoryDetailId}", cyclicInventoryDetailId.ToString());

        return await _api.GetAsync<ApiResponseDto<List<CyclicInventoryScanDto>>>(endpoint);
    }

    public async Task<ApiResponseDto<CyclicInventoryScanDto>> CreateCyclicInventoryScan(
        int cyclicInventoryId,
        int cyclicInventoryDetailId,
        CreateCyclicInventoryScanRequest request)
    {
        var endpoint = _apiEndpoints.CyclicInventory_CreateScan
            .Replace("{cyclicInventoryId}", cyclicInventoryId.ToString())
            .Replace("{cyclicInventoryDetailId}", cyclicInventoryDetailId.ToString());

        return await _api.PostAsync<CreateCyclicInventoryScanRequest, ApiResponseDto<CyclicInventoryScanDto>>(
            endpoint,
            request);
    }

    public async Task<ApiResponseDto<string>> DeleteCyclicInventoryScan(
        int cyclicInventoryId,
        int cyclicInventoryDetailId,
        int cyclicInventoryScanId)
    {
        var endpoint = _apiEndpoints.CyclicInventory_DeleteScan
            .Replace("{cyclicInventoryId}", cyclicInventoryId.ToString())
            .Replace("{cyclicInventoryDetailId}", cyclicInventoryDetailId.ToString())
            .Replace("{cyclicInventoryScanId}", cyclicInventoryScanId.ToString());

        return await _api.DeleteAsync<ApiResponseDto<string>>(endpoint);
    }

    public async Task<ApiResponseDto<List<CyclicInventoryScanDto>>> FinishCyclicInventoryLocation(
        int cyclicInventoryId,
        int cyclicInventoryDetailId)
    {
        var endpoint = _apiEndpoints.CyclicInventory_FinishLocation
            .Replace("{cyclicInventoryId}", cyclicInventoryId.ToString())
            .Replace("{cyclicInventoryDetailId}", cyclicInventoryDetailId.ToString());

        return await _api.PostAsync<object, ApiResponseDto<List<CyclicInventoryScanDto>>>(
            endpoint,
            new { });
    }
}
