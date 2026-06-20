using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.ASN;
using LD.Contracts.DTOs;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using LD.FormsX.Model.Lookup;
using LD.FormsX.Views.Dialogs;
using LD.FormsX.Views.Familias;
using LD.FormsX.Views.Ubicaciones;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;



namespace LD.FormsX.Views.ASN
{
    /// <summary>
    /// Lógica de interacción para ASNView.xaml
    /// </summary>
    public partial class ASNView : UserControl, INotifyPropertyChanged
    {
        private readonly AsnService _asnService;
        private readonly AsnDetailService _asnDetailService;
        private readonly AsnReceiptService _asnReceiptService;
        private readonly LookupService _lookupService;
        private readonly IServiceProvider _serviceProvider;
     

        private readonly WpfGridFilter<AsnDto> _gridFilter;
        private readonly WpfGridFilter<AsnDetailDto> _gridFilterDet;
        private readonly WpfGridFilter<AsnReceiptDetailDto> _gridFilterRec;

        private AsnDto? _selectedX;
        private AsnDetailDto? _selectedDetail;
        private List<AsnDto> _allAsns = new();
        private List<UserProjectClientDto> _userProjectClients = new();
        private bool _cargandoCombos;
        private bool _loaded;
        private int _selectedClientId;
        private int _selectedProjectId;
        private string _selectedClientText = string.Empty;
        private string _selectedProjectText = string.Empty;
        private bool _verSinConfirmar = true;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<LookupItem> ClientLookupItems { get; } = new();
        public ObservableCollection<LookupItem> ProjectLookupItems { get; } = new();

        public int SelectedClientId
        {
            get => _selectedClientId;
            set
            {
                if (_selectedClientId == value)
                    return;

                _selectedClientId = value;
                OnPropertyChanged(nameof(SelectedClientId));
            }
        }

        public int SelectedProjectId
        {
            get => _selectedProjectId;
            set
            {
                if (_selectedProjectId == value)
                    return;

                _selectedProjectId = value;
                OnPropertyChanged(nameof(SelectedProjectId));
            }
        }

        public string SelectedClientText
        {
            get => _selectedClientText;
            set
            {
                if (_selectedClientText == value)
                    return;

                _selectedClientText = value;
                OnPropertyChanged(nameof(SelectedClientText));
            }
        }

        public string SelectedProjectText
        {
            get => _selectedProjectText;
            set
            {
                if (_selectedProjectText == value)
                    return;

                _selectedProjectText = value;
                OnPropertyChanged(nameof(SelectedProjectText));
            }
        }

        public bool VerSinConfirmar
        {
            get => _verSinConfirmar;
            set
            {
                if (_verSinConfirmar == value)
                    return;

                _verSinConfirmar = value;
                OnPropertyChanged(nameof(VerSinConfirmar));
            }
        }

        public ASNView(
            AsnService asnService,
            AsnDetailService asnDetailService,
            AsnReceiptService asnReceiptService,
            LookupService lookupService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = this;
            _asnService = asnService;
            _asnDetailService = asnDetailService;
            _asnReceiptService = asnReceiptService;
            _lookupService = lookupService;
            _serviceProvider = serviceProvider;

            _gridFilter = new WpfGridFilter<AsnDto>(dgASN);
            _gridFilterDet = new WpfGridFilter<AsnDetailDto>(dgDetalleASN);
            _gridFilterRec = new WpfGridFilter<AsnReceiptDetailDto>(dgRecepcionASN);

            _gridFilter.SetHiddenColumns("AsnId");
            _gridFilterDet.SetHiddenColumns("AsnDetailId", "AsnId", "ProductId");
            _gridFilterRec.SetHiddenColumns("AsnReceiptDetailId", "AsnDetailId", "ProductId", "LocationId");
            UpdateActionButtons();
        }

        private static bool IsConfirmedStatus(string? status) =>
            string.Equals(status?.Trim(), "Confirmado", StringComparison.OrdinalIgnoreCase);

        private static bool IsCancelledStatus(string? status) =>
            string.Equals(status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase);

        private static bool IsLocatingStatus(string? status) =>
            string.Equals(status?.Trim(), "Ubicando", StringComparison.OrdinalIgnoreCase);

