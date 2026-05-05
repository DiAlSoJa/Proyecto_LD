using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using LD.Client.Services;
using LD.Contracts.ASN;
using LD.Contracts.DTOs.StandardLabel;
using LD.Contracts.Enums;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Product;
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
        private const string DefaultAsnStatus = "Creado";
        

        private readonly AsnService _asnService;
        private readonly AsnDetailService _asnDetailService;
        private readonly AsnReceiptService _asnReceiptService;
        private readonly LookupService _lookupService;
        private readonly LocationService _locationService;
        private readonly ProjectService _projectService;
        private readonly ProductService _productService;
        private readonly StandardLabelService _standardLabelService;
        private readonly InventaryStatusService _inventaryStatusService;
        private readonly IServiceProvider _serviceProvider;
        private readonly DataGridNavigationManager _detailGridNavigation;
        private readonly DataGridNavigationManager _receiptGridNavigation;
        private readonly HashSet<AsnDetailItem> _savingDetailRows = new();
        private readonly HashSet<AsnReceiptItem> _savingReceiptRows = new();
        private readonly HashSet<AsnDetailItem> _pendingDetailRows = new();
        private readonly HashSet<AsnReceiptItem> _pendingReceiptRows = new();
        private AsnDto? AsnSelected;
        private AsnDetailItem? _selectedDetailItem;
        private bool _cargandoDatos = false;
        private int _clientId;
        private int _projectId;
        private int? _defaultProjectLocationId;
        private string _defaultProjectLocationCode = string.Empty;
        private string _clientName = string.Empty;
        private string _projectName = string.Empty;
        private bool _projectScanRequired;
        private List<ScanConfigurationRequest> _projectScanConfigurations = [];

        public ObservableCollection<LookupItem> ProductLookupItems { get; } = new();
        public ObservableCollection<LookupItem> StatusLookupItems { get; } = new();
        public ObservableCollection<LookupItem> SdLookupItems { get; } = new();
        public ObservableCollection<LookupItem> LocationLookupItems { get; } = new();

        public ObservableCollection<AsnDetailItem> DetailItems { get; set; } = new();
        public ObservableCollection<AsnReceiptItem> ReceiptItems { get; set; } = new();

        public NuevoASNView(AsnService asnService, AsnDetailService asnDetailService, AsnReceiptService asnReceiptService, ProductService productService, StandardLabelService standardLabelService, LookupService lookupService, InventaryStatusService inventaryStatusService, LocationService locationService, ProjectService projectService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _asnService = asnService;
            _asnDetailService = asnDetailService;
            _asnReceiptService = asnReceiptService;
            _lookupService = lookupService;
            _productService = productService;
            _standardLabelService = standardLabelService;
            _inventaryStatusService = inventaryStatusService;
            _locationService = locationService;
            _projectService = projectService;
            _serviceProvider = serviceProvider;
            _detailGridNavigation = new DataGridNavigationManager(dgDetail);
            _receiptGridNavigation = new DataGridNavigationManager(dgUbicacionesAsignadas);
            DataContext = this;

            UpdateWindowTitle();
            HideScanSection();
        }

        public void SetAsn(AsnDto? _asnSelected)
        {
            AsnSelected = _asnSelected;
            _clientName = _asnSelected?.Client?.Trim() ?? string.Empty;
            _projectName = _asnSelected?.Project?.Trim() ?? string.Empty;
            UpdateWindowTitle();
            ApplyConfirmedState();

            if (AsnSelected != null)
                ShowScanSection();
        }

        public void SetClientProjectContext(int clientId, int projectId, string? clientName, string? projectName)
        {
            _clientId = clientId;
            _projectId = projectId;
            _clientName = clientName?.Trim() ?? string.Empty;
            _projectName = projectName?.Trim() ?? string.Empty;
            UpdateWindowTitle();

            if (IsLoaded)
            {
                _ = Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await LoadProductsForSelectedClientProjectAsync();
                    await LoadProjectDefaultLocationAsync();
                    await LoadProjectScanConfigurationsAsync();
                }), DispatcherPriority.Background);
            }
        }

        private void UpdateWindowTitle()
        {
            if (txtTituloVentana == null)
                return;

            var asnCode = AsnSelected?.AsnCode?.Trim();
            var clientName = string.IsNullOrWhiteSpace(_clientName) ? AsnSelected?.Client?.Trim() : _clientName;
            var projectName = string.IsNullOrWhiteSpace(_projectName) ? AsnSelected?.Project?.Trim() : _projectName;
            var titlePrefix = string.IsNullOrWhiteSpace(asnCode)
                ? ""
                : $" {asnCode}";

            if (string.IsNullOrWhiteSpace(clientName) && string.IsNullOrWhiteSpace(projectName))
            {
                txtTituloVentana.Text = titlePrefix;
                return;
            }

            if (string.IsNullOrWhiteSpace(clientName))
            {
                txtTituloVentana.Text = $"{titlePrefix} - {projectName}";
                return;
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                txtTituloVentana.Text = $"{titlePrefix} - {clientName}";
                return;
            }

            txtTituloVentana.Text = $"{titlePrefix} - {clientName} / {projectName}";
        }

        private static bool IsConfirmedStatus(string? status) =>
            string.Equals(status?.Trim(), "Confirmado", StringComparison.OrdinalIgnoreCase);

        private bool IsCurrentAsnConfirmed() => IsConfirmedStatus(AsnSelected?.Status);

        private bool IsScanRequiredForProject() => _projectScanRequired;

        private bool EnsureCurrentAsnEditable()
        {
            if (!IsCurrentAsnConfirmed())
                return true;

            DialogHelper.ShowWarning("El ASN esta confirmado y no permite agregar, editar ni eliminar registros.");
            return false;
        }

        private bool EnsureProjectScanDoesNotLockDetails()
        {
            if (!IsScanRequiredForProject())
                return true;

            DialogHelper.ShowWarning("El proyecto requiere escaneo obligatorio; los ASN Details no se pueden editar ni eliminar manualmente.");
            return false;
        }

        private bool EnsureProjectScanAllowsReceiptAction(string action)
        {
            if (!IsScanRequiredForProject())
                return true;

            DialogHelper.ShowWarning($"El proyecto requiere escaneo obligatorio; solo se permite editar estatus y ubicacion en ASN Receipt Details. No se puede {action}.");
            return false;
        }

        private void ApplyConfirmedState()
        {
            var isEditable = !IsCurrentAsnConfirmed();
            var scanRequired = IsScanRequiredForProject();

            if (btnGuardar != null)
                btnGuardar.IsEnabled = isEditable;

            if (btnBuscarVehiculo != null)
                btnBuscarVehiculo.IsEnabled = isEditable;

            if (txtNumeroFactura != null)
                txtNumeroFactura.IsEnabled = isEditable;

            if (txtNumeroGuia != null)
                txtNumeroGuia.IsEnabled = isEditable;

            if (dpEta != null)
                dpEta.IsEnabled = isEditable;

            if (txtBultos != null)
                txtBultos.IsEnabled = isEditable;

            if (chkEsDevolucion != null)
                chkEsDevolucion.IsEnabled = isEditable;

            if (chkMovimientoRequeridoCliente != null)
                chkMovimientoRequeridoCliente.IsEnabled = isEditable;

            if (txtLineaTransporte != null)
                txtLineaTransporte.IsEnabled = isEditable;

            if (txtTipoVehiculo != null)
                txtTipoVehiculo.IsEnabled = isEditable;

            if (txtChofer != null)
                txtChofer.IsEnabled = isEditable;

            if (txtPlacasVehiculo != null)
                txtPlacasVehiculo.IsEnabled = isEditable;

            if (txtSelloTransporte != null)
                txtSelloTransporte.IsEnabled = isEditable;

            ApplyDetailGridEditState(isEditable, scanRequired);
            ApplyReceiptGridEditState(isEditable, scanRequired);
        }

        private void ApplyDetailGridEditState(bool isEditable, bool scanRequired)
        {
            if (dgDetail == null)
                return;

            dgDetail.IsReadOnly = !isEditable || scanRequired;
            dgDetail.CanUserDeleteRows = isEditable && !scanRequired;
        }

        private void ApplyReceiptGridEditState(bool isEditable, bool scanRequired)
        {
            if (dgUbicacionesAsignadas == null)
                return;

            dgUbicacionesAsignadas.IsReadOnly = !isEditable;
            dgUbicacionesAsignadas.CanUserDeleteRows = isEditable && !scanRequired;

            foreach (var column in dgUbicacionesAsignadas.Columns)
                column.IsReadOnly = IsReceiptColumnReadOnly(column, isEditable, scanRequired);
        }

        private static bool IsReceiptColumnReadOnly(DataGridColumn column, bool isEditable, bool scanRequired)
        {
            if (!isEditable)
                return true;

            var header = column.Header?.ToString()?.Trim() ?? string.Empty;

            if (scanRequired)
                return !IsScanRequiredReceiptEditableColumn(header);

            return header.Equals("Número de Parte", StringComparison.OrdinalIgnoreCase)
                || header.Equals("Numero de Parte", StringComparison.OrdinalIgnoreCase)
                || header.Equals("Descripción", StringComparison.OrdinalIgnoreCase)
                || header.Equals("Descripcion", StringComparison.OrdinalIgnoreCase)
                || header.Equals("SD", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsScanRequiredReceiptEditableColumn(string header)
        {
            return header.Equals("Status", StringComparison.OrdinalIgnoreCase)
                || header.Equals("Estatus", StringComparison.OrdinalIgnoreCase)
                || header.Equals("Ubicación", StringComparison.OrdinalIgnoreCase)
                || header.Equals("Ubicacion", StringComparison.OrdinalIgnoreCase);
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                _cargandoDatos = true;

                var response = await _asnService.GetAsnById(AsnSelected?.AsnId ?? 0);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar la familia.");
                    return;
                }

                var item = response.Data;
                AsnSelected ??= new AsnDto();
                AsnSelected.AsnId = item.AsnId;
                AsnSelected.AsnCode = item.AsnCode;
                AsnSelected.Status = item.Status;

                _clientId = item.ClientId;
                _projectId = item.ProjectId;
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

                await LoadProjectDefaultLocationAsync();
                await LoadProjectScanConfigurationsAsync();
                await LoadProductsForSelectedClientProjectAsync();
                UpdateWindowTitle();
                ApplyConfirmedState();

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
                        StandardQuantity = dto.StandardQuantity,
                        MaximumQuantity = dto.MaximumQuantity,
                        Status = dto.Status,
                        SD = dto.SD,
                        LotNumber = dto.LotNumber,
                        ExpirationDate = dto.ExpirationDate,
                        CustomerReference = dto.CustomerReference,
                        ExchangeRate = dto.ExchangeRate,
                        PurchaseOrder = dto.PurchaseOrder,
                        CustomsDeclarationNumber = dto.CustomsDeclarationNumber
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

            await CargarDatosInicialesAsync();
        }

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                _cargandoDatos = true;

                await LoadDetailLookupsAsync();
                await LoadLocationLookupAsync();
                await LoadProjectDefaultLocationAsync();
                await LoadProjectScanConfigurationsAsync();
                await LoadProductsForSelectedClientProjectAsync();

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

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ToggleWindowState();
                return;
            }

            DragMove();
        }

        private void BtnMinimizarVentana_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximizarVentana_Click(object sender, RoutedEventArgs e)
        {
            ToggleWindowState();
        }

        private void BtnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            UpdateWindowButtons();
        }

        private void ToggleWindowState()
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void UpdateWindowButtons()
        {
            if (btnMaximizarVentana == null)
                return;

            btnMaximizarVentana.Content = WindowState == WindowState.Maximized ? "❐" : "□";
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtnEscanear_Click(object sender, RoutedEventArgs e)
        {
            if (!_projectScanConfigurations.Any())
                await LoadProjectScanConfigurationsAsync();

            if (!IsScanRequiredForProject()
                && (dgDetail.SelectedItem is not AsnDetailItem selectedDetail || IsEmptyDetailRow(selectedDetail)))
            {
                DialogHelper.ShowWarning("Selecciona una linea de ASN Details antes de escanear.");
                return;
            }

            if (!IsScanRequiredForProject() && dgDetail.SelectedItem is AsnDetailItem scanTarget)
                _selectedDetailItem = scanTarget;

            var view = _serviceProvider.GetRequiredService<NuevoASNEscaneoView>();
            view.Owner = this;
            view.SetScanConfigurations(_projectScanConfigurations);
            view.ScanCompleted += CreateReceiptFromCompletedScanAsync;
            view.UnmatchedScanReceived += ResolveUnmatchedScanAsync;
            view.ShowDialog();
            view.UnmatchedScanReceived -= ResolveUnmatchedScanAsync;
            view.ScanCompleted -= CreateReceiptFromCompletedScanAsync;
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!EnsureCurrentAsnEditable())
                    return;

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
                await RefreshAsnHeaderAsync(asnId);
                await SaveDetailsAsync(asnId);

                AsnSelected ??= new AsnDto();
                AsnSelected.AsnId = asnId;
                if (!string.IsNullOrWhiteSpace(result.Data) && !int.TryParse(result.Data, out _))
                    AsnSelected.AsnCode = result.Data.Trim();

                UpdateWindowTitle();

                ToastHelper.ShowSuccess("ASN guardado exitosamente.");
                ShowScanSection();

            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                ApplyConfirmedState();
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

        private async Task LoadProjectScanConfigurationsAsync()
        {
            _projectScanConfigurations = [];
            _projectScanRequired = false;

            if (_projectId <= 0)
            {
                ApplyConfirmedState();
                return;
            }

            var response = await _projectService.GetProjectById(_projectId);
            if (!response.IsSuccess || response.Data == null)
            {
                ApplyConfirmedState();
                return;
            }

            _projectScanRequired = response.Data.ScanRequired;
            _projectScanConfigurations = response.Data.ScanConfigurations
                .OrderBy(config => config.Order)
                .ThenBy(config => config.SystemFieldId)
                .ToList();

            ApplyConfirmedState();
        }

        private async Task<bool> CreateReceiptFromCompletedScanAsync(IReadOnlyList<ScanRuleResult> scanResults)
        {
            if (!EnsureCurrentAsnEditable())
                return false;

            AsnDetailItem detailRow;
            try
            {
                detailRow = GetScanTargetDetailRow(scanResults);
            }
            catch (InvalidOperationException ex)
            {
                DialogHelper.ShowWarning(ex.Message);
                return false;
            }

            _selectedDetailItem = detailRow;
            dgDetail.SelectedItem = detailRow;

            if (!await EnsureDetailPersistedAsync(detailRow))
                return false;

            var receiptRow = CreateScannedReceiptTemplate(detailRow);

            foreach (var result in scanResults)
                ApplyScannedValueToReceipt(receiptRow, result.Configuration, result.Value);

            if (!IsReceiptRowCompleted(receiptRow))
                return false;

            var response = await _asnReceiptService.CreateAsnReceipt(receiptRow.ToRequest());
            Log.Information(
                "Resultado CreateReceiptFromCompletedScanAsync. AsnDetailId: {AsnDetailId}. Success: {IsSuccess}. Code: {Code}. Message: {Message}. Data: {Data}",
                receiptRow.AsnDetailId, response.IsSuccess, response.Code, response.Message, response.Data);

            if (!response.IsSuccess)
            {
                DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo crear la recepción escaneada del ASN.");
                return false;
            }

            if (int.TryParse(response.Data, out var asnReceiptDetailId) && asnReceiptDetailId > 0)
                receiptRow.AsnReceiptDetailId = asnReceiptDetailId;

            ReceiptItems.Add(receiptRow);
            RemoveEmptyReceiptRows();
            dgUbicacionesAsignadas.Items.Refresh();
            return true;
        }

        private async Task<UnmatchedScanResult?> ResolveUnmatchedScanAsync(string scanValue)
        {
            var value = scanValue.Trim();
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (ProductLookupItems.Count == 0)
                await LoadProductsForSelectedClientProjectAsync();

            var product = ProductLookupItems.FirstOrDefault(item =>
                string.Equals(item.Code?.Trim(), value, StringComparison.OrdinalIgnoreCase));

            if (product != null && HasConfiguredScanField(SystemField_e.PartNumber))
            {
                return new UnmatchedScanResult(
                    true,
                    (int)SystemField_e.PartNumber,
                    product.Code?.Trim() ?? value,
                    string.Empty);
            }

            if (!HasConfiguredScanField(SystemField_e.StandardId))
            {
                return new UnmatchedScanResult(
                    false,
                    (int)SystemField_e.PartNumber,
                    value,
                    product != null
                        ? "El numero de parte existe, pero Numero de Parte no esta en la configuracion de escaneo."
                        : "No existe el numero de parte para este cliente y proyecto.");
            }

            var labelResponse = await _standardLabelService.GetByCode(value);
            if (labelResponse.IsSuccess && labelResponse.Data != null)
            {
                var label = labelResponse.Data;
                if (IsStandardLabelAvailableForScan(label))
                {
                    return new UnmatchedScanResult(
                        true,
                        (int)SystemField_e.StandardId,
                        label.StandarIdStr,
                        string.Empty);
                }

                return new UnmatchedScanResult(
                    false,
                    (int)SystemField_e.StandardId,
                    value,
                    "La etiqueta LD ya esta asignada a una recepcion, numero de parte, cliente o proyecto.");
            }

            if (labelResponse.Code != 404)
            {
                return new UnmatchedScanResult(
                    false,
                    (int)SystemField_e.StandardId,
                    value,
                    labelResponse.ErrorMessage ?? labelResponse.Message ?? "No se pudo validar la etiqueta LD.");
            }

            return new UnmatchedScanResult(
                false,
                (int)SystemField_e.PartNumber,
                value,
                "No existe la etiqueta LD ni el numero de parte para este cliente y proyecto.");
        }

        private static bool IsStandardLabelAvailableForScan(StandardLabelDto label)
        {
            return !label.IsAssigned
                && string.IsNullOrWhiteSpace(label.PartNumber)
                && label.ClientId == null
                && label.ProjectId == null;
        }

        private bool HasConfiguredScanField(SystemField_e systemField)
        {
            return _projectScanConfigurations.Any(config =>
                config.SystemFieldId == (int)systemField
                || IsConfiguredScanFieldName(config.SystemFieldName, systemField));
        }

        private static bool IsConfiguredScanFieldName(string? systemFieldName, SystemField_e systemField)
        {
            var fieldName = systemFieldName?.Trim();

            return systemField switch
            {
                SystemField_e.StandardId => string.Equals(fieldName, "standard_id", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(fieldName, "standardid", StringComparison.OrdinalIgnoreCase),
                SystemField_e.PartNumber => string.Equals(fieldName, "part_number", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(fieldName, "partnumber", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(fieldName, "numero de parte", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(fieldName, "número de parte", StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }

        private AsnReceiptItem CreateScannedReceiptTemplate(AsnDetailItem detailRow)
        {
            AsnReceiptItem receiptRow;

            if (dgUbicacionesAsignadas.CurrentItem is AsnReceiptItem selectedReceipt
                && !IsEmptyReceiptRow(selectedReceipt))
            {
                receiptRow = CloneReceiptRow(selectedReceipt);
                receiptRow.AsnReceiptDetailId = 0;
            }
            else
            {
                receiptRow = new AsnReceiptItem();
            }

            receiptRow.ApplyDefaultsFromDetail(detailRow, AsnSelected!.AsnId);
            SyncReceiptRowFromDetail(receiptRow, detailRow, AsnSelected.AsnId);
            ApplyProjectLocationDefaults(receiptRow);

            return receiptRow;
        }

        private AsnDetailItem GetScanTargetDetailRow(IReadOnlyList<ScanRuleResult> scanResults)
        {
            if (IsScanRequiredForProject())
            {
                var scannedPartNumber = GetScannedPartNumber(scanResults);
                if (!string.IsNullOrWhiteSpace(scannedPartNumber))
                    return GetOrCreateDetailRowForScannedPartNumber(scannedPartNumber);
            }

            if (_selectedDetailItem != null && !IsEmptyDetailRow(_selectedDetailItem))
                return _selectedDetailItem;

            if (dgDetail.SelectedItem is AsnDetailItem selectedDetail && !IsEmptyDetailRow(selectedDetail))
                return selectedDetail;

            if (dgDetail.CurrentItem is AsnDetailItem currentDetail && !IsEmptyDetailRow(currentDetail))
                return currentDetail;

            throw new InvalidOperationException("Selecciona una linea de ASN Details antes de escanear.");
        }

        private static string GetScannedPartNumber(IReadOnlyList<ScanRuleResult> scanResults)
        {
            return scanResults
                .FirstOrDefault(result =>
                    string.Equals(NormalizeSystemField(result.Configuration), "partnumber", StringComparison.OrdinalIgnoreCase))?
                .Value?
                .Trim() ?? string.Empty;
        }

        private AsnDetailItem GetOrCreateDetailRowForScannedPartNumber(string partNumber)
        {
            var existingDetail = DetailItems.FirstOrDefault(item =>
                !IsEmptyDetailRow(item)
                && string.Equals(item.PartNumber?.Trim(), partNumber.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existingDetail != null)
                return existingDetail;

            var productLookup = ProductLookupItems.FirstOrDefault(item =>
                string.Equals(item.Code?.Trim(), partNumber.Trim(), StringComparison.OrdinalIgnoreCase));

            if (productLookup?.Data is not ProductAutocompleteDto product)
                throw new InvalidOperationException($"No existe el numero de parte {partNumber} para este cliente y proyecto.");

            var detailRow = DetailItems.FirstOrDefault(IsEmptyDetailRow) ?? new AsnDetailItem();
            if (!DetailItems.Contains(detailRow))
                DetailItems.Add(detailRow);

            detailRow.ProductId = product.ItemId;
            detailRow.PartNumber = product.NumeroParte?.Trim() ?? partNumber.Trim();
            detailRow.Description = product.Descripcion ?? string.Empty;
            detailRow.StandardQuantity = product.StandardPackageValue;
            detailRow.MaximumQuantity = product.MaxUnitValue;

            EnsureTrailingEmptyDetailRow();
            return detailRow;
        }

        private static void ApplyScannedValueToReceipt(AsnReceiptItem receiptRow, ScanConfigurationRequest configuration, string scannedValue)
        {
            switch (NormalizeSystemField(configuration))
            {
                case "lot_number":
                    receiptRow.LotNumber = scannedValue;
                    break;
                case "customer_reference":
                    receiptRow.Reference = scannedValue;
                    break;
                case "purchase_order":
                    receiptRow.PurchaseOrder = scannedValue;
                    break;
                case "customs_declaration":
                    receiptRow.CustomsDeclarationNumber = scannedValue;
                    break;
                case "qty":
                    if (decimal.TryParse(scannedValue, out var quantity))
                        receiptRow.ReceivedQuantity = quantity;
                    break;
                case "standard_id":
                case "standardid":
                    receiptRow.StandardId = scannedValue;
                    break;
                case "part_number":
                case "partnumber":
                case "número de parte":
                case "numero de parte":
                    receiptRow.PartNumber = scannedValue;
                    break;
            }
        }

        private static string NormalizeSystemField(ScanConfigurationRequest configuration)
        {
            var fieldName = configuration.SystemFieldName?.Trim().ToLowerInvariant() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(fieldName))
                return fieldName;

            return configuration.SystemFieldId switch
            {
                1 => "lot_number",
                2 => "customer_reference",
                3 => "purchase_order",
                4 => "customs_declaration",
                5 => "qty",
                6 => "standard_id",
                7 => "partnumber",
                _ => string.Empty
            };
        }

        private Task<ApiResponseDto<string>> CreateASN(AsnRequest request) =>
           _asnService.CreateAsn(request);

        private Task<ApiResponseDto<string>> EditAsn(int asnId, AsnRequest request) =>
            _asnService.UpdateAsn(asnId, request);

        private async Task<ApiResponseDto<string>> SaveAsn(AsnRequest request)
        {
            if (AsnSelected == null && string.IsNullOrWhiteSpace(request.Status))
                request.Status = DefaultAsnStatus;

            return AsnSelected != null
                ? await EditAsn(AsnSelected.AsnId, request)
                : await CreateASN(request);
        }

        private async Task<bool> EnsureAsnPersistedAsync()
        {
            if (!EnsureCurrentAsnEditable())
                return false;

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
            await RefreshAsnHeaderAsync(asnId);
            if (!string.IsNullOrWhiteSpace(saveResult.Data) && !int.TryParse(saveResult.Data, out _))
                AsnSelected.AsnCode = saveResult.Data.Trim();

            UpdateWindowTitle();
            ShowScanSection();
            return true;
        }

        private async Task RefreshAsnHeaderAsync(int asnId)
        {
            if (asnId <= 0)
                return;

            var response = await _asnService.GetAsnById(asnId);
            if (!response.IsSuccess || response.Data == null)
                return;

            AsnSelected ??= new AsnDto();
            AsnSelected.AsnId = response.Data.AsnId;
            AsnSelected.AsnCode = response.Data.AsnCode;
            AsnSelected.Status = response.Data.Status;
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
                ClientId = _clientId,
                ProjectId = _projectId,
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
            RemoveEmptyReceiptRows();
        }

        private void RemoveEmptyReceiptRows()
        {
            var emptyRows = ReceiptItems.Where(IsEmptyReceiptRow).ToList();
            foreach (var emptyRow in emptyRows)
                ReceiptItems.Remove(emptyRow);
        }

        private bool CanDeleteReceiptRow(AsnReceiptItem receiptRow)
        {
            if (receiptRow.AsnDetailId <= 0)
                return true;

            var linkedReceiptsCount = ReceiptItems.Count(item =>
                !ReferenceEquals(item, receiptRow)
                && !IsEmptyReceiptRow(item)
                && item.AsnDetailId == receiptRow.AsnDetailId);

            if (linkedReceiptsCount > 0)
                return true;

            DialogHelper.ShowWarning("No se puede eliminar la recepción porque es el unico registro ligado al detail.");
            return false;
        }

        private static bool IsDetailRowCompleted(AsnDetailItem detailRow)
        {
            return detailRow.ProductId > 0
                && !string.IsNullOrWhiteSpace(detailRow.PartNumber);
        }

        private static bool IsEmptyReceiptRow(AsnReceiptItem item)
        {
            return item.AsnReceiptDetailId <= 0
                && item.AsnDetailId <= 0
                && item.ProductId == null
                && string.IsNullOrWhiteSpace(item.PartNumber)
                && string.IsNullOrWhiteSpace(item.Description)
                && item.StandardId == null
                && item.StandardQuantity == null
                && item.MaximumQuantity == null
                && string.IsNullOrWhiteSpace(item.SD)
                && item.ReceivedQuantity == null
                && string.IsNullOrWhiteSpace(item.Status)
                && item.LocationId == null
                && string.IsNullOrWhiteSpace(item.LocationCode)
                && string.IsNullOrWhiteSpace(item.LotNumber)
                && item.ExpirationDate == null
                && string.IsNullOrWhiteSpace(item.Reference)
                && string.IsNullOrWhiteSpace(item.PurchaseOrder)
                && string.IsNullOrWhiteSpace(item.CustomsDeclarationNumber);
        }

        private bool IsReceiptRowCompleted(AsnReceiptItem receiptRow)
        {
            if (receiptRow.AsnDetailId <= 0)
                return false;

            return !string.IsNullOrWhiteSpace(receiptRow.LocationCode)
                || receiptRow.LocationId.HasValue                
                || receiptRow.StandardQuantity.HasValue
                || receiptRow.MaximumQuantity.HasValue
                || receiptRow.ReceivedQuantity.HasValue
                || !string.IsNullOrWhiteSpace(receiptRow.Status)
                || !string.IsNullOrWhiteSpace(receiptRow.LotNumber)
                || receiptRow.ExpirationDate.HasValue
                || !string.IsNullOrWhiteSpace(receiptRow.Reference)
                || !string.IsNullOrWhiteSpace(receiptRow.PurchaseOrder)
                || !string.IsNullOrWhiteSpace(receiptRow.CustomsDeclarationNumber)
                || !string.IsNullOrWhiteSpace(receiptRow.StandardId);
        }

        private static void SyncReceiptRowFromDetail(AsnReceiptItem receiptRow, AsnDetailItem detailRow, int asnId)
        {
            receiptRow.AsnId = asnId;
            receiptRow.AsnDetailId = detailRow.AsnDetailId;
            receiptRow.ProductId = detailRow.ProductId;
            receiptRow.PartNumber = detailRow.PartNumber;
            receiptRow.Description = detailRow.Description;
            receiptRow.StandardQuantity = detailRow.StandardQuantity;
            receiptRow.MaximumQuantity = detailRow.MaximumQuantity;
            receiptRow.SD = detailRow.SD;
            receiptRow.Status = detailRow.Status;
            receiptRow.LotNumber = detailRow.LotNumber;
            receiptRow.ExpirationDate = detailRow.ExpirationDate;
            receiptRow.Reference = detailRow.CustomerReference;
            receiptRow.PurchaseOrder = detailRow.PurchaseOrder;
            receiptRow.CustomsDeclarationNumber = detailRow.CustomsDeclarationNumber;
        }

        private static void SeedSingleReceiptValuesFromDetail(AsnReceiptItem receiptRow, AsnDetailItem detailRow)
        {
            receiptRow.ReceivedQuantity = detailRow.Quantity;
            receiptRow.LotNumber = detailRow.LotNumber;
            receiptRow.SD = detailRow.SD;
            receiptRow.Status = detailRow.Status;
            receiptRow.ExpirationDate = detailRow.ExpirationDate;
            receiptRow.Reference = detailRow.CustomerReference;
            receiptRow.PurchaseOrder = detailRow.PurchaseOrder;
            receiptRow.CustomsDeclarationNumber = detailRow.CustomsDeclarationNumber;
        }

        private static AsnReceiptItem CloneReceiptRow(AsnReceiptItem source)
        {
            return new AsnReceiptItem
            {
                AsnReceiptDetailId = 0,
                AsnId = source.AsnId,
                AsnDetailId = source.AsnDetailId,
                ProductId = source.ProductId,
                StandardId = source.StandardId,
                PartNumber = source.PartNumber,
                Description = source.Description,
                StandardQuantity = source.StandardQuantity,
                MaximumQuantity = source.MaximumQuantity,
                SD = source.SD,
                ReceivedQuantity = source.ReceivedQuantity,
                Status = source.Status,
                LocationId = source.LocationId,
                LocationCode = source.LocationCode,
                LotNumber = source.LotNumber,
                ExpirationDate = source.ExpirationDate,
                Reference = source.Reference,
                PurchaseOrder = source.PurchaseOrder,
                CustomsDeclarationNumber = source.CustomsDeclarationNumber
            };
        }

        private static List<decimal> BuildSplitQuantities(decimal receivedQuantity, decimal maximumQuantity)
        {
            var quantities = new List<decimal>();
            var remaining = receivedQuantity;

            while (remaining > maximumQuantity)
            {
                quantities.Add(maximumQuantity);
                remaining -= maximumQuantity;
            }

            quantities.Add(remaining);
            return quantities;
        }

        private static string BuildSplitConfirmationMessage(List<decimal> splitQuantities, decimal maximumQuantity)
        {
            if (splitQuantities.Count == 0)
                return "No hay registros para generar.";

            var fullChunks = splitQuantities.Count(x => x == maximumQuantity);
            var remainder = splitQuantities.Last();

            if (splitQuantities.Count == 1)
                return $"¿Está seguro de generar 1 registro de {splitQuantities[0]:0.##}?";

            if (remainder == maximumQuantity)
                return $"¿Está seguro de generar {splitQuantities.Count} registros de {maximumQuantity:0.##}?";

            if (fullChunks <= 0)
                return $"¿Está seguro de generar {splitQuantities.Count} registros?";

            return $"¿Está seguro de generar {splitQuantities.Count} registros: {fullChunks} de {maximumQuantity:0.##} y 1 de {remainder:0.##}?";
        }

        private async Task SplitReceiptRowAsync(AsnReceiptItem receiptRow)
        {
            CommitReceiptGridEdits();

            var receivedQuantity = receiptRow.ReceivedQuantity ?? 0m;
            var maximumQuantity = receiptRow.MaximumQuantity ?? 0m;

            if (maximumQuantity <= 0)
            {
                DialogHelper.ShowError("La cantidad máxima debe ser mayor a cero para dividir el registro.");
                return;
            }

            if (receivedQuantity <= maximumQuantity)
            {
                DialogHelper.ShowError("La cantidad recibida debe ser mayor a la máxima para poder dividir el registro.");
                return;
            }

            var splitQuantities = BuildSplitQuantities(receivedQuantity, maximumQuantity);
            var confirmationMessage = BuildSplitConfirmationMessage(splitQuantities, maximumQuantity);

            var confirmResult = DialogHelper.ShowConfirm(
                confirmationMessage,
                "Confirmar split");

            if (!confirmResult)
                return;

            var currentIndex = ReceiptItems.IndexOf(receiptRow);
            if (currentIndex < 0)
                return;

            receiptRow.ReceivedQuantity = splitQuantities[0];
            await SaveReceiptRowAsync(receiptRow);

            for (var index = 1; index < splitQuantities.Count; index++)
            {
                var newRow = CloneReceiptRow(receiptRow);
                newRow.ReceivedQuantity = splitQuantities[index];
                ReceiptItems.Insert(currentIndex + index, newRow);
                await SaveReceiptRowAsync(newRow);
            }

            dgUbicacionesAsignadas.Items.Refresh();
        }

        private async Task SyncSingleReceiptFromDetailAsync(AsnDetailItem detailRow)
        {
            if (AsnSelected?.AsnId <= 0 || detailRow.AsnDetailId <= 0)
                return;

            var receiptsResponse = await _asnReceiptService.GetAsnReceiptsByAsnDetailId(detailRow.AsnDetailId);
            if (!receiptsResponse.IsSuccess || receiptsResponse.Data == null)
                return;

            var receiptRows = receiptsResponse.Data;
            var shouldSyncReceivedQuantity = receiptRows.Count == 1;

            foreach (var receiptDto in receiptRows)
            {
                var receiptRow = AsnReceiptItem.FromDto(receiptDto);
                receiptRow.AsnId = AsnSelected.AsnId;
                SyncReceiptRowFromDetail(receiptRow, detailRow, AsnSelected.AsnId);

                if (shouldSyncReceivedQuantity)
                    receiptRow.ReceivedQuantity = detailRow.Quantity;

                var response = await _asnReceiptService.UpdateAsnReceipt(receiptRow.AsnReceiptDetailId, receiptRow.ToRequest());
                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo sincronizar la recepción del ASN.");
                    return;
                }
            }

            if (_selectedDetailItem?.AsnDetailId == detailRow.AsnDetailId)
                await LoadReceiptItemsForSelectedDetailAsync();
        }

        private async Task SaveDetailRowAsync(AsnDetailItem detailRow)
        {
            if (!EnsureCurrentAsnEditable())
                return;

            if (!IsDetailRowCompleted(detailRow))
                return;

            if (_savingDetailRows.Contains(detailRow))
            {
                _pendingDetailRows.Add(detailRow);
                return;
            }

            if (!await EnsureAsnPersistedAsync())
                return;

            _savingDetailRows.Add(detailRow);

            try
            {
                detailRow.AsnId = AsnSelected!.AsnId;
                var isNewDetail = detailRow.AsnDetailId <= 0;


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

                if (isNewDetail && detailRow.AsnDetailId > 0)
                    await EnsureInitialReceiptCreatedAsync(detailRow);

                await SyncSingleReceiptFromDetailAsync(detailRow);

                EnsureTrailingEmptyDetailRow();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _savingDetailRows.Remove(detailRow);

                if (_pendingDetailRows.Remove(detailRow))
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _ = SaveDetailRowAsync(detailRow);
                    }), DispatcherPriority.Background);
                }
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
            ApplyProjectLocationDefaults(receiptRow);
        }

        private async Task LoadReceiptItemsForSelectedDetailAsync()
        {
            ReceiptItems.Clear();

            if (_selectedDetailItem == null)
                return;

            if (AsnSelected?.AsnId <= 0 || _selectedDetailItem.AsnDetailId <= 0)
                return;

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

            RemoveEmptyReceiptRows();
        }

        private async Task EnsureInitialReceiptCreatedAsync(AsnDetailItem detailRow)
        {
            if (AsnSelected?.AsnId <= 0 || detailRow.AsnDetailId <= 0)
                return;

            var receiptsResult = await _asnReceiptService.GetAsnReceipts();
            if (!receiptsResult.IsSuccess)
                return;

            var existingReceipt = receiptsResult.Data?.Any(x => x.AsnDetailId == detailRow.AsnDetailId) == true;
            if (existingReceipt)
                return;
            var receiptRows = ReceiptItems
              .Where(item => !IsEmptyReceiptRow(item))
              .ToList();

            var receiptItem = new AsnReceiptItem();
            receiptItem.ApplyDefaultsFromDetail(detailRow, AsnSelected.AsnId);
            SyncReceiptRowFromDetail(receiptItem, detailRow, AsnSelected.AsnId);
            ApplyProjectLocationDefaults(receiptItem);
            if (receiptRows.Count <= 1)
                SeedSingleReceiptValuesFromDetail(receiptItem, detailRow);
           

           
            var createResponse = await _asnReceiptService.CreateAsnReceipt(receiptItem.ToRequest());
            Log.Information(
                "Resultado EnsureInitialReceiptCreatedAsync. AsnDetailId: {AsnDetailId}. Success: {IsSuccess}. Code: {Code}. Message: {Message}. Data: {Data}",
                detailRow.AsnDetailId, createResponse.IsSuccess, createResponse.Code, createResponse.Message, createResponse.Data);

            if (!createResponse.IsSuccess)
            {
                DialogHelper.ShowError(createResponse.ErrorMessage ?? createResponse.Message ?? "No se pudo crear la recepción inicial del ASN.");
                return;
            }

            if (receiptItem.AsnReceiptDetailId <= 0
                && int.TryParse(createResponse.Data, out var asnReceiptDetailId)
                && asnReceiptDetailId > 0)
            {
                receiptItem.AsnReceiptDetailId = asnReceiptDetailId;
            }

            if (ReferenceEquals(_selectedDetailItem, detailRow))
            {
                ReceiptItems.Clear();
                ReceiptItems.Add(receiptItem);
                RemoveEmptyReceiptRows();
            }
        }

        private async Task SaveReceiptRowAsync(AsnReceiptItem receiptRow)
        {
            if (!EnsureCurrentAsnEditable())
                return;

            if (_savingReceiptRows.Contains(receiptRow))
            {
                _pendingReceiptRows.Add(receiptRow);
                return;
            }

            var detailRow = receiptRow.AsnDetailId > 0
                ? DetailItems.FirstOrDefault(x => x.AsnDetailId == receiptRow.AsnDetailId)
                : _selectedDetailItem;

            if (detailRow == null)
                detailRow = _selectedDetailItem;

            if (!await EnsureDetailPersistedAsync(detailRow))
                return;

            receiptRow.ApplyDefaultsFromDetail(detailRow!, AsnSelected!.AsnId);
            ApplyProjectLocationDefaults(receiptRow);
            if (!IsReceiptRowCompleted(receiptRow))
            {
                RemoveEmptyReceiptRows();
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

                RemoveEmptyReceiptRows();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _savingReceiptRows.Remove(receiptRow);

                if (_pendingReceiptRows.Remove(receiptRow))
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _ = SaveReceiptRowAsync(receiptRow);
                    }), DispatcherPriority.Background);
                }
            }
        }

        private async Task SaveDetailsAsync(int asnId)
        {
            if (!EnsureCurrentAsnEditable())
                return;

            var detailRows = DetailItems
                .Where(item => !IsEmptyDetailRow(item))
                .ToList();

            for (var index = 0; index < detailRows.Count; index++)
            {
                var row = detailRows[index];
                var isNewDetail = row.AsnDetailId <= 0;

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

                if (isNewDetail && row.AsnDetailId > 0)
                    await EnsureInitialReceiptCreatedAsync(row);
            }
        }

        private static bool IsEmptyDetailRow(AsnDetailItem item)
        {
            return item.ProductId <= 0
                && string.IsNullOrWhiteSpace(item.PartNumber)
                && string.IsNullOrWhiteSpace(item.Description)
                && item.Quantity <= 0
                && item.StandardQuantity == null
                && item.MaximumQuantity == null
                && string.IsNullOrWhiteSpace(item.Status)
                && string.IsNullOrWhiteSpace(item.SD)
                && string.IsNullOrWhiteSpace(item.LotNumber)
                && item.ExpirationDate == null
                && string.IsNullOrWhiteSpace(item.CustomerReference)
                && item.ExchangeRate == null
                && string.IsNullOrWhiteSpace(item.PurchaseOrder)
                && string.IsNullOrWhiteSpace(item.CustomsDeclarationNumber);
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

        private void DetailRowHeader_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DataGridRowHeader rowHeader || rowHeader.DataContext is not AsnDetailItem detailRow)
                return;

            dgDetail.SelectedItem = detailRow;
            dgDetail.CurrentItem = detailRow;
            _selectedDetailItem = detailRow;
            e.Handled = false;
        }

        private void ReceiptRowHeader_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DataGridRowHeader rowHeader || rowHeader.DataContext is not AsnReceiptItem receiptRow)
                return;

            dgUbicacionesAsignadas.SelectedItem = receiptRow;
            dgUbicacionesAsignadas.CurrentItem = receiptRow;
            e.Handled = false;
        }

        private async void DeleteDetailMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem || menuItem.DataContext is not AsnDetailItem detailRow)
                return;

            try
            {
                if (!EnsureCurrentAsnEditable())
                    return;

                if (!EnsureProjectScanDoesNotLockDetails())
                    return;

                if (detailRow.AsnDetailId > 0)
                {
                    var response = await _asnDetailService.DeleteAsnDetail(detailRow.AsnDetailId);
                    if (!response.IsSuccess)
                    {
                        DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo eliminar la partida.");
                        return;
                    }
                }

                DetailItems.Remove(detailRow);
                EnsureTrailingEmptyDetailRow();
                _selectedDetailItem = DetailItems.FirstOrDefault(item => !IsEmptyDetailRow(item)) ?? DetailItems.FirstOrDefault();
                await LoadReceiptItemsForSelectedDetailAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void DeleteReceiptMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem || menuItem.DataContext is not AsnReceiptItem receiptRow)
                return;

            try
            {
                if (!EnsureCurrentAsnEditable())
                    return;

                if (!EnsureProjectScanAllowsReceiptAction("eliminar recepciones manualmente"))
                    return;

                if (!CanDeleteReceiptRow(receiptRow))
                    return;

                if (receiptRow.AsnReceiptDetailId > 0)
                {
                    var response = await _asnReceiptService.DeleteAsnReceipt(receiptRow.AsnReceiptDetailId);
                    if (!response.IsSuccess)
                    {
                        DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo eliminar la recepción.");
                        return;
                    }
                }

                ReceiptItems.Remove(receiptRow);
                RemoveEmptyReceiptRows();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
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

        private async Task LoadProjectDefaultLocationAsync()
        {
            _defaultProjectLocationId = null;
            _defaultProjectLocationCode = string.Empty;

            if (_projectId <= 0)
                return;

            try
            {
                var response = await _projectService.GetProjectById(_projectId);
                if (!response.IsSuccess || response.Data?.LocationId == null)
                    return;

                _defaultProjectLocationId = response.Data.LocationId;
                _defaultProjectLocationCode = await FindLocationCodeByIdAsync(response.Data.LocationId);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task<string> FindLocationCodeByIdAsync(int? locationId)
        {
            if (locationId == null)
                return string.Empty;

            var targetLocationId = locationId.Value;
            var lookupItem = LocationLookupItems.FirstOrDefault(x =>
            {
                if (x.Id is int intId)
                    return intId == targetLocationId;

                var idText = x.Id?.ToString();
                return int.TryParse(idText, out var parsedId) && parsedId == targetLocationId;
            });

            if (!string.IsNullOrWhiteSpace(lookupItem?.Code))
                return lookupItem.Code;

            var locationsResponse = await _locationService.GetLocations();
            if (!locationsResponse.IsSuccess || locationsResponse.Data == null)
                return string.Empty;

            return locationsResponse.Data
                .FirstOrDefault(x => x.LocationId == targetLocationId)?
                .Ubicacion ?? string.Empty;
        }

        private void ApplyProjectLocationDefaults(AsnReceiptItem receiptRow)
        {
            if (receiptRow.LocationId.HasValue || !string.IsNullOrWhiteSpace(receiptRow.LocationCode))
                return;

            if (_defaultProjectLocationId == null)
                return;

            receiptRow.LocationId = _defaultProjectLocationId;
            receiptRow.LocationCode = _defaultProjectLocationCode;
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

                if (_clientId <= 0 || _projectId <= 0)
                    return;

                var response = await _productService.GetProductByClientId(_clientId, _projectId); // ajusta al método real

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

        private static void ApplyProductLookupDataToDetailRow(InlineLookupEditor editor, AsnDetailItem detailRow)
        {
            if (editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not ProductAutocompleteDto product)
                return;

            detailRow.StandardQuantity = product.StandardPackageValue;
            detailRow.MaximumQuantity = product.MaxUnitValue;
        }

        private async Task<bool> TryOpenNewArticleDialogAsync(InlineLookupEditor editor, AsnDetailItem detailRow)
        {
            var partNumber = (detailRow.PartNumber ?? editor.SelectedCode ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(partNumber) || editor.SelectedLookupItem != null)
                return false;

            if (_clientId <= 0 || _projectId <= 0)
            {
                DialogHelper.ShowWarning("Guarda primero el encabezado del ASN con cliente y proyecto.");
                return true;
            }

            var dialog = _serviceProvider.GetRequiredService<NuevoArticuloView>();
            dialog.Owner = this;
            dialog.SetContext(_clientId, _projectId);
            dialog.SetInitialPartNumber(partNumber);

            var dialogResult = dialog.ShowDialog();
            if (dialogResult != true)
                return true;

            await LoadProductsForSelectedClientProjectAsync();

            var createdLookupItem = ProductLookupItems
                .FirstOrDefault(item => string.Equals(item.Code?.Trim(), partNumber, StringComparison.OrdinalIgnoreCase));

            if (createdLookupItem?.Data is not ProductAutocompleteDto createdProduct)
            {
                DialogHelper.ShowWarning("El artículo se guardó, pero no se pudo recargar automáticamente en el ASN.");
                return true;
            }

            detailRow.ProductId = createdProduct.ItemId;
            detailRow.PartNumber = createdProduct.NumeroParte?.Trim() ?? partNumber;
            detailRow.Description = createdProduct.Descripcion ?? string.Empty;
            detailRow.StandardQuantity = createdProduct.StandardPackageValue;
            detailRow.MaximumQuantity = createdProduct.MaxUnitValue;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _detailGridNavigation.CommitCurrentEdit();
                _detailGridNavigation.MoveFocusToNextCell(detailRow);
                _ = SaveDetailRowAsync(detailRow);
            }), DispatcherPriority.Background);

            return true;
        }

        private async void InlineLookup_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is not InlineLookupEditor editor)
                    return;

                if (editor.DataContext is AsnDetailItem detailRow)
                {
                    if (await TryOpenNewArticleDialogAsync(editor, detailRow))
                        return;

                    ApplyProductLookupDataToDetailRow(editor, detailRow);

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

        private async void SplitReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!EnsureProjectScanAllowsReceiptAction("dividir recepciones manualmente"))
                    return;

                if (sender is not Button button)
                    return;

                if (button.DataContext is not AsnReceiptItem receiptRow)
                    return;

                await SplitReceiptRowAsync(receiptRow);
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
