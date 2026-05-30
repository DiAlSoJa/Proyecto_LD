using LD.Contracts.DamageReports;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;
using System.Net.Http.Headers;

namespace LD.Client.Services;

public class DamageReportService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public DamageReportService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<DamageReportDto>>> GetDamageReports(
        DateTime? desde = null,
        DateTime? hasta = null,
        int? standardId = null,
        int? warehouseId = null,
        string? warehouse = null,
        string? partNumber = null,
        string? damageType = null)
    {
        var query = new List<string>();

        if (desde.HasValue)
            query.Add($"desde={Uri.EscapeDataString(desde.Value.ToString("yyyy-MM-dd"))}");

        if (hasta.HasValue)
            query.Add($"hasta={Uri.EscapeDataString(hasta.Value.ToString("yyyy-MM-dd"))}");

        if (standardId.HasValue)
            query.Add($"standardId={standardId.Value}");

        if (warehouseId.HasValue)
            query.Add($"warehouseId={warehouseId.Value}");

        if (!string.IsNullOrWhiteSpace(warehouse))
            query.Add($"warehouse={Uri.EscapeDataString(warehouse.Trim())}");

        if (!string.IsNullOrWhiteSpace(partNumber))
            query.Add($"partNumber={Uri.EscapeDataString(partNumber.Trim())}");

        if (!string.IsNullOrWhiteSpace(damageType))
            query.Add($"damageType={Uri.EscapeDataString(damageType.Trim())}");

        var endpoint = query.Count == 0
            ? _apiEndpoints.DamageReport_GetAll
            : $"{_apiEndpoints.DamageReport_GetAll}?{string.Join("&", query)}";

        return await _api.GetAsync<ApiResponseDto<List<DamageReportDto>>>(endpoint);
    }

    public async Task<ApiResponseDto<DamageReportDto>> GetDamageReportById(int damageReportId)
    {
        return await _api.GetAsync<ApiResponseDto<DamageReportDto>>(
            _apiEndpoints.DamageReport_GetById.Replace("{damageReportId}", damageReportId.ToString()));
    }

    public async Task<ApiResponseDto<string>> CreateDamageReport(DamageReportRequest request)
    {
        return await _api.PostAsync<DamageReportRequest, ApiResponseDto<string>>(
            _apiEndpoints.DamageReport_Create,
            request);
    }

    public async Task<ApiResponseDto<DamageReportImageUploadDto>> UploadImage(string filePath, int photoNumber)
    {
        using var content = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(filePath);
        using var fileContent = new StreamContent(fileStream);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", Path.GetFileName(filePath));
        content.Add(new StringContent(photoNumber.ToString()), "photoNumber");

        return await _api.PostMultipartAsync<ApiResponseDto<DamageReportImageUploadDto>>(
            _apiEndpoints.DamageReport_UploadImage,
            content);
    }

    public string GetImageUrl(string relativePath)
    {
        return _apiEndpoints.DamageReport_GetImage.Replace("{path}", Uri.EscapeDataString(relativePath));
    }

    public async Task<byte[]?> GetImageBytesAsync(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return null;

        var endpoint = Uri.TryCreate(relativePath, UriKind.Absolute, out _)
            ? relativePath
            : GetImageUrl(relativePath);

        return await _api.GetByteArrayAsync(endpoint);
    }
}
