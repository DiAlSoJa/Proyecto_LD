using CommunityToolkit.Maui;
using LD.Client;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MauiAppLogin.ViewModels;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.Maui.OCR;
using ZXing.Net.Maui.Controls;
using static System.Net.WebRequestMethods;

namespace MauiAppLogin
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseLocalNotification()
                .UseBarcodeReader()
                .UseOcr()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });



            const string defaultApiUrl = "https://ld-api-prod-gzcccygmfnb7gkdz.mexicocentral-01.azurewebsites.net/api";
            var apiUrl = Preferences.Default.Get("ApiBaseUrl", defaultApiUrl);

            builder.Services.AddLDClient(options =>
            {
                options.BaseUrl = apiUrl;
            });

            #if DEBUG
            builder.Logging.AddDebug();
            #endif

            builder.Services.AddTransient<RegisterVehicule>();
            builder.Services.AddTransient<RegisterLicense>();
            builder.Services.AddTransient<SignatureDriver>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<ChangeLocationPage>();
            builder.Services.AddTransient<MovementPage>();
            builder.Services.AddTransient<MovementDetail>();
            builder.Services.AddTransient<DamageReportPage>();
            builder.Services.AddTransient<DamageReportDetailPage>();
            builder.Services.AddTransient<DamageReportPrintPage>();
            builder.Services.AddTransient<ReceptionPage>();
            builder.Services.AddTransient<PickingPage>();
            builder.Services.AddTransient<NewTask>();
            builder.Services.AddTransient<TaskResolve>();
            builder.Services.AddTransient<WarehouseOperations>();
            builder.Services.AddTransient<InventoryList>();
            builder.Services.AddTransient<ForkliftChecklistPage>();
            builder.Services.AddTransient<NoEquipmentPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<TaskList>();

            // ViewModels
            builder.Services.AddTransient<ForkliftChecklistViewModel>();
            builder.Services.AddTransient<NoEquipmentViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<RegisterLicenseViewModel>();
            builder.Services.AddTransient<RegisterVehiculeViewModel>();
            builder.Services.AddTransient<SignatureDriverViewModel>();
            builder.Services.AddTransient<ChangeLocationViewModel>();

            // Estado compartido del flujo de 3 pasos de seguridad
            builder.Services.AddSingleton<SecurityRegistrationContext>();

            builder.Services.AddSingleton<ILoaderService, LoaderService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<IAppStateService, AppStateService>();
            builder.Services.AddSingleton<NotificationOrchestrator>();

            // Control de Patio
            builder.Services.AddSingleton<IPatioNotificacionService, PatioNotificacionService>();
            builder.Services.AddSingleton<PatioContext>();

            builder.Services.AddTransient<PatioPendientesPage>();
            builder.Services.AddTransient<PatioDetallePage>();
            builder.Services.AddTransient<CortinaSeleccionPage>();
            builder.Services.AddTransient<TaskSecurityPage>();

            builder.Services.AddTransient<PatioPendientesViewModel>();
            builder.Services.AddTransient<PatioDetalleViewModel>();
            builder.Services.AddTransient<CortinaSeleccionViewModel>();
            builder.Services.AddTransient<TaskSecurityViewModel>();

            // Session service: maneja SecureStorage + refresh automático
            builder.Services.AddSingleton<MobileSessionService>();

            builder.Services.AddSingleton<AppShell>();

            var mauiApp = builder.Build();

            // Conectar el callback de refresh al ApiService singleton
            var sessionService  = mauiApp.Services.GetRequiredService<MobileSessionService>();
            var apiService      = mauiApp.Services.GetRequiredService<LD.Client.Services.ApiService>();
            var authService     = mauiApp.Services.GetRequiredService<LD.Client.Services.AuthService>();
            var signalRService  = mauiApp.Services.GetRequiredService<LD.Client.Services.SignalRService>();

            sessionService.Configure(apiService, authService);
            sessionService.ConfigureSignalR(signalRService);

            // Forzar construcción del orquestador para que su suscripción a SignalR
            // quede activa antes de cualquier login. Al ser singleton, el constructor
            // corre exactamente una vez: el += en SignalRService ocurre una sola vez.
            mauiApp.Services.GetRequiredService<NotificationOrchestrator>();

            return mauiApp;
        }
    }
}