        private static bool IsTerminalStatus(string? status) =>
            IsConfirmedStatus(status) || IsCancelledStatus(status);

        private static SolidColorBrush CreateBrush(string hexColor) =>
            new((Color)ColorConverter.ConvertFromString(hexColor));

        private void ConfigureActionButton(Button? button, bool canClick, bool showBlockedStyle, string tooltip)
        {
            if (button == null)
                return;

            button.Background = CreateBrush(showBlockedStyle ? "#FEF2F2" : "#F3F4F6");
            button.BorderBrush = CreateBrush(showBlockedStyle ? "#FCA5A5" : "#D1D5DB");
            button.Foreground = CreateBrush(showBlockedStyle ? "#991B1B" : "#374151");
            button.ToolTip = tooltip;
            button.Opacity = 1;

            if (showBlockedStyle)
            {
                button.IsEnabled = true;
                button.IsHitTestVisible = false;
                button.Focusable = false;
                button.Cursor = Cursors.No;
                return;
            }

            button.IsEnabled = canClick;
            button.IsHitTestVisible = canClick;
            button.Focusable = canClick;
            button.Cursor = canClick ? Cursors.Hand : Cursors.Arrow;
        }

        private void UpdateActionButtons()
        {
            var hasSelectedAsn = _selectedX != null;
            var isConfirmed = IsConfirmedStatus(_selectedX?.Status);
            var isCancelled = IsCancelledStatus(_selectedX?.Status);
            var isLocating = IsLocatingStatus(_selectedX?.Status);
            var isTerminal = isConfirmed || isCancelled;
            var canClick = hasSelectedAsn && !isTerminal;
            var canLocate = canClick && !isLocating;
            var terminalStatus = isCancelled ? "cancelado" : "confirmado";

            ConfigureActionButton(
                btnEditar,
                canClick,
                hasSelectedAsn && isTerminal,
                hasSelectedAsn && isTerminal
                    ? $"Este ASN esta {terminalStatus}. Ya no se puede editar."
                    : hasSelectedAsn
                        ? "Editar ASN"
                        : "Selecciona un ASN para editar.");

            ConfigureActionButton(
                btnConfirmarLlegada,
                canClick,
                hasSelectedAsn && isTerminal,
                hasSelectedAsn && isTerminal
                    ? $"Este ASN esta {terminalStatus}. Ya no se puede confirmar."
                    : hasSelectedAsn
                        ? "Confirmar entrada del ASN"
                        : "Selecciona un ASN para confirmar.");

            ConfigureActionButton(
                btnUbicarMercancia,
                canLocate,
                hasSelectedAsn && isTerminal,
                hasSelectedAsn && isTerminal
                    ? $"Este ASN esta {terminalStatus}. Ya no se puede ubicar."
                    : hasSelectedAsn && isLocating
                        ? "El ASN ya esta en estatus Ubicando."
                        : hasSelectedAsn
                            ? "Ubicar mercancia del ASN"
                            : "Selecciona un ASN para ubicar.");

            ConfigureActionButton(
                btnCancelar,
                canClick,
                hasSelectedAsn && isTerminal,
                hasSelectedAsn && isTerminal
                    ? $"Este ASN esta {terminalStatus}. Ya no se puede cancelar."
                    : hasSelectedAsn
                        ? "Cancelar ASN"
                        : "Selecciona un ASN para cancelar.");
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await CargarCombosAsync();
            _gridFilter.SetData(null);
            _gridFilterDet.SetData(null);
            _gridFilterRec.SetData(null);
            txtStatusDetalle.Text = "Sin detalle para mostrar";
            txtStatusRecepcion.Text = "Sin partidas recibidas";
        }

