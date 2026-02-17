using LD.Dialogs;
using LD.Forms;
using LD.Forms.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LD
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            //Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            ApplicationConfiguration.Initialize();
            var loginForm = host.Services.GetRequiredService<FrmLogin>();
            Application.Run(loginForm);
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // 🔹 Servicios
                    services.AddScoped<ApiService>();
                    services.AddScoped<AuthService>();
                    services.AddScoped<ClientService>();
                    services.AddScoped<ItemService>();
                    services.AddScoped<LocationService>();
                    services.AddScoped<ProjectService>();
                    services.AddScoped<WarehouseService>();

                    // 🔹 Forms
                    services.AddScoped<FrmLogin>();
                    services.AddScoped<FrmAlmacenes>();
                    services.AddScoped<FrmArticulos>();
                    services.AddScoped<FrmClientes>();
                    services.AddScoped<FrmMenu>();
                    services.AddScoped<FrmMovimientos>();
                    services.AddScoped<FrmPrincipal>();
                    services.AddScoped<FrmProyectos>();
                    services.AddScoped<FrmUbicaciones>();

                    // 🔹 Dialogs
                    services.AddScoped<FrmConfirm>();
                    services.AddScoped<FrmError>();
                    services.AddScoped<FrmInfo>();
                    services.AddScoped<FrmNuevaUbicacion>();
                    services.AddScoped<FrmNuevaUbicacionMasiva>();
                    services.AddScoped<FrmNuevoAlmacen>();
                    services.AddScoped<FrmNuevoArticuloMasiva>();
                    services.AddScoped<FrmNuevoArticulo>();
                    services.AddScoped<FrmNuevoArticulo>();
                    services.AddScoped<FrmNuevoCliente>();
                    services.AddScoped<FrmNuevoProyecto>();
                    services.AddScoped<FrmSuccess>();
                    services.AddScoped<FrmWarning>();
                });
        }
    }
}