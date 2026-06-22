using LD.Client.Services;
using LD.Client.Configuration;
using LD.Contracts.DTOs;
using LD.Contracts.Constants;
using LD.Contracts.Kitting;
using LD.FormsX.Features.Common;
using LD.FormsX.Features.Surtidos.Views;
using LD.FormsX.Helpers;
using LD.FormsX.Model.Lookup;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;

namespace LD.FormsX.Features.Embarques.Views
{
    public partial class EmbarquesView : UserControl, INotifyPropertyChanged
    {
        private readonly KittingService _kittingService;
        private readonly KittingDetailService _kittingDetailService;
        private readonly KittingIssueService _kittingIssueService;
        private readonly LookupService _lookupService;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<KittingDto> _gridFilter;
        private readonly WpfGridFilter<KittingDetailDto> _gridFilterDet;
        private readonly WpfGridFilter<KittingIssueDetailDto> _gridFilterIssue;

        private KittingDto? _selectedKitting;
        private KittingDetailDto? _selectedDetail;
        private List<KittingDto> _selectedKittings = new();
        private List<KittingDto> _allKittings = new();
        private List<UserProjectClientDto> _userProjectClients = new();
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

        public EmbarquesView(
            KittingService kittingService,
            KittingDetailService kittingDetailService,
            KittingIssueService kittingIssueService,
            LookupService lookupService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = this;

            _kittingService = kittingService;
            _kittingDetailService = kittingDetailService;
            _kittingIssueService = kittingIssueService;
            _lookupService = lookupService;
            _serviceProvider = serviceProvider;

            _gridFilter = new WpfGridFilter<KittingDto>(dgKitting);
            _gridFilterDet = new WpfGridFilter<KittingDetailDto>(dgDetalleKitting);
            _gridFilterIssue = new WpfGridFilter<KittingIssueDetailDto>(dgIssueKitting);

            _gridFilter.SetHiddenColumns(
                "KittingId",
                "Warehouse",
                "GuideNumber",
                "Eta",
                "PackagesQty",
                "IsReturn",
                "IsCustomerMovementRequired",
                "Colonia",
                "Ciudad",
                "Telefono",
                "CodigoPostal",
                "FechaProgramada");
            _gridFilter.SetColumnOrder(
                "KittingCode",
                "Client",
                "Project",
                "InvoiceNumber",
                "TransportLine",
                "VehicleType",
                "DriverName",
                "VehiclePlate",
                "SealNumber",
                "Status",
                "Contacto",
                "Direccion",
                "TipoEntrega");
            _gridFilterDet.SetHiddenColumns("KittingDetailId", "KittingId", "ProductId");
            _gridFilterDet.SetColumnOrder(
                "PartNumber",
                "Description",
                "Quantity",
                "CantidadSurtida",
                "Status",
                "SD",
                "LotNumber",
                "ExpirationDate",
                "CustomerReference",
                "ExchangeRate",
                "PurchaseOrder",
                "CustomsDeclarationNumber");
            _gridFilterIssue.SetHiddenColumns("KittingReceiptDetailId", "KittingDetailId", "ProductId", "LocationId");

            cmbFiltroEmbarques.SelectedIndex = 0;
            UpdateActionButtons();
        }

        private static bool IsConfirmedStatus(string? status) =>
            string.Equals(status?.Trim(), KittingStatusNames.Confirmado, StringComparison.OrdinalIgnoreCase) ||
            KittingStatusNames.IsValidation(status);

        private static bool IsCancelledStatus(string? status) =>
            KittingStatusNames.IsCancelled(status);

        private static bool IsValidatedStatus(string? status) =>
            KittingStatusNames.IsLoading(status);

        private static bool IsSurtidoStatus(string? status) =>
            KittingStatusNames.IsValidation(status);

        private static bool IsSurtiendoStatus(string? status) =>
            string.Equals(status?.Trim(), KittingStatusNames.Surtiendo, StringComparison.OrdinalIgnoreCase);

        private static bool IsEmbarcadoStatus(string? status) =>
            string.Equals(status?.Trim(), "Embarcado", StringComparison.OrdinalIgnoreCase);

