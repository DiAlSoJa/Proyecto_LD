using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using LD.Contracts.Responses;

namespace LD.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _http = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(20)
            };

            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetBearerToken(string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearToken()
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _http.GetAsync(endpoint);
            return await HandleResponse<T>(response);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body)
        {
            var response = await _http.PostAsJsonAsync(endpoint, body);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest body)
        {
            var response = await _http.PutAsJsonAsync(endpoint, body);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> PostMultipartAsync<TResponse>(string endpoint, MultipartFormDataContent content)
        {
            var response = await _http.PostAsync(endpoint, content);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            var response = await _http.DeleteAsync(endpoint);
            return await HandleResponse<T>(response);
        }

        public async Task<byte[]> GetByteArrayAsync(string endpoint)
        {
            var response = await _http.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        private static async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var contentDeserialize = JsonSerializer.Deserialize<T>(content, options);
                if (contentDeserialize is not null)
                    return contentDeserialize;
            }
            catch
            {
                // Si no se puede deserializar al tipo esperado, intentamos regresar un error legible.
            }

            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(ApiResponseDto<>))
            {
                var dataType = typeof(T).GetGenericArguments()[0];
                var fallbackType = typeof(ApiResponseDto<>).MakeGenericType(dataType);
                var fallback = Activator.CreateInstance(fallbackType);

                fallbackType.GetProperty(nameof(ApiResponseDto<object>.IsSuccess))?.SetValue(fallback, false);
                fallbackType.GetProperty(nameof(ApiResponseDto<object>.Code))?.SetValue(fallback, (int)response.StatusCode);
                fallbackType.GetProperty(nameof(ApiResponseDto<object>.Message))?.SetValue(
                    fallback,
                    string.IsNullOrWhiteSpace(content) ? response.ReasonPhrase ?? "Error" : content);

                return (T)fallback!;
            }

            throw new InvalidOperationException(
                string.IsNullOrWhiteSpace(content)
                    ? response.ReasonPhrase ?? "No se pudo procesar la respuesta del servidor."
                    : content);
        }
    }
}
