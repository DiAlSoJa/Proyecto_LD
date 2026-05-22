using LD.Contracts.DTOs.OperationalTasks;
using LD.Contracts.OperationalTasks;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;
using System.Net.Http.Headers;

namespace LD.Client.Services;

public class OperationalTaskService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public OperationalTaskService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<OperationalTaskDto>>> GetTasks(bool soloPendientes = false, int? warehouseId = null)
    {
        var query = new List<string>();
        if (soloPendientes)
            query.Add("soloPendientes=true");
        if (warehouseId.HasValue)
            query.Add($"warehouseId={warehouseId.Value}");

        var endpoint = query.Count > 0
            ? $"{_apiEndpoints.OperationalTask_GetAll}?{string.Join("&", query)}"
            : _apiEndpoints.OperationalTask_GetAll;

        return await _api.GetAsync<ApiResponseDto<List<OperationalTaskDto>>>(endpoint);
    }

    public async Task<ApiResponseDto<OperationalTaskDto>> GetTaskById(int taskId)
    {
        return await _api.GetAsync<ApiResponseDto<OperationalTaskDto>>(
            _apiEndpoints.OperationalTask_GetById.Replace("{taskId}", taskId.ToString()));
    }

    public async Task<ApiResponseDto<string>> CreateTask(OperationalTaskRequest request)
    {
        return await _api.PostAsync<OperationalTaskRequest, ApiResponseDto<string>>(
            _apiEndpoints.OperationalTask_Create,
            request);
    }

    public async Task<ApiResponseDto<string>> CompleteTask(int taskId, CompleteOperationalTaskRequest request)
    {
        var endpoint = _apiEndpoints.OperationalTask_Complete.Replace("{taskId}", taskId.ToString());
        return await _api.PutAsync<CompleteOperationalTaskRequest, ApiResponseDto<string>>(endpoint, request);
    }

    public async Task<ApiResponseDto<OperationalTaskImageUploadDto>> UploadImage(string filePath, int photoNumber)
    {
        using var content = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(filePath);
        using var fileContent = new StreamContent(fileStream);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", Path.GetFileName(filePath));
        content.Add(new StringContent(photoNumber.ToString()), "photoNumber");

        return await _api.PostMultipartAsync<ApiResponseDto<OperationalTaskImageUploadDto>>(
            _apiEndpoints.OperationalTask_UploadImage,
            content);
    }

    public string GetImageUrl(string relativePath)
    {
        return _apiEndpoints.OperationalTask_GetImage.Replace("{path}", Uri.EscapeDataString(relativePath));
    }
}
