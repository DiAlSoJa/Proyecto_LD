using LD.Client.Services;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Proyectos
{
    public partial class NuevoProyectoView : Window
    {
        private readonly ProjectService _projectService;
        private readonly LookupService _lookupService;

        private ProjectDto? _projectSelected;
        public bool ResponseForm { get; private set; }

        public NuevoProyectoView(ProjectService projectService, LookupService lookupService)
        {
            InitializeComponent();
            _projectService = projectService;
            _lookupService = lookupService;
        }

        public async void SetProject(ProjectDto? project)
        {
            _projectSelected = project;
            txtHeaderTitle.Text = "Editar proyecto";
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarCombosAsync();
            if (_projectSelected != null)
                await CargarDatosAsync();
        }

        private async Task CargarCombosAsync()
        {
            var clientes = await _lookupService.GetClientLookup();
            var almacenes = await _lookupService.GetWarehouseLookup();

            if (clientes.IsSuccess)
            {
                cmbCliente.ItemsSource = clientes.Data;
            }
            if (almacenes.IsSuccess)
            {
                cmbAlmacen.ItemsSource = almacenes.Data;
            }
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _projectService.GetProjectById(_projectSelected?.ProjectId ?? 0);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message);
                    return;
                }

                var p = response.Data;

                cmbCliente.SelectedValue  = p.ClientId?.ToString();
                cmbAlmacen.SelectedValue  = p.WarehouseId?.ToString();
                txtProjectName.Text        = p.ProjectName;

                checkActivo.IsChecked       = p.IsActive;
                checkAutoPicking.IsChecked  = p.AutoPicking;

                radioFifo.IsChecked      = p.StorageTypeId == 1;
                radioLifo.IsChecked      = p.StorageTypeId == 2;
                radioLote.IsChecked      = p.StorageTypeId == 3;
                radioCaducidad.IsChecked = p.StorageTypeId == 4;

                checkBackorder.IsChecked      = p.AllowsBackorder;
                checkDistribucion.IsChecked   = p.IsDistributionArea;
                checkAlmacenFiscal.IsChecked  = p.IsFiscalWarehouse;
                checkSobredimension.IsChecked = p.AllowsOversizedItems;
                checkEtiquetas.IsChecked      = p.RequiresLabels;

                cmbEntrada.Text        = p.Entrada;
                cmbAlmacenamiento.Text = p.StorageArea;
                cmbRetrabajo.Text      = p.ReworkArea;
                cmbSalida.Text         = p.Salida;

                checkNotRecibo.IsChecked   = p.ReceiptNotificationEnabled;
                cmbNotRecibo.Text          = p.ReceiptNotificationMethod ?? "";
                checkNotEmbarque.IsChecked = p.ShipmentNotificationEnabled;
                cmbNotEmbarque.Text        = p.ShipmentNotificationMethod ?? "";

                txtTiempoNormal.Text  = p.NormalHrs?.ToString() ?? "";
                txtTiempoUrgente.Text = p.UrgentHrs?.ToString() ?? "";

                txtNumeroAsn.Text   = p.AsnNumber   ?? "";
                txtPrefijoAsn.Text  = p.AsnPrefix   ?? "";
                txtNumeroKitting.Text  = p.KittingNumber  ?? "";
                txtPrefijoKitting.Text = p.KittingPrefix  ?? "";
                txtNumeroDo.Text    = p.DoNumber  ?? p.DeliveryOrderNumber ?? "";
                txtPrefijoDo.Text   = p.DoPrefix  ?? p.DeliveryOrderPrefix ?? "";
                checkRegistroRequerido.IsChecked = p.ReciveRequired;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private ProjectRequest BuildRequest() => new()
        {
            ProjectId   = _projectSelected?.ProjectId,
            ClientId    = int.TryParse(cmbCliente.SelectedValue?.ToString(), out int cId) ? cId : null,
            WarehouseId = int.TryParse(cmbAlmacen.SelectedValue?.ToString(), out int wId) ? wId : null,
            ProjectName = txtProjectName.Text.Trim(),
            IsActive    = checkActivo.IsChecked    ?? false,
            AutoPicking = checkAutoPicking.IsChecked ?? false,

            StorageTypeId = radioFifo.IsChecked      == true ? 1 :
                            radioLifo.IsChecked      == true ? 2 :
                            radioLote.IsChecked      == true ? 3 :
                            radioCaducidad.IsChecked == true ? 4 : null,

            AllowsBackorder      = checkBackorder.IsChecked      ?? false,
            IsDistributionArea   = checkDistribucion.IsChecked   ?? false,
            IsFiscalWarehouse    = checkAlmacenFiscal.IsChecked  ?? false,
            AllowsOversizedItems = checkSobredimension.IsChecked ?? false,
            RequiresLabels       = checkEtiquetas.IsChecked      ?? false,

            Entrada      = cmbEntrada.Text,
            StorageArea  = cmbAlmacenamiento.Text,
            ReworkArea   = cmbRetrabajo.Text,
            Salida       = cmbSalida.Text,

            ReceiptNotificationEnabled   = checkNotRecibo.IsChecked    ?? false,
            ReceiptNotificationMethod    = cmbNotRecibo.Text,
            ShipmentNotificationEnabled  = checkNotEmbarque.IsChecked  ?? false,
            ShipmentNotificationMethod   = cmbNotEmbarque.Text,
            InternalNotificationEnabled  = false,
            InternalNotificationMethod   = null,

            NormalHrs  = decimal.TryParse(txtTiempoNormal.Text,  out decimal n) ? n : null,
            UrgentHrs  = decimal.TryParse(txtTiempoUrgente.Text, out decimal u) ? u : null,

            AsnNumber      = txtNumeroAsn.Text,
            AsnPrefix      = txtPrefijoAsn.Text,
            KittingNumber  = txtNumeroKitting.Text,
            KittingPrefix  = txtPrefijoKitting.Text,
            DoNumber       = txtNumeroDo.Text,
            DoPrefix       = txtPrefijoDo.Text,
            DeliveryOrderNumber = txtNumeroDo.Text,
            DeliveryOrderPrefix = txtPrefijoDo.Text,
            ReciveRequired = checkRegistroRequerido.IsChecked ?? false,
        };

        private Task<ApiResponseDto<string>> SaveProject(ProjectRequest request) =>
            _projectSelected != null
                ? _projectService.UpdateProject(_projectSelected.ProjectId, request)
                : _projectService.CreateProject(request);

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;
                var request = BuildRequest();
                var result  = await SaveProject(request);

                if (result.IsSuccess)
                {
                    DialogHelper.ShowSuccess(result.Data ?? "Operación realizada correctamente.");
                    ResponseForm = true;
                    DialogResult = true;
                }
                else
                {
                    DialogHelper.ShowError(result.Message);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}

              
