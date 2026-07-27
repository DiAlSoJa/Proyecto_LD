using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.TruckType;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class TruckTypeService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public TruckTypeService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<TruckTypeDto>>> GetTruckTypes()
    {
        return await _api.GetAsync<ApiResponseDto<List<TruckTypeDto>>>(_apiEndpoints.TruckType_GetAll);
    }

    public async Task<ApiResponseDto<TruckTypeRequest>> GetTruckTypeById(int truckTypeId)
    {
        return await _api.GetAsync<ApiResponseDto<TruckTypeRequest>>(
            _apiEndpoints.TruckType_GetById.Replace("{truckTypeId}", truckTypeId.ToString()));
    }

    public async Task<ApiResponseDto<string>> CreateTruckType(TruckTypeRequest request)
    {
        return await _api.PostAsync<TruckTypeRequest, ApiResponseDto<string>>(
            _apiEndpoints.TruckType_Create,
            request);
    }

    public async Task<ApiResponseDto<string>> UpdateTruckType(int truckTypeId, TruckTypeRequest request)
    {
        return await _api.PutAsync<TruckTypeRequest, ApiResponseDto<string>>(
            _apiEndpoints.TruckType_Update.Replace("{truckTypeId}", truckTypeId.ToString()),
            request);
    }

    public async Task<ApiResponseDto<string>> DeleteTruckType(int truckTypeId)
    {
        return await _api.DeleteAsync<ApiResponseDto<string>>(
            _apiEndpoints.TruckType_Delete.Replace("{truckTypeId}", truckTypeId.ToString()));
    }
}
