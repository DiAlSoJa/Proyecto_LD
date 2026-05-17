using CommunityToolkit.Mvvm.Messaging;
using MauiAppLogin.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MauiAppLogin
{
    public partial class App : Application
    {
        private readonly MobileSessionService _sessionService;

        public App(AppShell appShell, MobileSessionService sessionService)
        {
            InitializeComponent();
            _sessionService = sessionService;
            MainPage = appShell;

            // Cuando cualquier petición falla el refresh, navegar a login
            WeakReferenceMessenger.Default.Register<SessionExpiredMessage>(this, async (_, _) =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await _sessionService.ClearAsync();
                    if (Shell.Current is not null)
                        await Shell.Current.GoToAsync("//login");
                });
            });
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await TryRestoreSessionAsync();
        }

        private async Task TryRestoreSessionAsync()
        {
            try
            {
                var restored = await _sessionService.TryRestoreFullSessionAsync();
                if (!restored) return;

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    if (Shell.Current is not null)
                        await Shell.Current.GoToAsync("//dashboard");
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TryRestoreSession] {ex}");
            }
        }
    }
}
