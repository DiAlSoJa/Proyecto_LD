using LD.Client;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Forms.Configuration;
using LD.FormsX.Features.Almacen.ViewModels;
using LD.FormsX.Features.Catalogos.Categorias.ViewModels;
using LD.FormsX.Features.Catalogos.Dimensionador.ViewModels;
using LD.FormsX.Features.Catalogos.Equipos.ViewModels;
using LD.FormsX.Features.Catalogos.Familias.ViewModels;
using LD.FormsX.Features.Catalogos.Monedas.ViewModels;
using LD.FormsX.Features.Catalogos.TiposCamion.ViewModels;
using LD.FormsX.Features.Catalogos.Unidades.ViewModels;
using LD.FormsX.Features.Clientes.ViewModels;
using LD.FormsX.Features.Proyectos.ViewModels;
using LD.FormsX.Features.ReporteDanos.ViewModels;
using LD.FormsX.Features.Tareas.ViewModels;
using LD.FormsX.Features.Ubicaciones.ViewModels;
using LD.FormsX.Features.Usuarios.ViewModels;
using LD.FormsX.Movimientos;
using LD.FormsX.Views;
using LD.FormsX.Views.Almacen;
using LD.FormsX.Views.Articulos;
using LD.FormsX.Views.ASN;
using LD.FormsX.Views.Auditar;
using LD.FormsX.Views.Catalogos;
using LD.FormsX.Views.Categorias;
using LD.FormsX.Features.CheckList.ViewModels;
using LD.FormsX.Features.Common;
using LD.FormsX.Features.Reportes.ViewModels;
using LD.FormsX.Views.CheckList;
using LD.FormsX.Views.CheckList.Tabs;
using LD.FormsX.Views.Common;
using LD.FormsX.Views.ControlPatio;
using LD.FormsX.Views.DatabaseDiagram;
using LD.FormsX.Views.Dialogs;
using LD.FormsX.Views.Dimensionador;
using LD.FormsX.Views.RegistroVehicular;
using LD.FormsX.Features.Surtidos.Views;
using LD.FormsX.Features.Surtidos.ViewModels;
using LD.FormsX.Features.Embarques.Views;
using LD.FormsX.Views.Equipos;
using LD.FormsX.Views.Familias;
using LD.FormsX.Views.Inventario;
using LD.FormsX.Views.InventarioAleatorio;
using LD.FormsX.Views.Login.ViewModels;
using LD.FormsX.Views.Monedas;
using LD.FormsX.Views.Proyectos;
using LD.FormsX.Views.ReporteDanos;
using LD.FormsX.Views.Reportes;
using LD.FormsX.Views.Status;
using LD.FormsX.Views.TiposCamion;
using LD.FormsX.Views.Tareas;
using LD.FormsX.Views.Ubicaciones;
using LD.FormsX.Views.Unidades;
using LD.FormsX.Views.Usuarios;
using LDForms.Features.DashBoard.ViewModels;
using LD.FormsX.Features.ControlPatio.ViewModels;
using LDForms.Features.DockDelivery.Views;
using LDForms;
using LDForms.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.ComponentModel;
using System.Windows;

namespace LD.FormsX
{
    public partial class App : Application
    {
        public static IHost? HostContainer { get; private set; }
        public static IServiceProvider Services => HostContainer!.Services;
        public static IConfiguration? Configuration { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Information()
                   .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                   .CreateLogger();

                Log.Information("App iniciada");
                HostContainer = Host.CreateDefaultBuilder()
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        var env =
                            Environment.GetEnvironmentVariable("DOTNET_LD_ENVIRONMENT")
                            ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
#if DEBUG
                            ?? "Development";
#else
                            ?? "Production";
#endif


                        env = "Development"; 
                        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", env);
                        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", env);

                        config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                        config.AddJsonFile("appsettings.json", optional: false);
                        config.AddJsonFile($"appsettings.{env}.json", optional: true);
                    })
                    .UseSerilog()
                    .ConfigureServices((context, services) =>
                    {
                        Configuration = context.Configuration;
                        RegisterServices(services, context.Configuration);
                    })
                    .Build();

