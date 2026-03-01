using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;




namespace MauiAppLogin
{
    public static class MauiProgram
    {

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder
            .UseMauiApp<App>()   
            .UseMauiCommunityToolkit();

            builder.Services.AddTransient<RegisterVehicule>();
            builder.Services.AddTransient<RegisterLicense>();
            builder.Services.AddTransient<SignatureDriver>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<ChangeLocationPage>();
            


            return builder.Build();


        }
    }
}