        private List<KittingDto> GetSelectedKittings()
        {
            if (dgKitting?.SelectedItems == null || dgKitting.SelectedItems.Count == 0)
                return new List<KittingDto>();

            return dgKitting.SelectedItems
                .OfType<KittingDto>()
                .ToList();
        }

        private bool HasMultipleKittingSelection() =>
            _selectedKittings.Count > 1;

        private static bool IsSendingStatus(string? status) =>
            KittingStatusNames.IsSending(status);

        private static string GetTerminalStatusLabel(string? status)
        {
            if (IsCancelledStatus(status))
                return "cancelado";

            if (IsValidatedStatus(status))
                return "cargando";

            if (IsSurtidoStatus(status))
                return "validación";

            return "confirmado";
        }

        private static bool IsTerminalStatus(string? status) =>
            IsConfirmedStatus(status) || IsValidatedStatus(status) || IsCancelledStatus(status);

        private void dgKitting_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!string.Equals(e.PropertyName, nameof(KittingDto.Status), StringComparison.OrdinalIgnoreCase))
                return;

            if (e.Column is not DataGridTextColumn textColumn)
                return;

            textColumn.Binding = new Binding(nameof(KittingDto.Status))
            {
                Converter = new KittingStatusDisplayConverter()
            };
        }

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
            var hasSelected = _selectedKittings.Count > 0;
            var hasMultipleSelection = HasMultipleKittingSelection();
            var isSending = IsSendingStatus(_selectedKitting?.Status);
            var isTerminal = IsTerminalStatus(_selectedKitting?.Status);
            var canEdit = hasSelected && !isTerminal;
            var terminalStatus = GetTerminalStatusLabel(_selectedKitting?.Status);
            var confirmTooltip = !hasSelected
                ? "Selecciona un embarque para pasar a validación."
                : isTerminal
                    ? $"Este embarque esta {terminalStatus}. Ya no se puede pasar a validación."
                    : !isSending
                        ? "El embarque debe estar en estatus Surtiendo para poder pasar a validación."
                        : "Pasar a validación";

            if (btnEditar != null)
                btnEditar.Visibility = hasSelected ? Visibility.Visible : Visibility.Collapsed;

            ConfigureActionButton(
                btnEditar,
                hasSelected,
                false,
                hasMultipleSelection
                    ? $"Editar transporte y entrega de {_selectedKittings.Count} embarques."
                    : "Editar transporte y entrega");

            ConfigureActionButton(
                btnEnviarASurtir,
                canEdit && !isSending && !hasMultipleSelection,
                hasSelected && isTerminal,
                hasSelected && isTerminal
                    ? $"Este embarque esta {terminalStatus}. Ya no se puede enviar a surtir."
                    : hasSelected && isSending
                        ? "El embarque ya esta en estatus Surtiendo."
                        : hasSelected
                            ? "Enviar embarque a surtir"
                            : "Selecciona un embarque para enviar a surtir.");

            ConfigureActionButton(
                btnConfirmar,
                hasSelected && isSending && !hasMultipleSelection,
                hasSelected && isTerminal,
                confirmTooltip);

            ConfigureActionButton(
                btnListaSurtido,
                hasSelected && !hasMultipleSelection,
                false,
                hasSelected
                    ? "Imprimir lista de validación"
                    : "Selecciona un embarque para imprimir la lista de validación.");

            ConfigureActionButton(
                btnValidar,
                hasSelected && !hasMultipleSelection,
                false,
                hasSelected
                    ? "Abrir el dialogo de validación del embarque."
                    : "Selecciona un embarque para validar.");

            ConfigureActionButton(
                btnImprimirDO,
                hasSelected && !hasMultipleSelection,
                false,
                hasSelected
                    ? "Imprimir orden de entrega DO"
                    : "Selecciona un embarque para imprimir el DO.");

            ConfigureActionButton(
                btnCancelar,
                canEdit && !hasMultipleSelection,
                hasSelected && isTerminal,
                hasSelected && isTerminal
                    ? $"Este embarque esta {terminalStatus}. Ya no se puede cancelar."
                    : hasSelected
                        ? "Cancelar embarque"
                        : "Selecciona un embarque para cancelar.");
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded)
                return;

            _loaded = true;
            await CargarCombosAsync();
            _gridFilter.SetData(null);
            _gridFilterDet.SetData(null);
            _gridFilterIssue.SetData(null);
            txtStatusDetalle.Text = "Sin detalle para mostrar";
            txtStatusIssues.Text = "Sin issues para mostrar";
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
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
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

        private async Task CargarDatosConLoaderAsync()
        {
            try
            {
                await CargarDatosAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task CargarDatosAsync()
        {
            var result = await _kittingService.GetKittings();
            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _allKittings = result.Data ?? new List<KittingDto>();
            AplicarFiltroKitting();
        }

        private void AplicarFiltroKitting()
        {
            if (_gridFilter == null)
                return;

            var filtered = _allKittings.AsEnumerable();

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

            if (IsCargandoFilterSelected())
                filtered = filtered.Where(x => IsValidatedStatus(x.Status));
            else
                filtered = filtered.Where(x => IsSurtidoStatus(x.Status));

            _gridFilter.SetData(filtered.ToList());
            _selectedKitting = null;
            _selectedDetail = null;
            _gridFilterDet.SetData(null);
            _gridFilterIssue.SetData(null);
            txtStatusDetalle.Text = "Sin detalle para mostrar";
            txtStatusIssues.Text = "Sin issues para mostrar";
            UpdateActionButtons();
        }

        private async Task CargarDatosAsyncDet()
        {
            _selectedDetail = null;
            _gridFilterIssue.SetData(null);
            txtStatusIssues.Text = "Sin issues para mostrar";

            if (_selectedKitting == null)
            {
                _gridFilterDet.SetData(null);
                txtStatusDetalle.Text = "Sin detalle para mostrar";
                return;
            }

            var result = await _kittingDetailService.GetKittingDetailsByKittingId(_selectedKitting.KittingId);
            if (!result.IsSuccess || result.Data == null)
            {
                _gridFilterDet.SetData(null);
                txtStatusDetalle.Text = "Sin detalle para mostrar";
                return;
            }

            _gridFilterDet.SetData(result.Data);
            txtStatusDetalle.Text = $"Registros: {result.Data.Count}";
        }

        private async Task CargarDatosAsyncIssue()
        {
            if (_selectedDetail == null || _selectedDetail.KittingDetailId <= 0)
            {
                _gridFilterIssue.SetData(null);
                txtStatusIssues.Text = "Sin issues para mostrar";
                return;
            }

            var result = await _kittingIssueService.GetKittingIssuesByKittingDetailId(_selectedDetail.KittingDetailId);
            if (!result.IsSuccess || result.Data == null)
            {
                _gridFilterIssue.SetData(null);
                txtStatusIssues.Text = "Sin issues para mostrar";
                return;
            }

            _gridFilterIssue.SetData(result.Data);
            txtStatusIssues.Text = result.Data.Count > 0
                ? $"Registros: {result.Data.Count}"
                : "Sin issues para mostrar";
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

            await CargarDatosConLoaderAsync();
        }

        private async void LookupCliente_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            if (sender is not InlineLookupEditor editor || editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not DropDownDto selectedClient || _cargandoCombos)
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

            AplicarFiltroKitting();
        }

        private async void LookupProyecto_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            if (sender is not InlineLookupEditor editor || editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not DropDownDto selectedProject || _cargandoCombos)
                return;

            SelectedProjectId = int.TryParse(selectedProject.Key, out var projectId) ? projectId : 0;
            SelectedProjectText = selectedProject.Value ?? string.Empty;

            if (SelectedClientId > 0 && SelectedProjectId > 0)
                await CargarDatosConLoaderAsync();
            else
                AplicarFiltroKitting();
        }

        private bool IsCargandoFilterSelected()
        {
            if (cmbFiltroEmbarques?.SelectedItem is ComboBoxItem selectedItem)
                return string.Equals(selectedItem.Tag?.ToString()?.Trim(), KittingStatusNames.Cargando, StringComparison.OrdinalIgnoreCase);

            return false;
        }

        private async void CmbFiltroEmbarques_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_gridFilter == null || _gridFilterDet == null || _gridFilterIssue == null)
                return;

            if (SelectedClientId > 0 && SelectedProjectId > 0 && _allKittings.Count == 0)
            {
                await CargarDatosConLoaderAsync();
                return;
            }

            AplicarFiltroKitting();
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoSurtidoView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetClientProjectContext(
                SelectedClientId,
                SelectedProjectId,
                SelectedClientText,
                SelectedProjectText);

            dialog.ShowDialog();
            if (dialog.HasChanges)
            {
                await CargarDatosConLoaderAsync();
            }
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var selectedKittings = GetSelectedKittings();
            if (selectedKittings.Count == 0)
            {
                DialogHelper.ShowWarning("Selecciona uno o más embarques para editar.");
                return;
            }

            if (selectedKittings.Any(item => IsEmbarcadoStatus(item.Status)))
            {
                DialogHelper.ShowWarning("Uno o más embarques seleccionados estan embarcados. Ya no se pueden editar.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<EditarSurtidoView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetTransportAndDeliveryOnlyMode();
            if (selectedKittings.Count == 1)
            {
                dialog.SetKitting(selectedKittings[0]);
            }
            else
            {
                dialog.SetKittings(selectedKittings);
            }

            dialog.ShowDialog();
            if (dialog.HasChanges)
            {
                await CargarDatosConLoaderAsync();
            }
        }

        private async void BtnEnviarASurtir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedKitting == null || _selectedKitting.KittingId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un embarque para enviar a surtir.");
                    return;
                }

                if (IsTerminalStatus(_selectedKitting.Status))
                {
                    DialogHelper.ShowWarning($"El embarque seleccionado ya esta {GetTerminalStatusLabel(_selectedKitting.Status)} y no se puede enviar a surtir.");
                    return;
                }

                if (IsSendingStatus(_selectedKitting.Status))
                {
                    DialogHelper.ShowWarning("El embarque seleccionado ya esta en estatus Surtiendo.");
                    return;
                }

                if (!DialogHelper.ShowConfirm("¿Está seguro de enviar a surtir?"))
                    return;

                var result = await _kittingService.SendToSupplyKitting(_selectedKitting.KittingId);
                if (!result.IsSuccess)
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "No se pudo cambiar el embarque a Surtiendo.");
                    return;
                }

                DialogHelper.ShowSuccess(result.Message ?? "Se cambió el embarque a estatus Surtiendo correctamente.");
                await CargarDatosConLoaderAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedKitting == null || _selectedKitting.KittingId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un embarque para surtir.");
                    return;
                }

                if (IsTerminalStatus(_selectedKitting.Status))
                {
                    DialogHelper.ShowWarning($"El embarque seleccionado ya esta {GetTerminalStatusLabel(_selectedKitting.Status)} y no se puede surtir.");
                    return;
                }

                if (!IsSendingStatus(_selectedKitting.Status))
                {
                    DialogHelper.ShowWarning("El embarque debe estar en estatus Surtiendo para poder pasar a validación.");
                    return;
                }

                if (!DialogHelper.ShowConfirm("¿Está seguro de surtir?"))
                    return;

                var result = await _kittingService.ConfirmKitting(_selectedKitting.KittingId);
                if (!result.IsSuccess)
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "No se pudo surtir el registro.");
                    return;
                }

                DialogHelper.ShowSuccess(result.Message ?? "Se cambió el embarque a estatus Validación correctamente.");
                await CargarDatosConLoaderAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedKitting == null || _selectedKitting.KittingId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un embarque para cancelar.");
                    return;
                }

                if (IsTerminalStatus(_selectedKitting.Status))
                {
                    DialogHelper.ShowWarning($"El embarque seleccionado ya esta {GetTerminalStatusLabel(_selectedKitting.Status)} y no se puede cancelar.");
                    return;
                }

                var confirmar = DialogHelper.ShowConfirm(
                    $"Deseas cancelar el embarque {_selectedKitting.KittingCode ?? _selectedKitting.KittingId.ToString()}?",
                    "Cancelar embarque");

                if (!confirmar)
                    return;

                var result = await _kittingService.CancelKitting(_selectedKitting.KittingId);
                if (!result.IsSuccess)
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "No se pudo cancelar el embarque.");
                    return;
                }

                DialogHelper.ShowSuccess(result.Message ?? "Se canceló el embarque correctamente.");
                await CargarDatosConLoaderAsync();
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
                if (_selectedKitting == null || _selectedKitting.KittingId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un embarque para imprimir la lista de validación.");
                    return;
                }

                var rows = await BuildListaSurtidoRowsAsync(_selectedKitting);
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

        private async void BtnValidar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedKitting == null || _selectedKitting.KittingId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un embarque para validar.");
                    return;
                }

            var dialog = _serviceProvider.GetRequiredService<ValidarEmbarqueDialog>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetKitting(_selectedKitting);
            dialog.ShowDialog();
            if (dialog.HasChanges)
            {
                await CargarDatosConLoaderAsync();
            }
        }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnImprimirDO_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedKitting == null || _selectedKitting.KittingId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un embarque para ver la vista previa del DO.");
                    return;
                }

                var rows = await BuildListaSurtidoRowsAsync(_selectedKitting);
                if (rows.Count == 0)
                {
                    DialogHelper.ShowWarning("No hay lineas para mostrar en vista previa del DO.");
                    return;
                }

                EmbarquesDoPrinter.Print(_selectedKitting, rows);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

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

        private async void dgKitting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedKittings = GetSelectedKittings();
                _selectedKitting = _selectedKittings.FirstOrDefault();
                UpdateActionButtons();

                if (_selectedKittings.Count == 1)
                {
                    await CargarDatosAsyncDet();
                }
                else
                {
                    _selectedDetail = null;
                    _gridFilterDet.SetData(null);
                    _gridFilterIssue.SetData(null);
                    txtStatusDetalle.Text = _selectedKittings.Count > 1
                        ? $"Seleccion multiple: {_selectedKittings.Count} embarques"
                        : "Sin detalle para mostrar";
                    txtStatusIssues.Text = "Sin issues para mostrar";
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void dgDetalleKitting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedDetail = _gridFilterDet.SelectedItem;
                await CargarDatosAsyncIssue();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task<List<ListaSurtidoPrinter.ListaSurtidoRow>> BuildListaSurtidoRowsAsync(KittingDto kitting)
        {
            var rows = new List<ListaSurtidoPrinter.ListaSurtidoRow>();

            var detailsResponse = await _kittingDetailService.GetKittingDetailsByKittingId(kitting.KittingId);
            if (!detailsResponse.IsSuccess || detailsResponse.Data == null)
                return rows;

            foreach (var detail in detailsResponse.Data)
            {
                if (detail.KittingDetailId <= 0)
                    continue;

                var issuesResponse = await _kittingIssueService.GetKittingIssuesByKittingDetailId(detail.KittingDetailId);
                if (!issuesResponse.IsSuccess || issuesResponse.Data == null)
                    continue;

                foreach (var issue in issuesResponse.Data.Where(x => x.ReceivedQuantity.GetValueOrDefault() > 0))
                {
                    rows.Add(new ListaSurtidoPrinter.ListaSurtidoRow
                    {
                        PartNumber = issue.PartNumber?.Trim() ?? string.Empty,
                        Description = issue.Description?.Trim() ?? string.Empty,
                        Quantity = issue.ReceivedQuantity.GetValueOrDefault(),
                        Status = issue.Status?.Trim() ?? string.Empty,
                        LotNumber = issue.LotNumber?.Trim() ?? string.Empty,
                        SD = issue.SD?.Trim() ?? string.Empty,
                        LocationCode = issue.LocationCode?.Trim() ?? string.Empty
                    });
                }
            }

            return rows;
        }
    }
}





