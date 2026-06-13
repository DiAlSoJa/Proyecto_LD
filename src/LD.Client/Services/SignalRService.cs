using LD.Contracts.SignalR;
using LD.Forms.Configuration;
using Microsoft.AspNetCore.SignalR.Client;

namespace LD.Client.Services;

public sealed class SignalRService : IAsyncDisposable
{
    private HubConnection? _connection;
    private readonly ApiEndpoints _endpoints;

    public bool IsConnected => _connection?.State == HubConnectionState.Connected;

    // Suscríbete a este evento en ViewModels para recibir notificaciones en tiempo real
    public event Action<HubNotification>? NotificationReceived;

    public SignalRService(ApiEndpoints endpoints)
    {
        _endpoints = endpoints;
    }

    public async Task ConnectAsync(string accessToken)
    {
        if (_connection?.State is HubConnectionState.Connected or HubConnectionState.Connecting)
            return;

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }

        _connection = new HubConnectionBuilder()
            .WithUrl(_endpoints.NotificationsHub, options =>
            {
                // El AccessTokenProvider se llama en cada intento de conexión,
                // por lo que siempre enviará el token más reciente de UserSession.
                options.AccessTokenProvider = () =>
                    Task.FromResult<string?>(UserSession.AccessToken ?? accessToken);

#if DEBUG
                options.HttpMessageHandlerFactory = _ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
#endif
            })
            .WithAutomaticReconnect([TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15)])
            .Build();

        _connection.On<HubNotification>("ReceiveNotification", notification =>
            NotificationReceived?.Invoke(notification));

        await _connection.StartAsync();
    }

    public async Task DisconnectAsync()
    {
        if (_connection is null) return;
        await _connection.StopAsync();
        await _connection.DisposeAsync();
        _connection = null;
    }

    /// <summary>
    /// Une al cliente al grupo del almacén para recibir broadcasts de ese almacén.
    /// Llamar desde el ViewModel después de hacer login/seleccionar almacén.
    /// </summary>
    public async Task JoinWarehouseGroupAsync(int warehouseId)
    {
        if (_connection?.State == HubConnectionState.Connected)
            await _connection.InvokeAsync("JoinWarehouseGroup", warehouseId);
    }

    public async Task LeaveWarehouseGroupAsync(int warehouseId)
    {
        if (_connection?.State == HubConnectionState.Connected)
            await _connection.InvokeAsync("LeaveWarehouseGroup", warehouseId);
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
