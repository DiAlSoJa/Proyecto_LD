using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LD.Contracts.Warehouse;
using LD.FormsX.Features.Almacen.ViewModels;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Almacen;
using Microsoft.Extensions.DependencyInjection;

namespace LDForms.Views
{
    public partial class AlmacenesView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<WarehouseDto> _gridFilter;
        private bool _loaded;

        private AlmacenesViewModel ViewModel => (AlmacenesViewModel)DataContext;

        public AlmacenesView(AlmacenesViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;

            _gridFilter = new WpfGridFilter<WarehouseDto>(dgAlmacenes, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                { "Activo", 80 },
                { "Id", 120 },
                { "NombreComercial", 220 },
                { "DomicilioComercial", 250 },
                { "Telefono", 140 },
                { "Ciudad", 140 },
                { "CodigoPostal", 120 },
                { "RazonSocial", 250 },
                { "Rfc", 150 },
                { "IsProvider", 120 }
            });

            viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await ViewModel.CargarDatosAsync();
        }

        private void dgAlmacenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedWarehouse = _gridFilter.SelectedItem;
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoAlmacenView>();
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedWarehouse is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevoAlmacenView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetWarehouse(ViewModel.SelectedWarehouse);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }
    }
}