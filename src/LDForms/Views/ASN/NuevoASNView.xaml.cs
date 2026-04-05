using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LD.Client.Services;
using LD.Contracts.ASN;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using LD.FormsX.ViewModels.ASN;
using LD.FormsX.Views.Articulos;

namespace LD.FormsX.Views.Dialogs
{
    public partial class NuevoASNView : Window
    {
        private readonly AsnService _asnService;
        private readonly IServiceProvider _serviceProvider;
        private readonly LookupService _lookupService;
        private readonly ProductService _productService;
        private AsnDto? AsnSelected;
        private bool _cargandoDatos = false;
        public ObservableCollection<AsnDetailRowVm> DetailItems { get; set; } = new();

        public NuevoASNView(AsnService asnService,ProductService productService, LookupService lookupService,IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _asnService = asnService;
            _serviceProvider = serviceProvider;
            _lookupService = lookupService;
            _productService = productService;
            DataContext = this;
            HideScanSection();
        }

        public async void SetAsn(AsnDto? _asnSelected)
        {
            AsnSelected = _asnSelected;

            // Si es edición y quieres mostrar la sección:
             if (AsnSelected != null)
                 ShowScanSection();

           /* if (DetailItems.Count == 0)
                DetailItems.Add(new AsnDetailRowVm());*/

            await CargarDatosAsync();
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
        private async void BtnBuscarProductoDetalle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbCliente.SelectedValue == null || !int.TryParse(cmbCliente.SelectedValue.ToString(), out int clientId) || clientId <= 0)
                {
                    DialogHelper.ShowWarning("Primero selecciona un cliente.");
                    return;
                }

                if (cmbProyecto.SelectedValue == null || !int.TryParse(cmbProyecto.SelectedValue.ToString(), out int projectId) || projectId <= 0)
                {
                    DialogHelper.ShowWarning("Primero selecciona un proyecto.");
                    return;
                }

                if (sender is not FrameworkElement fe || fe.Tag is not AsnDetailRowVm row)
                    return;

                var dlg = new ProductLookupWindow(_productService, clientId, projectId);
                dlg.Owner = this;

                var result = dlg.ShowDialog();
                if (result == true && dlg.SelectedProduct != null)
                {
                    row.ProductId = dlg.SelectedProduct.ProductId;
                    row.NumeroParte = dlg.SelectedProduct.PartNumber ?? string.Empty;
                    row.Descripcion = dlg.SelectedProduct.Description ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
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



    }
}