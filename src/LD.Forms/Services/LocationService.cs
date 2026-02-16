
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
        public LocationService()
        {
            _api = new ApiService();
            _api.SetBearerToken(UserSession.AccessToken??"");
        }
        public async Task<ApiResponseDto<LocationDto>> GetLocationById(int locationId)
        {
            return await _api.GetAsync<ApiResponseDto<LocationDto>>(ApiEndpoints.Location.GetById.Replace("{id}", locationId.ToString()));
        }
        public async Task<ApiResponseDto<List<LocationDto>>> GetLocations()
        {
            return await _api.GetAsync<ApiResponseDto<List<LocationDto>>>(ApiEndpoints.Location.GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateLocation(LocationRequest request)
        {
            return await _api.PostAsync<LocationRequest, ApiResponseDto<string>>(ApiEndpoints.Location.Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateLocation(int locationId, LocationRequest request)
        {
            return await _api.PutAsync<LocationRequest, ApiResponseDto<string>>(ApiEndpoints.Location.Update.Replace("{id}", locationId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveLocation(int locationId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(ApiEndpoints.Location.Delete.Replace("{id}", locationId.ToString()));
        }
    }
}
