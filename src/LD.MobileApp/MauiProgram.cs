using CommunityToolkit.Maui;
using LD.Client;
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


            builder.Services.AddLDClient(options =>
            {

                options.BaseUrl = "http://192.168.0.115:8050/api";

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
            builder.Services.AddTransient<TaskSecurity>();
            builder.Services.AddTransient<NewTask>();
            builder.Services.AddTransient<TaskResolve>();
            builder.Services.AddTransient<WarehouseOperations>();
            builder.Services.AddTransient<InventoryList>();
            builder.Services.AddTransient<ForkliftChecklistPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<TaskList>();

            //viewmodels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();


            builder.Services.AddSingleton<AppShell>();


            return builder.Build();


        }
    }
}
