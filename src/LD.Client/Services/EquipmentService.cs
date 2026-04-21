using LD.Contracts.Equipment;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

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

    public async Task<ApiResponseDto<string>> CreateEquipment(EquipmentRequest request)
    {
        return await _api.PostAsync<EquipmentRequest, ApiResponseDto<string>>(_apiEndpoints.Equipment_Create, request);
    }

    public async Task<ApiResponseDto<string>> UpdateEquipment(int equipmentId, EquipmentRequest request)
    {
        return await _api.PutAsync<EquipmentRequest, ApiResponseDto<string>>(
            _apiEndpoints.Equipment_Update.Replace("{equipmentId}", equipmentId.ToString()), request);
    }
}
