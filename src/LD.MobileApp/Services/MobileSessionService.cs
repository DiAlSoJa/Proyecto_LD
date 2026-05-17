using CommunityToolkit.Mvvm.Messaging;
using LD.Client.Configuration;
using LD.Client.Services;

namespace MauiAppLogin.Services;

public record SessionExpiredMessage;

public class MobileSessionService
{
    private const string KeyAccessToken  = "auth_access_token";
    private const string KeyRefreshToken = "auth_refresh_token";
    private const string KeyTokenExpiry  = "auth_token_expiry";

    // Instancias resueltas desde el root scope en MauiProgram.cs.
    // Son las MISMAS que usan todos los ViewModels, por lo que
    // SetBearerToken/ClearToken afecta todas las peticiones de la app.
    private ApiService?  _apiService;
    private AuthService? _authService;

    public void Configure(ApiService apiService, AuthService authService)
    {
        _apiService  = apiService;
        _authService = authService;
        apiService.OnUnauthorizedAsync = TryRefreshAsync;
    }

    public async Task PersistAsync(string accessToken, string refreshToken, DateTime expiry)
    {
        await SecureStorage.Default.SetAsync(KeyAccessToken,  accessToken);
        await SecureStorage.Default.SetAsync(KeyRefreshToken, refreshToken);
        await SecureStorage.Default.SetAsync(KeyTokenExpiry,  expiry.ToString("O"));
    }

    // Restaura el token en memoria + carga UserData desde el API.
    // Retorna true si la sesión está lista para navegar al dashboard.
    public async Task<bool> TryRestoreFullSessionAsync()
    {
        var tokenRestored = await TryRestoreTokenAsync();
        if (!tokenRestored) return false;

        // Usa la instancia raíz de AuthService/ApiService (misma que LoginViewModel)
        // para que el bearer token recién restaurado sea enviado correctamente.
        var getMeResponse = await _authService!.GetMeAsync();

        if (!getMeResponse.IsSuccess || getMeResponse.Data is null)
        {
            // Solo borrar SecureStorage si el servidor rechaza el token.
            // Un error de red no debe desloguear al usuario.
            if (getMeResponse.Code is 401 or 403)
                await ClearAsync();
            return false;
        }

        UserData.SetUserData(getMeResponse.Data);
        return true;
    }

    // Callback registrado en ApiService.OnUnauthorizedAsync.
    // Se invoca automáticamente cuando cualquier petición recibe 401.
    public async Task<bool> TryRefreshAsync()
    {
        var refreshToken = await SecureStorage.Default.GetAsync(KeyRefreshToken);
        if (string.IsNullOrEmpty(refreshToken))
        {
            await InvalidateSessionAsync();
            return false;
        }

        // El endpoint /auth/refresh no requiere Authorization header.
        var result = await _authService!.RefreshTokenAsync(refreshToken);

        if (!result.IsSuccess || result.Data is null)
        {
            await InvalidateSessionAsync();
            return false;
        }

        var newExpiry = DateTime.UtcNow.AddHours(8);
        await PersistAsync(result.Data.Accesstoken!, result.Data.RefreshToken!, newExpiry);

        UserSession.AccessToken  = result.Data.Accesstoken;
        UserSession.RefreshToken = result.Data.RefreshToken;
        _apiService?.SetBearerToken(result.Data.Accesstoken!);

        return true;
    }

    public async Task ClearAsync()
    {
        SecureStorage.Default.Remove(KeyAccessToken);
        SecureStorage.Default.Remove(KeyRefreshToken);
        SecureStorage.Default.Remove(KeyTokenExpiry);
        UserSession.LogOut();
        _apiService?.ClearToken();
    }

    private async Task<bool> TryRestoreTokenAsync()
    {
        var accessToken = await SecureStorage.Default.GetAsync(KeyAccessToken);
        var expiryStr   = await SecureStorage.Default.GetAsync(KeyTokenExpiry);

        if (string.IsNullOrEmpty(accessToken))
            return false;

        if (!string.IsNullOrEmpty(expiryStr)
            && DateTime.TryParse(expiryStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expiry)
            && DateTime.UtcNow >= expiry)
        {
            // Token expirado: intentar refresh silencioso antes de descartarlo
            return await TryRefreshAsync();
        }

        var refreshToken = await SecureStorage.Default.GetAsync(KeyRefreshToken);
        UserSession.AccessToken  = accessToken;
        UserSession.RefreshToken = refreshToken;
        _apiService?.SetBearerToken(accessToken);
        return true;
    }

    private async Task InvalidateSessionAsync()
    {
        await ClearAsync();
        WeakReferenceMessenger.Default.Send(new SessionExpiredMessage());
    }
}
