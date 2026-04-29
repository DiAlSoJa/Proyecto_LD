using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private bool _cargandoCombos;
        private bool _loaded;
        private int _selectedClientId;
        private int _selectedProjectId;
        private string _selectedClientText = string.Empty;
        private string _selectedProjectText = string.Empty;

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
            var isTerminal = isConfirmed || isCancelled;
            var canClick = hasSelectedAsn && !isTerminal;
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

                var clientes = await _lookupService.GetClientLookup();
                if (clientes.IsSuccess && clientes.Data != null)
                {
                    ClientLookupItems.Clear();

                    foreach (var item in clientes.Data.Select(ToLookupItem).OrderBy(x => x.Code))
                        ClientLookupItems.Add(item);
                }

                ClearProjectSelection();
                ProjectLookupItems.Clear();
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private async Task SetCombosProjectsAsync(string projectSel = "")
        {
            if (SelectedClientId <= 0)
            {
                ProjectLookupItems.Clear();
                ClearProjectSelection();
                return;
            }

            var proyectos = await _lookupService.GetProjectClientLookup(SelectedClientId);

            if (!proyectos.IsSuccess || proyectos.Data == null)
            {
                ProjectLookupItems.Clear();
                ClearProjectSelection();
                return;
            }

            ProjectLookupItems.Clear();
            foreach (var item in proyectos.Data.Select(ToLookupItem).OrderBy(x => x.Code))
                ProjectLookupItems.Add(item);

            if (!string.IsNullOrWhiteSpace(projectSel))
                ApplyProjectSelection(projectSel);
            else
                ClearProjectSelection();
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

        private void LookupProyecto_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            if (sender is not InlineLookupEditor editor || editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not DropDownDto selectedProject)
                return;

            if (_cargandoCombos)
                return;

            SelectedProjectId = int.TryParse(selectedProject.Key, out var projectId) ? projectId : 0;
            SelectedProjectText = selectedProject.Value ?? string.Empty;

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

        private async void BtnOrdenAlmacenamiento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedX == null || _selectedX.AsnId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un ASN para imprimir la orden de almacenamiento.");
                    return;
                }

                MostrarLoader(true, "Preparando orden de almacenamiento...");

                var detailsResult = await _asnDetailService.GetAsnDetailsByAsn(_selectedX.AsnId);
                if (!detailsResult.IsSuccess || detailsResult.Data == null || detailsResult.Data.Count == 0)
                {
                    DialogHelper.ShowWarning("El ASN seleccionado no tiene partidas para imprimir.");
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
