using LD.Client.Services;
using LD.Contracts.AvailableInventory;
using LD.Contracts.InventarioCiclico;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LD.FormsX.Views.InventarioAleatorio
{
    /// <summary>
    /// Logica de interaccion para InventarioCiclicoView.xaml
    /// </summary>
    public partial class InventarioCiclicoView : UserControl
    {
        private const string EstadoCoincide = "Coincide";
        private const string EstadoFaltaEscaneo = "Falta escaneo";
        private const string EstadoEscaneadoExtra = "Escaneado extra";
        private const string EstadoOtraUbicacion = "Otra ubicación";
        private const string EstadoNoDisponible = "No disponible";

        private static readonly Brush BrushCoincideBackground = CreateBrush("#DCFCE7");
        private static readonly Brush BrushCoincideForeground = CreateBrush("#166534");
        private static readonly Brush BrushFaltaBackground = CreateBrush("#FEE2E2");
        private static readonly Brush BrushFaltaForeground = CreateBrush("#991B1B");
        private static readonly Brush BrushExtraBackground = CreateBrush("#FEF3C7");
        private static readonly Brush BrushExtraForeground = CreateBrush("#92400E");
        private static readonly Brush BrushOtraUbicacionBackground = CreateBrush("#EDE9FE");
        private static readonly Brush BrushOtraUbicacionForeground = CreateBrush("#6B21A8");

        private readonly IServiceProvider _serviceProvider;
        private readonly CyclicInventoryService _cyclicInventoryService;
        private readonly AvailableInventoryService _availableInventoryService;

        private List<CyclicInventoryDto> _inventarios = [];
        private CyclicInventoryDto? _inventarioSeleccionado;
        private CyclicInventoryDetailDto? _detalleSeleccionado;
        private TakeLoadResult[] _takeResults = [TakeLoadResult.Empty, TakeLoadResult.Empty, TakeLoadResult.Empty, TakeLoadResult.Empty];
        private List<AvailableInventoryDto> _availableInventoriesCache = [];
        private bool _availableInventoriesLoaded;
        private bool _loaded;
        private bool _loadingLocationDetail;
        private bool _suppressDetailSelectionLoad;
        private int _detailLoadVersion;

        public InventarioCiclicoView(
            IServiceProvider serviceProvider,
            CyclicInventoryService cyclicInventoryService,
            AvailableInventoryService availableInventoryService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _cyclicInventoryService = cyclicInventoryService;
            _availableInventoryService = availableInventoryService;
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
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(
                dialog,
                LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

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
                _inventarioSeleccionado = null;
                LimpiarDetalle();
                return;
            }

            CargarDetalle(inventario);
        }

        private async void dgUbicacionesDetalle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_suppressDetailSelectionLoad || _inventarioSeleccionado is null)
            {
                return;
            }

            if (dgUbicacionesDetalle.SelectedItem is not CyclicInventoryDetailDto detalle)
            {
                LimpiarDetalleSeleccionado();
                return;
            }

            await CargarDetalleSeleccionadoAsync(detalle);
        }

        private void TabTomas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_loadingLocationDetail || _detalleSeleccionado is null || _inventarioSeleccionado is null)
            {
                return;
            }

            ActualizarResumenDetalleSeleccionado();
        }

        private async Task CargarInventariosConLoaderAsync()
        {
            try
            {
                MostrarLoaderInventarios(true, "Cargando inventarios...");
                LimpiarDetalle();
                _inventarioSeleccionado = null;
                _detalleSeleccionado = null;
                InvalidarCacheInventariosDisponibles();

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
            _inventarioSeleccionado = inventario;
            _detalleSeleccionado = null;
            InvalidarCacheInventariosDisponibles();

            var detalles = inventario.Detalles
                .OrderBy(x => NormalizeTakeNumber(x.TakeNumber))
                .ThenBy(x => x.Ubicacion)
                .ThenBy(x => x.PartNumber ?? string.Empty)
                .ToList();

            _suppressDetailSelectionLoad = true;
            dgUbicacionesDetalle.ItemsSource = detalles;
            dgUbicacionesDetalle.SelectedIndex = -1;
            _suppressDetailSelectionLoad = false;

            txtStatusUbicacionesBottom.Text = $"Ubicaciones: {detalles.Count}";
            LimpiarDetalleSeleccionado();

            if (detalles.Count > 0)
            {
                dgUbicacionesDetalle.SelectedIndex = 0;
            }
        }

        private void LimpiarDetalle()
        {
            _suppressDetailSelectionLoad = true;
            dgUbicacionesDetalle.ItemsSource = null;
            _suppressDetailSelectionLoad = false;

            LimpiarDetalleSeleccionado();
            txtStatusUbicacionesBottom.Text = "Ubicaciones";
            txtStatusResultados.Text = "Detalle de ubicación";
            txtDetalleUbicacionSeleccionada.Text = "Selecciona una ubicación a la izquierda para ver el detalle de standardId.";
        }

        private void LimpiarDetalleSeleccionado()
        {
            _detalleSeleccionado = null;
            _takeResults = [TakeLoadResult.Empty, TakeLoadResult.Empty, TakeLoadResult.Empty, TakeLoadResult.Empty];

            dgPrimeraToma.ItemsSource = null;
            dgSegundaToma.ItemsSource = null;
            dgTerceraToma.ItemsSource = null;
            dgCuartaToma.ItemsSource = null;

            txtDetalleUbicacionSeleccionada.Text = "Selecciona una ubicación a la izquierda para ver el detalle de standardId.";

            if (tabTomas is not null)
            {
                tabTomas.SelectedIndex = 0;
            }
        }

        private async Task CargarDetalleSeleccionadoAsync(CyclicInventoryDetailDto detalle)
        {
            if (_inventarioSeleccionado is null || _loadingLocationDetail)
            {
                return;
            }

            var version = ++_detailLoadVersion;
            _detalleSeleccionado = detalle;

            try
            {
                _loadingLocationDetail = true;
                MostrarLoaderDetalle(true, $"Cargando detalle de {detalle.Ubicacion}...");
                txtStatusResultados.Text = $"Detalle de ubicación: {detalle.Ubicacion}";

                var detallesMismaUbicacion = _inventarioSeleccionado.Detalles
                    .Where(x => x.LocationId == detalle.LocationId)
                    .OrderBy(x => NormalizeTakeNumber(x.TakeNumber))
                    .ThenBy(x => x.PartNumber ?? string.Empty)
                    .ToList();

                var availableInventories = await EnsureAvailableInventoriesAsync();
                if (version != _detailLoadVersion)
                {
                    return;
                }

                var availableForLocation = availableInventories
                    .Where(x => x.LocationId == detalle.LocationId)
                    .ToList();

                var take1 = await CargarTomaAsync(detallesMismaUbicacion, availableForLocation, 1);
                var take2 = await CargarTomaAsync(detallesMismaUbicacion, availableForLocation, 2);
                var take3 = await CargarTomaAsync(detallesMismaUbicacion, availableForLocation, 3);
                var take4 = await CargarTomaAsync(detallesMismaUbicacion, availableForLocation, 4);

                if (version != _detailLoadVersion)
                {
                    return;
                }

                _takeResults = [take1, take2, take3, take4];

                dgPrimeraToma.ItemsSource = take1.Rows;
                dgSegundaToma.ItemsSource = take2.Rows;
                dgTerceraToma.ItemsSource = take3.Rows;
                dgCuartaToma.ItemsSource = take4.Rows;

                var takeIndex = Math.Clamp(NormalizeTakeNumber(detalle.TakeNumber) - 1, 0, 3);
                if (tabTomas is not null)
                {
                    tabTomas.SelectedIndex = takeIndex;
                }

                ActualizarResumenDetalleSeleccionado();
            }
            catch (Exception ex)
            {
                if (version == _detailLoadVersion)
                {
                    DialogHelper.ShowError(ex.Message);
                }
            }
            finally
            {
                if (version == _detailLoadVersion)
                {
                    MostrarLoaderDetalle(false);
                }

                _loadingLocationDetail = false;
            }
        }

        private async Task<TakeLoadResult> CargarTomaAsync(
            IReadOnlyCollection<CyclicInventoryDetailDto> detallesMismaUbicacion,
            IReadOnlyCollection<AvailableInventoryDto> availableForLocation,
            int takeNumber)
        {
            var takeDetail = detallesMismaUbicacion
                .FirstOrDefault(x => NormalizeTakeNumber(x.TakeNumber) == takeNumber);

            if (takeDetail is null)
            {
                return TakeLoadResult.Empty;
            }

            var response = await _cyclicInventoryService.GetCyclicInventoryScans(
                takeDetail.InventarioCiclicoId,
                takeDetail.InventarioCiclicoDetalleId);

            if (!response.IsSuccess)
            {
                DialogHelper.ShowWarning(response.Message ?? $"No se pudieron cargar los escaneos de la toma {takeNumber}.");
                return TakeLoadResult.Empty;
            }

            var scans = response.Data ?? [];
            return BuildTakeLoadResult(availableForLocation, scans);
        }

        private static TakeLoadResult BuildTakeLoadResult(
            IReadOnlyCollection<AvailableInventoryDto> availableInventories,
            IReadOnlyCollection<CyclicInventoryScanDto> scans)
        {
            var availableGroups = availableInventories
                .GroupBy(GetAvailableComparisonKey, StringComparer.OrdinalIgnoreCase)
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .ToDictionary(x => x.Key, x => x.ToList(), StringComparer.OrdinalIgnoreCase);

            var scanGroups = scans
                .GroupBy(GetScanComparisonKey, StringComparer.OrdinalIgnoreCase)
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .ToDictionary(x => x.Key, x => x.ToList(), StringComparer.OrdinalIgnoreCase);

            var keys = availableGroups.Keys
                .Union(scanGroups.Keys, StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var rows = new List<LocationStandardComparisonRow>();
            var coinciden = 0;
            var faltan = 0;
            var extras = 0;
            var otrasUbicaciones = 0;

            foreach (var key in keys)
            {
                availableGroups.TryGetValue(key, out var availableList);
                scanGroups.TryGetValue(key, out var scanList);

                availableList ??= [];
                scanList ??= [];

                var availableCount = availableList.Count;
                var scannedCount = scanList.Count;
                var estado = ResolveEstado(availableList, scanList, availableCount, scannedCount);
                var (background, foreground) = ResolveBrushes(estado);

                if (estado == EstadoCoincide)
                {
                    coinciden++;
                }
                else if (estado == EstadoFaltaEscaneo)
                {
                    faltan++;
                }
                else if (estado == EstadoOtraUbicacion)
                {
                    otrasUbicaciones++;
                }
                else
                {
                    extras++;
                }

                rows.Add(new LocationStandardComparisonRow(
                    StandardId: key,
                    PartNumber: FirstNonEmpty(availableList.Select(x => x.PartNumber)),
                    Description: FirstNonEmpty(availableList.Select(x => x.Description)),
                    Disponible: availableCount.ToString(),
                    Escaneado: scannedCount.ToString(),
                    Estado: estado,
                    UbicacionActual: ResolveCurrentLocation(scanList),
                    RowBackground: background,
                    RowForeground: foreground,
                    SortPriority: GetStatusPriority(estado)));
            }

            rows = rows
                .OrderBy(x => x.SortPriority)
                .ThenBy(x => x.StandardId, StringComparer.OrdinalIgnoreCase)
                .ToList();

            return new TakeLoadResult(
                rows,
                availableInventories.Count,
                scans.Count,
                coinciden,
                faltan,
                extras,
                otrasUbicaciones);
        }

        private void ActualizarResumenDetalleSeleccionado()
        {
            if (_detalleSeleccionado is null || tabTomas is null)
            {
                return;
            }

            var takeIndex = Math.Clamp(tabTomas.SelectedIndex, 0, 3);
            var takeNumber = takeIndex + 1;
            var result = _takeResults.Length > takeIndex ? _takeResults[takeIndex] : TakeLoadResult.Empty;

            txtDetalleUbicacionSeleccionada.Text =
                $"Ubicación: {_detalleSeleccionado.Ubicacion} | Toma {takeNumber} | Disponible {result.AvailableCount} | Escaneado {result.ScannedCount} | Coinciden {result.CoincideCount} | Faltan {result.FaltanteCount} | Extra {result.ExtraCount} | Otra ubicación {result.OtraUbicacionCount}";
        }

        private async Task<List<AvailableInventoryDto>> EnsureAvailableInventoriesAsync()
        {
            if (_availableInventoriesLoaded)
            {
                return _availableInventoriesCache;
            }

            var response = await _availableInventoryService.GetAvailableInventories();

            if (!response.IsSuccess)
            {
                DialogHelper.ShowWarning(response.Message ?? "No se pudo cargar el inventario disponible.");
                _availableInventoriesCache = [];
                _availableInventoriesLoaded = true;
                return _availableInventoriesCache;
            }

            _availableInventoriesCache = response.Data ?? [];
            _availableInventoriesLoaded = true;
            return _availableInventoriesCache;
        }

        private void InvalidarCacheInventariosDisponibles()
        {
            _availableInventoriesCache = [];
            _availableInventoriesLoaded = false;
        }

        private static int NormalizeTakeNumber(int takeNumber)
        {
            return takeNumber <= 0 ? 1 : takeNumber;
        }

        private static string ResolveEstado(
            IReadOnlyCollection<AvailableInventoryDto> availableList,
            IReadOnlyCollection<CyclicInventoryScanDto> scanList,
            int availableCount,
            int scannedCount)
        {
            if (scanList.Any(x => x.IsInAnotherLocation))
            {
                return EstadoOtraUbicacion;
            }

            if (scanList.Any(x => x.InventoryNotAvailable))
            {
                return EstadoNoDisponible;
            }

            if (availableCount > 0 && scannedCount > 0 && availableCount == scannedCount)
            {
                return EstadoCoincide;
            }

            if (availableCount > 0 && scannedCount == 0)
            {
                return EstadoFaltaEscaneo;
            }

            if (availableCount == 0 && scannedCount > 0)
            {
                return EstadoEscaneadoExtra;
            }

            if (availableCount > scannedCount)
            {
                return EstadoFaltaEscaneo;
            }

            return EstadoEscaneadoExtra;
        }

        private static int GetStatusPriority(string estado)
        {
            return estado switch
            {
                EstadoOtraUbicacion => 0,
                EstadoNoDisponible => 0,
                EstadoFaltaEscaneo => 1,
                EstadoEscaneadoExtra => 1,
                EstadoCoincide => 2,
                _ => 3
            };
        }

        private static (Brush Background, Brush Foreground) ResolveBrushes(string estado)
        {
            return estado switch
            {
                EstadoCoincide => (BrushCoincideBackground, BrushCoincideForeground),
                EstadoFaltaEscaneo => (BrushFaltaBackground, BrushFaltaForeground),
                EstadoEscaneadoExtra => (BrushExtraBackground, BrushExtraForeground),
                EstadoNoDisponible => (BrushExtraBackground, BrushExtraForeground),
                EstadoOtraUbicacion => (BrushOtraUbicacionBackground, BrushOtraUbicacionForeground),
                _ => (Brushes.White, Brushes.Black)
            };
        }

        private static string GetAvailableComparisonKey(AvailableInventoryDto inventory)
        {
            if (!string.IsNullOrWhiteSpace(inventory.StandardIdStr))
            {
                return inventory.StandardIdStr.Trim();
            }

            if (inventory.StandardId.HasValue && inventory.StandardId.Value > 0)
            {
                return inventory.StandardId.Value.ToString();
            }

            return $"AV-{inventory.AvailableInventoryId}";
        }

        private static string GetScanComparisonKey(CyclicInventoryScanDto scan)
        {
            if (!string.IsNullOrWhiteSpace(scan.StandardId))
            {
                return scan.StandardId.Trim();
            }

            return $"SC-{scan.CyclicInventoryScanId}";
        }

        private static string FirstNonEmpty(IEnumerable<string?> values)
        {
            return values
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
                ?.Trim()
                ?? string.Empty;
        }

        private static string ResolveCurrentLocation(IEnumerable<CyclicInventoryScanDto> scans)
        {
            var currentLocation = scans
                .Where(x => x.IsInAnotherLocation && !string.IsNullOrWhiteSpace(x.CurrentLocation))
                .Select(x => x.CurrentLocation!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return currentLocation.Count == 0
                ? string.Empty
                : string.Join(", ", currentLocation);
        }

        private static Brush CreateBrush(string hex)
        {
            var color = (Color)ColorConverter.ConvertFromString(hex)!;
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            return brush;
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

        private void MostrarLoaderDetalle(bool mostrar, string mensaje = "Cargando detalle...")
        {
            TxtLoadingResultados.Text = mensaje;
            LoadingOverlayResultados.Visibility = mostrar ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ActualizarStatusInventarios()
        {
            txtStatusUbicacionesTop.Text = $"Inventarios: {_inventarios.Count}";
        }

        private sealed record LocationStandardComparisonRow(
            string StandardId,
            string PartNumber,
            string Description,
            string Disponible,
            string Escaneado,
            string Estado,
            string UbicacionActual,
            Brush RowBackground,
            Brush RowForeground,
            int SortPriority);

        private sealed record TakeLoadResult(
            List<LocationStandardComparisonRow> Rows,
            int AvailableCount,
            int ScannedCount,
            int CoincideCount,
            int FaltanteCount,
            int ExtraCount,
            int OtraUbicacionCount)
        {
            public static TakeLoadResult Empty { get; } = new(
                [],
                0,
                0,
                0,
                0,
                0,
                0);
        }
    }
}
