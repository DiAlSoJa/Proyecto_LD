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
using LD.Contracts.Vehicle;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using LD.FormsX.Model;
using LD.FormsX.Model.Lookup;
using LD.FormsX.Views.Articulos;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LD.FormsX.Views.Dialogs
{
    public partial class NuevoASNView : Window
    {
        private readonly AsnService _asnService;
        private readonly AsnDetailService _asnDetailService;
        private readonly AsnReceiptService _asnReceiptService;
        private readonly LookupService _lookupService;
        private readonly LocationService _locationService;
        private readonly ProductService _productService;
        private readonly InventaryStatusService _inventaryStatusService;
        private readonly IServiceProvider _serviceProvider;
        private readonly DataGridNavigationManager _detailGridNavigation;
        private readonly DataGridNavigationManager _receiptGridNavigation;
        private readonly HashSet<AsnDetailItem> _savingDetailRows = new();
        private readonly HashSet<AsnReceiptItem> _savingReceiptRows = new();
        private AsnDto? AsnSelected;
        private AsnDetailItem? _selectedDetailItem;
        private bool _cargandoDatos = false;

        public ObservableCollection<LookupItem> ProductLookupItems { get; } = new();
        public ObservableCollection<LookupItem> StatusLookupItems { get; } = new();
        public ObservableCollection<LookupItem> SdLookupItems { get; } = new();
        public ObservableCollection<LookupItem> LocationLookupItems { get; } = new();

        public ObservableCollection<AsnDetailItem> DetailItems { get; set; } = new();
        public ObservableCollection<AsnReceiptItem> ReceiptItems { get; set; } = new();

        public NuevoASNView(AsnService asnService, AsnDetailService asnDetailService, AsnReceiptService asnReceiptService, ProductService productService, LookupService lookupService, InventaryStatusService inventaryStatusService, LocationService locationService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _asnService = asnService;
            _asnDetailService = asnDetailService;
            _asnReceiptService = asnReceiptService;
            _lookupService = lookupService;
            _productService = productService;
            _inventaryStatusService = inventaryStatusService;
            _locationService = locationService;
            _serviceProvider = serviceProvider;
            _detailGridNavigation = new DataGridNavigationManager(dgDetail);
            _receiptGridNavigation = new DataGridNavigationManager(dgUbicacionesAsignadas);
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
            {
                DetailItems.Clear();
                ReceiptItems.Clear();
                _selectedDetailItem = null;
                return;
            }

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

            EnsureTrailingEmptyDetailRow();
            _selectedDetailItem = DetailItems.FirstOrDefault(item => !IsEmptyDetailRow(item)) ?? DetailItems.FirstOrDefault();
            await LoadReceiptItemsForSelectedDetailAsync();

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
                await LoadLocationLookupAsync();

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
                CommitReceiptGridEdits();

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
            try
            {
                var view = _serviceProvider.GetRequiredService<BuscarVehiculoView>();
                view.Owner = this;

                if (view.ShowDialog() != true || view.SelectedVehicle == null)
                    return;

                ApplySelectedVehicle(view.SelectedVehicle);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void ApplySelectedVehicle(VehicleDto vehicle)
        {
            txtTipoVehiculo.Text = vehicle.Tipo ?? string.Empty;
            txtPlacasVehiculo.Text = vehicle.Placas ?? string.Empty;

            if (string.IsNullOrWhiteSpace(txtLineaTransporte.Text))
                txtLineaTransporte.Text = vehicle.Nombre ?? string.Empty;
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
            EnsureTrailingEmptyDetailRow();
        }

        private void EnsureTrailingEmptyDetailRow()
        {
            if (!DetailItems.Any())
            {
                DetailItems.Add(new AsnDetailItem());
                return;
            }

            var emptyRows = DetailItems.Where(IsEmptyDetailRow).ToList();
            if (!emptyRows.Any())
            {
                DetailItems.Add(new AsnDetailItem());
                return;
            }

            for (var i = 0; i < emptyRows.Count - 1; i++)
                DetailItems.Remove(emptyRows[i]);

            var trailingEmptyRow = emptyRows.Last();
            var trailingEmptyIndex = DetailItems.IndexOf(trailingEmptyRow);
            if (trailingEmptyIndex >= 0 && trailingEmptyIndex != DetailItems.Count - 1)
            {
                DetailItems.RemoveAt(trailingEmptyIndex);
                DetailItems.Add(trailingEmptyRow);
            }
        }

        private void CommitReceiptGridEdits()
        {
            dgUbicacionesAsignadas.CommitEdit(DataGridEditingUnit.Cell, true);
            dgUbicacionesAsignadas.CommitEdit(DataGridEditingUnit.Row, true);
            EnsureTrailingEmptyReceiptRow();
        }

        private void EnsureTrailingEmptyReceiptRow()
        {
            if (!ReceiptItems.Any())
            {
                var newRow = new AsnReceiptItem();
                SeedReceiptRowFromSelectedDetail(newRow);
                ReceiptItems.Add(newRow);
                return;
            }

            var emptyRows = ReceiptItems.Where(IsEmptyReceiptRow).ToList();
            if (!emptyRows.Any())
            {
                var newRow = new AsnReceiptItem();
                SeedReceiptRowFromSelectedDetail(newRow);
                ReceiptItems.Add(newRow);
                return;
            }

            for (var i = 0; i < emptyRows.Count - 1; i++)
                ReceiptItems.Remove(emptyRows[i]);

            var trailingEmptyRow = emptyRows.Last();
            SeedReceiptRowFromSelectedDetail(trailingEmptyRow);

            var trailingEmptyIndex = ReceiptItems.IndexOf(trailingEmptyRow);
            if (trailingEmptyIndex >= 0 && trailingEmptyIndex != ReceiptItems.Count - 1)
            {
                ReceiptItems.RemoveAt(trailingEmptyIndex);
                ReceiptItems.Add(trailingEmptyRow);
            }
        }

        private static bool IsDetailRowCompleted(AsnDetailItem detailRow)
        {
            return detailRow.ProductId > 0
                && !string.IsNullOrWhiteSpace(detailRow.PartNumber);
        }

        private static bool IsEmptyReceiptRow(AsnReceiptItem item)
        {
            return item.StandardId == null
                && item.StandardQuantity == null
                && item.MaximumQuantity == null
                && item.ReceivedQuantity == null
                && string.IsNullOrWhiteSpace(item.Status)
                && item.LocationId == null
                && string.IsNullOrWhiteSpace(item.LocationCode)
                && string.IsNullOrWhiteSpace(item.LotNumber)
                && item.ExpirationDate == null
                && string.IsNullOrWhiteSpace(item.Reference);
        }

        private bool IsReceiptRowCompleted(AsnReceiptItem receiptRow)
        {
            if (receiptRow.AsnDetailId <= 0)
                return false;

            return !string.IsNullOrWhiteSpace(receiptRow.LocationCode)
                || receiptRow.LocationId.HasValue
                || receiptRow.StandardId.HasValue
                || receiptRow.StandardQuantity.HasValue
                || receiptRow.MaximumQuantity.HasValue
                || receiptRow.ReceivedQuantity.HasValue
                || !string.IsNullOrWhiteSpace(receiptRow.Status)
                || !string.IsNullOrWhiteSpace(receiptRow.LotNumber)
                || receiptRow.ExpirationDate.HasValue
                || !string.IsNullOrWhiteSpace(receiptRow.Reference);
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

                EnsureTrailingEmptyDetailRow();
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

        private async Task<bool> EnsureDetailPersistedAsync(AsnDetailItem? detailRow)
        {
            if (detailRow == null)
                return false;

            if (!await EnsureAsnPersistedAsync())
                return false;

            detailRow.AsnId = AsnSelected!.AsnId;

            if (detailRow.AsnDetailId > 0)
                return true;

            await SaveDetailRowAsync(detailRow);
            return detailRow.AsnDetailId > 0;
        }

        private void SeedReceiptRowFromSelectedDetail(AsnReceiptItem receiptRow)
        {
            if (_selectedDetailItem == null)
                return;

            receiptRow.ApplyDefaultsFromDetail(_selectedDetailItem, AsnSelected?.AsnId ?? _selectedDetailItem.AsnId);
        }

        private async Task LoadReceiptItemsForSelectedDetailAsync()
        {
            ReceiptItems.Clear();

            if (_selectedDetailItem == null)
            {
                EnsureTrailingEmptyReceiptRow();
                return;
            }

            if (AsnSelected?.AsnId <= 0 || _selectedDetailItem.AsnDetailId <= 0)
            {
                EnsureTrailingEmptyReceiptRow();
                return;
            }

            var result = await _asnReceiptService.GetAsnReceipts();
            if (result.IsSuccess && result.Data != null)
            {
                foreach (var dto in result.Data.Where(x => x.AsnDetailId == _selectedDetailItem.AsnDetailId))
                {
                    var item = AsnReceiptItem.FromDto(dto);
                    item.AsnId = AsnSelected.AsnId;
                    ReceiptItems.Add(item);
                }
            }

            EnsureTrailingEmptyReceiptRow();
        }

        private async Task SaveReceiptRowAsync(AsnReceiptItem receiptRow)
        {
            if (_savingReceiptRows.Contains(receiptRow))
                return;

            var detailRow = _selectedDetailItem;
            if (detailRow == null && receiptRow.AsnDetailId > 0)
                detailRow = DetailItems.FirstOrDefault(x => x.AsnDetailId == receiptRow.AsnDetailId);

            if (!await EnsureDetailPersistedAsync(detailRow))
                return;

            receiptRow.ApplyDefaultsFromDetail(detailRow!, AsnSelected!.AsnId);
            if (!IsReceiptRowCompleted(receiptRow))
            {
                EnsureTrailingEmptyReceiptRow();
                return;
            }

            _savingReceiptRows.Add(receiptRow);

            try
            {
                var request = receiptRow.ToRequest();
                var response = receiptRow.AsnReceiptDetailId > 0
                    ? await _asnReceiptService.UpdateAsnReceipt(receiptRow.AsnReceiptDetailId, request)
                    : await _asnReceiptService.CreateAsnReceipt(request);

                Log.Information(
                    "Resultado SaveReceiptRowAsync. AsnDetailId: {AsnDetailId}. AsnReceiptDetailId: {AsnReceiptDetailId}. Success: {IsSuccess}. Code: {Code}. Message: {Message}. Data: {Data}",
                    request.AsnDetailId, receiptRow.AsnReceiptDetailId, response.IsSuccess, response.Code, response.Message, response.Data);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo guardar el registro del ASN.");
                    return;
                }

                if (receiptRow.AsnReceiptDetailId <= 0 && int.TryParse(response.Data, out var asnReceiptDetailId) && asnReceiptDetailId > 0)
                    receiptRow.AsnReceiptDetailId = asnReceiptDetailId;

                EnsureTrailingEmptyReceiptRow();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _savingReceiptRows.Remove(receiptRow);
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
            EnsureTrailingEmptyDetailRow();

            Dispatcher.BeginInvoke(new Action(() =>
            {
                dgDetail.ScrollIntoView(newRow);
                _detailGridNavigation.MoveFocusToFirstEditableCell(newRow);
            }), DispatcherPriority.Background);

            return true;
        }

        private bool TryMoveToNextReceiptRowOrAppend()
        {
            if (dgUbicacionesAsignadas.CurrentCell.Column == null || dgUbicacionesAsignadas.CurrentItem is not AsnReceiptItem currentRow)
                return false;

            var lastEditableColumn = _receiptGridNavigation.GetLastEditableColumn();
            if (lastEditableColumn == null || !ReferenceEquals(dgUbicacionesAsignadas.CurrentCell.Column, lastEditableColumn))
                return false;

            var currentIndex = ReceiptItems.IndexOf(currentRow);
            if (currentIndex < 0)
                return false;

            if (currentIndex < ReceiptItems.Count - 1)
            {
                var nextRow = ReceiptItems[currentIndex + 1];
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _receiptGridNavigation.MoveFocusToFirstEditableCell(nextRow);
                }), DispatcherPriority.Background);

                return true;
            }

            if (IsEmptyReceiptRow(currentRow))
                return false;

            var newRow = new AsnReceiptItem();
            SeedReceiptRowFromSelectedDetail(newRow);
            ReceiptItems.Add(newRow);
            EnsureTrailingEmptyReceiptRow();

            Dispatcher.BeginInvoke(new Action(() =>
            {
                dgUbicacionesAsignadas.ScrollIntoView(newRow);
                _receiptGridNavigation.MoveFocusToFirstEditableCell(newRow);
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

        private async Task LoadLocationLookupAsync()
        {
            try
            {
                LocationLookupItems.Clear();

                var response = await _locationService.GetLocations();
                if (!response.IsSuccess || response.Data == null)
                    return;

                foreach (var item in response.Data
                    .Where(x => !string.IsNullOrWhiteSpace(x.Ubicacion))
                    .OrderBy(x => x.Ubicacion)
                    .Select(x => new LookupItem
                    {
                        Id = x.LocationId,
                        Code = x.Ubicacion ?? string.Empty,
                        Description = x.Almacen ?? string.Empty,
                        Data = x
                    }))
                {
                    LocationLookupItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
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

                if (editor.DataContext is AsnDetailItem detailRow)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _detailGridNavigation.CommitCurrentEdit();
                        _detailGridNavigation.MoveFocusToNextCell(detailRow);
                        _ = SaveDetailRowAsync(detailRow);
                    }), DispatcherPriority.Background);
                    return;
                }

                if (editor.DataContext is AsnReceiptItem receiptRow)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _receiptGridNavigation.CommitCurrentEdit();
                        _receiptGridNavigation.MoveFocusToNextCell(receiptRow);
                        _ = SaveReceiptRowAsync(receiptRow);
                    }), DispatcherPriority.Background);
                }
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

        private void dgUbicacionesAsignadas_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            if (e.NewItem is not AsnReceiptItem receiptRow)
                return;

            SeedReceiptRowFromSelectedDetail(receiptRow);
        }

        private void dgUbicacionesAsignadas_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (ReceiptItems == null)
                return;

            if (e.Row.Item is not AsnReceiptItem receiptRow)
                return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _ = SaveReceiptRowAsync(receiptRow);
            }), DispatcherPriority.Background);
        }

        private void dgUbicacionesAsignadas_CurrentCellChanged(object? sender, EventArgs e)
        {
            try
            {
                if (dgUbicacionesAsignadas.CurrentCell.Column == null)
                    return;

                if (dgUbicacionesAsignadas.CurrentCell.Item is not AsnReceiptItem)
                    return;

                if (dgUbicacionesAsignadas.CurrentCell.Column.IsReadOnly)
                    return;

                _receiptGridNavigation.HandleCurrentCellChanged();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void dgUbicacionesAsignadas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.OriginalSource is DependencyObject source &&
                    _receiptGridNavigation.IsEventInsideControl<InlineLookupEditor>(source))
                {
                    return;
                }

                if (e.Key != Key.Enter)
                    return;

                if (dgUbicacionesAsignadas.CurrentCell.Column == null || dgUbicacionesAsignadas.CurrentItem is not AsnReceiptItem)
                    return;

                e.Handled = TryMoveToNextReceiptRowOrAppend() || _receiptGridNavigation.HandleEnterKeyNavigation();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void dgDetail_CurrentCellChanged(object? sender, EventArgs e)
        {
            try
            {
                if (dgDetail.CurrentCell.Column == null)
                    return;

                if (dgDetail.CurrentCell.Item is not AsnDetailItem currentDetailItem)
                    return;

                if (!ReferenceEquals(_selectedDetailItem, currentDetailItem))
                {
                    _selectedDetailItem = currentDetailItem;
                    await LoadReceiptItemsForSelectedDetailAsync();
                }

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
