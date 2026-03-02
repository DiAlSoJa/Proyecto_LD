
using LD.Contracts;
using LD.Contracts.Client;
using LD.Contracts.Item;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Services
{
    public class LocationService
    {
        public readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public LocationService(ApiEndpoints apiEndpoints)
        {
            _api = new ApiService();
            _api.SetBearerToken(UserSession.AccessToken ?? "");
            _apiEndpoints = apiEndpoints;
        }
        public async Task<ApiResponseDto<LocationRequest>> GetLocationById(int locationId)
        {
            return await _api.GetAsync<ApiResponseDto<LocationRequest>>(_apiEndpoints.Location_GetById.Replace("{id}", locationId.ToString()));
        }
        public async Task<ApiResponseDto<List<LocationDto>>> GetLocations()
        {
            return await _api.GetAsync<ApiResponseDto<List<LocationDto>>>(_apiEndpoints.Location_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateLocation(LocationRequest request)
        {
            return await _api.PostAsync<LocationRequest, ApiResponseDto<string>>(_apiEndpoints.Location_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateLocation(int locationId, LocationRequest request)
        {
            return await _api.PutAsync<LocationRequest, ApiResponseDto<string>>(_apiEndpoints.Location_Update.Replace("{id}", locationId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveLocation(int locationId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.Location_Delete.Replace("{id}", locationId.ToString()));
        }
    }
}
