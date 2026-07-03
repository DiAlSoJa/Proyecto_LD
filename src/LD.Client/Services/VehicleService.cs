using LD.Contracts.Responses;
using LD.Contracts.Requests;
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

        public async Task<ApiResponseDto<string>> CreateVehicle(VechicleRequest request)
        {
            return await _api.PostAsync<VechicleRequest, ApiResponseDto<string>>(
                _apiEndpoints.Vehicle_Create,
                request);
        }

        public async Task<ApiResponseDto<string>> UpdateVehicle(string plates, VechicleRequest request)
        {
            return await _api.PutAsync<VechicleRequest, ApiResponseDto<string>>(
                _apiEndpoints.Vehicle_Update.Replace("{id}", plates),
                request);
        }

        public async Task<ApiResponseDto<string>> DeleteVehicle(string plates)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(
                _apiEndpoints.Vehicle_Delete.Replace("{id}", plates));
        }
    }
}
