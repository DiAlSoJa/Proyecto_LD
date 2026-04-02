using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Project;
using LD.FormsX.Helpers;
using LD.Formx.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.Proyectos
{
    public partial class ProyectosView : UserControl
    {
        private readonly ProjectService _projectService;
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<ProjectDto> _gridFilter;

        private ProjectDto? _selectedProject;
        private bool _loaded;

        public ProyectosView(ProjectService projectService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _projectService = projectService;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<ProjectDto>(dgProyectos, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                { "Activo",    70 },
                { "ProjectId", 80 },
                { "Proyecto",  260 },
                { "Cliente",   200 },
                { "Almacen",   180 },
            });
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;
            AplicarPermisos();
            await CargarDatosConLoaderAsync("Trayendo proyectos...");
        }

        private void AplicarPermisos()
        {
            btnNuevo.Visibility      = UserData.HasPermission(PermissionKeys.Project_Create) ? Visibility.Visible : Visibility.Collapsed;
            btnEditar.Visibility     = UserData.HasPermission(PermissionKeys.Project_Update) ? Visibility.Visible : Visibility.Collapsed;
            BtnActualizar.Visibility = UserData.HasPermission(PermissionKeys.Project_View)   ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task CargarDatosConLoaderAsync(string mensaje)
        {
            if (!UserData.HasPermission(PermissionKeys.Project_View))
            {
                dgProyectos.Visibility = Visibility.Collapsed;
                return;
            }

            try
            {
                MostrarLoader(true, mensaje);
                await CargarDatosAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                MostrarLoader(false);
            }
        }

        private void MostrarLoader(bool mostrar, string mensaje = "Cargando...")
        {
            TxtLoading.Text = mensaje;
            LoadingOverlay.Visibility = mostrar ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task CargarDatosAsync()
        {
            var result = await _projectService.GetProjects();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            var data = result.Data ?? new List<ProjectDto?>();
            _gridFilter.SetData(data.Where(x => x != null).Select(x => x!).ToList());
            _selectedProject = null;
            txtStatus.Text = $"Registros: {data.Count}";
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            await CargarDatosConLoaderAsync("Trayendo proyectos...");
        }

        private void DgProyectos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedProject = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoProyectoView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();
            if (result == true)
                await CargarDatosConLoaderAsync("Trayendo proyectos...");
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProject is null)
            {
                DialogHelper.ShowWarning("Selecciona un proyecto para editar.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<NuevoProyectoView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetProject(_selectedProject);

            var result = dialog.ShowDialog();
            if (result == true)
                await CargarDatosConLoaderAsync("Trayendo proyectos...");
        }
    }
}


