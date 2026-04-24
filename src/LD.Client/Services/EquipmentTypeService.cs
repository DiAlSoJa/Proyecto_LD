using LD.Contracts.EquipmentType;
using LD.Contracts.Equipment;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;
using System.IO;
using System.Net.Http.Headers;

namespace LD.Client.Services
{
    public class EquipmentTypeService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;

        public EquipmentTypeService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<ApiResponseDto<EquipmentTypeRequest>> GetEquipmentTypeById(int equipmentTypeId)
        {
            return await _api.GetAsync<ApiResponseDto<EquipmentTypeRequest>>(
                _apiEndpoints.EquipmentType_GetById.Replace("{equipmentTypeId}", equipmentTypeId.ToString()));
        }

        public async Task<ApiResponseDto<List<EquipmentTypeDto>>> GetEquipmentTypes()
        {
            return await _api.GetAsync<ApiResponseDto<List<EquipmentTypeDto>>>(_apiEndpoints.EquipmentType_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateEquipmentType(EquipmentTypeRequest request)
        {
            return await _api.PostAsync<EquipmentTypeRequest, ApiResponseDto<string>>(_apiEndpoints.EquipmentType_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateEquipmentType(int equipmentTypeId, EquipmentTypeRequest request)
        {
            return await _api.PutAsync<EquipmentTypeRequest, ApiResponseDto<string>>(
                _apiEndpoints.EquipmentType_Update.Replace("{equipmentTypeId}", equipmentTypeId.ToString()),
                request);
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
                _apiEndpoints.EquipmentType_UploadImage,
                content);
        }

        public string GetImageUrl(string relativePath)
        {
            return _apiEndpoints.EquipmentType_GetImage.Replace("{path}", Uri.EscapeDataString(relativePath));
        }

        public async Task<byte[]> DownloadImage(string relativePath)
        {
            return await _api.GetByteArrayAsync(GetImageUrl(relativePath));
        }
    }
}
