using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
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

namespace LD.FormsX.Views.Dialogs
{
    public partial class NuevoASNView : Window
    {
        private readonly AsnService _asnService;
        private readonly AsnDetailService _asnDetailService;
        private readonly IServiceProvider _serviceProvider;
        private readonly LookupService _lookupService;
        private readonly ProductService _productService;
        private readonly InventaryStatusService _inventaryStatusService;
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
            _serviceProvider = serviceProvider;
            _lookupService = lookupService;
            _productService = productService;
            _inventaryStatusService = inventaryStatusService;
            DataContext = this;

            HideScanSection();
        }

        public async void SetAsn(AsnDto? _asnSelected)
        {
            AsnSelected = _asnSelected;

            // Si es edición y quieres mostrar la sección:
            if (AsnSelected != null)
                ShowScanSection();



            await CargarDatosAsync();
            await CargarDatosAsyncDet();
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
                        CustomerReference = dto.CustomerReference
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
                    await CargarDatosAsync();
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
            // Aquí va tu lógica de guardado
            try
            {
                btnGuardar.IsEnabled = false;

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

                if (result.IsSuccess)
                {
                    ToastHelper.ShowSuccess("ASN guardado exitosamente.");
                    ShowScanSection();
                }
                else
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? "Hubo un error al guardar.");
                }
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

        private void PartNumberLookup_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is not InlineLookupEditor editor)
                    return;

                if (editor.SelectedLookupItem is not LookupItem lookupItem)
                    return;

                if (editor.DataContext is not AsnDetailItem row)
                    return;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    CommitCurrentDetailEdit();
                    MoverFocoASiguienteCelda(row);
                }), DispatcherPriority.Background);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void StatusLookup_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            AdvanceAfterInlineLookupSelection(sender);
        }

        private void SdLookup_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            AdvanceAfterInlineLookupSelection(sender);
        }

        private void AdvanceAfterInlineLookupSelection(object sender)
        {
            try
            {
                if (sender is not InlineLookupEditor editor)
                    return;

                if (editor.DataContext is not AsnDetailItem row)
                    return;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    CommitCurrentDetailEdit();
                    MoverFocoASiguienteCelda(row);
                }), DispatcherPriority.Background);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void CommitCurrentDetailEdit()
        {
            dgDetail.CommitEdit(DataGridEditingUnit.Cell, true);
            dgDetail.CommitEdit(DataGridEditingUnit.Row, true);
        }
        private void MoverFocoASiguienteCelda(AsnDetailItem row)
        {
            try
            {
                if (dgDetail.CurrentCell.Column == null)
                    return;

                int currentIndex = dgDetail.Columns.IndexOf(dgDetail.CurrentCell.Column);
                if (currentIndex < 0)
                    return;

                var nextColumn = dgDetail.Columns
                    .Skip(currentIndex + 1)
                    .FirstOrDefault(c => !c.IsReadOnly);

                if (nextColumn == null)
                    return;

                dgDetail.CurrentCell = new DataGridCellInfo(row, nextColumn);
                dgDetail.ScrollIntoView(row, nextColumn);
                dgDetail.UpdateLayout();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    dgDetail.Focus();
                    var cell = GetDataGridCell(row, nextColumn);
                    if (cell != null)
                    {
                        cell.Focus();
                    }

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        dgDetail.BeginEdit();

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            var refreshedCell = GetDataGridCell(row, nextColumn);
                            if (refreshedCell == null)
                                return;

                            FocusEditableContent(refreshedCell);
                        }), DispatcherPriority.Input);
                    }), DispatcherPriority.Input);
                }), DispatcherPriority.Background);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private DataGridCell? GetDataGridCell(object item, DataGridColumn column)
        {
            var rowContainer = dgDetail.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
            if (rowContainer == null)
                return null;

            var presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
            if (presenter == null)
            {
                dgDetail.ScrollIntoView(item, column);
                rowContainer.UpdateLayout();
                presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
            }

            return presenter?.ItemContainerGenerator.ContainerFromIndex(column.DisplayIndex) as DataGridCell;
        }

        private void FocusEditableContent(DataGridCell cell)
        {
            if (FindVisualChild<InlineLookupEditor>(cell) is InlineLookupEditor inlineLookup)
            {
                inlineLookup.Focus();
                Keyboard.Focus(inlineLookup);
                return;
            }

            if (FindVisualChild<TextBox>(cell) is TextBox textBox)
            {
                textBox.Focus();
                textBox.SelectAll();
                Keyboard.Focus(textBox);
                return;
            }

            cell.Focus();
            Keyboard.Focus(cell);
        }

        private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T correctlyTyped)
                    return correctlyTyped;

                var descendant = FindVisualChild<T>(child);
                if (descendant != null)
                    return descendant;
            }

            return null;
        }







        private void dgDetail_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (DetailItems == null)
                return;
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

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!dgDetail.IsKeyboardFocusWithin)
                        dgDetail.Focus();

                    dgDetail.BeginEdit();
                }), DispatcherPriority.Background);
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
                    FindVisualParent<InlineLookupEditor>(source) != null)
                {
                    return;
                }

                if (e.Key != Key.F4 && e.Key != Key.Enter)
                    return;

                if (dgDetail.CurrentCell == null || dgDetail.CurrentItem is not AsnDetailItem row)
                    return;

                var currentColumn = dgDetail.CurrentCell.Column;
                if (currentColumn == null)
                    return;

                if (e.Key == Key.Enter)
                {
                    CommitCurrentDetailEdit();
                    MoverFocoASiguienteCelda(row);
                    e.Handled = true;
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

        private static T? FindVisualParent<T>(DependencyObject? child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T parent)
                    return parent;

                child = VisualTreeHelper.GetParent(child);
            }

            return null;
        }




    }
}