        private async Task CargarCombosAsync()
        {
            try
            {
                _cargandoCombos = true;
                ClientLookupItems.Clear();
                ProjectLookupItems.Clear();

                if (string.IsNullOrWhiteSpace(UserData.Id))
                {
                    DialogHelper.ShowWarning("No se pudo identificar el usuario actual para cargar clientes y proyectos.");
                    ClearProjectSelection();
                    return;
                }

                var response = await _lookupService.GetProjectClientsByUserWarehouses(UserData.Id);
                if (response.IsSuccess && response.Data != null)
                {
                    _userProjectClients = response.Data;

                    var clientes = _userProjectClients
                        .GroupBy(x => x.ClientId)
                        .Select(group => new DropDownDto
                        {
                            Key = group.Key.ToString(),
                            Value = group.First().Client
                        })
                        .OrderBy(x => x.Value);

                    foreach (var item in clientes.Select(ToLookupItem))
                        ClientLookupItems.Add(item);
                }
                else
                {
                    _userProjectClients.Clear();
                    DialogHelper.ShowWarning(response.Message ?? "No se pudieron cargar clientes y proyectos del usuario.");
                }

                ClearProjectSelection();
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private Task SetCombosProjectsAsync(string projectSel = "")
        {
            if (SelectedClientId <= 0)
            {
                ProjectLookupItems.Clear();
                ClearProjectSelection();
                return Task.CompletedTask;
            }

            ProjectLookupItems.Clear();
            var proyectos = _userProjectClients
                .Where(x => x.ClientId == SelectedClientId)
                .GroupBy(x => x.ProjectId)
                .Select(group => new DropDownDto
                {
                    Key = group.Key.ToString(),
                    Value = group.First().Project
                })
                .OrderBy(x => x.Value);

            foreach (var item in proyectos.Select(ToLookupItem))
                ProjectLookupItems.Add(item);

            if (!string.IsNullOrWhiteSpace(projectSel))
                ApplyProjectSelection(projectSel);
            else
                ClearProjectSelection();

            return Task.CompletedTask;
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
            TxtLoadingASN.Text = mensaje;
            LoadingOverlayASN.Visibility = mostrar ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task CargarDatosAsync()
        {
            var result = await _asnService.GetAsn();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _allAsns = result.Data ?? new List<AsnDto>();
            AplicarFiltroAsn();
        }

        private void AplicarFiltroAsn()
        {
            var filtered = _allAsns.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SelectedClientText))
            {
                filtered = filtered.Where(x =>
                    string.Equals(x.Client?.Trim(), SelectedClientText.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SelectedProjectText))
            {
                filtered = filtered.Where(x =>
                    string.Equals(x.Project?.Trim(), SelectedProjectText.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            if (VerSinConfirmar)
            {
                filtered = filtered.Where(x => !IsConfirmedStatus(x.Status));
            }

            _gridFilter.SetData(filtered.ToList());
            _selectedX = null;
            _selectedDetail = null;
            _gridFilterDet.SetData(null);
            _gridFilterRec.SetData(null);
            txtStatusDetalle.Text = "Sin detalle para mostrar";
            txtStatusRecepcion.Text = "Sin partidas recibidas";
            UpdateActionButtons();
        }

        private async Task CargarDatosAsyncDet()
        {
            _selectedDetail = null;
            _gridFilterRec.SetData(null);
            txtStatusRecepcion.Text = "Sin partidas recibidas";

            if (_selectedX == null)
            {
                _gridFilterDet.SetData(null);
                txtStatusDetalle.Text = "Sin detalle para mostrar";
                return;
            }

            var result = await _asnDetailService.GetAsnDetailsByAsn(_selectedX.AsnId);

            if (!result.IsSuccess || result.Data == null)
            {
                _gridFilterDet.SetData(null);
                txtStatusDetalle.Text = "Sin detalle para mostrar";
                return;
            }

            _gridFilterDet.SetData(result.Data);
            txtStatusDetalle.Text = $"Registros: {result.Data?.Count ?? 0}";
        }

        private async Task CargarDatosAsyncRecepcion()
        {
            if (_selectedDetail == null || _selectedDetail.AsnDetailId <= 0)
            {
                _gridFilterRec.SetData(null);
                txtStatusRecepcion.Text = "Sin partidas recibidas";
                return;
            }

            var result = await _asnReceiptService.GetAsnReceiptsByAsnDetailId(_selectedDetail.AsnDetailId);
            if (!result.IsSuccess || result.Data == null)
            {
                _gridFilterRec.SetData(null);
                txtStatusRecepcion.Text = "Sin partidas recibidas";
                return;
            }

            _gridFilterRec.SetData(result.Data);
            txtStatusRecepcion.Text = result.Data.Count > 0
                ? $"Registros: {result.Data.Count}"
                : "Sin partidas recibidas";
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedClientId <= 0)
            {
                DialogHelper.ShowWarning("Selecciona un cliente.");
                return;
            }

            if (SelectedProjectId <= 0)
            {
                DialogHelper.ShowWarning("Selecciona un proyecto.");
                return;
            }

            await CargarDatosConLoaderAsync("Trayendo ASN...");
        }

        private async void LookupCliente_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            if (sender is not InlineLookupEditor editor || editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not DropDownDto selectedClient)
                return;

            if (_cargandoCombos)
                return;

            SelectedClientId = int.TryParse(selectedClient.Key, out var clientId) ? clientId : 0;
            SelectedClientText = selectedClient.Value ?? string.Empty;

            try
            {
                _cargandoCombos = true;
                ClearProjectSelection();
                await SetCombosProjectsAsync();
            }
            finally
            {
                _cargandoCombos = false;
            }

            AplicarFiltroAsn();
        }

        private async void LookupProyecto_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            if (sender is not InlineLookupEditor editor || editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not DropDownDto selectedProject)
                return;

            if (_cargandoCombos)
                return;

            SelectedProjectId = int.TryParse(selectedProject.Key, out var projectId) ? projectId : 0;
            SelectedProjectText = selectedProject.Value ?? string.Empty;

            if (SelectedClientId > 0 && SelectedProjectId > 0)
                await CargarDatosConLoaderAsync("Trayendo ASN...");
            else
                AplicarFiltroAsn();
        }

        private async void ChkVerSinConfirmar_Changed(object sender, RoutedEventArgs e)
        {
            if (SelectedClientId > 0 && SelectedProjectId > 0 && _allAsns.Count == 0)
            {
                await CargarDatosConLoaderAsync("Trayendo ASN...");
                return;
            }

            AplicarFiltroAsn();
        }

        private async void BtnNuevoASN_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoASNView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetClientProjectContext(
                SelectedClientId,
                SelectedProjectId,
                SelectedClientText,
                SelectedProjectText);

            var result = dialog.ShowDialog();           
           await CargarDatosConLoaderAsync("Trayendo ASN...");
           
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedX is null)
                return;

            if (IsTerminalStatus(_selectedX.Status))
            {
                DialogHelper.ShowWarning("El ASN seleccionado esta confirmado o cancelado. Ya no se puede editar.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<NuevoASNView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetClientProjectContext(0, 0, _selectedX.Client, _selectedX.Project);
            dialog.SetAsn(_selectedX);

            var result = dialog.ShowDialog();           
                await CargarDatosConLoaderAsync("Trayendo ASN...");
           
        }

        private async void BtnConfirmarLlegada_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedX == null || _selectedX.AsnId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un ASN para confirmar.");
                    return;
                }

                if (IsConfirmedStatus(_selectedX.Status))
                {
                    DialogHelper.ShowWarning("El ASN seleccionado ya esta confirmado.");
                    return;
                }

                if (IsCancelledStatus(_selectedX.Status))
                {
                    DialogHelper.ShowWarning("El ASN seleccionado esta cancelado y no se puede confirmar.");
                    return;
                }

                var confirmar = DialogHelper.ShowConfirm(
                    $"¿Deseas confirmar la entrada del ASN {_selectedX.AsnCode ?? _selectedX.AsnId.ToString()}?",
                    "Confirmar entrada");

                if (!confirmar)
                    return;

                btnConfirmarLlegada.IsEnabled = false;
                MostrarLoader(true, "Confirmando ASN...");

                var result = await _asnService.ConfirmAsn(_selectedX.AsnId);

                if (!result.IsSuccess)
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "No se pudo confirmar el ASN.");
                    return;
                }

