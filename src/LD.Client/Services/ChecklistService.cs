using LD.Contracts.Checklist;
using LD.Contracts.Equipment;
using LD.Contracts.Responses;
using LD.Forms.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LD.Client.Services;

public class ChecklistService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public ChecklistService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<SubmitChecklistResponse>> SubmitAsync(SubmitChecklistRequest request)
    {
        return await _api.PostAsync<SubmitChecklistRequest, ApiResponseDto<SubmitChecklistResponse>>(
            _apiEndpoints.Checklist_Submit, request);
    }

    public async Task<ApiResponseDto<List<ChecklistSummaryDto>>> GetChecklistsAsync(GetChecklistsQueryRequest filters)
    {
        var query = BuildQueryString(filters);
        return await _api.GetAsync<ApiResponseDto<List<ChecklistSummaryDto>>>(
            $"{_apiEndpoints.Checklist_GetAll}{query}");
    }

    public async Task<ApiResponseDto<ChecklistDetailDto>> GetByIdAsync(int checklistId)
    {
        return await _api.GetAsync<ApiResponseDto<ChecklistDetailDto>>(
            _apiEndpoints.Checklist_GetById.Replace("{checklistId}", checklistId.ToString()));
    }

    // Sube foto a partir de bytes en memoria (captura de cámara MAUI sin escribir a disco).
    public async Task<ApiResponseDto<EquipmentImageUploadDto>> UploadPhotoAsync(
        byte[] fileBytes, string fileName, string side)
    {
        using var content = new MultipartFormDataContent();
        using var ms = new MemoryStream(fileBytes);
        using var fileContent = new StreamContent(ms);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);
        content.Add(new StringContent(side), "side");

        return await _api.PostMultipartAsync<ApiResponseDto<EquipmentImageUploadDto>>(
            _apiEndpoints.Checklist_UploadPhoto, content);
    }

    // Consulta si el usuario tiene equipo asignado y si completó su checklist en las últimas 24 horas.
    public async Task<ApiResponseDto<ChecklistDailyStatusDto>> GetDailyStatusAsync()
    {
        return await _api.GetAsync<ApiResponseDto<ChecklistDailyStatusDto>>(
            _apiEndpoints.Checklist_DailyStatus);
    }

    // Devuelve la URL del endpoint de foto dado su relativePath.
    public string GetPhotoUrl(string relativePath)
    {
        return _apiEndpoints.Checklist_GetPhoto.Replace("{path}", Uri.EscapeDataString(relativePath));
    }

    // Descarga los bytes de una foto vía el cliente autenticado.
    public async Task<byte[]> GetPhotoBytesAsync(string relativePath)
    {
        return await _api.GetByteArrayAsync(GetPhotoUrl(relativePath));
    }

    private static string BuildQueryString(GetChecklistsQueryRequest filters)
    {
        var parts = new List<string>();

        if (filters.From.HasValue)
            parts.Add($"from={Uri.EscapeDataString(filters.From.Value.ToString("o"))}");
        if (filters.To.HasValue)
            parts.Add($"to={Uri.EscapeDataString(filters.To.Value.ToString("o"))}");
        if (filters.EquipmentTypeId.HasValue)
            parts.Add($"equipmentTypeId={filters.EquipmentTypeId.Value}");
        if (filters.EquipmentId.HasValue)
            parts.Add($"equipmentId={filters.EquipmentId.Value}");

        return parts.Count > 0 ? "?" + string.Join("&", parts) : string.Empty;
    }
}
