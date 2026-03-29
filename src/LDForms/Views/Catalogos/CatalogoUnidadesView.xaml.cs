using System;
using System.Collections.Generic;
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
using LD.Contracts.InventaryStatus;
using LD.Contracts.Location;
using LD.Contracts.Units;
using LD.FormsX.Helpers;

namespace LD.FormsX.Views
{
    /// <summary>
    /// Lógica de interacción para CatalogoStatusView.xaml
    /// </summary>
    public partial class CatalogoUnidadesView : UserControl
    {
        private readonly UnitService _service;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<UnitDto> _gridFilter;

        private UnitDto? _selectedX;
        private bool _loaded;
        public CatalogoUnidadesView(UnitService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<UnitDto>(dg, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                
                 { "Unidad", 150 },
                 { "Descripcion", 250 }
                
            });

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await CargarDatosConLoaderAsync("Trayendo unidades...");
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
            var result = await _service.GetUnits();

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
            await CargarDatosConLoaderAsync("Trayendo unidades...");
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
            try
            {
                /*
                var win = _serviceProvider.GetRequiredService<NuevoAlmacenWindow>();
                win.Owner = Window.GetWindow(this);

                var result = win.ShowDialog();

                if (result == true)
                    await CargarDatosConLoaderAsync("Trayendo almacenes...");
                */
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                if (_selectedWarehouse == null)
                {
                    DialogHelper.ShowInfo("Selecciona un almacén.");
                    return;
                }

                var win = _serviceProvider.GetRequiredService<NuevoAlmacenWindow>();
                win.Owner = Window.GetWindow(this);
                win.SetWarehouse(_selectedWarehouse);

                var result = win.ShowDialog();

                if (result == true)
                    await CargarDatosConLoaderAsync("Trayendo almacenes...");
                */
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
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