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
using LD.Client.Services;
using LD.Contracts.Location;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Dialogs;
using LD.FormsX.Views.Ubicaciones;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    /// <summary>
    /// Lógica de interacción para UbicacionesView.xaml
    /// </summary>
    public partial class UbicacionesView : UserControl
    {
        private readonly LocationService _service;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<LocationDto> _gridFilter;        

        private LocationDto? _selectedX;
        private bool _loaded;
        public UbicacionesView(LocationService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<LocationDto>(dg, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
               /* { "Activo", 80 },
                { "Id", 120 },
                { "NombreComercial", 220 },
                { "DomicilioComercial", 250 },
                { "Telefono", 140 },
                { "Ciudad", 140 },
                { "CodigoPostal", 120 },
                { "RazonSocial", 250 },
                { "Rfc", 150 },
                { "IsProvider", 120 }*/
            });

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await CargarDatosConLoaderAsync("Trayendo ubicaciones...");
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
            var result = await _service.GetLocations();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _gridFilter.SetData(result.Data);
            _selectedX = null;
            txtStatus.Text = $"Registros: {result.Data.Count}";
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            await CargarDatosConLoaderAsync("Trayendo ubicaciones...");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            _gridFilter.ClearFilter();
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedX = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaUbicacionView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosAsync();
            }
        }
        private async void BtnNuevoMasivo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaUbicacionMasivaView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosAsync();
            }
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {

            if (_selectedX is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevaUbicacionView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetLocation(_selectedX);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosAsync();
            }
        }

        private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedX == null)
                {
                    DialogHelper.ShowInfo("Selecciona un almacén.");
                    return;
                }

                bool confirmar = DialogHelper.ShowConfirm("¿Estás seguro de eliminar el almacén seleccionado?");

                if (!confirmar)
                    return;

                // Ajusta este bloque al método real de tu servicio:
                // var result = await _warehouseService.DeleteWarehouse(_selectedWarehouse.Id);

                // if (!result.IsSuccess)
                // {
                //     DialogHelper.ShowWarning(result.Message);
                //     return;
                // }

                DialogHelper.ShowSuccess("El almacén se eliminó correctamente.");
                await CargarDatosConLoaderAsync("Trayendo almacenes...");
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }
    }
}