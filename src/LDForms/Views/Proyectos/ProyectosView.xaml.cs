using LD.Client.Services;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Almacen;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LD.FormsX.Views.Proyectos
{
    /// <summary>
    /// Lógica de interacción para ProyectosView.xaml
    /// </summary>
    public partial class ProyectosView : UserControl
    {
        private readonly WarehouseService _warehouseService;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<WarehouseDto> _gridFilter;
        private ICollectionView _almacenesView;

        private WarehouseDto? _selectedWarehouse;
        private bool _loaded;
        public ProyectosView()
        {
            InitializeComponent();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await CargarDatosConLoaderAsync("Trayendo almacenes...");
        }

        private async Task CargarDatosConLoaderAsync(string mensaje)
        {
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
            var result = await _warehouseService.GetWarehouses();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _gridFilter.SetData(result.Data);
            _selectedWarehouse = null;
            txtStatus.Text = $"Registros: {result.Data.Count}";
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            await CargarDatosConLoaderAsync("Trayendo almacenes...");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            _gridFilter.ClearFilter();
        }

        private void dgAlmacenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedWarehouse = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoAlmacenView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosConLoaderAsync("Trayendo almacenes...");
            }
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedWarehouse is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevoAlmacenView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetWarehouse(_selectedWarehouse);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosConLoaderAsync("Trayendo almacenes...");
            }
        }
        private void DgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedWarehouse = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

    }
}
