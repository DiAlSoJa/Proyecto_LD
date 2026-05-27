using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.User;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class AuthService
    {
        public readonly ApiService _api;
        public readonly ApiEndpoints _endpoints;

        public AuthService(ApiService api, ApiEndpoints endpoints)
        {
            _api       = api;
            _endpoints = endpoints;
        }

        public async Task<ApiResponseDto<GetMeReponse?>> GetMeAsync()
            => await _api.GetAsync<ApiResponseDto<GetMeReponse?>>(_endpoints.GetMe);

        public async Task<ApiResponseDto<LoginResponse>> LoginAsync(string user, string password)
        {
            var result = await _api.PostAsync<LoginRequest, ApiResponseDto<LoginResponse>>(
                _endpoints.Login,
                new LoginRequest { Username = user, Password = password });

            if (result.IsSuccess)
                _api.SetBearerToken(result.Data?.Accesstoken!);

            return result;
        }

        public async Task<ApiResponseDto<LoginResponse>> RefreshTokenAsync(string refreshToken)
            => await _api.PostWithoutAuthorizationAsync<RefreshTokenRequest, ApiResponseDto<LoginResponse>>(
                _endpoints.RefreshToken,
                new RefreshTokenRequest { RefreshToken = refreshToken });
    }
}
