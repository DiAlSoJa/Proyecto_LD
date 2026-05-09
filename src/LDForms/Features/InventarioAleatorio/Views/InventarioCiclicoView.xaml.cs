using LD.Client.Services;
using LD.Contracts.InventarioCiclico;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.InventarioAleatorio
{
    /// <summary>
    /// Logica de interaccion para InventarioCiclicoView.xaml
    /// </summary>
    public partial class InventarioCiclicoView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CyclicInventoryService _cyclicInventoryService;
        private List<CyclicInventoryDto> _inventarios = [];
        private bool _loaded;

        public InventarioCiclicoView(
            IServiceProvider serviceProvider,
            CyclicInventoryService cyclicInventoryService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _cyclicInventoryService = cyclicInventoryService;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded)
            {
                return;
            }

            _loaded = true;
            await CargarInventariosConLoaderAsync();
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            await CargarInventariosConLoaderAsync();
        }

        private async void BtnNuevoInventario_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoInventarioCiclicoView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();
            if (result == true)
            {
                await CargarInventariosConLoaderAsync();
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e) { }
        private void dgFechas_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void dgAuditores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAuditores.SelectedItem is not CyclicInventoryDto inventario)
            {
                LimpiarDetalle();
                return;
            }

            CargarDetalle(inventario);
        }

        private async Task CargarInventariosConLoaderAsync()
        {
            try
            {
                MostrarLoaderInventarios(true, "Cargando inventarios...");
                LimpiarDetalle();

                var response = await _cyclicInventoryService.GetCyclicInventories(
                    dpDesde.SelectedDate,
                    dpHasta.SelectedDate,
                    ObtenerEstatusSeleccionado());

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowWarning(response.Message);
                    _inventarios = [];
                    dgAuditores.ItemsSource = _inventarios;
                    ActualizarStatusInventarios();
                    return;
                }

                _inventarios = response.Data ?? [];
                dgAuditores.ItemsSource = _inventarios;
                ActualizarStatusInventarios();

                if (_inventarios.Count > 0)
                {
                    dgAuditores.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                MostrarLoaderInventarios(false);
            }
        }

        private void CargarDetalle(CyclicInventoryDto inventario)
        {
            var detalles = inventario.Detalles
                .OrderBy(x => x.Ubicacion)
                .ToList();

            var resultados = detalles
                .Where(x => !string.IsNullOrWhiteSpace(x.PartNumber))
                .OrderBy(x => x.PartNumber)
                .Select(x => new ResultadoInventarioRow(x.PartNumber!, x.Escaneado))
                .ToList();

            dgUbicacionesDetalle.ItemsSource = detalles;
            dgResultados.ItemsSource = resultados;

            txtStatusUbicacionesBottom.Text = $"Ubicaciones: {detalles.Count}";
            txtStatusResultados.Text = $"Resultados: {resultados.Count}";
        }

        private void LimpiarDetalle()
        {
            dgUbicacionesDetalle.ItemsSource = null;
            dgResultados.ItemsSource = null;
            txtStatusUbicacionesBottom.Text = "Ubicaciones";
            txtStatusResultados.Text = "Resultados";
        }

        private void ActualizarStatusInventarios()
        {
            txtStatusUbicacionesTop.Text = $"Inventarios: {_inventarios.Count}";
        }

        private string? ObtenerEstatusSeleccionado()
        {
            return (cmbEstatus.SelectedItem as ComboBoxItem)?.Content?.ToString();
        }

        private void MostrarLoaderInventarios(bool mostrar, string mensaje = "Cargando inventarios...")
        {
            TxtLoadingUbicacionesTop.Text = mensaje;
            LoadingOverlayUbicacionesTop.Visibility = mostrar ? Visibility.Visible : Visibility.Collapsed;
        }

        private sealed record ResultadoInventarioRow(string NoParte, bool Escaneado);
    }
}
