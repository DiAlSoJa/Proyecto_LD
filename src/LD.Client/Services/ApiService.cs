using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using LD.Contracts.Responses;

namespace LD.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        // Registrado por la app móvil para manejar 401 automáticamente.
        // Debe retornar true si el token se renovó con éxito (y ya está aplicado),
        // o false si la sesión expiró definitivamente.
        // Si es null (WPF), los 401 se devuelven sin reintentar.
        public Func<Task<bool>>? OnUnauthorizedAsync { get; set; }

        private readonly SemaphoreSlim _refreshLock = new(1, 1);
        private bool _isRefreshing;
        private Task<bool>? _refreshTask;
        private static readonly AsyncLocal<int> RefreshDepth = new();

        public ApiService()
        {
#if DEBUG
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
#else
            var handler = new HttpClientHandler();
#endif

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
            var response = await SendRequestAsync(() => _http.GetAsync(endpoint));

            if (response.StatusCode == HttpStatusCode.Unauthorized && await TryRefreshTokenAsync())
                response = await SendRequestAsync(() => _http.GetAsync(endpoint));

            return await HandleResponse<T>(response);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body)
        {
            var response = await SendRequestAsync(() => _http.PostAsJsonAsync(endpoint, body));

            if (response.StatusCode == HttpStatusCode.Unauthorized && await TryRefreshTokenAsync())
                response = await SendRequestAsync(() => _http.PostAsJsonAsync(endpoint, body));

            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest body)
        {
            var response = await SendRequestAsync(() => _http.PutAsJsonAsync(endpoint, body));

            if (response.StatusCode == HttpStatusCode.Unauthorized && await TryRefreshTokenAsync())
                response = await SendRequestAsync(() => _http.PutAsJsonAsync(endpoint, body));

            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> PostMultipartAsync<TResponse>(string endpoint, MultipartFormDataContent content)
        {
            // No se reintenta multipart porque el stream puede estar consumido
            var response = await SendRequestAsync(() => _http.PostAsync(endpoint, content));
            return await HandleResponse<TResponse>(response);
        }

        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            var response = await SendRequestAsync(() => _http.DeleteAsync(endpoint));

            if (response.StatusCode == HttpStatusCode.Unauthorized && await TryRefreshTokenAsync())
                response = await SendRequestAsync(() => _http.DeleteAsync(endpoint));

            return await HandleResponse<T>(response);
        }

        public async Task<byte[]> GetByteArrayAsync(string endpoint)
        {
            var response = await SendRequestAsync(() => _http.GetAsync(endpoint));
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(GetHttpErrorMessage(response.StatusCode));
            }

            return await response.Content.ReadAsByteArrayAsync();
        }

        private static async Task<HttpResponseMessage> SendRequestAsync(
            Func<Task<HttpResponseMessage>> request)
        {
            try
            {
                return await request();
            }
            catch (TaskCanceledException ex)
            {
                throw new InvalidOperationException(
                    "La solicitud tardó demasiado tiempo. Verifica tu conexión e inténtalo nuevamente.",
                    ex);
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException(
                    "No fue posible comunicarse con el servidor. Verifica tu conexión e inténtalo nuevamente.",
                    ex);
            }
        }

        // Intenta hacer refresh. Usa lock para que peticiones concurrentes
        // no disparen múltiples refreshes. Devuelve false si no hay callback
        // o si estamos ya en medio de un refresh (evita recursión).
        private async Task<bool> TryRefreshTokenAsync()
        {
            if (OnUnauthorizedAsync is null)
                return false;

            // Evita recursión si el propio flujo de refresh devuelve 401.
            if (RefreshDepth.Value > 0)
                return false;

            Task<bool> currentRefreshTask;

            await _refreshLock.WaitAsync();
            try
            {
                if (_isRefreshing && _refreshTask is not null)
                {
                    currentRefreshTask = _refreshTask;
                }
                else
                {
                    _isRefreshing = true;
                    _refreshTask = ExecuteRefreshAsync();
                    currentRefreshTask = _refreshTask;
                }
            }
            finally
            {
                _refreshLock.Release();
            }

            return await currentRefreshTask;
        }

        private async Task<bool> ExecuteRefreshAsync()
        {
            try
            {
                RefreshDepth.Value++;
                return await OnUnauthorizedAsync!();
            }
            catch
            {
                return false;
            }
            finally
            {
                RefreshDepth.Value = Math.Max(0, RefreshDepth.Value - 1);

                await _refreshLock.WaitAsync();
                try
                {
                    _isRefreshing = false;
                    _refreshTask = null;
                }
                finally
                {
                    _refreshLock.Release();
                }
            }
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
                    GetHttpErrorMessage(response.StatusCode));

                return (T)fallback!;
            }

            throw new InvalidOperationException(
                response.IsSuccessStatusCode
                    ? "No se pudo interpretar la respuesta del servidor."
                    : GetHttpErrorMessage(response.StatusCode));
        }

        private static string GetHttpErrorMessage(HttpStatusCode statusCode) =>
            statusCode switch
            {
                HttpStatusCode.BadRequest => "La solicitud contiene datos no válidos. Revisa la información capturada.",
                HttpStatusCode.Unauthorized => "Tu sesión no es válida o ha expirado. Inicia sesión nuevamente.",
                HttpStatusCode.Forbidden => "No tienes permiso para realizar esta acción.",
                HttpStatusCode.NotFound => "No se encontró la información solicitada.",
                HttpStatusCode.Conflict => "La operación genera un conflicto con la información existente.",
                HttpStatusCode.UnprocessableEntity => "No se pudo procesar la información capturada.",
                HttpStatusCode.InternalServerError => "Ocurrió un error en el servidor. Inténtalo nuevamente.",
                HttpStatusCode.BadGateway => "El servidor no está disponible temporalmente. Inténtalo más tarde.",
                HttpStatusCode.ServiceUnavailable => "El servicio no está disponible temporalmente. Inténtalo más tarde.",
                HttpStatusCode.GatewayTimeout => "El servidor tardó demasiado tiempo en responder. Inténtalo nuevamente.",
                _ => $"No se pudo completar la solicitud. Código de respuesta: {(int)statusCode}."
            };
    }
}
