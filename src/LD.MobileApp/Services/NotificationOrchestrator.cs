using LD.Client.Services;
using LD.Contracts.SignalR;
using Plugin.LocalNotification;

namespace MauiAppLogin.Services;

// Singleton. El constructor se ejecuta UNA sola vez en toda la vida de la app
// (DI garantiza que no se construya dos veces), por lo que el += ocurre exactamente
// una vez — no hay riesgo de suscripción duplicada.
public sealed class NotificationOrchestrator
{
    private readonly IAppStateService _appState;
    private int _nextId = 1000;

    public NotificationOrchestrator(SignalRService signalR, IAppStateService appState)
    {
        _appState = appState;
        signalR.NotificationReceived += OnNotificationReceived;
    }

    private void OnNotificationReceived(HubNotification notification)
    {
        if (notification.Type != "task_assigned") return;
        if (!_appState.IsBackground) return;

        var id = Interlocked.Increment(ref _nextId);

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await LocalNotificationCenter.Current.Show(new NotificationRequest
            {
                NotificationId = id,
                Title = notification.Title,
                Description = notification.Message,
            });
        });
    }
}
