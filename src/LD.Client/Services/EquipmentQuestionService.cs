using LD.Contracts.EquipmentQuestion;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class EquipmentQuestionService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public EquipmentQuestionService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<EquipmentQuestionDto>>> GetByEquipmentType(int equipmentTypeId)
    {
        return await _api.GetAsync<ApiResponseDto<List<EquipmentQuestionDto>>>(
            _apiEndpoints.EquipmentQuestion_GetByEquipmentType.Replace("{equipmentTypeId}", equipmentTypeId.ToString()));
    }

    public async Task<ApiResponseDto<string>> Save(EquipmentQuestionRequest request)
    {
        return await _api.PostAsync<EquipmentQuestionRequest, ApiResponseDto<string>>(_apiEndpoints.EquipmentQuestion_Save, request);
    }

    public async Task<ApiResponseDto<string>> Delete(int equipmentQuestionDetId)
    {
        return await _api.DeleteAsync<ApiResponseDto<string>>(
            _apiEndpoints.EquipmentQuestion_Delete.Replace("{equipmentQuestionDetId}", equipmentQuestionDetId.ToString()));
    }
}
