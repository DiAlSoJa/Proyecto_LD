using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LD.Contracts.Location;
using LD.FormsX.Features.Ubicaciones.ViewModels;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Ubicaciones;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    public partial class UbicacionesView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<LocationDto> _gridFilter;
        private bool _loaded;

        private UbicacionesViewModel ViewModel => (UbicacionesViewModel)DataContext;

        public UbicacionesView(UbicacionesViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;

            _gridFilter = new WpfGridFilter<LocationDto>(dg, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
            });

            viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await ViewModel.CargarDatosAsync();
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedLocation = _gridFilter.SelectedItem;
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaUbicacionView>();
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }

        private async void BtnNuevoMasivo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaUbicacionMasivaView>();
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedLocation is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevaUbicacionView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetLocation(ViewModel.SelectedLocation);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }
    }
}