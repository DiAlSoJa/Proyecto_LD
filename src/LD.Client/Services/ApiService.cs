using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace LD.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService()
        {
            _http = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(20)
            };

            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // =========================
        // TOKEN
        // =========================
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

        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string endpoint, TRequest body)
        {
            var response = await _http.PostAsJsonAsync(endpoint, body);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(
            string endpoint, TRequest body)
        {
            var response = await _http.PutAsJsonAsync(endpoint, body);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            var response = await _http.DeleteAsync(endpoint);
            return await HandleResponse<T>(response);

        }

        private static async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            var contentDeserialize = JsonSerializer.Deserialize<T>(
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive =true
                })!;
            return contentDeserialize;
        }
    }
}
