using LD.Contracts.Responses;
using LD.Contracts.Vehicle;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class VehicleService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;

        public VehicleService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<ApiResponseDto<List<VehicleDto>>> GetVehicles()
        {
            return await _api.GetAsync<ApiResponseDto<List<VehicleDto>>>(_apiEndpoints.Vehicle_GetAll);
        }

        public async Task<ApiResponseDto<VehicleDto>> GetVehicleByPlates(string plates)
        {
            return await _api.GetAsync<ApiResponseDto<VehicleDto>>(
                _apiEndpoints.Vehicle_GetById.Replace("{id}", plates));
        }
    }
}
