using LD.Contracts.Driver;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class DriverService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public DriverService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<DriverDto>>> GetDrivers()
        => await _api.GetAsync<ApiResponseDto<List<DriverDto>>>(_apiEndpoints.Driver_GetAll);

    public async Task<ApiResponseDto<DriverRequest>> GetDriverById(int driverId)
        => await _api.GetAsync<ApiResponseDto<DriverRequest>>(
            _apiEndpoints.Driver_GetById.Replace("{id}", driverId.ToString()));

    public async Task<ApiResponseDto<string>> CreateDriver(DriverRequest request)
        => await _api.PostAsync<DriverRequest, ApiResponseDto<string>>(_apiEndpoints.Driver_Create, request);

    public async Task<ApiResponseDto<string>> UpdateDriver(int driverId, DriverRequest request)
        => await _api.PutAsync<DriverRequest, ApiResponseDto<string>>(
            _apiEndpoints.Driver_Update.Replace("{id}", driverId.ToString()),
            request);

    public async Task<ApiResponseDto<string>> DeleteDriver(int driverId)
        => await _api.DeleteAsync<ApiResponseDto<string>>(
            _apiEndpoints.Driver_Delete.Replace("{id}", driverId.ToString()));
}
