using LD.Contracts.Requests;
using LD.Contracts.Responses;
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
}
