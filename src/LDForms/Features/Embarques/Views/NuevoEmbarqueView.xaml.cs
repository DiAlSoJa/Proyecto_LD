using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Kitting;
using LD.Contracts.Location;
using LD.Contracts.Product;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using LD.FormsX.Model.Lookup;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace LD.FormsX.Features.Embarques.Views
{
    public partial class NuevoEmbarqueView : Window
    {
        private readonly KittingService _kittingService;
        private readonly KittingDetailService _kittingDetailService;
        private readonly KittingIssueService _kittingIssueService;
        private readonly LookupService _lookupService;
        private readonly ProjectService _projectService;
        private readonly ProductService _productService;
        private readonly InventaryStatusService _inventaryStatusService;
        private readonly LocationService _locationService;

        private readonly HashSet<KittingDetailDto> _savingDetailRows = new();
        private readonly HashSet<KittingIssueDetailDto> _savingIssueRows = new();
        private readonly HashSet<KittingDetailDto> _pendingDetailRows = new();
        private readonly HashSet<KittingIssueDetailDto> _pendingIssueRows = new();

        private KittingDto? _selectedKitting;
        private KittingDetailDto? _selectedDetail;
        private bool _loadingData;
        private int _clientId;
        private int _projectId;
        private string _clientName = string.Empty;
        private string _projectName = string.Empty;

        public bool HasChanges { get; private set; }

        public ObservableCollection<LookupItem> ProductLookupItems { get; } = new();
        public ObservableCollection<LookupItem> StatusLookupItems { get; } = new();
        public ObservableCollection<LookupItem> SdLookupItems { get; } = new();
        public ObservableCollection<LookupItem> LocationLookupItems { get; } = new();

        public ObservableCollection<KittingDetailDto> DetailItems { get; } = new();
        public ObservableCollection<KittingIssueDetailDto> IssueItems { get; } = new();

        public NuevoEmbarqueView(
            KittingService kittingService,
            KittingDetailService kittingDetailService,
            KittingIssueService kittingIssueService,
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
            _lookupService = lookupService;
            _projectService = projectService;
            _productService = productService;
            _inventaryStatusService = inventaryStatusService;
            _locationService = locationService;

            UpdateWindowTitle();
            ApplyEditState();
        }

        public void SetKitting(KittingDto? kitting)
        {
            _selectedKitting = kitting;
            _clientName = kitting?.Client?.Trim() ?? string.Empty;
            _projectName = kitting?.Project?.Trim() ?? string.Empty;
            UpdateWindowTitle();
            ApplyEditState();
        }

        public void SetClientProjectContext(int clientId, int projectId, string? clientName, string? projectName)
        {
            _clientId = clientId;
            _projectId = projectId;
            _clientName = clientName?.Trim() ?? string.Empty;
            _projectName = projectName?.Trim() ?? string.Empty;
            UpdateWindowTitle();
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
                _loadingData = true;

                await LoadStatusLookupAsync();
                await LoadSdLookupAsync();
                await LoadLocationLookupAsync();
                await LoadClientsAsync();

                if (_selectedKitting != null)
                {
                    await LoadHeaderAsync();
                    await LoadDetailItemsAsync();
                }
                else
                {
                    if (_clientId > 0)
                    {
                        cmbCliente.SelectedValue = _clientId.ToString();
                        await LoadProjectsAsync(_projectId > 0 ? _projectId.ToString() : null);

                        if (_projectId > 0)
                            cmbProyecto.SelectedValue = _projectId.ToString();
                    }

                    await LoadProductsForSelectedClientProjectAsync();
                    await UpdateCodePreviewAsync();
                }

                ApplyEditState();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _loadingData = false;
            }
        }

        private async Task LoadClientsAsync()
        {
            var response = await _lookupService.GetClientLookup();
            if (!response.IsSuccess)
            {
                DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudieron cargar los clientes.");
                return;
            }

            cmbCliente.ItemsSource = response.Data ?? new List<DropDownDto>();
        }

        private async Task LoadProjectsAsync(string? selectedProjectId = null)
        {
            var clientId = GetSelectedId(cmbCliente);
            if (clientId <= 0)
            {
                cmbProyecto.ItemsSource = null;
                _projectId = 0;
                _projectName = string.Empty;
                return;
            }

            var response = await _lookupService.GetProjectClientLookup(clientId);
            if (!response.IsSuccess)
            {
                cmbProyecto.ItemsSource = null;
                DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudieron cargar los proyectos.");
                return;
            }

            cmbProyecto.ItemsSource = response.Data ?? new List<DropDownDto>();

            if (!string.IsNullOrWhiteSpace(selectedProjectId))
            {
                cmbProyecto.SelectedValue = selectedProjectId;
            }
            else if (_projectId > 0)
            {
                cmbProyecto.SelectedValue = _projectId.ToString();
            }
            else
            {
                cmbProyecto.SelectedIndex = -1;
            }
        }

        private async Task LoadHeaderAsync()
        {
            var response = await _kittingService.GetKittingById(_selectedKitting?.KittingId ?? 0);
            if (!response.IsSuccess || response.Data == null)
            {
                DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo cargar el embarque.");
                return;
            }

            var item = response.Data;
            _clientId = item.ClientId;
            _projectId = item.ProjectId;

            cmbCliente.SelectedValue = item.ClientId.ToString();
            await LoadProjectsAsync(item.ProjectId.ToString());
            cmbProyecto.SelectedValue = item.ProjectId.ToString();

            txtNumeroFactura.Text = item.InvoiceNumber ?? string.Empty;
            txtNumeroGuia.Text = item.GuideNumber ?? string.Empty;
            dpEta.SelectedDate = item.Eta;
            txtBultos.Text = item.PackagesQty?.ToString() ?? string.Empty;
            chkEsDevolucion.IsChecked = item.IsReturn;
            chkMovimientoRequeridoCliente.IsChecked = item.IsCustomerMovementRequired;
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
            txtTipoEntrega.Text = item.TipoEntrega ?? string.Empty;
            dpFechaProgramada.SelectedDate = item.FechaProgramada;

            _selectedKitting ??= new KittingDto();
            _selectedKitting.KittingId = item.KittingId;
            _selectedKitting.KittingCode = item.KittingCode ?? string.Empty;
            _selectedKitting.Status = item.Status ?? string.Empty;

            await LoadProductsForSelectedClientProjectAsync();
            await UpdateCodePreviewAsync();
            UpdateWindowTitle();
        }

        private async Task LoadDetailItemsAsync()
        {
            DetailItems.Clear();
            IssueItems.Clear();
            _selectedDetail = null;

            if (_selectedKitting?.KittingId <= 0)
                return;

            var result = await _kittingDetailService.GetKittingDetailsByKittingId(_selectedKitting.KittingId);
            if (!result.IsSuccess || result.Data == null)
                return;

            foreach (var item in result.Data)
                DetailItems.Add(CloneDetail(item));

            ApplyEditState();
        }

        private async Task LoadIssueItemsForSelectedDetailAsync()
        {
            IssueItems.Clear();

            if (_selectedDetail?.KittingDetailId <= 0)
            {
                ApplyEditState();
                return;
            }

            var result = await _kittingIssueService.GetKittingIssuesByKittingDetailId(_selectedDetail.KittingDetailId);
            if (!result.IsSuccess || result.Data == null)
            {
                ApplyEditState();
                return;
            }

            foreach (var item in result.Data)
                IssueItems.Add(CloneIssue(item));

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

        private async Task LoadStatusLookupAsync()
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
            var titlePrefix = string.IsNullOrWhiteSpace(code)
                ? "Embarque"
                : $"Embarque {code}";

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

        private static bool IsConfirmedStatus(string? status) =>
            string.Equals(status?.Trim(), "Confirmado", StringComparison.OrdinalIgnoreCase);

        private static bool IsCancelledStatus(string? status) =>
            string.Equals(status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase);

        private bool IsCurrentKittingEditable() =>
            !IsConfirmedStatus(_selectedKitting?.Status) && !IsCancelledStatus(_selectedKitting?.Status);

        private bool EnsureCurrentKittingEditable()
        {
            if (IsCurrentKittingEditable())
                return true;

            DialogHelper.ShowWarning("El embarque esta confirmado o cancelado y ya no permite cambios.");
            return false;
        }

        private void ApplyEditState()
        {
            var isEditable = IsCurrentKittingEditable();
            var hasDetailSelection = _selectedDetail != null && !IsEmptyDetailRow(_selectedDetail);

            btnGuardar.IsEnabled = isEditable;
            btnGenerarIssueBase.IsEnabled = isEditable && hasDetailSelection;

            cmbCliente.IsEnabled = isEditable;
            cmbProyecto.IsEnabled = isEditable;
            txtNumeroFactura.IsEnabled = isEditable;
            txtNumeroGuia.IsEnabled = isEditable;
            dpEta.IsEnabled = isEditable;
            txtBultos.IsEnabled = isEditable;
            chkEsDevolucion.IsEnabled = isEditable;
            chkMovimientoRequeridoCliente.IsEnabled = isEditable;
            txtLineaTransporte.IsEnabled = isEditable;
            txtTipoVehiculo.IsEnabled = isEditable;
            txtChofer.IsEnabled = isEditable;
            txtPlacasVehiculo.IsEnabled = isEditable;
            txtSelloTransporte.IsEnabled = isEditable;
            txtContacto.IsEnabled = isEditable;
            txtDireccion.IsEnabled = isEditable;
            txtColonia.IsEnabled = isEditable;
            txtCiudad.IsEnabled = isEditable;
            txtTelefono.IsEnabled = isEditable;
            txtCodigoPostal.IsEnabled = isEditable;
            txtTipoEntrega.IsEnabled = isEditable;
            dpFechaProgramada.IsEnabled = isEditable;

            dgDetail.IsReadOnly = !isEditable;
            dgDetail.CanUserAddRows = isEditable;
            dgIssue.IsReadOnly = !isEditable;
            dgIssue.CanUserAddRows = isEditable && hasDetailSelection;
        }

        private async Task UpdateCodePreviewAsync()
        {
            if (_selectedKitting?.KittingId > 0 && !string.IsNullOrWhiteSpace(_selectedKitting.KittingCode))
            {
                txtCodigoPreview.Text = $"Codigo actual: {_selectedKitting.KittingCode}";
                txtCodigoPreview.Foreground = Brushes.DarkGreen;
                return;
            }

            var projectId = GetSelectedId(cmbProyecto);
            if (projectId <= 0)
            {
                txtCodigoPreview.Text = "Selecciona un proyecto para generar el codigo.";
                txtCodigoPreview.Foreground = Brushes.DarkGreen;
                return;
            }

            var response = await _projectService.GetProjectById(projectId);
            if (!response.IsSuccess || response.Data == null)
            {
                txtCodigoPreview.Text = response.Message ?? "No se pudo cargar el proyecto.";
                txtCodigoPreview.Foreground = Brushes.Firebrick;
                return;
            }

            if (string.IsNullOrWhiteSpace(response.Data.KittingPrefix))
            {
                txtCodigoPreview.Text = "El proyecto no tiene configurado KittingPrefix.";
                txtCodigoPreview.Foreground = Brushes.Firebrick;
                return;
            }

            var nextNumber = 1;
            if (!string.IsNullOrWhiteSpace(response.Data.KittingNumber)
                && int.TryParse(response.Data.KittingNumber, out var parsedNumber)
                && parsedNumber > 0)
            {
                nextNumber = parsedNumber;
            }

            txtCodigoPreview.Text = $"Codigo sugerido: {response.Data.KittingPrefix}{nextNumber:D5}";
            txtCodigoPreview.Foreground = Brushes.DarkGreen;
        }

        private async void cmbCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_loadingData)
                return;

            try
            {
                _loadingData = true;
                _clientId = GetSelectedId(cmbCliente);
                _clientName = (cmbCliente.SelectedItem as DropDownDto)?.Value?.Trim() ?? string.Empty;
                _projectId = 0;
                _projectName = string.Empty;
                await LoadProjectsAsync();
                await LoadProductsForSelectedClientProjectAsync();
                await UpdateCodePreviewAsync();
                UpdateWindowTitle();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _loadingData = false;
            }
        }

        private async void cmbProyecto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_loadingData)
                return;

            try
            {
                _projectId = GetSelectedId(cmbProyecto);
                _projectName = (cmbProyecto.SelectedItem as DropDownDto)?.Value?.Trim() ?? string.Empty;
                await LoadProductsForSelectedClientProjectAsync();
                await UpdateCodePreviewAsync();
                UpdateWindowTitle();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
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

                foreach (var detailRow in DetailItems.Where(item => !IsEmptyDetailRow(item)).ToList())
                    await SaveDetailRowAsync(detailRow);

                foreach (var issueRow in IssueItems.Where(item => !IsEmptyIssueRow(item)).ToList())
                    await SaveIssueRowAsync(issueRow);

                await LoadDetailItemsAsync();
                await LoadIssueItemsForSelectedDetailAsync();
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
                DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "No se pudo guardar el embarque.");
                return false;
            }

            var kittingId = ResolveSavedKittingId(result);
            await RefreshKittingHeaderAsync(kittingId);
            HasChanges = true;

            if (showSuccessToast)
                ToastHelper.ShowSuccess("Embarque guardado correctamente.");

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

            await UpdateCodePreviewAsync();
            UpdateWindowTitle();
        }

        private int ResolveSavedKittingId(ApiResponseDto<string> result)
        {
            if (_selectedKitting?.KittingId > 0)
                return _selectedKitting.KittingId;

            if (int.TryParse(result.Data, out var kittingId) && kittingId > 0)
                return kittingId;

            throw new InvalidOperationException("No se pudo obtener el Id del embarque guardado.");
        }

        private KittingRequest BuildHeaderRequest()
        {
            _clientId = GetSelectedId(cmbCliente);
            _projectId = GetSelectedId(cmbProyecto);
            _clientName = (cmbCliente.SelectedItem as DropDownDto)?.Value?.Trim() ?? _clientName;
            _projectName = (cmbProyecto.SelectedItem as DropDownDto)?.Value?.Trim() ?? _projectName;

            return new KittingRequest
            {
                KittingId = _selectedKitting?.KittingId ?? 0,
                ClientId = _clientId,
                ProjectId = _projectId,
                InvoiceNumber = NullIfWhiteSpace(txtNumeroFactura.Text),
                GuideNumber = NullIfWhiteSpace(txtNumeroGuia.Text),
                Eta = dpEta.SelectedDate,
                PackagesQty = int.TryParse(txtBultos.Text, out var packages) ? packages : null,
                IsReturn = chkEsDevolucion.IsChecked == true,
                IsCustomerMovementRequired = chkMovimientoRequeridoCliente.IsChecked == true,
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
                TipoEntrega = NullIfWhiteSpace(txtTipoEntrega.Text),
                FechaProgramada = dpFechaProgramada.SelectedDate,
                Status = _selectedKitting?.Status
            };
        }

        private Task<ApiResponseDto<string>> SaveHeaderRequestAsync(KittingRequest request)
        {
            return _selectedKitting?.KittingId > 0
                ? _kittingService.UpdateKitting(_selectedKitting.KittingId, request)
                : _kittingService.CreateKitting(request);
        }

        private void CommitGridEdits()
        {
            dgDetail.CommitEdit(DataGridEditingUnit.Cell, true);
            dgDetail.CommitEdit(DataGridEditingUnit.Row, true);
            dgIssue.CommitEdit(DataGridEditingUnit.Cell, true);
            dgIssue.CommitEdit(DataGridEditingUnit.Row, true);
        }

        private async void dgDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedDetail = dgDetail.SelectedItem as KittingDetailDto;
                await LoadIssueItemsForSelectedDetailAsync();
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
        }

        private void dgIssue_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
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
                var request = BuildDetailRequest(detailRow);
                var response = detailRow.KittingDetailId > 0
                    ? await _kittingDetailService.UpdateKittingDetail(detailRow.KittingDetailId, request)
                    : await _kittingDetailService.CreateKittingDetail(request);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo guardar la linea del embarque.");
                    return;
                }

                if (detailRow.KittingDetailId <= 0
                    && int.TryParse(response.Data, out var detailId)
                    && detailId > 0)
                {
                    detailRow.KittingDetailId = detailId;
                }

                await RefreshDetailRowFromServerAsync(detailRow);
                await EnsureInitialIssueCreatedAsync(detailRow);
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
                DialogHelper.ShowWarning("La linea del embarque debe tener producto y cantidad antes de capturar issues.");
                return false;
            }

            await SaveDetailRowAsync(detailRow);
            return detailRow.KittingDetailId > 0;
        }

        private async Task SaveIssueRowAsync(KittingIssueDetailDto issueRow)
        {
            if (!EnsureCurrentKittingEditable())
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
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo guardar el issue del embarque.");
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
                DialogHelper.ShowError(createResponse.ErrorMessage ?? createResponse.Message ?? "No se pudo crear el issue base del embarque.");
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

                if (_selectedDetail == null)
                {
                    DialogHelper.ShowWarning("Selecciona una linea para generar su issue base.");
                    return;
                }

                if (!await EnsureDetailPersistedAsync(_selectedDetail))
                    return;

                var currentIssues = await _kittingIssueService.GetKittingIssuesByKittingDetailId(_selectedDetail.KittingDetailId);
                if (currentIssues.IsSuccess && currentIssues.Data?.Any() == true)
                {
                    DialogHelper.ShowWarning("La linea seleccionada ya tiene issue details.");
                    await LoadIssueItemsForSelectedDetailAsync();
                    return;
                }

                await EnsureInitialIssueCreatedAsync(_selectedDetail);
                await LoadIssueItemsForSelectedDetailAsync();
                ToastHelper.ShowSuccess("Issue base generado correctamente.");
                HasChanges = true;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
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
                        _ = SaveDetailRowAsync(detailRow);
                    }), DispatcherPriority.Background);
                    return;
                }

                if (editor.DataContext is KittingIssueDetailDto issueRow)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
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
            issueRow.SD ??= detailRow.SD;
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
                Status = NullIfWhiteSpace(detailRow.Status),
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
            issueRow.PartNumber = request.PartNumber ?? string.Empty;
            issueRow.Description = request.Description ?? string.Empty;
            issueRow.StandardQuantity = request.StandardQuantity;
            issueRow.MaximumQuantity = request.MaximumQuantity;
            issueRow.SD = request.SD ?? string.Empty;
            issueRow.ReceivedQuantity = request.ReceivedQuantity;
            issueRow.Status = request.Status ?? string.Empty;
            issueRow.LocationId = request.LocationId;
            issueRow.LocationCode = request.LocationCode ?? string.Empty;
            issueRow.LotNumber = request.LotNumber ?? string.Empty;
            issueRow.ExpirationDate = request.ExpirationDate;
            issueRow.Reference = request.Reference ?? string.Empty;
            issueRow.PurchaseOrder = request.PurchaseOrder ?? string.Empty;
            issueRow.CustomsDeclarationNumber = request.CustomsDeclarationNumber ?? string.Empty;
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

        private static KittingIssueDetailDto CloneIssue(KittingIssueDetailDto source)
        {
            return new KittingIssueDetailDto
            {
                KittingReceiptDetailId = source.KittingReceiptDetailId,
                KittingDetailId = source.KittingDetailId,
                ProductId = source.ProductId,
                StandardId = source.StandardId,
                PartNumber = source.PartNumber,
                Description = source.Description,
                StandardQuantity = source.StandardQuantity,
                MaximumQuantity = source.MaximumQuantity,
                SD = source.SD,
                ReceivedQuantity = source.ReceivedQuantity,
                Status = source.Status,
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
