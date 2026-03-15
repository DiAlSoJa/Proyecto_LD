using CommunityToolkit.Maui;
using LD.Client;
using MauiAppLogin.ViewModels;
using Microsoft.Extensions.Logging;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddLDClient(options =>
            {
                options.BaseUrl = "http://192.168.1.74:8050/api";
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

            //viewmodels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();


            return builder.Build();


        }
    }
}
