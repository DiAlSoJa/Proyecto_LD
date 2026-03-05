using LD.Forms;
using LD.Forms.Configuration;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Dialogs;
using LD.Forms.Views.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LD
{
    internal static class Program
    {
        /// <summary>5
        ///  The main entry point for the application.
        /// </summary>
        /// 
        public static IConfiguration? Configuration { get; private set; }
 
        [STAThread]
        static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                  .ConfigureAppConfiguration((context, config) =>
                  {
                      var env = Environment.GetEnvironmentVariable("DOTNET_LD_ENVIRONMENT") ?? "Production";

                      config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                      config.AddJsonFile("appsettings.json", optional: false);
                      config.AddJsonFile($"appsettings.{env}.json", optional: true);
                  })
                  .ConfigureServices((context, services) =>
                  {
                      services.Configure<ApiSettings>(
                          context.Configuration.GetSection("ApiSettings"));
                      RegisterServices(services);
                  })
                  .Build();

            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            ApplicationConfiguration.Initialize();


            var appContext = new AppApplicationContext(host.Services,host.Services.GetService<TabService>());
            Application.Run(appContext);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<AppApplicationContext>();
            services.AddSingleton<ApiEndpoints>();

            // 🔹 Servicios
            services.AddScoped<ApiService>();
            services.AddScoped<AuthService>();
            services.AddScoped<ClientService>();
            services.AddScoped<ItemService>();
            services.AddScoped<LocationService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<WarehouseService>();
            services.AddScoped<UserService>();
            services.AddScoped<LookupService>();


            // 🔹 Servicios de formularios
            services.AddSingleton<TabService>();
            services.AddSingleton<NavigationService>();
            services.AddSingleton<DialogMessageService>();
            services.AddSingleton<DialogFormService>();


            // 🔹 Forms
            services.AddTransient<FrmPrincipal>();
            services.AddTransient<FrmLogin>();
            services.AddTransient<FrmMenu>();

            services.AddTransient<FrmAlmacenes>();
            services.AddTransient<FrmArticulos>();
            services.AddTransient<FrmClientes>();
            services.AddTransient<FrmMovimientos>();
            services.AddTransient<FrmProyectos>();
            services.AddTransient<FrmUbicaciones>();
            services.AddTransient<FrmInventario>();
            services.AddTransient<FrmAuditar>();
            services.AddTransient<FrmAleatorio>();
            services.AddTransient<FrmUsuarios>();
            services.AddTransient<FrmASN>();



            // 🔹 Dialogs states
            services.AddTransient<FrmConfirm>();
            services.AddTransient<FrmError>();
            services.AddTransient<FrmInfo>();
            services.AddTransient<FrmSuccess>();
            services.AddTransient<FrmWarning>();


            // 🔹 Dialogs 
            services.AddTransient<FrmNuevaUbicacion>();
            services.AddTransient<FrmNuevaUbicacionMasiva>();
            services.AddTransient<FrmNuevoAlmacen>();
            services.AddTransient<FrmNuevoArticuloMasiva>();
            services.AddTransient<FrmNuevoArticulo>();
            services.AddTransient<FrmNuevoCliente>();
            services.AddTransient<FrmNuevoProyecto>();
            services.AddTransient<FrmNuevoASN>();
            services.AddTransient<FrmNuevoASNEscaneo>();
            services.AddTransient<FrmVehiculosRegistrados>();
            services.AddTransient<FrmNuevoUsuario>();



        }
    }
}