using CommunityToolkit.Mvvm.Messaging;
using MauiAppLogin.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MauiAppLogin
{
    public partial class App : Application
    {
        private readonly MobileSessionService _sessionService;
        private readonly IAppStateService _appState;

        public App(AppShell appShell, MobileSessionService sessionService, IAppStateService appState)
        {
            InitializeComponent();
            _sessionService = sessionService;
            _appState = appState;
            MainPage = appShell;

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

        protected override void OnSleep()
        {
            base.OnSleep();
            _appState.SetBackground(true);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _appState.SetBackground(false);
        }
    }
}
