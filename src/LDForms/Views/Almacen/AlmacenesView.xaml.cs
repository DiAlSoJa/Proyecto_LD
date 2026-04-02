using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.Constants;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Almacen;
using LD.FormsX.Views.Dialogs;
using LD.Formx.Core;
using Microsoft.Extensions.DependencyInjection;

namespace LDForms.Views
{
    public partial class AlmacenesView : UserControl
    {
        private readonly WarehouseService _warehouseService;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<WarehouseDto> _gridFilter;
        private ICollectionView _almacenesView;

        private WarehouseDto? _selectedWarehouse;
        private bool _loaded;
        public AlmacenesView(WarehouseService warehouseService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _warehouseService = warehouseService;
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

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            AplicarPermisos();
            await CargarDatosConLoaderAsync("Trayendo almacenes...");
        }

        private void AplicarPermisos()
        {
            btnNuevo.Visibility      = UserData.HasPermission(PermissionKeys.Warehouse_Create) ? Visibility.Visible : Visibility.Collapsed;
            btnEditar.Visibility     = UserData.HasPermission(PermissionKeys.Warehouse_Update) ? Visibility.Visible : Visibility.Collapsed;
            BtnActualizar.Visibility = UserData.HasPermission(PermissionKeys.Warehouse_View)   ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task CargarDatosConLoaderAsync(string mensaje)
        {
            if (!UserData.HasPermission(PermissionKeys.Warehouse_View))
            {
                dgAlmacenes.Visibility = Visibility.Collapsed;
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

       
    }
}