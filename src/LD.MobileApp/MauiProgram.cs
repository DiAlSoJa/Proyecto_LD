using CommunityToolkit.Maui;
using LD.Client;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MauiAppLogin.ViewModels;
using Microsoft.Extensions.Logging;
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
                .UseMauiApp<App>()                       // una sola vez
                .UseMauiCommunityToolkit()               // Community Toolkit
                .UseBarcodeReader()                      // ZXing barcode reader
                .UseOcr()                                // Plugin.Maui.OCR
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            const string defaultApiUrl = "http://192.168.0.103:8050/api";
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

            //viewmodels
            builder.Services.AddTransient<ForkliftChecklistViewModel>();
            builder.Services.AddTransient<NoEquipmentViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<RegisterLicenseViewModel>();
            builder.Services.AddTransient<RegisterVehiculeViewModel>();
            builder.Services.AddTransient<SignatureDriverViewModel>();
            builder.Services.AddTransient<ChangeLocationViewModel>();

            // shared state for the 3-step security registration flow
            builder.Services.AddSingleton<SecurityRegistrationContext>();

            builder.Services.AddSingleton<ILoaderService, LoaderService>();

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

            builder.Services.AddSingleton<AppShell>();


            return builder.Build();


        }
    }
}
