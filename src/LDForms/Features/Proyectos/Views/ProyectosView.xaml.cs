using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LD.Contracts.Project;
using LD.FormsX.Features.Proyectos.ViewModels;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.Proyectos
{
    public partial class ProyectosView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<ProjectDto> _gridFilter;
        private bool _loaded;

        private ProyectosViewModel ViewModel => (ProyectosViewModel)DataContext;

        public ProyectosView(ProyectosViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;

            _gridFilter = new WpfGridFilter<ProjectDto>(dgProyectos, txtBuscar);
            _gridFilter.SetHiddenColumns(new string[] { "ProjectId" });
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                { "Activo",    70 },                
                { "Proyecto",  260 },
                { "Cliente",   200 },
                { "Almacen",   180 },
            });

            viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await ViewModel.CargarDatosAsync();
        }

        private void DgProyectos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedProject = _gridFilter.SelectedItem;
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoProyectoView>();
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedProject is null)
            {
                DialogHelper.ShowWarning("Selecciona un proyecto para editar.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<NuevoProyectoView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetProject(ViewModel.SelectedProject);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }
    }
}