                await HostContainer.StartAsync();

                ConfigureAuthRefresh();

                Log.Information("Api BaseUrl configurada: {BaseUrl}", Configuration?["ApiSettings:BaseUrl"]);

                var login = Services.GetRequiredService<MainWindow>();
                login.Show();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error fatal al iniciar");
            }
        }

        private static void ConfigureAuthRefresh()
        {
            var apiService = Services.GetRequiredService<ApiService>();
            var authService = Services.GetRequiredService<AuthService>();

            apiService.OnUnauthorizedAsync = async () =>
            {
                var refreshToken = UserSession.RefreshToken;
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    UserSession.LogOut();
                    apiService.ClearToken();
                    return false;
                }

                var refreshResponse = await authService.RefreshTokenAsync(refreshToken);
                if (!refreshResponse.IsSuccess || refreshResponse.Data is null)
                {
                    UserSession.LogOut();
                    apiService.ClearToken();
                    return false;
                }

                UserSession.AccessToken = refreshResponse.Data.Accesstoken;
                UserSession.RefreshToken = refreshResponse.Data.RefreshToken;
                apiService.SetBearerToken(refreshResponse.Data.Accesstoken!);
                return true;
            };
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            if (HostContainer is not null)
            {
                await HostContainer.StopAsync();
                HostContainer.Dispose();
            }

            Log.Information("App cerrada");
            Log.CloseAndFlush();
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

            services.AddTransient<LoginViewModel>();
            services.AddTransient<AlmacenesViewModel>();
            services.AddTransient<NuevoAlmacenViewModel>();
            services.AddTransient<CortinasViewModel>();
            services.AddTransient<NuevaCortinaViewModel>();
            services.AddTransient<CatalogoCategoriasViewModel>();
            services.AddTransient<NuevaCategoriaViewModel>();
            services.AddTransient<CatalogoDimensionadorViewModel>();
            services.AddTransient<NuevoDimensionadorViewModel>();
            services.AddTransient<CatalogoFamiliasViewModel>();
            services.AddTransient<NuevaFamiliaViewModel>();
            services.AddTransient<CatalogoMonedasViewModel>();
            services.AddTransient<NuevaMonedaViewModel>();
            services.AddTransient<CatalogoTiposCamionViewModel>();
            services.AddTransient<NuevoTipoCamionViewModel>();
            services.AddTransient<CatalogoUnidadesViewModel>();
            services.AddTransient<NuevaUnidadViewModel>();
            services.AddTransient<CatalogoEquiposViewModel>();
            services.AddTransient<NuevoEquipoViewModel>();
            services.AddTransient<ProyectosViewModel>();
            services.AddTransient<NuevoProyectoViewModel>();
            services.AddTransient<ClientesViewModel>();
            services.AddTransient<NuevoClienteViewModel>();
            services.AddTransient<UbicacionesViewModel>();
            services.AddTransient<NuevaUbicacionViewModel>();
            services.AddTransient<UsuariosViewModel>();
            services.AddTransient<NuevoUsuarioViewModel>();
            services.AddTransient<UsuarioAlmacenViewModel>();
            services.AddTransient<DamageReportViewModel>();
            services.AddTransient<TasksViewModel>();
            services.AddTransient<DashBoardViewModel>();
            services.AddTransient<ControlPatioViewModel>();
            services.AddTransient<MainWindow>();
            services.AddTransient<DashBoard>();
            services.AddTransient<CatalogosClientesView>();
            services.AddTransient<AlmacenesView>();
            services.AddTransient<CortinasView>();
            services.AddTransient<UbicacionesView>();
            services.AddTransient<ArticulosView>();
            services.AddTransient<CatalogoStatusView>();
            services.AddTransient<CatalogoCategoriasView>();
            services.AddTransient<CatalogoUnidadesView>();
            services.AddTransient<CatalogoEquiposView>();
            services.AddTransient<CatalogoMonedasView>();
            services.AddTransient<CatalogoTiposCamionView>();
            services.AddTransient<CatalogoFamiliasView>();
            services.AddTransient<CatalogosView>();
            services.AddTransient<CatalogoDimensionadorView>();
            services.AddTransient<InventarioView>();
            services.AddTransient<MovimientosView>();
            services.AddTransient<ASNView>();
            services.AddTransient<CapturaFoliosViewModel>();
            services.AddTransient<CapturaFoliosView>();
            services.AddTransient<SurtidosView>();
            services.AddTransient<EmbarquesView>();
            services.AddTransient<AuditarView>();
            services.AddTransient<InventarioCiclicoView>();
            services.AddTransient<ChecklistViewModel>();
            services.AddTransient<EquiposTabViewModel>();
            services.AddTransient<ResumenTabViewModel>();
            services.AddTransient<ResumenBateriasTabViewModel>();
            services.AddTransient<ConfiguracionPreguntasTabViewModel>();
            services.AddTransient<ReportesViewModel>();
            services.AddTransient<ReportQueryEditorViewModel>();
            services.AddTransient<ReportQueryParametersViewModel>();
            services.AddTransient<NuevoInventarioCiclicoView>();
            services.AddTransient<CheckListView>();
            services.AddTransient<EquiposTab>();
            services.AddTransient<ResumenTab>();
            services.AddTransient<ResumenBateriasTab>();
            services.AddTransient<ConfiguracionPreguntasTab>();
            services.AddTransient<ReportesView>();
            services.AddTransient<ReportQueryEditorView>();
            services.AddTransient<ReportQueryParametersView>();
            services.AddTransient<ReportQueryResultView>();
            services.AddTransient<DatabaseDiagramView>();
            services.AddTransient<DamageReportView>();
            services.AddTransient<TasksView>();
            services.AddTransient<DockDeliveryView>();
            services.AddTransient<ControlPatioView>();
            services.AddTransient<RegistroVehicularView>();
            services.AddTransient<UsuariosView>();
            services.AddTransient<ProyectosView>();



            // dialogs
            services.AddTransient<DialogWindow>();
            services.AddTransient<PermissionLoginDialog>();
            services.AddTransient<NuevoClienteView>();
            services.AddTransient<NuevoAlmacenView>();
            services.AddTransient<NuevaCortinaView>();
            services.AddTransient<NuevaUbicacionView>();
            services.AddTransient<NuevaUbicacionMasivaView>();
            services.AddTransient<NuevoStatusView>();
            services.AddTransient<NuevaCategoriaView>();
            services.AddTransient<NuevaUnidadView>();
            services.AddTransient<NuevoEquipoView>();
            services.AddTransient<NuevaMonedaView>();
            services.AddTransient<NuevoTipoCamionView>();
            services.AddTransient<NuevaFamiliaView>();
            services.AddTransient<NuevoArticuloView>();
            services.AddTransient<CargaMasivaArticulosView>();
            services.AddTransient<NuevaAuditoriaView>();
            services.AddTransient<NuevoEquipoCheckListView>();
            services.AddTransient<AsignarUsuarioEquipoView>();
            services.AddTransient<NuevoProveedorCheckListView>();
            services.AddTransient<NuevoASNView>();
            services.AddTransient<CuadreASNView>();
            services.AddTransient<CapturaFolioDetalleView>();
            services.AddTransient<CapturaFolioPreviewDialog>();
            services.AddTransient<NuevoSurtidoView>();
            services.AddTransient<EditarSurtidoView>();
            services.AddTransient<ValidarEmbarqueDialog>();
            services.AddTransient<NuevoASNEscaneoView>();
            services.AddTransient<BuscarVehiculoView>();
            services.AddTransient<VehiculosDockDeliveryView>();
            services.AddTransient<NuevoVehiculoDockDeliveryView>();
            services.AddTransient<ChoferesDockDeliveryView>();
            services.AddTransient<NuevoChoferDockDeliveryView>();

            services.AddTransient<NuevoUsuarioView>();
            services.AddTransient<UsuarioAlmacenView>();
            services.AddTransient<RolesView>();
            services.AddTransient<NuevoRolView>();
            services.AddTransient<NuevoProyectoView>();

            services.AddTransient<NuevoDimensionadorView>();

        }
    }
}
