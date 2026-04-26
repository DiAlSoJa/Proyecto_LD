using LD.Contracts.DTOs.Security;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class PatioClientService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public PatioClientService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api          = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<SecurityRegistrationDto>>> GetVehiculosSinSalidaAsync()
        => await _api.GetAsync<ApiResponseDto<List<SecurityRegistrationDto>>>(
               _apiEndpoints.Security_GetSinSalida);

    public async Task<ApiResponseDto<List<CortinaDto>>> GetCortinasDisponiblesAsync(int? warehouseId = null)
    {
        var url = _apiEndpoints.Security_GetCortinas;
        if (warehouseId.HasValue) url += $"?warehouseId={warehouseId}";
        return await _api.GetAsync<ApiResponseDto<List<CortinaDto>>>(url);
    }

    public async Task<ApiResponseDto<string>> AsignarCortinaAsync(int registroId, int cortinaId)
    {
        var url     = _apiEndpoints.Security_AsignarCortina.Replace("{id}", registroId.ToString());
        var request = new AsignarCortinaRequest { CortinaId = cortinaId };
        return await _api.PutAsync<AsignarCortinaRequest, ApiResponseDto<string>>(url, request);
    }

    public async Task<ApiResponseDto<List<SecurityTaskDto>>> GetTasksAsync(bool soloPendientes = false)
    {
        var url = _apiEndpoints.Security_GetTasks;
        if (soloPendientes) url += "?soloPendientes=true";
        return await _api.GetAsync<ApiResponseDto<List<SecurityTaskDto>>>(url);
    }

    public async Task<ApiResponseDto<string>> AbrirCortinaAsync(int taskId, string? realizadaPor = null)
    {
        var url = _apiEndpoints.Security_AbrirCortina.Replace("{taskId}", taskId.ToString());
        if (!string.IsNullOrEmpty(realizadaPor)) url += $"?realizadaPor={Uri.EscapeDataString(realizadaPor)}";
        return await _api.PutAsync<object, ApiResponseDto<string>>(url, new { });
    }

    public async Task<ApiResponseDto<string>> CerrarRegistroAsync(int taskId, string? realizadaPor = null)
    {
        var url = _apiEndpoints.Security_CerrarRegistro.Replace("{taskId}", taskId.ToString());
        if (!string.IsNullOrEmpty(realizadaPor)) url += $"?realizadaPor={Uri.EscapeDataString(realizadaPor)}";
        return await _api.PutAsync<object, ApiResponseDto<string>>(url, new { });
    }
}
