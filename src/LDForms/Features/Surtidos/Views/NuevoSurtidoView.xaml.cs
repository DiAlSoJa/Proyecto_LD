using LD.Client.Services;
using LD.Contracts.AvailableInventory;
using LD.Contracts.Constants;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Kitting;
using LD.Contracts.Location;
using LD.Contracts.Product;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using LD.FormsX.Model.Lookup;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace LD.FormsX.Features.Surtidos.Views
{
    public partial class NuevoSurtidoView : Window
    {
        private const string DefaultKittingStatus = "Creado";
        private const string DisponibleStatus = "Disponible";
        private const string SurtidoStatus = "Surtido";
        private readonly KittingService _kittingService;
        private readonly KittingDetailService _kittingDetailService;
        private readonly KittingIssueService _kittingIssueService;
        private readonly AvailableInventoryService _availableInventoryService;
        private readonly LookupService _lookupService;
        private readonly ProjectService _projectService;
        private readonly ProductService _productService;
        private readonly InventaryStatusService _inventaryStatusService;
        private readonly LocationService _locationService;

        private readonly HashSet<KittingDetailDto> _savingDetailRows = new();
        private readonly HashSet<KittingIssueDetailDto> _savingIssueRows = new();
        private readonly HashSet<KittingDetailDto> _pendingDetailRows = new();
        private readonly HashSet<KittingIssueDetailDto> _pendingIssueRows = new();
        private readonly Dictionary<int, string> _detailSnapshots = new();
        private readonly DataGridNavigationManager _detailGridNavigation;
        private readonly DataGridNavigationManager _issueGridNavigation;
        private bool _issueGenerationEnabled;

        private KittingDto? _selectedKitting;
        private KittingDetailDto? _selectedDetail;
        private bool _loadingData;
        private int _clientId;
        private int _projectId;
        private string _guideNumber = string.Empty;
        private DateTime? _eta;
        private int? _packagesQty;
        private bool _isReturn;
        private bool _isCustomerMovementRequired;
        private string _kittingCodePreview = string.Empty;
        private string _clientName = string.Empty;
        private string _projectName = string.Empty;

        public bool HasChanges { get; private set; }

        public ObservableCollection<LookupItem> ProductLookupItems { get; } = new();
        public ObservableCollection<LookupItem> StatusLookupItems { get; } = new();
        public ObservableCollection<LookupItem> SdLookupItems { get; } = new();
        public ObservableCollection<LookupItem> LocationLookupItems { get; } = new();

        public ObservableCollection<KittingDetailDto> DetailItems { get; } = new();
        public ObservableCollection<KittingIssueDetailDto> IssueItems { get; } = new();

        public NuevoSurtidoView(
            KittingService kittingService,
            KittingDetailService kittingDetailService,
            KittingIssueService kittingIssueService,
            AvailableInventoryService availableInventoryService,
            LookupService lookupService,
            ProjectService projectService,
            ProductService productService,
            InventaryStatusService inventaryStatusService,
            LocationService locationService)
        {
            InitializeComponent();
            DataContext = this;

            _kittingService = kittingService;
            _kittingDetailService = kittingDetailService;
            _kittingIssueService = kittingIssueService;
            _availableInventoryService = availableInventoryService;
            _lookupService = lookupService;
            _projectService = projectService;
            _productService = productService;
            _inventaryStatusService = inventaryStatusService;
            _locationService = locationService;
            _detailGridNavigation = new DataGridNavigationManager(dgDetail);
            _issueGridNavigation = new DataGridNavigationManager(dgIssue);

            UpdateWindowTitle();
            ApplyEditState();
            HideDetailSections();
        }

        public void SetKitting(KittingDto? kitting)
        {
            _selectedKitting = kitting;
            _clientName = kitting?.Client?.Trim() ?? string.Empty;
            _projectName = kitting?.Project?.Trim() ?? string.Empty;
            _clientId = 0;
            _projectId = 0;
            UpdateWindowTitle();
            ApplyEditState();
            ShowDetailSections();
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
                    await LoadStatusLookupAsync(_clientId, _projectId);
                    await UpdateCodePreviewAsync();
                }), DispatcherPriority.Background);
            }
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            await LoadInitialDataAsync();
        }

        private async Task LoadInitialDataAsync()
        {
            try
            {
                SetLoadingState(true, "Cargando informacion...");

                await LoadSdLookupAsync();
                await LoadLocationLookupAsync();

                if (_selectedKitting != null)
                {
                    await LoadHeaderAsync();
                    ShowDetailSections();
                    await LoadDetailItemsAsync();
                }
                else
                {
                    await LoadProductsForSelectedClientProjectAsync();
                    await LoadStatusLookupAsync(_clientId, _projectId);
                    await UpdateCodePreviewAsync();
                    HideDetailSections();
                }

                ApplyEditState();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                SetLoadingState(false);
                ApplyEditState();
            }
        }

        private async Task LoadHeaderAsync()
        {
            var response = await _kittingService.GetKittingById(_selectedKitting?.KittingId ?? 0);
            if (!response.IsSuccess || response.Data == null)
            {
                DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo cargar el surtido.");
                return;
            }

            var item = response.Data;
            _clientId = item.ClientId;
            _projectId = item.ProjectId;
            _guideNumber = item.GuideNumber ?? string.Empty;
            _eta = item.Eta;
            _packagesQty = item.PackagesQty;
            _isReturn = item.IsReturn;
            _isCustomerMovementRequired = item.IsCustomerMovementRequired;
            _clientName = _selectedKitting?.Client?.Trim() ?? _clientName;
            _projectName = _selectedKitting?.Project?.Trim() ?? _projectName;

            txtNumeroFactura.Text = item.InvoiceNumber ?? string.Empty;
            txtLineaTransporte.Text = item.TransportLine ?? string.Empty;
            txtTipoVehiculo.Text = item.VehicleType ?? string.Empty;
            txtChofer.Text = item.DriverName ?? string.Empty;
            txtPlacasVehiculo.Text = item.VehiclePlate ?? string.Empty;
            txtSelloTransporte.Text = item.SealNumber ?? string.Empty;
            txtContacto.Text = item.Contacto ?? string.Empty;
            txtDireccion.Text = item.Direccion ?? string.Empty;
            txtColonia.Text = item.Colonia ?? string.Empty;
            txtCiudad.Text = item.Ciudad ?? string.Empty;
            txtTelefono.Text = item.Telefono ?? string.Empty;
            txtCodigoPostal.Text = item.CodigoPostal ?? string.Empty;
            SelectDeliveryType(item.TipoEntrega);
            dpFechaProgramada.SelectedDate = item.FechaProgramada;
            UpdateFechaProgramadaVisibility();

            _selectedKitting ??= new KittingDto();
            _selectedKitting.KittingId = item.KittingId;
            _selectedKitting.KittingCode = item.KittingCode ?? string.Empty;
            _selectedKitting.Status = item.Status ?? string.Empty;
            _kittingCodePreview = _selectedKitting.KittingCode;

            await LoadProductsForSelectedClientProjectAsync();
            await LoadStatusLookupAsync(_clientId, _projectId);
            await UpdateCodePreviewAsync();
            UpdateWindowTitle();
        }

        private async Task LoadDetailItemsAsync()
        {
            DetailItems.Clear();
            IssueItems.Clear();
            _detailSnapshots.Clear();
            _selectedDetail = null;
            _issueGenerationEnabled = false;

            if (_selectedKitting?.KittingId <= 0)
                return;

            var result = await _kittingDetailService.GetKittingDetailsByKittingId(_selectedKitting.KittingId);
            if (!result.IsSuccess || result.Data == null)
                return;

            foreach (var item in result.Data)
            {
                var detailItem = CloneDetail(item);
                DetailItems.Add(detailItem);
                RegisterDetailSnapshot(detailItem);
            }

            ApplyEditState();
        }

        private async Task LoadIssueItemsForSelectedDetailAsync()
        {
            await LoadIssueItemsForDetailIdAsync(_selectedDetail?.KittingDetailId ?? 0);
        }

        private async Task LoadIssueItemsForDetailIdAsync(int kittingDetailId)
        {
            IssueItems.Clear();
            _issueGenerationEnabled = false;

            if (kittingDetailId <= 0)
            {
                ApplyEditState();
                return;
            }

            var result = await _kittingIssueService.GetKittingIssuesByKittingDetailId(kittingDetailId);
            if (!result.IsSuccess || result.Data == null)
            {
                ApplyEditState();
                return;
            }

            foreach (var item in result.Data)
                IssueItems.Add(CloneIssue(item));

            _issueGenerationEnabled = IssueItems.Count > 0;
            ApplyEditState();
        }

        private async Task LoadProductsForSelectedClientProjectAsync()
        {
            ProductLookupItems.Clear();

            if (_clientId <= 0 || _projectId <= 0)
                return;

            var response = await _productService.GetProductByClientId(_clientId, _projectId);
            if (!response.IsSuccess || response.Data == null)
                return;

            foreach (var item in response.Data
                .Where(IsProductForCurrentClientProject)
                .OrderBy(x => x.NumeroParte)
                .Select(x => new LookupItem
                {
                    Id = x.ItemId,
                    Code = x.NumeroParte ?? string.Empty,
                    Description = x.Descripcion ?? string.Empty,
                    Data = x
                }))
            {
                ProductLookupItems.Add(item);
            }
        }

        private async Task LoadStatusLookupAsync(int? clientId = null, int? projectId = null)
        {
            clientId = clientId.HasValue && clientId.Value > 0 ? clientId : null;
            projectId = projectId.HasValue && projectId.Value > 0 ? projectId : null;

            StatusLookupItems.Clear();

            var response = await _inventaryStatusService.GetInventaryStatus(clientId, projectId);
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

        private async Task LoadSdLookupAsync()
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

        private async Task LoadLocationLookupAsync()
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

        private void UpdateWindowTitle()
        {
            if (txtTituloVentana == null)
                return;

            var code = _selectedKitting?.KittingCode?.Trim();
            if (string.IsNullOrWhiteSpace(code))
                code = _kittingCodePreview?.Trim();

            var titlePrefix = string.IsNullOrWhiteSpace(code)
                ? "Surtido"
                : $"Surtido {code}";

            if (_loadingData)
                titlePrefix = $"{titlePrefix} (Cargando...)";

            if (string.IsNullOrWhiteSpace(_clientName) && string.IsNullOrWhiteSpace(_projectName))
            {
                txtTituloVentana.Text = titlePrefix;
                return;
            }

            if (string.IsNullOrWhiteSpace(_clientName))
            {
                txtTituloVentana.Text = $"{titlePrefix} - {_projectName}";
                return;
            }

            if (string.IsNullOrWhiteSpace(_projectName))
            {
                txtTituloVentana.Text = $"{titlePrefix} - {_clientName}";
                return;
            }

            txtTituloVentana.Text = $"{titlePrefix} - {_clientName} / {_projectName}";
        }

        private void SetLoadingState(bool isLoading, string? message = null)
        {
            _loadingData = isLoading;

            if (bdLoadingHeader != null)
                bdLoadingHeader.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;

            if (loadingOverlay != null)
                loadingOverlay.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;

            if (!string.IsNullOrWhiteSpace(message))
            {
                if (txtLoadingHeader != null)
                    txtLoadingHeader.Text = message;

                if (txtLoadingBody != null)
                    txtLoadingBody.Text = message;
            }

            UpdateWindowTitle();
            ApplyEditState();
        }

        private static bool IsConfirmedStatus(string? status) =>
            string.Equals(status?.Trim(), KittingStatusNames.Confirmado, StringComparison.OrdinalIgnoreCase) ||
            KittingStatusNames.IsValidation(status) ||
            KittingStatusNames.IsLoading(status);

        private static bool IsCancelledStatus(string? status) =>
            string.Equals(status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase);

        private bool IsCurrentKittingEditable() =>
            !IsConfirmedStatus(_selectedKitting?.Status) && !IsCancelledStatus(_selectedKitting?.Status);

        private bool IsNewKittingRecord() =>
            _selectedKitting is null || _selectedKitting.KittingId <= 0;

        private bool IsPersistedKittingRecord() =>
            _selectedKitting?.KittingId > 0;

        private bool EnsureCurrentKittingEditable()
        {
            if (IsCurrentKittingEditable())
                return true;

            DialogHelper.ShowWarning("El surtido esta en Validación, Cargando, confirmado o cancelado y ya no permite cambios.");
            return false;
        }

        private void ApplyEditState()
        {
            var isEditable = IsCurrentKittingEditable();
            var hasDetailSelection = _selectedDetail != null && !IsEmptyDetailRow(_selectedDetail);
            var canInteract = isEditable && !_loadingData;
            var showSaveButton = canInteract && IsNewKittingRecord();
            var showIssueButtons = canInteract && IsPersistedKittingRecord();

            btnGuardar.Visibility = showSaveButton ? Visibility.Visible : Visibility.Collapsed;
            btnGuardar.IsEnabled = showSaveButton;
            btnGenerarIssueBase.Visibility = showIssueButtons ? Visibility.Visible : Visibility.Collapsed;
            btnGenerarIssueBase.IsEnabled = showIssueButtons && hasDetailSelection;
            btnAutopicking.Visibility = showIssueButtons ? Visibility.Visible : Visibility.Collapsed;
            btnAutopicking.IsEnabled = showIssueButtons;
            btnListaSurtido.Visibility = showIssueButtons ? Visibility.Visible : Visibility.Collapsed;
            btnListaSurtido.IsEnabled = showIssueButtons;
            txtNumeroFactura.IsEnabled = canInteract;
            txtLineaTransporte.IsEnabled = canInteract;
            txtTipoVehiculo.IsEnabled = canInteract;
            txtChofer.IsEnabled = canInteract;
            txtPlacasVehiculo.IsEnabled = canInteract;
            txtSelloTransporte.IsEnabled = canInteract;
            txtContacto.IsEnabled = canInteract;
            txtDireccion.IsEnabled = canInteract;
            txtColonia.IsEnabled = canInteract;
            txtCiudad.IsEnabled = canInteract;
            txtTelefono.IsEnabled = canInteract;
            txtCodigoPostal.IsEnabled = canInteract;
            cbTipoEntrega.IsEnabled = canInteract;
            dpFechaProgramada.IsEnabled = canInteract && IsProgramadaDeliverySelected();

            dgDetail.IsReadOnly = !canInteract;
            dgDetail.CanUserAddRows = canInteract;
            dgIssue.IsReadOnly = !canInteract;
            dgIssue.CanUserAddRows = false;
            dgDetail.IsEnabled = canInteract;
            dgIssue.IsEnabled = canInteract;

            UpdateFechaProgramadaVisibility();
        }

        private void ShowDetailSections()
        {
            if (bdKittingDetails != null)
                bdKittingDetails.Visibility = Visibility.Visible;

            if (bdIssueDetails != null)
                bdIssueDetails.Visibility = Visibility.Visible;
        }

        private void HideDetailSections()
        {
            if (bdKittingDetails != null)
                bdKittingDetails.Visibility = Visibility.Collapsed;

            if (bdIssueDetails != null)
                bdIssueDetails.Visibility = Visibility.Collapsed;
        }

        private async Task UpdateCodePreviewAsync()
        {
            if (_selectedKitting?.KittingId > 0 && !string.IsNullOrWhiteSpace(_selectedKitting.KittingCode))
            {
                _kittingCodePreview = _selectedKitting.KittingCode.Trim();
                UpdateWindowTitle();
                return;
            }

            if (_projectId <= 0)
            {
                _kittingCodePreview = string.Empty;
                UpdateWindowTitle();
                return;
            }

            var response = await _projectService.GetProjectById(_projectId);
            if (!response.IsSuccess || response.Data == null)
            {
                _kittingCodePreview = string.Empty;
                UpdateWindowTitle();
                return;
            }

            if (string.IsNullOrWhiteSpace(response.Data.KittingPrefix))
            {
                _kittingCodePreview = string.Empty;
                UpdateWindowTitle();
                return;
            }

            var nextNumber = 1;
            if (!string.IsNullOrWhiteSpace(response.Data.KittingNumber)
                && int.TryParse(response.Data.KittingNumber, out var parsedNumber)
                && parsedNumber > 0)
            {
                nextNumber = parsedNumber;
            }

            _kittingCodePreview = $"{response.Data.KittingPrefix}{nextNumber:D5}";
            UpdateWindowTitle();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!EnsureCurrentKittingEditable())
                    return;

                btnGuardar.IsEnabled = false;
                CommitGridEdits();
                RemoveEmptyDetailRows();
                RemoveEmptyIssueRows();

                if (!await SaveHeaderAsync(showSuccessToast: true))
                    return;

                ShowDetailSections();

                foreach (var detailRow in DetailItems.Where(item => !IsEmptyDetailRow(item)).ToList())
                    await SaveDetailRowAsync(detailRow);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                ApplyEditState();
            }
        }

        private async Task<bool> SaveHeaderAsync(bool showSuccessToast)
        {
            var request = BuildHeaderRequest();
            if (string.IsNullOrWhiteSpace(txtNumeroFactura.Text))
            {
                DialogHelper.ShowWarning("La factura es obligatoria.");
                return false;
            }

            if (IsProgramadaDeliverySelected() && !dpFechaProgramada.SelectedDate.HasValue)
            {
                DialogHelper.ShowWarning("Captura la fecha programada.");
                return false;
            }

            if (request.ClientId <= 0)
            {
                DialogHelper.ShowWarning("Selecciona un cliente.");
                return false;
            }

            if (request.ProjectId <= 0)
            {
                DialogHelper.ShowWarning("Selecciona un proyecto.");
                return false;
            }

            var result = await SaveHeaderRequestAsync(request);
            if (!result.IsSuccess)
            {
                DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "No se pudo guardar el surtido.");
                return false;
            }

            var kittingId = ResolveSavedKittingId(result);
            _selectedKitting ??= new KittingDto();
            _selectedKitting.KittingId = kittingId;
            if (string.IsNullOrWhiteSpace(_selectedKitting.Status))
                _selectedKitting.Status = DefaultKittingStatus;
            ApplyEditState();
            await RefreshKittingHeaderAsync(kittingId);
            HasChanges = true;

            if (showSuccessToast)
                ToastHelper.ShowSuccess("Validación guardada correctamente.");

            ApplyEditState();
            return true;
        }

        private async Task<bool> EnsureKittingPersistedAsync()
        {
            if (_selectedKitting?.KittingId > 0)
                return true;

            return await SaveHeaderAsync(showSuccessToast: false);
        }

        private async Task RefreshKittingHeaderAsync(int kittingId)
        {
            var response = await _kittingService.GetKittingById(kittingId);
            if (!response.IsSuccess || response.Data == null)
                return;

            _clientId = response.Data.ClientId;
            _projectId = response.Data.ProjectId;

            _selectedKitting ??= new KittingDto();
            _selectedKitting.KittingId = response.Data.KittingId;
            _selectedKitting.KittingCode = response.Data.KittingCode ?? string.Empty;
            _selectedKitting.Status = response.Data.Status ?? string.Empty;
            _selectedKitting.Client = _clientName;
            _selectedKitting.Project = _projectName;
            _kittingCodePreview = _selectedKitting.KittingCode;

            await UpdateCodePreviewAsync();
            UpdateWindowTitle();
        }

        private int ResolveSavedKittingId(ApiResponseDto<string> result)
        {
            if (_selectedKitting?.KittingId > 0)
                return _selectedKitting.KittingId;

            if (int.TryParse(result.Data, out var kittingId) && kittingId > 0)
                return kittingId;

            throw new InvalidOperationException("No se pudo obtener el Id del surtido guardado.");
        }

        private KittingRequest BuildHeaderRequest()
        {
            var tipoEntrega = GetSelectedDeliveryType();
            var fechaProgramada = tipoEntrega == "Programada"
                ? dpFechaProgramada.SelectedDate
                : null;

            return new KittingRequest
            {
                KittingId = _selectedKitting?.KittingId ?? 0,
                ClientId = _clientId,
                ProjectId = _projectId,
                InvoiceNumber = NullIfWhiteSpace(txtNumeroFactura.Text),
                GuideNumber = NullIfWhiteSpace(_guideNumber),
                Eta = _eta,
                PackagesQty = _packagesQty,
                IsReturn = _isReturn,
                IsCustomerMovementRequired = _isCustomerMovementRequired,
                TransportLine = NullIfWhiteSpace(txtLineaTransporte.Text),
                VehicleType = NullIfWhiteSpace(txtTipoVehiculo.Text),
                DriverName = NullIfWhiteSpace(txtChofer.Text),
                VehiclePlate = NullIfWhiteSpace(txtPlacasVehiculo.Text),
                SealNumber = NullIfWhiteSpace(txtSelloTransporte.Text),
                Contacto = NullIfWhiteSpace(txtContacto.Text),
                Direccion = NullIfWhiteSpace(txtDireccion.Text),
                Colonia = NullIfWhiteSpace(txtColonia.Text),
                Ciudad = NullIfWhiteSpace(txtCiudad.Text),
                Telefono = NullIfWhiteSpace(txtTelefono.Text),
                CodigoPostal = NullIfWhiteSpace(txtCodigoPostal.Text),
                TipoEntrega = NullIfWhiteSpace(tipoEntrega),
                FechaProgramada = fechaProgramada,
                Status = GetHeaderStatusForRequest()
            };
        }

        private string? GetHeaderStatusForRequest()
        {
            if (_selectedKitting?.KittingId > 0)
                return _selectedKitting.Status;

            return DefaultKittingStatus;
        }

        private Task<ApiResponseDto<string>> SaveHeaderRequestAsync(KittingRequest request)
        {
            return _selectedKitting?.KittingId > 0
                ? _kittingService.UpdateKitting(_selectedKitting.KittingId, request)
                : _kittingService.CreateKitting(request);
        }

        private void CommitGridEdits()
        {
            _detailGridNavigation.CommitCurrentEdit();
            _issueGridNavigation.CommitCurrentEdit();
        }

        private void dgDetail_CurrentCellChanged(object? sender, EventArgs e)
        {
            try
            {
                if (dgDetail.CurrentCell.Column == null)
                    return;

                if (dgDetail.CurrentCell.Item is not KittingDetailDto currentDetailItem)
                    return;

                if (_selectedDetail == null || _selectedDetail.KittingDetailId != currentDetailItem.KittingDetailId)
                {
                    _selectedDetail = currentDetailItem;
                    _ = LoadIssueItemsForDetailIdAsync(currentDetailItem.KittingDetailId);
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

        private void dgDetail_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            if (e.NewItem is not KittingDetailDto detailRow)
                return;

            detailRow.KittingId = _selectedKitting?.KittingId ?? 0;
            detailRow.Status = string.Empty;
        }

        private void dgIssue_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            if (!_issueGenerationEnabled)
                return;

            if (e.NewItem is not KittingIssueDetailDto issueRow)
                return;

            SeedIssueRowFromSelectedDetail(issueRow);
        }

        private void dgDetail_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.Item is not KittingDetailDto detailRow)
                return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _ = SaveDetailRowAsync(detailRow);
            }), DispatcherPriority.Background);
        }

        private void dgIssue_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.Item is not KittingIssueDetailDto issueRow)
                return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _ = SaveIssueRowAsync(issueRow);
            }), DispatcherPriority.Background);
        }

        private void dgIssue_CurrentCellChanged(object? sender, EventArgs e)
        {
            try
            {
                if (dgIssue.CurrentCell.Column == null)
                    return;

                if (dgIssue.CurrentCell.Item is not KittingIssueDetailDto)
                    return;

                if (dgIssue.CurrentCell.Column.IsReadOnly)
                    return;

                _issueGridNavigation.HandleCurrentCellChanged();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void CbTipoEntrega_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFechaProgramadaVisibility();
        }

        private string GetSelectedDeliveryType()
        {
            if (cbTipoEntrega?.SelectedItem is ComboBoxItem selectedItem)
                return selectedItem.Content?.ToString()?.Trim() ?? string.Empty;

            return string.Empty;
        }

        private bool IsProgramadaDeliverySelected() =>
            string.Equals(GetSelectedDeliveryType(), "Programada", StringComparison.OrdinalIgnoreCase);

        private void SelectDeliveryType(string? tipoEntrega)
        {
            if (cbTipoEntrega == null)
                return;

            var normalized = tipoEntrega?.Trim();
            if (string.IsNullOrWhiteSpace(normalized))
            {
                cbTipoEntrega.SelectedIndex = -1;
                return;
            }

            foreach (var item in cbTipoEntrega.Items.OfType<ComboBoxItem>())
            {
                if (string.Equals(item.Content?.ToString()?.Trim(), normalized, StringComparison.OrdinalIgnoreCase))
                {
                    cbTipoEntrega.SelectedItem = item;
                    return;
                }
            }

            cbTipoEntrega.SelectedIndex = -1;
        }

        private void UpdateFechaProgramadaVisibility()
        {
            if (grFechaProgramada == null)
                return;

            var visible = IsProgramadaDeliverySelected();
            grFechaProgramada.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            if (dpFechaProgramada != null)
                dpFechaProgramada.IsEnabled = (btnGuardar?.IsEnabled ?? true) && visible;

            if (!visible)
                dpFechaProgramada.SelectedDate = null;
        }

        private bool TryMoveToNextDetailRowOrAppend()
        {
            if (dgDetail.CurrentCell.Column == null || dgDetail.CurrentItem is not KittingDetailDto currentRow)
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

            var newRow = new KittingDetailDto();
            DetailItems.Add(newRow);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                dgDetail.ScrollIntoView(newRow);
                _detailGridNavigation.MoveFocusToFirstEditableCell(newRow);
            }), DispatcherPriority.Background);

            return true;
        }

        private bool TryMoveToNextIssueRowOrAppend()
        {
            if (dgIssue.CurrentCell.Column == null || dgIssue.CurrentItem is not KittingIssueDetailDto currentRow)
                return false;

            var lastEditableColumn = _issueGridNavigation.GetLastEditableColumn();
            if (lastEditableColumn == null || !ReferenceEquals(dgIssue.CurrentCell.Column, lastEditableColumn))
                return false;

            var currentIndex = IssueItems.IndexOf(currentRow);
            if (currentIndex < 0)
                return false;

            if (currentIndex < IssueItems.Count - 1)
            {
                var nextRow = IssueItems[currentIndex + 1];
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    _issueGridNavigation.MoveFocusToFirstEditableCell(nextRow);
                }), DispatcherPriority.Background);

                return true;
            }

            if (IsEmptyIssueRow(currentRow) || _selectedDetail == null)
                return false;

            var newRow = new KittingIssueDetailDto();
            SeedIssueRowFromSelectedDetail(newRow);
            IssueItems.Add(newRow);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                dgIssue.ScrollIntoView(newRow);
                _issueGridNavigation.MoveFocusToFirstEditableCell(newRow);
            }), DispatcherPriority.Background);

            return true;
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

                if (dgDetail.CurrentCell == null || dgDetail.CurrentItem is not KittingDetailDto)
                    return;

                var currentColumn = dgDetail.CurrentCell.Column;
                if (currentColumn == null)
                    return;

                if (e.Key == Key.Enter)
                {
                    e.Handled = TryMoveToNextDetailRowOrAppend() || _detailGridNavigation.HandleEnterKeyNavigation();
                    return;
                }

                var header = currentColumn.Header?.ToString() ?? string.Empty;
                if (!header.Equals("Numero de Parte", StringComparison.OrdinalIgnoreCase))
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

        private void dgIssue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.OriginalSource is DependencyObject source &&
                    _issueGridNavigation.IsEventInsideControl<InlineLookupEditor>(source))
                {
                    return;
                }

                if (e.Key != Key.Enter)
                    return;

                if (dgIssue.CurrentCell.Column == null || dgIssue.CurrentItem is not KittingIssueDetailDto)
                    return;

                e.Handled = TryMoveToNextIssueRowOrAppend() || _issueGridNavigation.HandleEnterKeyNavigation();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task SaveDetailRowAsync(KittingDetailDto detailRow)
        {
            if (!EnsureCurrentKittingEditable())
                return;

            if (IsEmptyDetailRow(detailRow))
                return;

            if (!IsDetailRowCompleted(detailRow))
                return;

            if (_savingDetailRows.Contains(detailRow))
            {
                _pendingDetailRows.Add(detailRow);
                return;
            }

            if (!await EnsureKittingPersistedAsync())
                return;

            _savingDetailRows.Add(detailRow);

            try
            {
                detailRow.KittingId = _selectedKitting?.KittingId ?? detailRow.KittingId;

                if (detailRow.KittingDetailId > 0 && !HasDetailChanged(detailRow))
                    return;

                var request = BuildDetailRequest(detailRow);
                var response = detailRow.KittingDetailId > 0
                    ? await _kittingDetailService.UpdateKittingDetail(detailRow.KittingDetailId, request)
                    : await _kittingDetailService.CreateKittingDetail(request);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo guardar la linea del surtido.");
                    return;
                }

                if (detailRow.KittingDetailId <= 0
                    && int.TryParse(response.Data, out var detailId)
                    && detailId > 0)
                {
                    detailRow.KittingDetailId = detailId;
                }

                await RefreshDetailRowFromServerAsync(detailRow);
                RegisterDetailSnapshot(detailRow);
                HasChanges = true;
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

        private async Task<bool> EnsureDetailPersistedAsync(KittingDetailDto? detailRow)
        {
            if (detailRow == null)
                return false;

            if (detailRow.KittingDetailId > 0)
                return true;

            if (!IsDetailRowCompleted(detailRow))
            {
                DialogHelper.ShowWarning("La linea del surtido debe tener producto y cantidad antes de capturar issues.");
                return false;
            }

            // If the row is already in flight, wait for the existing save to finish
            // instead of aborting the issue generation flow.
            if (!_savingDetailRows.Contains(detailRow))
                await SaveDetailRowAsync(detailRow);

            for (var attempt = 0; attempt < 30 && detailRow.KittingDetailId <= 0; attempt++)
            {
                await Task.Delay(100);
                await Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Background);
            }

            return detailRow.KittingDetailId > 0;
        }

        private async Task SaveIssueRowAsync(KittingIssueDetailDto issueRow)
        {
            if (!EnsureCurrentKittingEditable())
                return;

            if (!_issueGenerationEnabled)
                return;

            if (IsEmptyIssueRow(issueRow))
                return;

            var detailRow = ResolveDetailRow(issueRow);
            if (!await EnsureDetailPersistedAsync(detailRow))
                return;

            SeedIssueRowFromDetail(issueRow, detailRow!);
            if (!IsIssueRowCompleted(issueRow))
                return;

            if (_savingIssueRows.Contains(issueRow))
            {
                _pendingIssueRows.Add(issueRow);
                return;
            }

            _savingIssueRows.Add(issueRow);

            try
            {
                var request = BuildIssueRequest(issueRow);
                var response = issueRow.KittingReceiptDetailId > 0
                    ? await _kittingIssueService.UpdateKittingIssue(issueRow.KittingReceiptDetailId, request)
                    : await _kittingIssueService.CreateKittingIssue(request);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo guardar el issue del surtido.");
                    return;
                }

                if (issueRow.KittingReceiptDetailId <= 0
                    && int.TryParse(response.Data, out var issueId)
                    && issueId > 0)
                {
                    issueRow.KittingReceiptDetailId = issueId;
                }

                await RefreshIssueRowFromServerAsync(issueRow);
                HasChanges = true;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _savingIssueRows.Remove(issueRow);

                if (_pendingIssueRows.Remove(issueRow))
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _ = SaveIssueRowAsync(issueRow);
                    }), DispatcherPriority.Background);
                }
            }
        }

        private async Task RefreshDetailRowFromServerAsync(KittingDetailDto detailRow)
        {
            if (detailRow.KittingDetailId <= 0)
                return;

            var response = await _kittingDetailService.GetKittingDetailById(detailRow.KittingDetailId);
            if (!response.IsSuccess || response.Data == null)
                return;

            ApplyDetailRequest(detailRow, response.Data);
            RegisterDetailSnapshot(detailRow);
        }

        private async Task RefreshIssueRowFromServerAsync(KittingIssueDetailDto issueRow)
        {
            if (issueRow.KittingReceiptDetailId <= 0)
                return;

            var response = await _kittingIssueService.GetKittingIssueById(issueRow.KittingReceiptDetailId);
            if (!response.IsSuccess || response.Data == null)
                return;

            ApplyIssueRequest(issueRow, response.Data);
        }

        private async Task EnsureInitialIssueCreatedAsync(KittingDetailDto detailRow)
        {
            if (detailRow.KittingDetailId <= 0)
                return;

            var issuesResult = await _kittingIssueService.GetKittingIssuesByKittingDetailId(detailRow.KittingDetailId);
            if (!issuesResult.IsSuccess)
                return;

            if (issuesResult.Data?.Any() == true)
                return;

            var request = BuildBaseIssueRequest(detailRow);
            var createResponse = await _kittingIssueService.CreateKittingIssue(request);
            if (!createResponse.IsSuccess)
            {
                DialogHelper.ShowError(createResponse.ErrorMessage ?? createResponse.Message ?? "No se pudo crear el issue base del surtido.");
                return;
            }

            if (_selectedDetail?.KittingDetailId == detailRow.KittingDetailId)
                await LoadIssueItemsForSelectedDetailAsync();
        }

        private async void BtnGenerarIssueBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!EnsureCurrentKittingEditable())
                    return;

                CommitGridEdits();
                RemoveEmptyDetailRows();
                RemoveEmptyIssueRows();

                if (_selectedDetail == null)
                {
                    DialogHelper.ShowWarning("Selecciona una linea para generar su issue base.");
                    return;
                }

                if (!await EnsureDetailPersistedAsync(_selectedDetail))
                    return;

                var dialog = new SurtirInventarioDialog(
                    _availableInventoryService,
                    _selectedDetail.PartNumber,
                    _clientName,
                    _projectName,
                    _clientId,
                    _projectId)
                {
                    Owner = Window.GetWindow(this)
                };

                if (dialog.ShowDialog() != true)
                    return;

                var selectedInventories = dialog.SelectedInventories.ToList();
                if (selectedInventories.Count == 0)
                {
                    DialogHelper.ShowWarning("Selecciona al menos un registro de inventario.");
                    return;
                }

                ShowDetailSections();
                _issueGenerationEnabled = true;

                var pendingRows = BuildIssueRowsFromInventories(selectedInventories);
                if (pendingRows.Count == 0)
                {
                    DialogHelper.ShowWarning("Los registros seleccionados ya estaban surtidos.");
                    await LoadIssueItemsForSelectedDetailAsync();
                    return;
                }

                var createdRows = new List<KittingIssueDetailDto>();
                foreach (var issueRow in pendingRows)
                {
                    if (await CreateIssueFromInventorySelectionAsync(issueRow))
                    {
                        createdRows.Add(issueRow);
                        IssueItems.Add(issueRow);
                    }
                }

                if (createdRows.Count == 0)
                {
                    DialogHelper.ShowWarning("No se pudo insertar ninguna linea en issue.");
                    await LoadIssueItemsForSelectedDetailAsync();
                    return;
                }

                await LoadIssueItemsForSelectedDetailAsync();

                ToastHelper.ShowSuccess($"{createdRows.Count} linea(s) agregada(s) a issue.");
                HasChanges = true;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var firstNewRow = IssueItems.FirstOrDefault(x =>
                        createdRows.Any(created =>
                            created.KittingReceiptDetailId > 0 &&
                            created.KittingReceiptDetailId == x.KittingReceiptDetailId));

                    if (firstNewRow == null)
                        return;

                    dgIssue.ScrollIntoView(firstNewRow);
                    dgIssue.SelectedItem = firstNewRow;
                    dgIssue.CurrentItem = firstNewRow;
                    dgIssue.Focus();
                    _issueGridNavigation.MoveFocusToFirstEditableCell(firstNewRow);
                }), DispatcherPriority.Background);

            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private bool IsProductForCurrentClientProject(ProductAutocompleteDto product)
        {
            if (_clientId > 0 && product.ClientId.HasValue && product.ClientId.Value != _clientId)
                return false;

            if (_projectId > 0 && product.ProjectId.HasValue && product.ProjectId.Value != _projectId)
                return false;

            return true;
        }

        private async void BtnAutopicking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!EnsureCurrentKittingEditable())
                    return;

                CommitGridEdits();
                RemoveEmptyDetailRows();
                RemoveEmptyIssueRows();

                var detailRows = DetailItems
                    .Where(IsDetailRowCompleted)
                    .ToList();

                if (detailRows.Count == 0)
                {
                    DialogHelper.ShowWarning("No hay lineas validas para autopicking.");
                    return;
                }

                ShowDetailSections();
                _issueGenerationEnabled = true;

                var createdRows = new List<KittingIssueDetailDto>();
                foreach (var detailRow in detailRows)
                {
                    if (!await EnsureDetailPersistedAsync(detailRow))
                        continue;

                    var autoPickedInventories = await BuildAutoPickedInventoriesAsync(detailRow);
                    var pendingRows = BuildIssueRowsFromInventories(autoPickedInventories);

                    if (pendingRows.Count == 0)
                        continue;

                    foreach (var issueRow in pendingRows)
                    {
                        if (await CreateIssueFromInventorySelectionAsync(issueRow))
                        {
                            createdRows.Add(issueRow);
                            IssueItems.Add(issueRow);
                        }
                    }
                }

                if (createdRows.Count == 0)
                {
                    DialogHelper.ShowWarning("No se pudo generar autopicking.");
                    await LoadIssueItemsForSelectedDetailAsync();
                    return;
                }

                await LoadIssueItemsForSelectedDetailAsync();
                ToastHelper.ShowSuccess($"{createdRows.Count} linea(s) agregada(s) a issue.");
                HasChanges = true;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnListaSurtido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedKitting?.KittingId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un surtido guardado para imprimir la lista de surtido.");
                    return;
                }

                CommitGridEdits();
                RemoveEmptyDetailRows();
                RemoveEmptyIssueRows();

                var rows = await BuildListaSurtidoRowsAsync();
                if (rows.Count == 0)
                {
                    DialogHelper.ShowWarning("No hay lineas surtidas para imprimir.");
                    return;
                }

                ListaSurtidoPrinter.Print(_selectedKitting, rows);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task<bool> CreateIssueFromInventorySelectionAsync(KittingIssueDetailDto issueRow)
        {
            if (!EnsureCurrentKittingEditable())
                return false;

            var detailRow = ResolveDetailRow(issueRow);
            if (!await EnsureDetailPersistedAsync(detailRow))
                return false;

            SeedIssueRowFromDetail(issueRow, detailRow!);
            if (!IsIssueRowCompleted(issueRow))
                return false;

            try
            {
                var request = BuildIssueRequest(issueRow);
                var response = await _kittingIssueService.CreateKittingIssue(request);
                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo guardar el issue del surtido.");
                    return false;
                }

                if (int.TryParse(response.Data, out var issueId) && issueId > 0)
                    issueRow.KittingReceiptDetailId = issueId;

                await RefreshIssueRowFromServerAsync(issueRow);
                HasChanges = true;
                return true;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
                return false;
            }
        }

        private async void InlineLookup_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is not InlineLookupEditor editor)
                    return;

                if (editor.DataContext is KittingDetailDto detailRow)
                {
                    ApplyProductLookupToDetailRow(editor, detailRow);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _detailGridNavigation.CommitCurrentEdit();
                        _detailGridNavigation.MoveFocusToNextCell(detailRow);
                        _ = SaveDetailRowAsync(detailRow);
                    }), DispatcherPriority.Background);
                    return;
                }

                if (editor.DataContext is KittingIssueDetailDto issueRow)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _issueGridNavigation.CommitCurrentEdit();
                        _issueGridNavigation.MoveFocusToNextCell(issueRow);
                        _ = SaveIssueRowAsync(issueRow);
                    }), DispatcherPriority.Background);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private static void ApplyProductLookupToDetailRow(InlineLookupEditor editor, KittingDetailDto detailRow)
        {
            if (editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not ProductAutocompleteDto product)
                return;

            detailRow.ProductId = product.ItemId;
            detailRow.PartNumber = product.NumeroParte?.Trim() ?? detailRow.PartNumber;
            detailRow.Description = product.Descripcion ?? detailRow.Description;
            detailRow.StandardQuantity = product.StandardPackageValue;
            detailRow.MaximumQuantity = product.MaxUnitValue;
        }

        private void SeedIssueRowFromSelectedDetail(KittingIssueDetailDto issueRow)
        {
            if (_selectedDetail == null)
                return;

            SeedIssueRowFromDetail(issueRow, _selectedDetail);
            issueRow.KittingReceiptDetailId = 0;
        }

        private static void SeedIssueRowFromDetail(KittingIssueDetailDto issueRow, KittingDetailDto detailRow)
        {
            issueRow.KittingDetailId = detailRow.KittingDetailId;
            issueRow.ProductId = detailRow.ProductId;
            issueRow.PartNumber = detailRow.PartNumber;
            issueRow.Description = detailRow.Description;
            issueRow.StandardQuantity = detailRow.StandardQuantity;
            issueRow.MaximumQuantity = detailRow.MaximumQuantity;
            if (string.IsNullOrWhiteSpace(issueRow.SD))
                issueRow.SD = detailRow.SD ?? string.Empty;
            issueRow.ReceivedQuantity ??= detailRow.Quantity;
            issueRow.Status ??= detailRow.Status;
            issueRow.LotNumber ??= detailRow.LotNumber;
            issueRow.ExpirationDate ??= detailRow.ExpirationDate;
            issueRow.Reference ??= detailRow.CustomerReference;
            issueRow.PurchaseOrder ??= detailRow.PurchaseOrder;
            issueRow.CustomsDeclarationNumber ??= detailRow.CustomsDeclarationNumber;
        }

        private KittingDetailDto? ResolveDetailRow(KittingIssueDetailDto issueRow)
        {
            if (issueRow.KittingDetailId > 0)
            {
                var existing = DetailItems.FirstOrDefault(x => x.KittingDetailId == issueRow.KittingDetailId);
                if (existing != null)
                    return existing;
            }

            return _selectedDetail;
        }

        private static KittingDetailRequest BuildDetailRequest(KittingDetailDto detailRow)
        {
            return new KittingDetailRequest
            {
                KittingDetailId = detailRow.KittingDetailId,
                KittingId = detailRow.KittingId,
                ProductId = detailRow.ProductId ?? 0,
                PartNumber = detailRow.PartNumber ?? string.Empty,
                Description = NullIfWhiteSpace(detailRow.Description),
                Quantity = detailRow.Quantity,
                Status = GetDetailStatus(detailRow),
                SD = NullIfWhiteSpace(detailRow.SD),
                LotNumber = NullIfWhiteSpace(detailRow.LotNumber),
                ExpirationDate = detailRow.ExpirationDate,
                CustomerReference = NullIfWhiteSpace(detailRow.CustomerReference),
                ExchangeRate = detailRow.ExchangeRate,
                PurchaseOrder = NullIfWhiteSpace(detailRow.PurchaseOrder),
                CustomsDeclarationNumber = NullIfWhiteSpace(detailRow.CustomsDeclarationNumber),
                StandardQuantity = detailRow.StandardQuantity,
                MaximumQuantity = detailRow.MaximumQuantity
            };
        }

        private static KittingIssueRequest BuildIssueRequest(KittingIssueDetailDto issueRow)
        {
            return new KittingIssueRequest
            {
                KittingReceiptDetailId = issueRow.KittingReceiptDetailId,
                KittingDetailId = issueRow.KittingDetailId,
                ProductId = issueRow.ProductId,
                StandardId = NullIfWhiteSpace(issueRow.StandardId),
                PartNumber = issueRow.PartNumber ?? string.Empty,
                Description = NullIfWhiteSpace(issueRow.Description),
                StandardQuantity = issueRow.StandardQuantity,
                MaximumQuantity = issueRow.MaximumQuantity,
                SD = NullIfWhiteSpace(issueRow.SD),
                ReceivedQuantity = issueRow.ReceivedQuantity,
                Status = NullIfWhiteSpace(issueRow.Status),
                SupplyStatus = NullIfWhiteSpace(issueRow.SupplyStatus),
                LocationId = issueRow.LocationId,
                LocationCode = NullIfWhiteSpace(issueRow.LocationCode),
                LotNumber = NullIfWhiteSpace(issueRow.LotNumber),
                ExpirationDate = issueRow.ExpirationDate,
                Reference = NullIfWhiteSpace(issueRow.Reference),
                PurchaseOrder = NullIfWhiteSpace(issueRow.PurchaseOrder),
                CustomsDeclarationNumber = NullIfWhiteSpace(issueRow.CustomsDeclarationNumber)
            };
        }

        private static KittingIssueRequest BuildBaseIssueRequest(KittingDetailDto detailRow)
        {
            return new KittingIssueRequest
            {
                KittingDetailId = detailRow.KittingDetailId,
                ProductId = detailRow.ProductId,
                PartNumber = detailRow.PartNumber ?? string.Empty,
                Description = NullIfWhiteSpace(detailRow.Description),
                StandardQuantity = detailRow.StandardQuantity,
                MaximumQuantity = detailRow.MaximumQuantity,
                SD = NullIfWhiteSpace(detailRow.SD),
                ReceivedQuantity = detailRow.Quantity,
                Status = NullIfWhiteSpace(detailRow.Status),
                SupplyStatus = NullIfWhiteSpace(detailRow.Status),
                LotNumber = NullIfWhiteSpace(detailRow.LotNumber),
                ExpirationDate = detailRow.ExpirationDate,
                Reference = NullIfWhiteSpace(detailRow.CustomerReference),
                PurchaseOrder = NullIfWhiteSpace(detailRow.PurchaseOrder),
                CustomsDeclarationNumber = NullIfWhiteSpace(detailRow.CustomsDeclarationNumber)
            };
        }

        private static void ApplyDetailRequest(KittingDetailDto detailRow, KittingDetailRequest request)
        {
            detailRow.KittingDetailId = request.KittingDetailId;
            detailRow.KittingId = request.KittingId;
            detailRow.ProductId = request.ProductId;
            detailRow.PartNumber = request.PartNumber ?? string.Empty;
            detailRow.Description = request.Description ?? string.Empty;
            detailRow.Quantity = request.Quantity;
            detailRow.Status = request.Status ?? string.Empty;
            detailRow.SD = request.SD ?? string.Empty;
            detailRow.LotNumber = request.LotNumber ?? string.Empty;
            detailRow.ExpirationDate = request.ExpirationDate;
            detailRow.CustomerReference = request.CustomerReference ?? string.Empty;
            detailRow.ExchangeRate = request.ExchangeRate;
            detailRow.PurchaseOrder = request.PurchaseOrder ?? string.Empty;
            detailRow.CustomsDeclarationNumber = request.CustomsDeclarationNumber ?? string.Empty;
            detailRow.StandardQuantity = request.StandardQuantity;
            detailRow.MaximumQuantity = request.MaximumQuantity;
        }

        private static void ApplyIssueRequest(KittingIssueDetailDto issueRow, KittingIssueRequest request)
        {
            issueRow.KittingReceiptDetailId = request.KittingReceiptDetailId;
            issueRow.KittingDetailId = request.KittingDetailId;
            issueRow.ProductId = request.ProductId;
            issueRow.StandardId = request.StandardId ?? string.Empty;
            issueRow.StandardIdStr = request.StandardId ?? string.Empty;
            issueRow.PartNumber = request.PartNumber ?? string.Empty;
            issueRow.Description = request.Description ?? string.Empty;
            issueRow.StandardQuantity = request.StandardQuantity;
            issueRow.MaximumQuantity = request.MaximumQuantity;
            issueRow.SD = request.SD ?? string.Empty;
            issueRow.ReceivedQuantity = request.ReceivedQuantity;
            issueRow.Status = request.Status ?? string.Empty;
            issueRow.SupplyStatus = request.SupplyStatus ?? string.Empty;
            issueRow.LocationId = request.LocationId;
            issueRow.LocationCode = request.LocationCode ?? string.Empty;
            issueRow.LotNumber = request.LotNumber ?? string.Empty;
            issueRow.ExpirationDate = request.ExpirationDate;
            issueRow.Reference = request.Reference ?? string.Empty;
            issueRow.PurchaseOrder = request.PurchaseOrder ?? string.Empty;
            issueRow.CustomsDeclarationNumber = request.CustomsDeclarationNumber ?? string.Empty;
        }

        private List<KittingIssueDetailDto> BuildIssueRowsFromInventories(IEnumerable<AvailableInventoryDto> inventories)
        {
            var pendingRows = new List<KittingIssueDetailDto>();

            foreach (var inventory in inventories)
            {
                var standardId = GetInventoryStandardId(inventory);
                if (string.IsNullOrWhiteSpace(standardId))
                    continue;

                if (IssueItems.Any(existing => IssueMatchesInventory(existing, inventory, standardId)))
                    continue;

                var issueRow = CreateIssueRowFromInventory(inventory, standardId);
                pendingRows.Add(issueRow);
            }

            return pendingRows;
        }

        private KittingIssueDetailDto CreateIssueRowFromInventory(AvailableInventoryDto inventory, string standardId)
        {
            return new KittingIssueDetailDto
            {
                KittingDetailId = _selectedDetail?.KittingDetailId ?? 0,
                ProductId = inventory.ProductId ?? _selectedDetail?.ProductId,
                StandardId = standardId,
                StandardIdStr = standardId,
                PartNumber = inventory.PartNumber?.Trim() ?? _selectedDetail?.PartNumber ?? string.Empty,
                Description = inventory.Description?.Trim() ?? _selectedDetail?.Description ?? string.Empty,
                StandardQuantity = _selectedDetail?.StandardQuantity,
                MaximumQuantity = _selectedDetail?.MaximumQuantity,
                SD = _selectedDetail?.SD ?? string.Empty,
                ReceivedQuantity = inventory.FinalAvailable > 0 ? inventory.FinalAvailable : inventory.Qty,
                Status = string.IsNullOrWhiteSpace(inventory.StatusId)
                    ? (_selectedDetail?.Status ?? DefaultKittingStatus)
                    : inventory.StatusId.Trim(),
                SupplyStatus = string.IsNullOrWhiteSpace(inventory.AvailableStatus)
                    ? (string.IsNullOrWhiteSpace(inventory.StatusId)
                        ? (_selectedDetail?.Status ?? DefaultKittingStatus)
                        : inventory.StatusId.Trim())
                    : inventory.AvailableStatus.Trim(),
                LocationId = inventory.LocationId,
                LocationCode = inventory.Ubicacion?.Trim() ?? string.Empty,
                LotNumber = inventory.LotNumber?.Trim() ?? string.Empty,
                ExpirationDate = inventory.ExpirationDate,
                Reference = inventory.Reference?.Trim() ?? string.Empty,
                PurchaseOrder = inventory.PurchaseOrder?.Trim() ?? string.Empty,
                CustomsDeclarationNumber = inventory.CustomsDeclarationNumber?.Trim() ?? string.Empty
            };
        }

        private static bool IssueMatchesInventory(KittingIssueDetailDto issueRow, AvailableInventoryDto inventory, string standardId)
        {
            return string.Equals(issueRow.StandardId?.Trim(), standardId, StringComparison.OrdinalIgnoreCase)
                && string.Equals(issueRow.PartNumber?.Trim(), inventory.PartNumber?.Trim() ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                && string.Equals(issueRow.LotNumber?.Trim(), inventory.LotNumber?.Trim() ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                && string.Equals(issueRow.Reference?.Trim(), inventory.Reference?.Trim() ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                && string.Equals(issueRow.PurchaseOrder?.Trim(), inventory.PurchaseOrder?.Trim() ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                && string.Equals(issueRow.CustomsDeclarationNumber?.Trim(), inventory.CustomsDeclarationNumber?.Trim() ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                && issueRow.LocationId == inventory.LocationId;
        }

        private static string? GetInventoryStandardId(AvailableInventoryDto inventory)
        {
            if (inventory.StandardId.HasValue && inventory.StandardId.Value > 0)
                return inventory.StandardId.Value.ToString(CultureInfo.InvariantCulture);

            return string.IsNullOrWhiteSpace(inventory.StandardIdStr)
                ? null
                : inventory.StandardIdStr.Trim();
        }

        private static bool IsDisponibleInventory(AvailableInventoryDto inventory)
        {
            var availableStatus = inventory.AvailableStatus?.Trim();
            if (!string.IsNullOrWhiteSpace(availableStatus))
                return string.Equals(availableStatus, DisponibleStatus, StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(availableStatus, SurtidoStatus, StringComparison.OrdinalIgnoreCase);

            var statusId = inventory.StatusId?.Trim();
            return string.Equals(statusId, DisponibleStatus, StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(statusId, SurtidoStatus, StringComparison.OrdinalIgnoreCase);
        }

        private async Task<List<AvailableInventoryDto>> BuildAutoPickedInventoriesAsync(KittingDetailDto detailRow)
        {
            var response = await _availableInventoryService.GetAvailableInventories();
            if (!response.IsSuccess || response.Data == null)
            {
                DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo cargar el inventario para autopicking.");
                return new List<AvailableInventoryDto>();
            }

            var partNumber = detailRow.PartNumber?.Trim() ?? string.Empty;
            var status = detailRow.Status?.Trim() ?? string.Empty;
            var lotNumber = detailRow.LotNumber?.Trim() ?? string.Empty;
            var requiredQuantity = detailRow.Quantity - IssueItems
                .Where(item => item.KittingDetailId == detailRow.KittingDetailId)
                .Sum(item => item.ReceivedQuantity.GetValueOrDefault());

            if (requiredQuantity <= 0)
                return new List<AvailableInventoryDto>();

            return response.Data
                .Where(IsInventoryForCurrentClientProject)
                .Where(inventory => inventory.FinalAvailable > 0 || inventory.Qty.GetValueOrDefault() > 0)
                .Where(IsDisponibleInventory)
                .Where(inventory => string.Equals(inventory.PartNumber?.Trim(), partNumber, StringComparison.OrdinalIgnoreCase))
                .Where(inventory => string.IsNullOrWhiteSpace(status) ||
                                   string.Equals(inventory.StatusId?.Trim(), status, StringComparison.OrdinalIgnoreCase))
                .Where(inventory => string.IsNullOrWhiteSpace(lotNumber) ||
                                   string.Equals(inventory.LotNumber?.Trim(), lotNumber, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(inventory => inventory.FinalAvailable > 0 ? inventory.FinalAvailable : inventory.Qty)
                .ThenByDescending(inventory => inventory.Fecha)
                .ThenByDescending(inventory => inventory.Hora)
                .TakeWhile((_, index) =>
                {
                    var accumulated = response.Data
                        .Where(IsInventoryForCurrentClientProject)
                        .Where(inventory => inventory.FinalAvailable > 0 || inventory.Qty.GetValueOrDefault() > 0)
                        .Where(IsDisponibleInventory)
                        .Where(inventory => string.Equals(inventory.PartNumber?.Trim(), partNumber, StringComparison.OrdinalIgnoreCase))
                        .Where(inventory => string.IsNullOrWhiteSpace(status) ||
                                           string.Equals(inventory.StatusId?.Trim(), status, StringComparison.OrdinalIgnoreCase))
                        .Where(inventory => string.IsNullOrWhiteSpace(lotNumber) ||
                                           string.Equals(inventory.LotNumber?.Trim(), lotNumber, StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(inventory => inventory.FinalAvailable > 0 ? inventory.FinalAvailable : inventory.Qty)
                        .ThenByDescending(inventory => inventory.Fecha)
                        .ThenByDescending(inventory => inventory.Hora)
                        .Take(index + 1)
                        .Sum(inventory => inventory.FinalAvailable > 0 ? inventory.FinalAvailable : inventory.Qty.GetValueOrDefault());

                    return accumulated <= requiredQuantity;
                })
                .ToList();
        }

        private bool IsInventoryForCurrentClientProject(AvailableInventoryDto inventory)
        {
            if (_clientId > 0 && inventory.ClientId != _clientId)
                return false;

            if (_projectId > 0 && inventory.ProjectId != _projectId)
                return false;

            return true;
        }

        private async Task<List<ListaSurtidoPrinter.ListaSurtidoRow>> BuildListaSurtidoRowsAsync()
        {
            var rows = new List<ListaSurtidoPrinter.ListaSurtidoRow>();
            var detailRows = DetailItems.Where(IsDetailRowCompleted).ToList();

            if (detailRows.Count == 0 && _selectedDetail != null && IsDetailRowCompleted(_selectedDetail))
                detailRows.Add(_selectedDetail);

            foreach (var detailRow in detailRows)
            {
                IReadOnlyList<KittingIssueDetailDto> issueRows;
                if (_selectedDetail != null && _selectedDetail.KittingDetailId == detailRow.KittingDetailId)
                {
                    issueRows = IssueItems.ToList();
                }
                else
                {
                    var response = await _kittingIssueService.GetKittingIssuesByKittingDetailId(detailRow.KittingDetailId);
                    if (!response.IsSuccess || response.Data == null)
                        continue;

                    issueRows = response.Data.Select(CloneIssue).ToList();
                }

                foreach (var issueRow in issueRows.Where(IsIssueRowCompleted))
                {
                    rows.Add(new ListaSurtidoPrinter.ListaSurtidoRow
                    {
                        PartNumber = issueRow.PartNumber?.Trim() ?? string.Empty,
                        Description = issueRow.Description?.Trim() ?? string.Empty,
                        Quantity = issueRow.ReceivedQuantity.GetValueOrDefault(),
                        Status = issueRow.Status?.Trim() ?? string.Empty,
                        LotNumber = issueRow.LotNumber?.Trim() ?? string.Empty,
                        SD = issueRow.SD?.Trim() ?? string.Empty,
                        LocationCode = issueRow.LocationCode?.Trim() ?? string.Empty
                    });
                }
            }

            return rows;
        }

        private static bool IsDetailRowCompleted(KittingDetailDto detailRow)
        {
            return detailRow.ProductId.GetValueOrDefault() > 0
                && !string.IsNullOrWhiteSpace(detailRow.PartNumber)
                && detailRow.Quantity > 0;
        }

        private static bool IsIssueRowCompleted(KittingIssueDetailDto issueRow)
        {
            return issueRow.KittingDetailId > 0
                && !string.IsNullOrWhiteSpace(issueRow.PartNumber)
                && issueRow.ReceivedQuantity.GetValueOrDefault() > 0;
        }

        private static bool IsEmptyDetailRow(KittingDetailDto detailRow)
        {
            return detailRow.KittingDetailId <= 0
                && detailRow.KittingId <= 0
                && detailRow.ProductId == null
                && string.IsNullOrWhiteSpace(detailRow.PartNumber)
                && string.IsNullOrWhiteSpace(detailRow.Description)
                && detailRow.Quantity <= 0
                && string.IsNullOrWhiteSpace(detailRow.Status)
                && string.IsNullOrWhiteSpace(detailRow.SD)
                && string.IsNullOrWhiteSpace(detailRow.LotNumber)
                && detailRow.ExpirationDate == null
                && string.IsNullOrWhiteSpace(detailRow.CustomerReference)
                && detailRow.ExchangeRate == null
                && string.IsNullOrWhiteSpace(detailRow.PurchaseOrder)
                && string.IsNullOrWhiteSpace(detailRow.CustomsDeclarationNumber)
                && detailRow.StandardQuantity == null
                && detailRow.MaximumQuantity == null;
        }

        private static bool IsEmptyIssueRow(KittingIssueDetailDto issueRow)
        {
            return issueRow.KittingReceiptDetailId <= 0
                && issueRow.KittingDetailId <= 0
                && issueRow.ProductId == null
                && string.IsNullOrWhiteSpace(issueRow.StandardId)
                && string.IsNullOrWhiteSpace(issueRow.PartNumber)
                && string.IsNullOrWhiteSpace(issueRow.Description)
                && issueRow.StandardQuantity == null
                && issueRow.MaximumQuantity == null
                && string.IsNullOrWhiteSpace(issueRow.SD)
                && issueRow.ReceivedQuantity == null
                && string.IsNullOrWhiteSpace(issueRow.Status)
                && issueRow.LocationId == null
                && string.IsNullOrWhiteSpace(issueRow.LocationCode)
                && string.IsNullOrWhiteSpace(issueRow.LotNumber)
                && issueRow.ExpirationDate == null
                && string.IsNullOrWhiteSpace(issueRow.Reference)
                && string.IsNullOrWhiteSpace(issueRow.PurchaseOrder)
                && string.IsNullOrWhiteSpace(issueRow.CustomsDeclarationNumber);
        }

        private void RemoveEmptyDetailRows()
        {
            foreach (var detailRow in DetailItems.Where(IsEmptyDetailRow).ToList())
                DetailItems.Remove(detailRow);
        }

        private void RemoveEmptyIssueRows()
        {
            foreach (var issueRow in IssueItems.Where(IsEmptyIssueRow).ToList())
                IssueItems.Remove(issueRow);
        }

        private async void DeleteDetailMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem || menuItem.DataContext is not KittingDetailDto detailRow)
                return;

            try
            {
                if (!EnsureCurrentKittingEditable())
                    return;

                var confirm = DialogHelper.ShowConfirm(
                    $"Deseas eliminar la linea {detailRow.PartNumber}?",
                    "Eliminar detail");

                if (!confirm)
                    return;

                if (detailRow.KittingDetailId > 0)
                {
                    var response = await _kittingDetailService.DeleteKittingDetail(detailRow.KittingDetailId);
                    if (!response.IsSuccess)
                    {
                        DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo eliminar la linea.");
                        return;
                    }
                }

                DetailItems.Remove(detailRow);
                if (ReferenceEquals(_selectedDetail, detailRow))
                {
                    _selectedDetail = null;
                    IssueItems.Clear();
                }

                if (detailRow.KittingDetailId > 0)
                    _detailSnapshots.Remove(detailRow.KittingDetailId);

                HasChanges = true;
                ApplyEditState();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void DeleteIssueMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem || menuItem.DataContext is not KittingIssueDetailDto issueRow)
                return;

            try
            {
                if (!EnsureCurrentKittingEditable())
                    return;

                var confirm = DialogHelper.ShowConfirm(
                    $"Deseas eliminar el issue de {issueRow.PartNumber}?",
                    "Eliminar issue");

                if (!confirm)
                    return;

                if (issueRow.KittingReceiptDetailId > 0)
                {
                    var response = await _kittingIssueService.DeleteKittingIssue(issueRow.KittingReceiptDetailId);
                    if (!response.IsSuccess)
                    {
                        DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo eliminar el issue.");
                        return;
                    }
                }

                IssueItems.Remove(issueRow);
                HasChanges = true;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void DetailRowHeader_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DataGridRowHeader rowHeader || rowHeader.DataContext is not KittingDetailDto detailRow)
                return;

            dgDetail.SelectedItem = detailRow;
            dgDetail.CurrentItem = detailRow;
            _selectedDetail = detailRow;
            e.Handled = false;
        }

        private void IssueRowHeader_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DataGridRowHeader rowHeader || rowHeader.DataContext is not KittingIssueDetailDto issueRow)
                return;

            dgIssue.SelectedItem = issueRow;
            dgIssue.CurrentItem = issueRow;
            e.Handled = false;
        }

        private static KittingDetailDto CloneDetail(KittingDetailDto source)
        {
            return new KittingDetailDto
            {
                KittingDetailId = source.KittingDetailId,
                KittingId = source.KittingId,
                ProductId = source.ProductId,
                PartNumber = source.PartNumber,
                Description = source.Description,
                Quantity = source.Quantity,
                Status = source.Status,
                SD = source.SD,
                LotNumber = source.LotNumber,
                ExpirationDate = source.ExpirationDate,
                CustomerReference = source.CustomerReference,
                ExchangeRate = source.ExchangeRate,
                PurchaseOrder = source.PurchaseOrder,
                CustomsDeclarationNumber = source.CustomsDeclarationNumber,
                StandardQuantity = source.StandardQuantity,
                MaximumQuantity = source.MaximumQuantity
            };
        }

        private void RegisterDetailSnapshot(KittingDetailDto detailRow)
        {
            if (detailRow.KittingDetailId <= 0)
                return;

            _detailSnapshots[detailRow.KittingDetailId] = BuildDetailSnapshot(detailRow);
        }

        private bool HasDetailChanged(KittingDetailDto detailRow)
        {
            if (detailRow.KittingDetailId <= 0)
                return true;

            if (!_detailSnapshots.TryGetValue(detailRow.KittingDetailId, out var previousSnapshot))
                return true;

            return !string.Equals(previousSnapshot, BuildDetailSnapshot(detailRow), StringComparison.Ordinal);
        }

        private static string BuildDetailSnapshot(KittingDetailDto detailRow)
        {
            var request = BuildDetailRequest(detailRow);

            static string Safe(string? value) => value?.Trim() ?? string.Empty;

            return string.Join("|", new[]
            {
                request.KittingId.ToString(CultureInfo.InvariantCulture),
                request.ProductId.ToString(CultureInfo.InvariantCulture),
                Safe(request.PartNumber),
                Safe(request.Description),
                request.Quantity.ToString(CultureInfo.InvariantCulture),
                Safe(request.Status),
                Safe(request.SD),
                Safe(request.LotNumber),
                request.ExpirationDate?.ToString("O", CultureInfo.InvariantCulture) ?? string.Empty,
                Safe(request.CustomerReference),
                request.ExchangeRate?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                Safe(request.PurchaseOrder),
                Safe(request.CustomsDeclarationNumber),
                request.StandardQuantity?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                request.MaximumQuantity?.ToString(CultureInfo.InvariantCulture) ?? string.Empty
            });
        }

        private static KittingIssueDetailDto CloneIssue(KittingIssueDetailDto source)
        {
            return new KittingIssueDetailDto
            {
                KittingReceiptDetailId = source.KittingReceiptDetailId,
                KittingDetailId = source.KittingDetailId,
                ProductId = source.ProductId,
                StandardId = source.StandardId,
                StandardIdStr = source.StandardIdStr,
                PartNumber = source.PartNumber,
                Description = source.Description,
                StandardQuantity = source.StandardQuantity,
                MaximumQuantity = source.MaximumQuantity,
                SD = source.SD,
                ReceivedQuantity = source.ReceivedQuantity,
                Status = source.Status,
                SupplyStatus = source.SupplyStatus,
                LocationCode = source.LocationCode,
                LocationId = source.LocationId,
                LotNumber = source.LotNumber,
                ExpirationDate = source.ExpirationDate,
                Reference = source.Reference,
                PurchaseOrder = source.PurchaseOrder,
                CustomsDeclarationNumber = source.CustomsDeclarationNumber
            };
        }

        private static int GetSelectedId(ComboBox combo)
        {
            return int.TryParse(combo.SelectedValue?.ToString(), out var id) ? id : 0;
        }

        private static string? NullIfWhiteSpace(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static string? GetDetailStatus(KittingDetailDto detailRow)
        {
            if (!string.IsNullOrWhiteSpace(detailRow.Status))
                return detailRow.Status.Trim();

            return null;
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

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            btnMaximizarVentana.Content = WindowState == WindowState.Maximized ? "[ ]" : "[]";
        }

        private void ToggleWindowState()
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }
    }
}
