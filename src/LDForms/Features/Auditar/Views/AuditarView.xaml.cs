using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.DeliveryOrder;
using LD.Contracts.DTOs.LoadMapping;
using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.Auditar
{
    /// <summary>
    /// Lógica de interacción para AuditarView.xaml
    /// </summary>
    public partial class AuditarView : UserControl
    {
        private const string CorrectSoundFileName = "correcto.mp3";
        private const string ErrorSoundFileName = "error.mp3";
        private const string ScanSideLeft = "Izquierda";
        private const string ScanSideRight = "Derecha";

        private static readonly Brush SuccessMessageBackground = new SolidColorBrush(Color.FromRgb(240, 253, 244));
        private static readonly Brush SuccessMessageForeground = new SolidColorBrush(Color.FromRgb(22, 101, 52));
        private static readonly Brush SuccessMessageBorder = new SolidColorBrush(Color.FromRgb(34, 197, 94));
        private static readonly Brush ErrorMessageBackground = new SolidColorBrush(Color.FromRgb(254, 242, 242));
        private static readonly Brush ErrorMessageForeground = new SolidColorBrush(Color.FromRgb(153, 27, 27));
        private static readonly Brush ErrorMessageBorder = new SolidColorBrush(Color.FromRgb(239, 68, 68));
        private static readonly Brush SuccessCardBackground = new SolidColorBrush(Color.FromRgb(240, 253, 244));
        private static readonly Brush SuccessCardBorder = new SolidColorBrush(Color.FromRgb(34, 197, 94));
        private static readonly Brush SuccessCardTitle = new SolidColorBrush(Color.FromRgb(15, 23, 42));
        private static readonly Brush SuccessCardText = new SolidColorBrush(Color.FromRgb(71, 85, 105));
        private static readonly Brush ErrorCardBackground = new SolidColorBrush(Color.FromRgb(254, 226, 226));
        private static readonly Brush ErrorCardBorder = new SolidColorBrush(Color.FromRgb(220, 38, 38));
        private static readonly Brush ErrorCardTitle = new SolidColorBrush(Color.FromRgb(127, 29, 29));
        private static readonly Brush ErrorCardText = new SolidColorBrush(Color.FromRgb(153, 27, 27));

        private readonly IServiceProvider _serviceProvider;
        private readonly LoadMappingService _loadMappingService;
        private readonly DeliveryOrderService _deliveryOrderService;
        private readonly KittingService _kittingService;
        private readonly KittingDetailService _kittingDetailService;
        private readonly KittingIssueService _kittingIssueService;
        private readonly List<MediaPlayer> _activePlayers = [];
        private int _orderLoadVersion;
        private int _selectedLoadMappingId;
        private bool _isProcessingScan;
        private bool _isRemovingScanCard;

        public ObservableCollection<LocationCardPreview> LeftCards { get; } = new();
        public ObservableCollection<LocationCardPreview> RightCards { get; } = new();
        public ObservableCollection<LoadMappingDto> MapeoCargaItems { get; } = new();
        public ObservableCollection<KittingDto> KittingItems { get; } = new();
        public ObservableCollection<KittingIssueScanItem> KittingIssueItems { get; } = new();

        public AuditarView(
            IServiceProvider serviceProvider,
            LoadMappingService loadMappingService,
            DeliveryOrderService deliveryOrderService,
            KittingService kittingService,
            KittingDetailService kittingDetailService,
            KittingIssueService kittingIssueService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _loadMappingService = loadMappingService;
            _deliveryOrderService = deliveryOrderService;
            _kittingService = kittingService;
            _kittingDetailService = kittingDetailService;
            _kittingIssueService = kittingIssueService;
            DataContext = this;
            rbScanIzquierda.IsChecked = true;
            SetScanMessage("Selecciona una orden y escanea un StandardId.", false);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadMapeosAsync();
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            await LoadMapeosAsync();
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = _serviceProvider.GetRequiredService<NuevaAuditoriaView>();
                WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

                if (dialog.ShowDialog() != true)
                    return;

                if (dialog.SelectedOrder is null)
                {
                    DialogHelper.ShowWarning("Selecciona una orden de entrega valida.");
                    return;
                }

                var request = new CreateLoadMappingRequest
                {
                    ClientId = dialog.SelectedOrder.ClientId,
                    ProjectId = dialog.SelectedOrder.ProjectId,
                    DeliveryOrderCode = dialog.SelectedOrder.OrdenEntrega?.Trim()
                };

                var response = await _loadMappingService.CreateLoadMapping(request);
                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo crear el mapeo de carga.");
                    return;
                }

                DialogHelper.ShowSuccess(response.Message ?? "Mapeo de carga creado correctamente.");
                await LoadMapeosAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnTerminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgMapeoCarga.SelectedItem is not LoadMappingDto selectedMapping)
            {
                DialogHelper.ShowWarning("Selecciona una orden de entrega valida.");
                return;
            }

            var deliveryOrderCode = selectedMapping.OrdenEntrega?.Trim();
            if (string.IsNullOrWhiteSpace(deliveryOrderCode))
            {
                DialogHelper.ShowWarning("La orden seleccionada no tiene folio.");
                return;
            }

            if (!DialogHelper.ShowConfirm("¿Está seguro de terminar la carga?", "Advertencia"))
                return;

            try
            {
                SetAuditoriaLoading(true, $"Terminando {deliveryOrderCode}...");
                SetDetalleLoading(true, "Validando parciales...");

                var response = await _deliveryOrderService.FinishDeliveryOrderLoading(deliveryOrderCode);
                if (!response.IsSuccess)
                {
                    SetScanMessage(response.Message ?? "No se pudo cerrar la orden de entrega.", true);
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo cerrar la orden de entrega.");
                    return;
                }

                var result = response.Data;
                var message = result is null
                    ? response.Message ?? "Orden de entrega cerrada correctamente."
                    : BuildFinishLoadingMessage(result);

                if (result?.HasPartials == true)
                    DialogHelper.ShowWarning(message);
                else
                    DialogHelper.ShowSuccess(message);

                await LoadMapeosAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                SetDetalleLoading(false);
                SetAuditoriaLoading(false);
            }
        }

        private void dgDetalleAuditoria_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private async void dgMapeoCarga_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMapeoCarga.SelectedItem is not LoadMappingDto selectedMapping)
            {
                ClearSelectedOrderData();
                return;
            }

            await LoadSelectedOrderAsync(selectedMapping);
        }

        private async Task LoadMapeosAsync()
        {
            try
            {
                ++_orderLoadVersion;
                SetAuditoriaLoading(true, "Cargando mapeos...");
                ClearSelectedOrderData();

                var response = await _loadMappingService.GetLoadMappings();
                if (!response.IsSuccess)
                {
                    MapeoCargaItems.Clear();
                    txtStatusAuditoria.Text = "Sin mapeos de carga por mostrar";
                    DialogHelper.ShowWarning(response.Message ?? "No se pudieron cargar los mapeos de carga.");
                    return;
                }

                MapeoCargaItems.Clear();
                foreach (var item in response.Data ?? [])
                    MapeoCargaItems.Add(item);

                txtStatusAuditoria.Text = MapeoCargaItems.Count > 0
                    ? $"Mapeos de carga: {MapeoCargaItems.Count}"
                    : "Sin mapeos de carga por mostrar";
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                SetAuditoriaLoading(false);
            }
        }

        private async Task LoadSelectedOrderAsync(LoadMappingDto selectedMapping)
        {
            var loadVersion = ++_orderLoadVersion;
            var deliveryOrderCode = selectedMapping.OrdenEntrega?.Trim();
            var client = selectedMapping.Cliente?.Trim();
            var project = selectedMapping.Proyecto?.Trim();

            if (string.IsNullOrWhiteSpace(deliveryOrderCode))
            {
                ClearSelectedOrderData("La orden seleccionada no tiene folio.");
                return;
            }

            try
            {
                SetAuditoriaLoading(true, $"Cargando kittings de {deliveryOrderCode}...");
                SetDetalleLoading(true, "Cargando kitting issues...");
                ClearSelectedOrderData($"Cargando detalle de {deliveryOrderCode}...");
                _selectedLoadMappingId = selectedMapping.MapeoCargaId;

                var kittingResponse = await _kittingService.GetKittings();
                if (loadVersion != _orderLoadVersion)
                    return;

                if (!kittingResponse.IsSuccess || kittingResponse.Data == null)
                {
                    txtStatusDetalle.Text = "Sin detalle para mostrar";
                    DialogHelper.ShowWarning(kittingResponse.Message ?? "No se pudieron cargar los kittings.");
                    return;
                }

                var orderKittings = kittingResponse.Data
                    .Where(x => string.Equals(x.DeliveryOrderCode?.Trim(), deliveryOrderCode, StringComparison.OrdinalIgnoreCase))
                    .Where(x => string.IsNullOrWhiteSpace(client) ||
                                string.Equals(x.Client?.Trim(), client, StringComparison.OrdinalIgnoreCase))
                    .Where(x => string.IsNullOrWhiteSpace(project) ||
                                string.Equals(x.Project?.Trim(), project, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => x.KittingCode)
                    .ToList();

                foreach (var kitting in orderKittings)
                    KittingItems.Add(kitting);

                txtStatusAuditoria.Text = orderKittings.Count > 0
                    ? $"Orden {deliveryOrderCode}: {orderKittings.Count} kitting(s)"
                    : $"Orden {deliveryOrderCode}: sin kittings relacionados";

                if (orderKittings.Count == 0)
                {
                    ResetScanWorkspace();
                    await LoadSavedScansAsync(selectedMapping.MapeoCargaId);
                    txtStatusDetalle.Text = "Sin kitting issues para mostrar";
                    return;
                }

                var issues = await LoadIssuesForKittingsAsync(orderKittings);
                if (loadVersion != _orderLoadVersion)
                    return;

                foreach (var issue in issues)
                    KittingIssueItems.Add(issue);

                ResetScanWorkspace();
                await LoadSavedScansAsync(selectedMapping.MapeoCargaId);
                txtStatusDetalle.Text = issues.Count > 0
                    ? $"Orden {deliveryOrderCode}: {issues.Count} kitting issue(s) para escanear"
                    : "Sin kitting issues para mostrar";
            }
            catch (Exception ex)
            {
                ClearSelectedOrderData("Sin detalle para mostrar");
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                if (loadVersion == _orderLoadVersion)
                {
                    SetAuditoriaLoading(false);
                    SetDetalleLoading(false);
                }
            }
        }

        private async Task<List<KittingIssueScanItem>> LoadIssuesForKittingsAsync(List<KittingDto> kittings)
        {
            var kittingCodeById = kittings
                .Where(x => x.KittingId > 0)
                .GroupBy(x => x.KittingId)
                .ToDictionary(
                    group => group.Key,
                    group => string.IsNullOrWhiteSpace(group.First().KittingCode)
                        ? group.Key.ToString()
                        : group.First().KittingCode.Trim());

            var detailTasks = kittings
                .Where(x => x.KittingId > 0)
                .Select(x => _kittingDetailService.GetKittingDetailsByKittingId(x.KittingId));

            var detailResponses = await Task.WhenAll(detailTasks);
            var details = detailResponses
                .Where(x => x.IsSuccess && x.Data != null)
                .SelectMany(x => x.Data!)
                .Where(x => x.KittingDetailId > 0)
                .ToList();

            if (details.Count == 0)
                return new List<KittingIssueScanItem>();

            var kittingIdByDetailId = details
                .GroupBy(x => x.KittingDetailId)
                .ToDictionary(group => group.Key, group => group.First().KittingId);

            var issueTasks = details.Select(x => _kittingIssueService.GetKittingIssuesByKittingDetailId(x.KittingDetailId));
            var issueResponses = await Task.WhenAll(issueTasks);

            return issueResponses
                .Where(x => x.IsSuccess && x.Data != null)
                .SelectMany(x => x.Data!)
                .Select(issue => new KittingIssueScanItem
                {
                    Kitting = ResolveKittingCode(issue, kittingIdByDetailId, kittingCodeById),
                    Issue = issue
                })
                .OrderBy(x => x.Kitting)
                .ThenBy(GetStandardIdText)
                .ThenBy(x => x.PartNumber ?? string.Empty)
                .ThenBy(x => x.LotNumber ?? string.Empty)
                .ToList();
        }

        private void ClearSelectedOrderData(string detailMessage = "Sin detalle para mostrar")
        {
            _selectedLoadMappingId = 0;
            KittingItems.Clear();
            KittingIssueItems.Clear();
            LeftCards.Clear();
            RightCards.Clear();
            txtStatusDetalle.Text = detailMessage;
            ResetScanSide();
            SetScanMessage("Selecciona una orden y escanea un StandardId.", false);
            txtScanStandardId?.Clear();
        }

        private void UpdateIssueCards(List<KittingIssueScanItem> issues)
        {
            LeftCards.Clear();
            RightCards.Clear();

            for (var i = 0; i < issues.Count; i++)
            {
                var issue = issues[i];
                var card = new LocationCardPreview
                {
                    Title = string.IsNullOrWhiteSpace(GetStandardIdText(issue))
                        ? "Sin StandardId"
                        : GetStandardIdText(issue),
                    Subtitle = $"Parte: {issue.PartNumber}",
                    Details = $"{issue.Kitting} | Cantidad: {issue.ReceivedQuantity.GetValueOrDefault():0.##} | Lote: {issue.LotNumber}"
                };

                if (i % 2 == 0)
                    LeftCards.Add(card);
                else
                    RightCards.Add(card);
            }
        }

        private void ResetScanWorkspace()
        {
            LeftCards.Clear();
            RightCards.Clear();
            ResetScanSide();
            txtScanStandardId.Clear();
            SetScanMessage("Listo para escanear StandardId.", false);
            txtScanStandardId.Focus();
        }

        private async Task LoadSavedScansAsync(int loadMappingId)
        {
            if (loadMappingId <= 0)
                return;

            var response = await _loadMappingService.GetLoadMappingScans(loadMappingId);
            if (loadMappingId != _selectedLoadMappingId)
                return;

            if (!response.IsSuccess)
            {
                SetScanMessage(response.ErrorMessage ?? response.Message ?? "No se pudieron cargar los escaneos guardados.", true);
                return;
            }

            var scans = response.Data ?? [];
            foreach (var scan in scans)
                AddSavedScanToUi(scan);

            SetScanMessage(scans.Count > 0
                ? $"Se cargaron {scans.Count} escaneo(s) guardados."
                : "Listo para escanear StandardId.",
                false);
        }

        private async void TxtScanStandardId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;

            if (_isProcessingScan)
                return;

            var scanValue = txtScanStandardId.Text.Trim();
            if (string.IsNullOrWhiteSpace(scanValue))
                return;

            try
            {
                _isProcessingScan = true;
                txtScanStandardId.IsEnabled = false;
                await ProcessAuditScanAsync(scanValue);
            }
            finally
            {
                _isProcessingScan = false;
                txtScanStandardId.IsEnabled = true;
                txtScanStandardId.Clear();
                txtScanStandardId.Focus();
            }
        }

        private async Task ProcessAuditScanAsync(string scanValue)
        {
            var selectedSide = GetSelectedScanSide();
            var loadMappingId = _selectedLoadMappingId;

            try
            {
                if (KittingIssueItems.Count == 0)
                {
                    await AddRejectedScanAsync(loadMappingId, selectedSide, scanValue, "Selecciona primero una orden de entrega con kitting issues.");
                    return;
                }

                var match = FindMatchingIssue(scanValue);
                if (match is null)
                {
                    await AddRejectedScanAsync(loadMappingId, selectedSide, scanValue, "No pertenece a los StandardId que se deben escanear en esta orden.");
                    return;
                }

                if (IsLoadedSupplyStatus(match.SupplyStatus))
                {
                    await AddRejectedScanAsync(loadMappingId, selectedSide, scanValue, $"El StandardId {GetStandardIdText(match)} ya esta Cargado.", match);
                    return;
                }

                var response = await _kittingIssueService.ConfirmAuditKittingIssue(
                    match.Issue.KittingReceiptDetailId,
                    new KittingIssueValidateRequest
                    {
                        StandardIdStr = scanValue.Trim()
                    });

                if (!response.IsSuccess)
                {
                    await AddRejectedScanAsync(
                        loadMappingId,
                        selectedSide,
                        scanValue,
                        response.ErrorMessage ?? response.Message ?? "No se pudo cargar el kitting issue.",
                        match);
                    return;
                }

                match.Issue.SupplyStatus = KittingStatusNames.Cargado;
                dgDetalleAuditoria.Items.Refresh();

                await AddAcceptedScanAsync(
                    loadMappingId,
                    selectedSide,
                    scanValue,
                    match,
                    response.Message ?? "Kitting issue cargado correctamente.");
            }
            finally
            {
                ToggleScanSide(selectedSide);
            }
        }

        private KittingIssueScanItem? FindMatchingIssue(string scanValue)
        {
            var matches = KittingIssueItems
                .Where(issue => MatchesScan(GetStandardIdText(issue), scanValue))
                .ToList();

            return matches.FirstOrDefault(issue => !IsLoadedSupplyStatus(issue.SupplyStatus))
                ?? matches.FirstOrDefault();
        }

        private static bool MatchesScan(string? source, string scanValue) =>
            !string.IsNullOrWhiteSpace(source) &&
            string.Equals(source.Trim(), scanValue.Trim(), StringComparison.OrdinalIgnoreCase);

        private async Task AddAcceptedScanAsync(int loadMappingId, string side, string scanValue, KittingIssueScanItem issue, string message)
        {
            var standardId = string.IsNullOrWhiteSpace(GetStandardIdText(issue))
                ? scanValue.Trim()
                : GetStandardIdText(issue);

            var savedScan = await SaveScanAsync(loadMappingId, side, standardId, "OK", message, issue, true);
            if (savedScan is null)
                return;

            if (loadMappingId != _selectedLoadMappingId)
                return;

            AddSavedScanToUi(savedScan);
            SetScanMessage($"{side}: {standardId} cargado.", false);
            PlayCorrectSound();
        }

        private async Task AddRejectedScanAsync(int loadMappingId, string side, string scanValue, string message, KittingIssueScanItem? issue = null)
        {
            var title = string.IsNullOrWhiteSpace(scanValue) ? "Escaneo vacio" : scanValue.Trim();

            var savedScan = await SaveScanAsync(loadMappingId, side, title, "ERROR", message, issue, false);
            if (savedScan is not null && loadMappingId == _selectedLoadMappingId)
                AddSavedScanToUi(savedScan);

            if (loadMappingId == _selectedLoadMappingId)
            {
                SetScanMessage($"{side}: {message}", true);
                PlayErrorSound();
            }
        }

        private async Task<LoadMappingScanDto?> SaveScanAsync(
            int loadMappingId,
            string side,
            string standardId,
            string result,
            string message,
            KittingIssueScanItem? issue,
            bool isSuccess)
        {
            if (loadMappingId <= 0)
            {
                SetScanMessage("No hay un mapeo de carga seleccionado para guardar el escaneo.", true);
                return null;
            }

            var response = await _loadMappingService.CreateLoadMappingScan(
                loadMappingId,
                new CreateLoadMappingScanRequest
                {
                    KittingReceiptDetailId = issue?.Issue.KittingReceiptDetailId,
                    Side = side,
                    StandardId = standardId,
                    Result = result,
                    IsSuccess = isSuccess,
                    Kitting = issue?.Kitting ?? string.Empty,
                    PartNumber = issue?.PartNumber ?? string.Empty,
                    Description = issue?.Description ?? string.Empty,
                    Quantity = issue?.ReceivedQuantity,
                    LotNumber = issue?.LotNumber ?? string.Empty,
                    Message = message
                });

            if (response.IsSuccess && response.Data is not null)
                return response.Data;

            if (loadMappingId == _selectedLoadMappingId)
                SetScanMessage(response.ErrorMessage ?? response.Message ?? "No se pudo guardar el escaneo.", true);

            return null;
        }

        private void AddSavedScanToUi(LoadMappingScanDto scan)
        {
            AddCardToSide(scan.Side, CreateScanCard(scan));
        }

        private static LocationCardPreview CreateScanCard(LoadMappingScanDto scan)
        {
            var subtitle = string.IsNullOrWhiteSpace(scan.PartNumber)
                ? scan.IsSuccess ? "Escaneo cargado" : "Escaneo rechazado"
                : $"Parte: {scan.PartNumber}";

            var details = scan.IsSuccess
                ? $"{scan.Kitting} | Cantidad: {scan.Quantity.GetValueOrDefault():0.##} | Lote: {scan.LotNumber}"
                : string.IsNullOrWhiteSpace(scan.Kitting)
                    ? scan.Message
                    : $"{scan.Kitting} | {scan.Message}";

            return CreateLocationCard(
                string.IsNullOrWhiteSpace(scan.StandardId) ? "Sin StandardId" : scan.StandardId,
                subtitle,
                details,
                !scan.IsSuccess,
                scan.LoadMappingScanId,
                scan.LoadMappingId,
                scan.Side);
        }

        private static LocationCardPreview CreateLocationCard(
            string title,
            string subtitle,
            string details,
            bool isError,
            int loadMappingScanId = 0,
            int loadMappingId = 0,
            string side = "")
        {
            return new LocationCardPreview
            {
                LoadMappingScanId = loadMappingScanId,
                LoadMappingId = loadMappingId,
                Side = side,
                Title = title,
                Subtitle = subtitle,
                Details = details,
                Background = isError ? ErrorCardBackground : SuccessCardBackground,
                BorderBrush = isError ? ErrorCardBorder : SuccessCardBorder,
                TitleForeground = isError ? ErrorCardTitle : SuccessCardTitle,
                TextForeground = isError ? ErrorCardText : SuccessCardText
            };
        }

        private async void AuditCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (sender is not FrameworkElement { DataContext: LocationCardPreview card })
                return;

            await RemoveScanCardWithAuthorizationAsync(card);
        }

        private async Task RemoveScanCardWithAuthorizationAsync(LocationCardPreview card)
        {
            if (_isRemovingScanCard)
                return;

            if (card.LoadMappingScanId <= 0 || card.LoadMappingId <= 0)
            {
                SetScanMessage("Esta tarjeta no tiene un escaneo guardado para retirar.", true);
                PlayErrorSound();
                return;
            }

            try
            {
                _isRemovingScanCard = true;

                var dialog = _serviceProvider.GetRequiredService<PermissionLoginDialog>();
                dialog.Configure(
                    PermissionKeys.LoadMappingScan_Delete,
                    "Autorizar retiro",
                    $"Ingresa un usuario con permiso para retirar el escaneo {card.Title}.");

                WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

                if (dialog.ShowDialog() != true)
                    return;

                try
                {
                    var response = await _loadMappingService.DeleteLoadMappingScan(card.LoadMappingId, card.LoadMappingScanId);
                    if (!response.IsSuccess)
                    {
                        SetScanMessage(response.ErrorMessage ?? response.Message ?? "No se pudo retirar el escaneo.", true);
                        PlayErrorSound();
                        return;
                    }

                    RemoveCardFromUi(card);
                    SetScanMessage($"Escaneo {card.Title} retirado del mapeo.", false);
                    PlayCorrectSound();
                }
                finally
                {
                    dialog.RestoreOriginalSessionToken();
                }
            }
            catch (Exception ex)
            {
                SetScanMessage("No se pudo retirar el escaneo.", true);
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _isRemovingScanCard = false;
                txtScanStandardId.Focus();
            }
        }

        private void RemoveCardFromUi(LocationCardPreview card)
        {
            var leftCard = LeftCards.FirstOrDefault(x => x.LoadMappingScanId == card.LoadMappingScanId);
            if (leftCard != null)
            {
                LeftCards.Remove(leftCard);
                return;
            }

            var rightCard = RightCards.FirstOrDefault(x => x.LoadMappingScanId == card.LoadMappingScanId);
            if (rightCard != null)
                RightCards.Remove(rightCard);
        }

        private void AddCardToSide(string side, LocationCardPreview card)
        {
            if (string.Equals(side, ScanSideRight, StringComparison.OrdinalIgnoreCase))
                RightCards.Add(card);
            else
                LeftCards.Add(card);
        }

        private string GetSelectedScanSide() =>
            rbScanDerecha.IsChecked == true ? ScanSideRight : ScanSideLeft;

        private void ToggleScanSide(string currentSide)
        {
            if (string.Equals(currentSide, ScanSideLeft, StringComparison.OrdinalIgnoreCase))
                rbScanDerecha.IsChecked = true;
            else
                rbScanIzquierda.IsChecked = true;

            UpdateSelectedSideText();
        }

        private void ResetScanSide()
        {
            rbScanIzquierda.IsChecked = true;
            UpdateSelectedSideText();
        }

        private void ScanSide_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSelectedSideText();
            txtScanStandardId?.Focus();
        }

        private void UpdateSelectedSideText()
        {
            if (txtSelectedScanSide is null)
                return;

            txtSelectedScanSide.Text = $"Siguiente: {GetSelectedScanSide()}";
        }

        private void SetScanMessage(string message, bool isError)
        {
            txtUltimoMensajeEscaneo.Text = message;
            txtUltimoMensajeEscaneo.Background = isError ? ErrorMessageBackground : SuccessMessageBackground;
            txtUltimoMensajeEscaneo.Foreground = isError ? ErrorMessageForeground : SuccessMessageForeground;
            txtUltimoMensajeEscaneo.BorderBrush = isError ? ErrorMessageBorder : SuccessMessageBorder;
        }

        private static bool IsLoadedSupplyStatus(string? status) =>
            KittingStatusNames.IsLoaded(status);

        private static string BuildFinishLoadingMessage(FinishDeliveryOrderLoadingResultDto result)
        {
            var lines = new List<string>
            {
                result.HasPartials
                    ? $"Orden {result.DeliveryOrderCode} cerrada con parciales."
                    : $"Orden {result.DeliveryOrderCode} cerrada correctamente.",
                $"Kittings en Cargado: {result.LoadedKittings}/{result.TotalKittings}"
            };

            if (result.PartialKittings > 0)
            {
                lines.Add("Kittings con Cargado Parcial:");

                foreach (var kitting in result.Kittings.Where(x => x.IsPartial))
                {
                    var missingLabels = kitting.MissingLabels.Count > 0
                        ? string.Join(", ", kitting.MissingLabels)
                        : "sin etiquetas cargadas";

                    lines.Add($"- {kitting.KittingCode}: faltan {kitting.MissingLabels.Count} etiqueta(s) ({missingLabels})");
                }
            }

            return string.Join(Environment.NewLine, lines);
        }

        private void PlayCorrectSound()
        {
            PlaySound(CorrectSoundFileName);
        }

        private void PlayErrorSound()
        {
            PlaySound(ErrorSoundFileName);
        }

        private void PlaySound(string fileName)
        {
            try
            {
                var soundPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Raw", fileName);
                if (!File.Exists(soundPath))
                    return;

                var player = new MediaPlayer();
                player.MediaEnded += OnMediaPlayerFinished;
                player.MediaFailed += OnMediaPlayerFailed;
                _activePlayers.Add(player);
                player.Open(new Uri(soundPath, UriKind.Absolute));
                player.Play();
            }
            catch
            {
            }
        }

        private void OnMediaPlayerFinished(object? sender, EventArgs e)
        {
            DisposeMediaPlayer(sender as MediaPlayer);
        }

        private void OnMediaPlayerFailed(object? sender, ExceptionEventArgs e)
        {
            DisposeMediaPlayer(sender as MediaPlayer);
        }

        private void DisposeMediaPlayer(MediaPlayer? player)
        {
            if (player == null)
                return;

            player.MediaEnded -= OnMediaPlayerFinished;
            player.MediaFailed -= OnMediaPlayerFailed;
            player.Close();
            _activePlayers.Remove(player);
        }

        private void SetAuditoriaLoading(bool isLoading, string message = "Cargando auditorias...")
        {
            LoadingOverlayAuditoria.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            TxtLoadingAuditoria.Text = message;
            dgMapeoCarga.IsEnabled = !isLoading;
            dgAuditoriasKitting.IsEnabled = !isLoading;
        }

        private void SetDetalleLoading(bool isLoading, string message = "Cargando detalle...")
        {
            LoadingOverlayDetalle.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            TxtLoadingDetalle.Text = message;
            dgDetalleAuditoria.IsEnabled = !isLoading;
        }

        private static string ResolveKittingCode(
            KittingIssueDetailDto issue,
            Dictionary<int, int> kittingIdByDetailId,
            Dictionary<int, string> kittingCodeById)
        {
            if (!kittingIdByDetailId.TryGetValue(issue.KittingDetailId, out var kittingId))
                return string.Empty;

            return kittingCodeById.TryGetValue(kittingId, out var kittingCode)
                ? kittingCode
                : kittingId.ToString();
        }

        private static string GetStandardIdText(KittingIssueScanItem issue) =>
            !string.IsNullOrWhiteSpace(issue.StandardIdStr)
                ? issue.StandardIdStr.Trim()
                : issue.StandardId?.Trim() ?? string.Empty;

    }

    public sealed class LocationCardPreview
    {
        public int LoadMappingScanId { get; set; }
        public int LoadMappingId { get; set; }
        public string Side { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public Brush Background { get; set; } = new SolidColorBrush(Color.FromRgb(248, 250, 252));
        public Brush BorderBrush { get; set; } = new SolidColorBrush(Color.FromRgb(215, 222, 232));
        public Brush TitleForeground { get; set; } = new SolidColorBrush(Color.FromRgb(15, 23, 42));
        public Brush TextForeground { get; set; } = new SolidColorBrush(Color.FromRgb(71, 85, 105));
    }

    public sealed class KittingIssueScanItem
    {
        public string Kitting { get; set; } = string.Empty;
        public KittingIssueDetailDto Issue { get; set; } = new();
        public string? StandardId => Issue.StandardId;
        public string? StandardIdStr => Issue.StandardIdStr;
        public string PartNumber => Issue.PartNumber;
        public string Description => Issue.Description;
        public decimal? ReceivedQuantity => Issue.ReceivedQuantity;
        public string Status => Issue.Status;
        public string SupplyStatus => Issue.SupplyStatus;
        public string LotNumber => Issue.LotNumber;
    }

}
