using LD.Contracts.Kitting;
using LD.Contracts.DTOs.Kitting;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;
using System.Net.Http.Headers;

namespace LD.Client.Services;

public class KittingService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public KittingService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<KittingRequest>> GetKittingById(int kittingId)
    {
        return await _api.GetAsync<ApiResponseDto<KittingRequest>>(
            _apiEndpoints.Kitting_GetById.Replace("{kittingId}", kittingId.ToString()));
    }

    public async Task<ApiResponseDto<List<KittingDto>>> GetKittings()
    {
        return await _api.GetAsync<ApiResponseDto<List<KittingDto>>>(_apiEndpoints.Kitting_GetAll);
    }

    public async Task<ApiResponseDto<List<KittingDto>>> GetKittingsByClient(int clientId, int projectId)
    {
        return await _api.GetAsync<ApiResponseDto<List<KittingDto>>>(
            _apiEndpoints.Kitting_GetByClient
                .Replace("{clientId}", clientId.ToString())
                .Replace("{projectId}", projectId.ToString()));
    }

    public async Task<ApiResponseDto<string>> CreateKitting(KittingRequest request)
    {
        return await _api.PostAsync<KittingRequest, ApiResponseDto<string>>(_apiEndpoints.Kitting_Create, request);
    }

    public async Task<ApiResponseDto<string>> UpdateKitting(int kittingId, KittingRequest request)
    {
        return await _api.PutAsync<KittingRequest, ApiResponseDto<string>>(
            _apiEndpoints.Kitting_Update.Replace("{kittingId}", kittingId.ToString()),
            request);
    }

    public async Task<ApiResponseDto<string>> ConfirmKitting(int kittingId)
    {
        return await _api.PostAsync<object, ApiResponseDto<string>>(
            _apiEndpoints.Kitting_Confirm.Replace("{kittingId}", kittingId.ToString()),
            new { });
    }

    public async Task<ApiResponseDto<string>> CancelKitting(int kittingId)
    {
        return await _api.PostAsync<object, ApiResponseDto<string>>(
            _apiEndpoints.Kitting_Cancel.Replace("{kittingId}", kittingId.ToString()),
            new { });
    }

    public async Task<ApiResponseDto<string>> LocateKitting(int kittingId)
    {
        return await _api.PostAsync<object, ApiResponseDto<string>>(
            _apiEndpoints.Kitting_Locate.Replace("{kittingId}", kittingId.ToString()),
            new { });
    }

    public async Task<ApiResponseDto<string>> SendToSupplyKitting(int kittingId)
    {
        return await _api.PostAsync<object, ApiResponseDto<string>>(
            _apiEndpoints.Kitting_SendToSupply.Replace("{kittingId}", kittingId.ToString()),
            new { });
    }

    public async Task<ApiResponseDto<List<KittingValidationPhotoDto>>> GetValidationPhotos(int kittingId)
    {
        return await _api.GetAsync<ApiResponseDto<List<KittingValidationPhotoDto>>>(
            _apiEndpoints.Kitting_ValidationPhotos.Replace("{kittingId}", kittingId.ToString()));
    }

    public async Task<ApiResponseDto<KittingValidationPhotoDto>> UploadValidationImage(int kittingId, string filePath)
    {
        using var content = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(filePath);
        using var fileContent = new StreamContent(fileStream);

        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(fileContent, "file", Path.GetFileName(filePath));

        return await _api.PostMultipartAsync<ApiResponseDto<KittingValidationPhotoDto>>(
            _apiEndpoints.Kitting_ValidationPhotos.Replace("{kittingId}", kittingId.ToString()),
            content);
    }

    public async Task<ApiResponseDto<string>> DeleteValidationImage(int kittingId, string photoKey)
    {
        return await _api.DeleteAsync<ApiResponseDto<string>>(
            _apiEndpoints.Kitting_ValidationPhotoByKey
                .Replace("{kittingId}", kittingId.ToString())
                .Replace("{photoKey}", Uri.EscapeDataString(photoKey)));
    }

    public string GetImageUrl(string relativePath)
    {
        return _apiEndpoints.Kitting_GetImage.Replace("{path}", Uri.EscapeDataString(relativePath));
    }
}
