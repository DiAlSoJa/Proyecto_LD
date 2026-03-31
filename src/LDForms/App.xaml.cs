using System;
using System.ComponentModel;
using System.Windows;
using LD.Client;
using LD.Client.Services;
using LD.Forms.Configuration;
using LD.FormsX.Movimientos;
using LD.FormsX.Views;
using LD.FormsX.Views.Almacen;
using LD.FormsX.Views.Articulos;
using LD.FormsX.Views.ASN;
using LD.FormsX.Views.Auditar;
using LD.FormsX.Views.Catalogos;
using LD.FormsX.Views.Categorias;
using LD.FormsX.Views.CheckList;
using LD.FormsX.Views.Common;
using LD.FormsX.Views.ControlPatio;
using LD.FormsX.Views.Dialogs;
using LD.FormsX.Views.Dimensionador;
using LD.FormsX.Views.Familias;
using LD.FormsX.Views.Inventario;
using LD.FormsX.Views.InventarioAleatorio;
using LD.FormsX.Views.Monedas;
using LD.FormsX.Views.Proyectos;
using LD.FormsX.Views.Reportes;
using LD.FormsX.Views.Status;
using LD.FormsX.Views.Ubicaciones;
using LD.FormsX.Views.Unidades;
using LD.FormsX.Views.Usuarios;
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
            services.AddTransient<CatalogoDimensionadorView>();
            services.AddTransient<InventarioView>();
            services.AddTransient<MovimientosView>();
            services.AddTransient<ASNView>();
            services.AddTransient<AuditarView>();
            services.AddTransient<InventarioCiclicoView>();
            services.AddTransient<CheckListView>();
            services.AddTransient<ReportesView>();
            services.AddTransient<ControlPatioView>();
            services.AddTransient<UsuariosView>();
            services.AddTransient<ProyectosView>();



            // dialogs
            services.AddTransient<DialogWindow>();
            services.AddTransient<NuevoClienteView>();
            services.AddTransient<NuevoAlmacenView>();
            services.AddTransient<NuevaUbicacionView>();
            services.AddTransient<NuevaUbicacionMasivaView>();
            services.AddTransient<NuevoStatusView>();
            services.AddTransient<NuevaCategoriaView>();
            services.AddTransient<NuevaUnidadView>();
            services.AddTransient<NuevaMonedaView>();
            services.AddTransient<NuevaFamiliaView>();
            services.AddTransient<NuevoArticuloView>();
            services.AddTransient<NuevaAuditoriaView>();
            services.AddTransient<NuevoEquipoCheckListView>();

            services.AddTransient<NuevoUsuarioView>();
            services.AddTransient < NuevoProyectoView>();

            services.AddTransient<NuevoDimensionadorView>();

        }
    }
}