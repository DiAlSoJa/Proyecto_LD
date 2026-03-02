using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;


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
            


            return builder.Build();


        }
    }
}
