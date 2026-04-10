using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using LD.Client.Services;
using LD.Contracts.ASN;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using LD.FormsX.Model;
using LD.FormsX.Model.Lookup;
using LD.FormsX.Views.Articulos;
using Serilog;

namespace LD.FormsX.Views.Dialogs
{
    public partial class NuevoASNView : Window
    {
        private readonly AsnService _asnService;
        private readonly AsnDetailService _asnDetailService;
        private readonly LookupService _lookupService;
        private readonly ProductService _productService;
        private readonly InventaryStatusService _inventaryStatusService;
        private readonly DataGridNavigationManager _detailGridNavigation;
        private readonly HashSet<AsnDetailItem> _savingDetailRows = new();
        private AsnDto? AsnSelected;
        private bool _cargandoDatos = false;

        public ObservableCollection<LookupItem> ProductLookupItems { get; } = new();
        public ObservableCollection<LookupItem> StatusLookupItems { get; } = new();
        public ObservableCollection<LookupItem> SdLookupItems { get; } = new();

        public ObservableCollection<AsnDetailItem> DetailItems { get; set; } = new();

        public NuevoASNView(AsnService asnService, AsnDetailService asnDetailService, ProductService productService, LookupService lookupService, InventaryStatusService inventaryStatusService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _asnService = asnService;
            _asnDetailService = asnDetailService;
            _lookupService = lookupService;
            _productService = productService;
            _inventaryStatusService = inventaryStatusService;
            _detailGridNavigation = new DataGridNavigationManager(dgDetail);
            DataContext = this;

            HideScanSection();
        }

        public void SetAsn(AsnDto? _asnSelected)
        {
            AsnSelected = _asnSelected;

            if (AsnSelected != null)
                ShowScanSection();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                _cargandoDatos = true;

                if (cmbCliente.Items.Count == 0)
                    await SetCombos();

                var response = await _asnService.GetAsnById(AsnSelected?.AsnId ?? 0);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar la familia.");
                    return;
                }

                var item = response.Data;

                cmbCliente.SelectedValue = item.ClientId.ToString();
                await SetCombosProjects(item.ProjectId.ToString());
                txtNumeroFactura.Text = item.InvoiceNumber;
                txtNumeroGuia.Text = item.GuideNumber;
                dpEta.SelectedDate = item.Eta;
                txtBultos.Text = item.PackagesQty?.ToString() ?? string.Empty;
                chkEsDevolucion.IsChecked = item.IsReturn;
                chkMovimientoRequeridoCliente.IsChecked = item.IsCustomerMovementRequired;
                txtLineaTransporte.Text = item.TransportLine;
                txtTipoVehiculo.Text = item.VehicleType;
                txtChofer.Text = item.DriverName;
                txtPlacasVehiculo.Text = item.VehiclePlate;
                txtSelloTransporte.Text = item.SealNumber;





