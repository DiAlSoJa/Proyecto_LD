using System;
using System.Windows;
using LD.Client;
using LD.Client.Services;
using LD.Forms.Configuration;
using LD.FormsX.Views;
using LD.FormsX.Views.Catalogos;
using LDForms;
using LDForms.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LD.FormsX
{
    public partial class App : Application
    {
        public static IHost? HostContainer { get; private set; }
        public static IServiceProvider Services => HostContainer!.Services;
        public static IConfiguration? Configuration { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            HostContainer = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    var env = Environment.GetEnvironmentVariable("DOTNET_LD_ENVIRONMENT") ?? "Production";
                    env = "Development";

                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false);
                    config.AddJsonFile($"appsettings.{env}.json", optional: true);
                })
                .ConfigureServices((context, services) =>
                {
                    Configuration = context.Configuration;
                    RegisterServices(services, context.Configuration);
                })
                .Build();

            await HostContainer.StartAsync();

            var login = Services.GetRequiredService<MainWindow>();
            login.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (HostContainer is not null)
            {
                await HostContainer.StopAsync();
                HostContainer.Dispose();
            }

            base.OnExit(e);
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services.AddLDClient(options =>
            {
                options.BaseUrl = configuration["ApiSettings:BaseUrl"] ?? "";
            });
            // services.AddLDClient(configuration);

            services.AddTransient<AuthService>();

            //services.AddSingleton<DialogMessageService>();
            //services.AddSingleton<TabService>();

            services.AddTransient<MainWindow>();
            services.AddTransient<DashBoard>();
            services.AddTransient<CatalogosClientesView>();
            services.AddTransient<AlmacenesView>();
            services.AddTransient<UbicacionesView>();
            services.AddTransient<ArticulosView>();
            services.AddTransient<CatalogoStatusView>();
            services.AddTransient<CatalogoCategoriasView>(); 
            services.AddTransient<CatalogoUnidadesView>();
            services.AddTransient<CatalogoMonedasView>();
            services.AddTransient<CatalogoFamiliasView>();
            services.AddTransient<CatalogosView>();
        }
    }
}