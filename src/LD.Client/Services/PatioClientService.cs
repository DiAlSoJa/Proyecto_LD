using LD.Contracts.DTOs.Security;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class PatioClientService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public PatioClientService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<SecurityRegistrationDto>>> GetVehiculosSinSalidaAsync()
    {
        return await _api.GetAsync<ApiResponseDto<List<SecurityRegistrationDto>>>(
            _apiEndpoints.Security_GetSinSalida);
    }
}