                DialogHelper.ShowSuccess(result.Message ?? "ASN confirmado correctamente.");
                var confirmedAsnId = _selectedX.AsnId;
                await CargarDatosAsync();

                _selectedX = _allAsns.FirstOrDefault(x => x.AsnId == confirmedAsnId);
                UpdateActionButtons();
                await CargarDatosAsyncDet();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                MostrarLoader(false);
                UpdateActionButtons();
            }
        }

        private async void BtnUbicarMercancia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedX == null || _selectedX.AsnId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un ASN para ubicar.");
                    return;
                }

                if (IsConfirmedStatus(_selectedX.Status))
                {
                    DialogHelper.ShowWarning("El ASN seleccionado ya esta confirmado y no se puede ubicar.");
                    return;
                }

                if (IsCancelledStatus(_selectedX.Status))
                {
                    DialogHelper.ShowWarning("El ASN seleccionado esta cancelado y no se puede ubicar.");
                    return;
                }

                if (IsLocatingStatus(_selectedX.Status))
                {
                    DialogHelper.ShowWarning("El ASN seleccionado ya esta en estatus Ubicando.");
                    return;
                }

                var confirmar = DialogHelper.ShowConfirm(
                    $"Deseas ubicar la mercancia del ASN {_selectedX.AsnCode ?? _selectedX.AsnId.ToString()}?",
                    "Ubicar mercancia");

                if (!confirmar)
                    return;

                btnUbicarMercancia.IsEnabled = false;
                MostrarLoader(true, "Actualizando ASN...");

                var result = await _asnService.LocateAsn(_selectedX.AsnId);

                if (!result.IsSuccess)
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "No se pudo marcar el ASN como Ubicando.");
                    return;
                }

                DialogHelper.ShowSuccess(result.Message ?? "ASN marcado como Ubicando correctamente.");
                var locatingAsnId = _selectedX.AsnId;
                await CargarDatosAsync();

                _selectedX = _allAsns.FirstOrDefault(x => x.AsnId == locatingAsnId);
                UpdateActionButtons();
                await CargarDatosAsyncDet();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                MostrarLoader(false);
                UpdateActionButtons();
            }
        }

        private void BtnImprimirEtiquetas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var receiptDetails = dgRecepcionASN.Items
                    .OfType<AsnReceiptDetailDto>()
                    .Where(x => !string.IsNullOrWhiteSpace(x.StandardId))
                    .OrderBy(x => x.StandardId)
                    .ToList();

                if (receiptDetails.Count == 0)
                {
                    DialogHelper.ShowWarning("No hay partidas recibidas con StandardId para imprimir.");
                    return;
                }

                AsnReceiptLabelPrinter.PrintLabels(receiptDetails, _selectedX, _selectedDetail);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnExportarExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedX == null || _selectedX.AsnId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un ASN para exportar.");
                    return;
                }

                MostrarLoader(true, "Preparando Excel...");

                var detailsResult = await _asnDetailService.GetAsnDetailsByAsn(_selectedX.AsnId);
                if (!detailsResult.IsSuccess || detailsResult.Data == null || detailsResult.Data.Count == 0)
                {
                    DialogHelper.ShowWarning("El ASN seleccionado no tiene partidas para exportar.");
                    return;
                }

                var receiptDetails = new List<AsnReceiptDetailDto>();
                foreach (var detail in detailsResult.Data)
                {
                    var receiptResult = await _asnReceiptService.GetAsnReceiptsByAsnDetailId(detail.AsnDetailId);
                    if (receiptResult.IsSuccess && receiptResult.Data != null)
                        receiptDetails.AddRange(receiptResult.Data);
                }

                if (receiptDetails.Count == 0)
                {
                    DialogHelper.ShowWarning("El ASN seleccionado no tiene partidas recibidas para exportar.");
                    return;
                }

                var dialog = new SaveFileDialog
                {
                    Title = "Exportar ASN a Excel",
                    Filter = "Archivo de Excel (*.xlsx)|*.xlsx",
                    FileName = $"{SanitizeFileName(_selectedX.AsnCode)}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                    AddExtension = true,
                    DefaultExt = ".xlsx"
                };

                if (dialog.ShowDialog(Window.GetWindow(this)) != true)
                    return;

                ExportAsnReceiptsToExcel(dialog.FileName, _selectedX, receiptDetails);
                DialogHelper.ShowSuccess("Excel exportado correctamente.");
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

        private async void BtnOrdenAlmacenamiento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedX == null || _selectedX.AsnId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un ASN para ver la vista previa de la orden de almacenamiento.");
                    return;
                }

                MostrarLoader(true, "Preparando orden de almacenamiento...");

                var detailsResult = await _asnDetailService.GetAsnDetailsByAsn(_selectedX.AsnId);
                if (!detailsResult.IsSuccess || detailsResult.Data == null || detailsResult.Data.Count == 0)
                {
                    DialogHelper.ShowWarning("El ASN seleccionado no tiene partidas para mostrar en vista previa.");
                    return;
                }

                var receiptDetails = new List<AsnReceiptDetailDto>();
                foreach (var detail in detailsResult.Data)
                {
                    var receiptResult = await _asnReceiptService.GetAsnReceiptsByAsnDetailId(detail.AsnDetailId);
                    if (receiptResult.IsSuccess && receiptResult.Data != null)
                        receiptDetails.AddRange(receiptResult.Data);
                }

                AsnStorageOrderPrinter.PrintOrder(_selectedX, detailsResult.Data, receiptDetails);
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

        private static void ExportAsnReceiptsToExcel(
            string filePath,
            AsnDto asn,
            IEnumerable<AsnReceiptDetailDto> receiptDetails)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            using var archive = ZipFile.Open(filePath, ZipArchiveMode.Create);

            AddZipEntry(
                archive,
                "[Content_Types].xml",
                """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
                  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
                  <Default Extension="xml" ContentType="application/xml"/>
                  <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
                  <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
                  <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
                  <Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml"/>
                  <Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml"/>
                </Types>
                """);

            AddZipEntry(
                archive,
                "_rels/.rels",
                """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
                  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
                  <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" Target="docProps/core.xml"/>
                  <Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties" Target="docProps/app.xml"/>
                </Relationships>
                """);

            AddZipEntry(
                archive,
                "xl/_rels/workbook.xml.rels",
                """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
                  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
                  <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
                </Relationships>
                """);

            AddZipEntry(
                archive,
                "xl/workbook.xml",
                """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
                  <sheets>
                    <sheet name="ASN" sheetId="1" r:id="rId1"/>
                  </sheets>
                </workbook>
                """);

            AddZipEntry(archive, "xl/styles.xml", BuildExcelStylesXml());
            AddZipEntry(archive, "xl/worksheets/sheet1.xml", BuildAsnWorksheetXml(asn, receiptDetails));

            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            AddZipEntry(
                archive,
                "docProps/core.xml",
                $"""
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:dcmitype="http://purl.org/dc/dcmitype/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                  <dc:creator>LD</dc:creator>
                  <cp:lastModifiedBy>LD</cp:lastModifiedBy>
                  <dcterms:created xsi:type="dcterms:W3CDTF">{timestamp}</dcterms:created>
                  <dcterms:modified xsi:type="dcterms:W3CDTF">{timestamp}</dcterms:modified>
                </cp:coreProperties>
                """);

            AddZipEntry(
                archive,
                "docProps/app.xml",
                """
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties" xmlns:vt="http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes">
                  <Application>LD</Application>
                </Properties>
                """);
        }

        private static string BuildAsnWorksheetXml(AsnDto asn, IEnumerable<AsnReceiptDetailDto> receiptDetails)
        {
            var rows = new StringBuilder();
            var rowIndex = 1;

            for (; rowIndex <= 4; rowIndex++)
                rows.Append(Row(rowIndex));

            rows.Append(Row(rowIndex++, TextCell("A5", "Tipo:", 1), TextCell("B5", asn.VehicleType), TextCell("D5", FormatDateTime(asn.Eta))));
            rows.Append(Row(rowIndex++, TextCell("A6", "Factura:", 1), TextCell("B6", asn.InvoiceNumber), TextCell("D6", "Guia:", 1), TextCell("E6", asn.GuideNumber)));
            rows.Append(Row(rowIndex++, TextCell("A7", "Linea:", 1), TextCell("B7", asn.TransportLine), TextCell("D7", "Transporte:", 1), TextCell("E7", asn.Project)));
            rows.Append(Row(rowIndex++, TextCell("A8", "Chofer:", 1), TextCell("B8", asn.DriverName)));
            rows.Append(Row(rowIndex++, TextCell("A9", "Sello:", 1), TextCell("B9", asn.SealNumber), TextCell("D9", "Placas:", 1), TextCell("E9", asn.VehiclePlate)));
            rows.Append(Row(rowIndex++));

            rows.Append(Row(
                rowIndex,
                TextCell($"A{rowIndex}", "FOLIO", 2),
                TextCell($"B{rowIndex}", "No. de parte", 2),
                TextCell($"C{rowIndex}", "Cantidad", 2),
                TextCell($"D{rowIndex}", "Dub", 2),
                TextCell($"E{rowIndex}", "SKID", 2)));

            rowIndex++;

            foreach (var receipt in receiptDetails.OrderBy(x => x.AsnReceiptDetailId))
            {
                var rowStyle = rowIndex % 2 == 0 ? 3 : 0;
                rows.Append(Row(
                    rowIndex,
                    NumberCell($"A{rowIndex}", receipt.AsnReceiptDetailId, rowStyle),
                    TextCell($"B{rowIndex}", receipt.PartNumber, rowStyle),
                    NumberCell($"C{rowIndex}", receipt.ReceivedQuantity ?? 0, rowStyle),
                    TextCell($"D{rowIndex}", receipt.SD, rowStyle),
                    TextCell($"E{rowIndex}", receipt.StandardId ?? string.Empty, rowStyle)));
                rowIndex++;
            }

            return $"""
                <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
                <worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
                  <sheetViews>
                    <sheetView workbookViewId="0"/>
                  </sheetViews>
                  <sheetFormatPr defaultRowHeight="18"/>
                  <cols>
                    <col min="1" max="1" width="10" customWidth="1"/>
                    <col min="2" max="2" width="34" customWidth="1"/>
                    <col min="3" max="3" width="14" customWidth="1"/>
                    <col min="4" max="4" width="14" customWidth="1"/>
                    <col min="5" max="5" width="18" customWidth="1"/>
                    <col min="6" max="6" width="18" customWidth="1"/>
                  </cols>
                  <sheetData>
                {rows}
                  </sheetData>
                </worksheet>
                """;
        }

        private static string BuildExcelStylesXml() =>
            """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
              <fonts count="3">
                <font><sz val="11"/><name val="Calibri"/></font>
                <font><b/><sz val="11"/><name val="Calibri"/></font>
                <font><b/><color rgb="FFFFFFFF"/><sz val="11"/><name val="Calibri"/></font>
              </fonts>
              <fills count="4">
                <fill><patternFill patternType="none"/></fill>
                <fill><patternFill patternType="gray125"/></fill>
                <fill><patternFill patternType="solid"><fgColor rgb="FF4F81BD"/><bgColor indexed="64"/></patternFill></fill>
                <fill><patternFill patternType="solid"><fgColor rgb="FFD8E4F1"/><bgColor indexed="64"/></patternFill></fill>
              </fills>
              <borders count="2">
                <border><left/><right/><top/><bottom/><diagonal/></border>
                <border><left style="thin"><color rgb="FFD9D9D9"/></left><right style="thin"><color rgb="FFD9D9D9"/></right><top style="thin"><color rgb="FFD9D9D9"/></top><bottom style="thin"><color rgb="FFD9D9D9"/></bottom><diagonal/></border>
              </borders>
              <cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>
              <cellXfs count="4">
                <xf numFmtId="0" fontId="0" fillId="0" borderId="1" xfId="0"/>
                <xf numFmtId="0" fontId="1" fillId="0" borderId="1" xfId="0" applyFont="1"/>
                <xf numFmtId="0" fontId="2" fillId="2" borderId="1" xfId="0" applyFont="1" applyFill="1"/>
                <xf numFmtId="0" fontId="0" fillId="3" borderId="1" xfId="0" applyFill="1"/>
              </cellXfs>
              <cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles>
              <dxfs count="0"/>
              <tableStyles count="0" defaultTableStyle="TableStyleMedium2" defaultPivotStyle="PivotStyleLight16"/>
            </styleSheet>
            """;

        private static void AddZipEntry(ZipArchive archive, string entryName, string content)
        {
            var entry = archive.CreateEntry(entryName);
            using var stream = entry.Open();
            using var writer = new StreamWriter(stream, new UTF8Encoding(false));
            writer.Write(content);
        }

        private static string Row(int index, params string[] cells) =>
            $"    <row r=\"{index}\">{string.Concat(cells)}</row>{Environment.NewLine}";

        private static string TextCell(string reference, string? value, int style = 0)
        {
            var escaped = SecurityElement.Escape(value ?? string.Empty) ?? string.Empty;
            return $"<c r=\"{reference}\" s=\"{style}\" t=\"inlineStr\"><is><t>{escaped}</t></is></c>";
        }

        private static string NumberCell(string reference, decimal value, int style = 0) =>
            $"<c r=\"{reference}\" s=\"{style}\"><v>{value.ToString(CultureInfo.InvariantCulture)}</v></c>";

        private static string FormatDateTime(DateTime? dateTime) =>
            dateTime.HasValue
                ? dateTime.Value.ToString("M/d/yy H:mm", CultureInfo.InvariantCulture)
                : DateTime.Now.ToString("M/d/yy H:mm", CultureInfo.InvariantCulture);

        private static string SanitizeFileName(string? fileName)
        {
            var invalid = System.IO.Path.GetInvalidFileNameChars();
            var sanitized = new string((string.IsNullOrWhiteSpace(fileName) ? "ASN" : fileName)
                .Select(ch => invalid.Contains(ch) ? '_' : ch)
                .ToArray());

            return string.IsNullOrWhiteSpace(sanitized) ? "ASN" : sanitized;
        }

        private async void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedX == null || _selectedX.AsnId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un ASN para cancelar.");
                    return;
                }

                if (IsConfirmedStatus(_selectedX.Status))
                {
                    DialogHelper.ShowWarning("El ASN seleccionado ya esta confirmado y no se puede cancelar.");
                    return;
                }

                if (IsCancelledStatus(_selectedX.Status))
                {
                    DialogHelper.ShowWarning("El ASN seleccionado ya esta cancelado.");
                    return;
                }

                var confirmar = DialogHelper.ShowConfirm(
                    $"Deseas cancelar el ASN {_selectedX.AsnCode ?? _selectedX.AsnId.ToString()}?",
                    "Cancelar ASN");

                if (!confirmar)
                    return;

                btnCancelar.IsEnabled = false;
                MostrarLoader(true, "Cancelando ASN...");

                var result = await _asnService.CancelAsn(_selectedX.AsnId);

                if (!result.IsSuccess)
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "No se pudo cancelar el ASN.");
                    return;
                }

                DialogHelper.ShowSuccess(result.Message ?? "ASN cancelado correctamente.");
                var cancelledAsnId = _selectedX.AsnId;
                await CargarDatosAsync();

                _selectedX = _allAsns.FirstOrDefault(x => x.AsnId == cancelledAsnId);
                UpdateActionButtons();
                await CargarDatosAsyncDet();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                MostrarLoader(false);
                UpdateActionButtons();
            }
        }

        private void BtnEscanear_Click(object sender, RoutedEventArgs e) { }

        private static LookupItem ToLookupItem(DropDownDto item)
        {
            return new LookupItem
            {
                Id = int.TryParse(item.Key, out var value) ? value : 0,
                Code = item.Value ?? string.Empty,
                Description = item.Key ?? string.Empty,
                Data = item
            };
        }

        private void ApplyProjectSelection(string projectKey)
        {
            var projectLookup = ProjectLookupItems.FirstOrDefault(x =>
                x.Data is DropDownDto dto && string.Equals(dto.Key, projectKey, StringComparison.OrdinalIgnoreCase));

            if (projectLookup?.Data is not DropDownDto selectedProject)
            {
                ClearProjectSelection();
                return;
            }

            SelectedProjectId = int.TryParse(selectedProject.Key, out var projectId) ? projectId : 0;
            SelectedProjectText = selectedProject.Value ?? string.Empty;
        }

        private void ClearProjectSelection()
        {
            SelectedProjectId = 0;
            SelectedProjectText = string.Empty;
            lookupProyecto?.ClearSelection();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void dgASN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedX = _gridFilter.SelectedItem;
                UpdateActionButtons();
                await CargarDatosAsyncDet();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void dgDetalleASN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedDetail = _gridFilterDet.SelectedItem;
                await CargarDatosAsyncRecepcion();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

    }
}
