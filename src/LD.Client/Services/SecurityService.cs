using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.DTOs.Security;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class SecurityService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public SecurityService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<string>> RegisterAsync(SecurityRegistrationRequest request)
    {
        return await _api.PostAsync<SecurityRegistrationRequest, ApiResponseDto<string>>(
            _apiEndpoints.Security_Register, request);
    }

    public async Task<ApiResponseDto<List<SecurityRegistrationDto>>> GetRegistrationsAsync(int? hours = 24, int? days = null)
    {
        var endpoint = _apiEndpoints.Security_GetRegistrations;

        if (days.HasValue)
            endpoint = $"{endpoint}?days={days.Value}";
        else if (hours.HasValue)
            endpoint = $"{endpoint}?hours={hours.Value}";

        return await _api.GetAsync<ApiResponseDto<List<SecurityRegistrationDto>>>(endpoint);
    }
}
