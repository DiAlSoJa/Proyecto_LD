using CommunityToolkit.Mvvm.Messaging;
using LD.Client.Configuration;
using LD.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MauiAppLogin.Services;

public record SessionExpiredMessage;

public class MobileSessionService
{
    private const string KeyAccessToken  = "auth_access_token";
    private const string KeyRefreshToken = "auth_refresh_token";
    private const string KeyTokenExpiry  = "auth_token_expiry";

    private readonly IServiceScopeFactory _scopeFactory;
    private ApiService? _apiService;

    public MobileSessionService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    // Llamar después de resolver ApiService desde el root del DI.
    // Registra el callback de refresh para que ApiService lo use en 401.
    public void Configure(ApiService apiService)
    {
        _apiService = apiService;
        apiService.OnUnauthorizedAsync = TryRefreshAsync;
    }

    public async Task PersistAsync(string accessToken, string refreshToken, DateTime expiry)
    {
        await SecureStorage.Default.SetAsync(KeyAccessToken,  accessToken);
        await SecureStorage.Default.SetAsync(KeyRefreshToken, refreshToken);
        await SecureStorage.Default.SetAsync(KeyTokenExpiry,  expiry.ToString("O"));
    }

    // Intenta restaurar la sesión completa (token + UserData) desde SecureStorage.
    // Retorna true si la sesión quedó lista para usar.
    public async Task<bool> TryRestoreFullSessionAsync()
    {
        var tokenRestored = await TryRestoreTokenAsync();
        if (!tokenRestored) return false;

        using var scope = _scopeFactory.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

        var getMeResponse = await authService.GetMeAsync();
        if (!getMeResponse.IsSuccess || getMeResponse.Data is null)
        {
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

        using var scope = _scopeFactory.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
        var result = await authService.RefreshTokenAsync(refreshToken);

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