                await LoadProductsForSelectedClientProjectAsync();

            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _cargandoDatos = false;
            }
        }

        private async Task CargarDatosAsyncDet()
        {
            if (AsnSelected == null || AsnSelected.AsnId <= 0)
                return;

            var result = await _asnDetailService.GetAsnDetailsByAsn(AsnSelected.AsnId);

            /*if (!result.IsSuccess || result.Data == null)
                return;*/

            DetailItems.Clear();
            if (result.Data != null)
            {

                foreach (var dto in result.Data)
                {
                    DetailItems.Add(AsnDetailItem.FromRequest(new AsnDetailRequest
                    {
                        AsnDetailId = dto.AsnDetailId,
                        AsnId = dto.AsnId,
                        ProductId = dto.ProductId ?? 0,
                        PartNumber = dto.PartNumber,
                        Description = dto.Description,
                        Quantity = dto.Quantity,
                        Status = dto.Status,
                        SD = dto.SD,
                        LotNumber = dto.LotNumber,
                        ExpirationDate = dto.ExpirationDate,
                        CustomerReference = dto.CustomerReference,
                        ExchangeRate = dto.ExchangeRate,
                        PurchaseOrder = dto.PurchaseOrder,
                        CustomsDeclarationNumber = dto.CustomsDeclarationNumber,
                        Split = dto.Split
                    }));
                }
            }


            if (!DetailItems.Any())
            {
                DetailItems.Add(new AsnDetailItem());
            }
        }



        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (cmbCliente.Items.Count == 0)
                await CargarDatosInicialesAsync();
        }

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                _cargandoDatos = true;

                await SetCombos();
                await LoadDetailLookupsAsync();

                if (AsnSelected != null)
                {
                    await CargarDatosAsync();
                    await CargarDatosAsyncDet();
                }
            }
            finally
            {
                _cargandoDatos = false;
            }
        }

        private async Task SetCombos()
        {
            var clientes = await _lookupService.GetClientLookup();

            if (clientes.IsSuccess && clientes.Data != null)
            {
                cmbCliente.ItemsSource = clientes.Data;
                cmbCliente.DisplayMemberPath = "Value";
                cmbCliente.SelectedValuePath = "Key";
                cmbCliente.SelectedIndex = -1;
            }
        }

        private async Task SetCombosProjects(string projectSel = "")
        {
            if (cmbCliente.SelectedValue == null)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            if (!int.TryParse(cmbCliente.SelectedValue.ToString(), out int clienteId) || clienteId <= 0)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            var proyectos = await _lookupService.GetProjectClientLookup(clienteId);

            if (!proyectos.IsSuccess || proyectos.Data == null)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            cmbProyecto.DisplayMemberPath = "Value";
            cmbProyecto.SelectedValuePath = "Key";
            cmbProyecto.ItemsSource = proyectos.Data;

            if (!string.IsNullOrWhiteSpace(projectSel))
                cmbProyecto.SelectedValue = projectSel;
            else if (proyectos.Data.Count > 1)
                cmbProyecto.SelectedIndex = -1;
        }



        private async void cmbCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cargandoDatos)
                return;

            await SetCombosProjects();

        }
        private async void cmbProyecto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cargandoDatos)
                return;

            await LoadProductsForSelectedClientProjectAsync();
        }
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                return;

            DragMove();
        }

        private void BtnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnEscanear_Click(object sender, RoutedEventArgs e)
        {
            // Abrir diálogo de escaneo
            // var view = _serviceProvider.GetRequiredService<NuevoAsnEscaneoWindow>();
            // view.Owner = this;
            // view.ShowDialog();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnGuardar.IsEnabled = false;
                CommitDetailGridEdits();

                var request = BuildRequest();
                if (request.ClientId <= 0)
                {
                    DialogHelper.ShowWarning("Cliente es obligatorio");
                    return;
                }

                if (request.ProjectId <= 0)
                {
                    DialogHelper.ShowWarning("Proyecto es obligatorio");
                    return;
                }

                var result = await SaveAsn(request);
                Log.Information("Resultado SaveAsn. Success: {IsSuccess}. Code: {Code}. Message: {Message}. Data: {Data}",
                    result.IsSuccess, result.Code, result.Message, result.Data);

                if (!result.IsSuccess)
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? "Hubo un error al guardar.");
                    return;
                }

                var asnId = ResolveSavedAsnId(result);
                Log.Information("ASN resuelto para guardar detalles: {AsnId}", asnId);
                await SaveDetailsAsync(asnId);

                AsnSelected ??= new AsnDto();
                AsnSelected.AsnId = asnId;

                ToastHelper.ShowSuccess("ASN guardado exitosamente.");
                ShowScanSection();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnGuardar.IsEnabled = true;
            }

        }

        private void BtnBuscarVehiculo_Click(object sender, RoutedEventArgs e)
        {
            // Abrir diálogo de vehículos registrados
            // var view = _serviceProvider.GetRequiredService<VehiculosRegistradosWindow>();
            // view.Owner = this;
            // view.ShowDialog();
        }

        private void BtnCrearAsn_Click(object sender, RoutedEventArgs e)
        {
            // Guardar ASN
        }

        private void HideScanSection()
        {
            btnEscanear.Visibility = Visibility.Collapsed;
            gridScanSection.Visibility = Visibility.Collapsed;

            Height = 520;
        }

        private void ShowScanSection()
        {
            btnEscanear.Visibility = Visibility.Visible;
            gridScanSection.Visibility = Visibility.Visible;

            Height = 900;
        }

        private Task<ApiResponseDto<string>> CreateASN(AsnRequest request) =>
           _asnService.CreateAsn(request);

        private Task<ApiResponseDto<string>> EditAsn(int asnId, AsnRequest request) =>
            _asnService.UpdateAsn(asnId, request);

        private async Task<ApiResponseDto<string>> SaveAsn(AsnRequest request)
        {
            return AsnSelected != null
                ? await EditAsn(AsnSelected.AsnId, request)
                : await CreateASN(request);
        }

        private async Task<bool> EnsureAsnPersistedAsync()
        {
            if (AsnSelected?.AsnId > 0)
                return true;

            var request = BuildRequest();
            if (request.ClientId <= 0 || request.ProjectId <= 0)
            {
                DialogHelper.ShowWarning("Guarda primero el encabezado del ASN con cliente y proyecto.");
                return false;
            }

            var saveResult = await SaveAsn(request);
            Log.Information("Resultado EnsureAsnPersistedAsync. Success: {IsSuccess}. Code: {Code}. Message: {Message}. Data: {Data}",
                saveResult.IsSuccess, saveResult.Code, saveResult.Message, saveResult.Data);

            if (!saveResult.IsSuccess)
            {
                DialogHelper.ShowError(saveResult.ErrorMessage ?? saveResult.Message ?? "No se pudo guardar el ASN.");
                return false;
            }

            var asnId = ResolveSavedAsnId(saveResult);
            AsnSelected ??= new AsnDto();
            AsnSelected.AsnId = asnId;
            ShowScanSection();
            return true;
        }

        private int ResolveSavedAsnId(ApiResponseDto<string> result)
        {
            if (AsnSelected != null && AsnSelected.AsnId > 0)
                return AsnSelected.AsnId;

            if (int.TryParse(result.Data, out var asnId) && asnId > 0)
                return asnId;

            if (!string.IsNullOrWhiteSpace(result.Data))
            {
                var asnByCode = ResolveAsnByCodeAsync(result.Data).GetAwaiter().GetResult();
                if (asnByCode?.AsnId > 0)
                {
                    AsnSelected = asnByCode;
                    return asnByCode.AsnId;
                }
            }

            throw new InvalidOperationException("No se pudo obtener el Id del ASN guardado.");
        }

        private async Task<AsnDto?> ResolveAsnByCodeAsync(string? asnCode)
        {
            if (string.IsNullOrWhiteSpace(asnCode))
                return null;

            var asnList = await _asnService.GetAsn();
            if (!asnList.IsSuccess || asnList.Data == null)
                return null;

            return asnList.Data.FirstOrDefault(x =>
                string.Equals(x.AsnCode, asnCode, StringComparison.OrdinalIgnoreCase));
        }

        private AsnRequest BuildRequest()
        {
            return new AsnRequest
            {
                InvoiceNumber = txtNumeroFactura.Text.Trim(),
                ClientId = int.TryParse(cmbCliente.SelectedValue?.ToString(), out int clienteId) ? clienteId : 0,
                ProjectId = int.TryParse(cmbProyecto.SelectedValue?.ToString(), out int projectId) ? projectId : 0,
                GuideNumber = txtNumeroGuia.Text.Trim(),
                Eta = dpEta.SelectedDate,
                PackagesQty = int.TryParse(txtBultos.Text.Trim(), out int packagesQty) ? packagesQty : (int?)null,
                IsReturn = chkEsDevolucion.IsChecked == true,
                IsCustomerMovementRequired = chkMovimientoRequeridoCliente.IsChecked == true,
                TransportLine = txtLineaTransporte.Text.Trim(),
                VehicleType = txtTipoVehiculo.Text.Trim(),
                DriverName = txtChofer.Text.Trim(),
                VehiclePlate = txtPlacasVehiculo.Text.Trim(),
                SealNumber = txtSelloTransporte.Text.Trim()
            };
        }

        private void CommitDetailGridEdits()
        {
            dgDetail.CommitEdit(DataGridEditingUnit.Cell, true);
            dgDetail.CommitEdit(DataGridEditingUnit.Row, true);
            _detailGridNavigation.CommitCurrentEdit();
        }

        private static bool IsDetailRowCompleted(AsnDetailItem detailRow)
        {
            return detailRow.ProductId > 0
                && !string.IsNullOrWhiteSpace(detailRow.PartNumber)
                && detailRow.Quantity > 0;
        }

        private async Task SaveDetailRowAsync(AsnDetailItem detailRow)
        {
            if (!IsDetailRowCompleted(detailRow))
                return;

            if (_savingDetailRows.Contains(detailRow))
                return;

            if (!await EnsureAsnPersistedAsync())
                return;

            _savingDetailRows.Add(detailRow);

            try
            {
                detailRow.AsnId = AsnSelected!.AsnId;

                var request = detailRow.ToRequest();
                request.AsnId = AsnSelected.AsnId;

                var response = detailRow.AsnDetailId > 0
                    ? await _asnDetailService.UpdateAsnDetail(detailRow.AsnDetailId, request)
                    : await _asnDetailService.CreateAsnDetail(request);

                Log.Information(
                    "Resultado SaveDetailRowAsync. AsnId: {AsnId}. AsnDetailId: {AsnDetailId}. Success: {IsSuccess}. Code: {Code}. Message: {Message}. Data: {Data}",
                    request.AsnId, detailRow.AsnDetailId, response.IsSuccess, response.Code, response.Message, response.Data);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo guardar el detalle del ASN.");
                    return;
                }

                if (detailRow.AsnDetailId <= 0 && int.TryParse(response.Data, out var asnDetailId) && asnDetailId > 0)
                    detailRow.AsnDetailId = asnDetailId;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _savingDetailRows.Remove(detailRow);
            }
        }

        private async Task SaveDetailsAsync(int asnId)
        {
            var detailRows = DetailItems
                .Where(item => !IsEmptyDetailRow(item))
                .ToList();

            for (var index = 0; index < detailRows.Count; index++)
            {
                var row = detailRows[index];

                if (string.IsNullOrWhiteSpace(row.PartNumber))
                    throw new InvalidOperationException($"La partida {index + 1} debe tener número de parte.");

                if (row.Quantity <= 0)
                    throw new InvalidOperationException($"La partida {index + 1} debe tener cantidad mayor a 0.");

                row.AsnId = asnId;

                var request = row.ToRequest();
                request.AsnId = asnId;

                var response = row.AsnDetailId > 0
                    ? await _asnDetailService.UpdateAsnDetail(row.AsnDetailId, request)
                    : await _asnDetailService.CreateAsnDetail(request);

                Log.Information(
                    "Resultado SaveAsnDetail. Partida: {Index}. AsnDetailId: {AsnDetailId}. Success: {IsSuccess}. Code: {Code}. Message: {Message}. Data: {Data}",
                    index + 1, row.AsnDetailId, response.IsSuccess, response.Code, response.Message, response.Data);

                if (!response.IsSuccess)
                    throw new InvalidOperationException(response.Message ?? response.ErrorMessage ?? $"No se pudo guardar la partida {index + 1}.");

                if (row.AsnDetailId <= 0 && int.TryParse(response.Data, out var asnDetailId) && asnDetailId > 0)
                    row.AsnDetailId = asnDetailId;
            }
        }

        private static bool IsEmptyDetailRow(AsnDetailItem item)
        {
            return item.ProductId <= 0
                && string.IsNullOrWhiteSpace(item.PartNumber)
                && string.IsNullOrWhiteSpace(item.Description)
                && item.Quantity <= 0
                && string.IsNullOrWhiteSpace(item.Status)
                && string.IsNullOrWhiteSpace(item.SD)
                && string.IsNullOrWhiteSpace(item.LotNumber)
                && item.ExpirationDate == null
                && string.IsNullOrWhiteSpace(item.CustomerReference)
                && item.ExchangeRate == null
                && string.IsNullOrWhiteSpace(item.PurchaseOrder)
                && string.IsNullOrWhiteSpace(item.CustomsDeclarationNumber)
                && item.Split <= 0;
        }

        private bool TryMoveToNextRowOrAppend()
        {
            if (dgDetail.CurrentCell.Column == null || dgDetail.CurrentItem is not AsnDetailItem currentRow)
                return false;

            var lastEditableColumn = _detailGridNavigation.GetLastEditableColumn();
            if (lastEditableColumn == null || !ReferenceEquals(dgDetail.CurrentCell.Column, lastEditableColumn))
                return false;

            var currentIndex = DetailItems.IndexOf(currentRow);
            if (currentIndex < 0)
                return false;

            if (currentIndex < DetailItems.Count - 1)
            {
                var nextRow = DetailItems[currentIndex + 1];
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _detailGridNavigation.MoveFocusToFirstEditableCell(nextRow);
                }), DispatcherPriority.Background);

                return true;
            }

            if (IsEmptyDetailRow(currentRow))
                return false;

            var newRow = new AsnDetailItem();
            DetailItems.Add(newRow);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                dgDetail.ScrollIntoView(newRow);
                _detailGridNavigation.MoveFocusToFirstEditableCell(newRow);
            }), DispatcherPriority.Background);

            return true;
        }

        private void dgDetail_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {

        }

        private async Task LoadDetailLookupsAsync()
        {
            await LoadStatusLookupAsync();
            await LoadSdLookupAsync();
        }

        private async Task LoadStatusLookupAsync()
        {
            try
            {
                StatusLookupItems.Clear();

                var response = await _inventaryStatusService.GetInventaryStatus();
                if (!response.IsSuccess || response.Data == null)
                    return;

                foreach (var item in response.Data
                    .OrderBy(x => x.StatusId)
                    .Select(x => new LookupItem
                    {
                        Id = x.StatusId ?? string.Empty,
                        Code = x.StatusId ?? string.Empty,
                        Description = x.Descripcion ?? string.Empty,
                        Data = x
                    }))
                {
                    StatusLookupItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task LoadSdLookupAsync()
        {
            try
            {
                SdLookupItems.Clear();

                var response = await _lookupService.GetDimensionerLookup();
                if (!response.IsSuccess || response.Data == null)
                    return;

                foreach (var item in response.Data
                    .OrderBy(x => x.Key)
                    .Select(x => new LookupItem
                    {
                        Id = x.Key ?? string.Empty,
                        Code = x.Key ?? string.Empty,
                        Description = x.Value ?? string.Empty,
                        Data = x
                    }))
                {
                    SdLookupItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task LoadProductsForSelectedClientProjectAsync()
        {
            try
            {
                ProductLookupItems.Clear();

                if (cmbCliente.SelectedValue == null || cmbProyecto.SelectedValue == null)
                    return;

                if (!int.TryParse(cmbCliente.SelectedValue.ToString(), out int clientId) || clientId <= 0)
                    return;

                if (!int.TryParse(cmbProyecto.SelectedValue.ToString(), out int projectId) || projectId <= 0)
                    return;

                var response = await _productService.GetProductByClientId(clientId, projectId); // ajusta al método real

                if (!response.IsSuccess || response.Data == null)
                    return;
                foreach (var item in response.Data
                    .Select(x => new LookupItem
                    {
                        Id = x.ItemId,
                        Code = x.NumeroParte ?? string.Empty,
                        Description = x.Descripcion ?? string.Empty,
                        Data = x
                    })
                    .OrderBy(x => x.Code))
                {
                    ProductLookupItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void InlineLookup_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is not InlineLookupEditor editor)
                    return;

                if (editor.DataContext is not AsnDetailItem row)
                    return;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _detailGridNavigation.CommitCurrentEdit();
                    _detailGridNavigation.MoveFocusToNextCell(row);
                    _ = SaveDetailRowAsync(row);
                }), DispatcherPriority.Background);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }


        private void dgDetail_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (DetailItems == null)
                return;

            if (e.Row.Item is not AsnDetailItem detailRow)
                return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _ = SaveDetailRowAsync(detailRow);
            }), DispatcherPriority.Background);
        }

        private void dgDetail_CurrentCellChanged(object? sender, EventArgs e)
        {
            try
            {
                if (dgDetail.CurrentCell.Column == null)
                    return;

                if (dgDetail.CurrentCell.Item is not AsnDetailItem)
                    return;

                if (dgDetail.CurrentCell.Column.IsReadOnly)
                    return;

                _detailGridNavigation.HandleCurrentCellChanged();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void dgDetail_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.OriginalSource is DependencyObject source &&
                    _detailGridNavigation.IsEventInsideControl<InlineLookupEditor>(source))
                {
                    return;
                }

                if (e.Key != Key.F4 && e.Key != Key.Enter)
                    return;

                if (dgDetail.CurrentCell == null || dgDetail.CurrentItem is not AsnDetailItem)
                    return;

                var currentColumn = dgDetail.CurrentCell.Column;
                if (currentColumn == null)
                    return;

                if (e.Key == Key.Enter)
                {
                    e.Handled = TryMoveToNextRowOrAppend() || _detailGridNavigation.HandleEnterKeyNavigation();
                    return;
                }

                var header = currentColumn.Header?.ToString() ?? string.Empty;
                if (!header.Equals("Número de Parte", StringComparison.OrdinalIgnoreCase))
                    return;

                if (!ProductLookupItems.Any())
                {
                    DialogHelper.ShowWarning("No hay productos cargados para el cliente/proyecto seleccionado.");
                    return;
                }

                dgDetail.BeginEdit();

                e.Handled = true;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

    }
}
