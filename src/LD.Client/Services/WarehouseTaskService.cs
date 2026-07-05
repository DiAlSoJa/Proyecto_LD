using LD.Contracts.DTOs.Realtime;
using LD.Contracts.DTOs.WarehouseTasks;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;
using System.Net.Http.Headers;

namespace LD.Client.Services;

public class WarehouseTaskService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public WarehouseTaskService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<WarehouseTaskDto>>> GetTasksAsync(bool soloPendientes = false, int? warehouseId = null)
    {
        var query = new List<string>();
        if (soloPendientes)
            query.Add("soloPendientes=true");
        if (warehouseId.HasValue)
            query.Add($"warehouseId={warehouseId.Value}");

        var endpoint = query.Count > 0
            ? $"{_apiEndpoints.WarehouseTask_GetAll}?{string.Join("&", query)}"
            : _apiEndpoints.WarehouseTask_GetAll;

        return await _api.GetAsync<ApiResponseDto<List<WarehouseTaskDto>>>(endpoint);
    }

    public async Task<ApiResponseDto<WarehouseTaskDto?>> GetMyAssignedTaskAsync()
        => await _api.GetAsync<ApiResponseDto<WarehouseTaskDto?>>(_apiEndpoints.WarehouseTask_MyAssigned);

    public async Task<ApiResponseDto<List<ConnectedUserDto>>> GetConnectedUsersAsync()
        => await _api.GetAsync<ApiResponseDto<List<ConnectedUserDto>>>(_apiEndpoints.WarehouseTask_ConnectedUsers);

    public async Task<ApiResponseDto<string>> MarkUserAvailableAsync()
        => await _api.PostAsync<object, ApiResponseDto<string>>(_apiEndpoints.WarehouseTask_MarkAvailable, new { });

    public async Task<ApiResponseDto<string>> CancelWaitingAsync()
        => await _api.PostAsync<object, ApiResponseDto<string>>(_apiEndpoints.WarehouseTask_CancelWaiting, new { });

    public async Task<ApiResponseDto<string>> CreateTaskAsync(WarehouseTaskRequest request)
        => await _api.PostAsync<WarehouseTaskRequest, ApiResponseDto<string>>(_apiEndpoints.WarehouseTask_Create, request);

    public async Task<ApiResponseDto<string>> CompleteTaskAsync(int taskId, CompleteWarehouseTaskRequest request)
    {
        var endpoint = _apiEndpoints.WarehouseTask_Complete.Replace("{taskId}", taskId.ToString());
        return await _api.PutAsync<CompleteWarehouseTaskRequest, ApiResponseDto<string>>(endpoint, request);
    }

    public async Task<ApiResponseDto<WarehouseTaskImageUploadDto>> UploadImageAsync(string filePath, int photoNumber)
    {
        using var content = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(filePath);
        using var fileContent = new StreamContent(fileStream);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", Path.GetFileName(filePath));
        content.Add(new StringContent(photoNumber.ToString()), "photoNumber");

        return await _api.PostMultipartAsync<ApiResponseDto<WarehouseTaskImageUploadDto>>(
            _apiEndpoints.WarehouseTask_UploadImage,
            content);
    }

    public string GetImageUrl(string relativePath)
        => _apiEndpoints.WarehouseTask_GetImage.Replace("{path}", Uri.EscapeDataString(relativePath));
}
