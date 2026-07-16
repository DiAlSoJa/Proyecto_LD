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

    public async Task<ApiResponseDto<List<PatioMonitorDto>>> GetPatioMonitorAsync()
        => await _api.GetAsync<ApiResponseDto<List<PatioMonitorDto>>>(
               _apiEndpoints.Security_GetPatioMonitor);

    public async Task<ApiResponseDto<List<CortinaDto>>> GetCortinasDisponiblesAsync()
    {
        return await _api.GetAsync<ApiResponseDto<List<CortinaDto>>>(_apiEndpoints.Security_GetCortinas);
    }

    public async Task<ApiResponseDto<string>> AsignarCortinaAsync(int securityRegistrationId, int cortinaId)
    {
        var url     = _apiEndpoints.Security_AsignarCortina.Replace("{id}", securityRegistrationId.ToString());
        var request = new AsignarCortinaRequest { CortinaId = cortinaId };
        return await _api.PutAsync<AsignarCortinaRequest, ApiResponseDto<string>>(url, request);
    }

    public async Task<ApiResponseDto<List<SecurityTaskDto>>> GetTasksAsync(
        bool soloPendientes = false,
        int? securityRegistrationId = null)
    {
        var url = _apiEndpoints.Security_GetTasks;
        var query = new List<string>();

        if (soloPendientes)
        {
            query.Add("soloPendientes=true");
        }

        if (securityRegistrationId.HasValue)
        {
            query.Add($"securityRegistrationId={securityRegistrationId.Value}");
        }

        if (query.Count > 0)
        {
            url += "?" + string.Join("&", query);
        }

        return await _api.GetAsync<ApiResponseDto<List<SecurityTaskDto>>>(url);
    }

    public async Task<ApiResponseDto<string>> AbrirCortinaAsync(int taskId, string fotoBase64, string? realizadaPor = null)
    {
        var url = _apiEndpoints.Security_AbrirCortina.Replace("{taskId}", taskId.ToString());
        var request = new SecurityTaskActionRequest
        {
            RealizadaPor = realizadaPor,
            FotoBase64 = fotoBase64
        };
        return await _api.PutAsync<SecurityTaskActionRequest, ApiResponseDto<string>>(url, request);
    }

    public async Task<ApiResponseDto<string>> CerrarRegistroAsync(int taskId, string fotoBase64, string? realizadaPor = null)
    {
        var url = _apiEndpoints.Security_CerrarRegistro.Replace("{taskId}", taskId.ToString());
        var request = new SecurityTaskActionRequest
        {
            RealizadaPor = realizadaPor,
            FotoBase64 = fotoBase64
        };
        return await _api.PutAsync<SecurityTaskActionRequest, ApiResponseDto<string>>(url, request);
    }

    public async Task<ApiResponseDto<string>> IniciarOperacionAsync(int taskId, string fotoBase64, string? realizadaPor = null)
    {
        var url = _apiEndpoints.Security_IniciarOperacion.Replace("{taskId}", taskId.ToString());
        var request = new SecurityTaskActionRequest
        {
            RealizadaPor = realizadaPor,
            FotoBase64 = fotoBase64
        };
        return await _api.PutAsync<SecurityTaskActionRequest, ApiResponseDto<string>>(url, request);
    }

    public async Task<ApiResponseDto<string>> FinalizarOperacionAsync(int taskId, string fotoBase64, string? realizadaPor = null)
    {
        var url = _apiEndpoints.Security_FinalizarOperacion.Replace("{taskId}", taskId.ToString());
        var request = new SecurityTaskActionRequest
        {
            RealizadaPor = realizadaPor,
            FotoBase64 = fotoBase64
        };
        return await _api.PutAsync<SecurityTaskActionRequest, ApiResponseDto<string>>(url, request);
    }

    public async Task<ApiResponseDto<string>> GenerarCierreCortinaAsync(int securityRegistrationId)
    {
        var url = _apiEndpoints.Security_GenerarCierreCortina.Replace("{id}", securityRegistrationId.ToString());
        return await _api.PostAsync<object, ApiResponseDto<string>>(url, new { });
    }
}
