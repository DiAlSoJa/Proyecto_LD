using LD.Contracts.Equipment;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LD.Client.Services;

public class EquipmentService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public EquipmentService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<EquipmentRequest>> GetEquipmentById(int equipmentId)
    {
        return await _api.GetAsync<ApiResponseDto<EquipmentRequest>>(
            _apiEndpoints.Equipment_GetById.Replace("{equipmentId}", equipmentId.ToString()));
    }

    public async Task<ApiResponseDto<List<EquipmentDto>>> GetEquipments()
    {
        return await _api.GetAsync<ApiResponseDto<List<EquipmentDto>>>(_apiEndpoints.Equipment_GetAll);
    }

    public async Task<ApiResponseDto<EquipmentDto?>> GetAssignedToMeAsync()
    {
        return await _api.GetAsync<ApiResponseDto<EquipmentDto?>>(_apiEndpoints.Equipment_AssignedToMe);
    }

    public async Task<ApiResponseDto<string>> CreateEquipment(EquipmentRequest request)
    {
        return await _api.PostAsync<EquipmentRequest, ApiResponseDto<string>>(_apiEndpoints.Equipment_Create, request);
    }

    public async Task<ApiResponseDto<string>> UpdateEquipment(int equipmentId, EquipmentRequest request)
    {
        return await _api.PutAsync<EquipmentRequest, ApiResponseDto<string>>(
            _apiEndpoints.Equipment_Update.Replace("{equipmentId}", equipmentId.ToString()), request);
    }

    public async Task<ApiResponseDto<EquipmentImageUploadDto>> UploadImage(string filePath, string side)
    {
        using var content = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(filePath);
        using var fileContent = new StreamContent(fileStream);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", Path.GetFileName(filePath));
        content.Add(new StringContent(side), "side");

        return await _api.PostMultipartAsync<ApiResponseDto<EquipmentImageUploadDto>>(
            _apiEndpoints.Equipment_UploadImage,
            content);
    }

    public string GetImageUrl(string relativePath)
    {
        return _apiEndpoints.Equipment_GetImage.Replace("{path}", Uri.EscapeDataString(relativePath));
    }

    public async Task<byte[]> DownloadImage(string relativePath)
    {
        return await _api.GetByteArrayAsync(GetImageUrl(relativePath));
    }

    public async Task<byte[]> DownloadImage(int equipmentId, string side)
    {
        var endpoint = _apiEndpoints.Equipment_GetImageBySide
            .Replace("{equipmentId}", equipmentId.ToString())
            .Replace("{side}", side);

        return await _api.GetByteArrayAsync(endpoint);
    }
}
