using LD.Contracts.EquipmentSupplier;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class EquipmentSupplierService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public EquipmentSupplierService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<EquipmentSupplierDto>>> GetEquipmentSuppliers()
    {
        return await _api.GetAsync<ApiResponseDto<List<EquipmentSupplierDto>>>(_apiEndpoints.EquipmentSupplier_GetAll);
    }

    public async Task<ApiResponseDto<string>> CreateEquipmentSupplier(EquipmentSupplierRequest request)
    {
        return await _api.PostAsync<EquipmentSupplierRequest, ApiResponseDto<string>>(
            _apiEndpoints.EquipmentSupplier_Create,
            request);
    }
}
