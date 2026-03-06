using LD.Contracts.Item;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Services;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevoProyecto : DraggableForm
    {

        private readonly ProjectService _projectService;
        private ProjectDto? ProjectSelected;
        public FrmNuevoProyecto(ProjectService projectService)
        {
            InitializeComponent();
            _projectService = projectService;
            EnableDrag(panel2);
            EnableDrag(panel1);

        }

        public async void SetProject(ProjectDto? project)
        {
            ProjectSelected = project;
            await CargarDatosAsync();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);



        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _projectService.GetProjectById(ProjectSelected?.ProjectId ?? 0);

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var project = response.Data;
                comboCliente.SelectedValue = project.ClientId;
                comboAlmacen.SelectedValue = project.WarehouseId;
                //project.StorageTypeId;
                txtProjectName.Text = project.ProjectName;

                checkAutoPicking.Checked = project.AutoPicking;
                checkActivo.Checked = project.IsActive;

                checkBackoder.Checked = project.AllowsBackorder;
                checkDistribucion.Checked = project.IsDistributionArea;
                checkAlmacenFiscal.Checked = project.IsFiscalWarehouse;
                checkSobredimension.Checked = project.AllowsOversizedItems;
                checkEtiquetas.Checked = project.RequiresLabels;

                comboEntrada.Text = project.Entrada;
                comboAlmacenamiento.Text = project.StorageArea;
                comboRetrabajo.Text = project.ReworkArea;
                comboSalida.Text = project.Salida;

                checkNotRecibo.Checked = project.ReceiptNotificationEnabled;
                comboNotRecibo.Text = project.ReceiptNotificationMethod;

                checkNotEmbarque.Checked = project.ShipmentNotificationEnabled;
                comboNotEmbarque.Text = project.ShipmentNotificationMethod;

                checkNotInterna.Checked = project.InternalNotificationEnabled;
                comboNotInterna.Text = project.InternalNotificationMethod;

                textTiempoNormal.Text = project.NormalHrs.ToString();
                textTiempoUrgente.Text = project.UrgentHrs.ToString();

                textNumeroAsn.Text = project.AsnNumber;
                textPrefijoAsn.Text = project.AsnPrefix;

                // Kitting
                textNumeroKitting.Text = project.KittingNumber;
                textPrefijoKitting.Text = project.KittingPrefix;

                // Delivery Order (DO)
                textNumeroOrdenEntrega.Text = project.DeliveryOrderNumber;
                textPrefijoOrdenEntrega.Text = project.DeliveryOrderPrefix;

                textNumeroOrdenEntrega.Text = project.DoNumber;
                textPrefijoOrdenEntrega.Text = project.DoPrefix;


                checkRegistroRequerido.Checked = project.ReciveRequired;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Task<ApiResponseDto<string>> CreateClient(ProjectRequest request) =>
          _projectService.CreateProject(request);

        private Task<ApiResponseDto<string>> EditClient(int clientId, ProjectRequest request) =>
            _projectService.UpdateProject(clientId, request);
        private async Task<ApiResponseDto<string>> SaveClient(ProjectRequest request)
        {
            return ProjectSelected != null
                ? await EditClient(ProjectSelected?.ProjectId ?? 0, request)
                : await CreateClient(request);
        }
        private ProjectRequest BuildRequest()
        {
            return new ProjectRequest
            {
                ProjectId = 0,
                ClientId = int.TryParse( comboCliente.SelectedValue?.ToString(),out int cId)?cId:0,
                WarehouseId = int.TryParse(comboAlmacen.SelectedValue?.ToString(), out int wId) ? wId : 0,
                StorageTypeId = null,
                ProjectName = txtProjectName.Text,
                AutoPicking = checkAutoPicking.Checked,

                AllowsBackorder = checkBackoder.Checked,
                IsDistributionArea = checkDistribucion.Checked,
                IsFiscalWarehouse = checkAlmacenFiscal.Checked,
                AllowsOversizedItems = checkSobredimension.Checked,
                RequiresLabels = checkEtiquetas.Checked,

                Entrada = comboEntrada.Text,
                StorageArea = comboAlmacenamiento.Text,
                ReworkArea = comboRetrabajo.Text,
                Salida = comboSalida.Text,

                ReceiptNotificationEnabled = checkNotRecibo.Checked,
                ReceiptNotificationMethod = comboNotRecibo.Text,

                ShipmentNotificationEnabled = checkNotEmbarque.Checked,
                ShipmentNotificationMethod = comboNotEmbarque.Text,

                InternalNotificationEnabled = checkNotInterna.Checked,
                InternalNotificationMethod = comboNotInterna.Text,

                NormalHrs = decimal.TryParse( textTiempoNormal.Text,out decimal nHrs)? nHrs :0,
                UrgentHrs = decimal.TryParse(textTiempoUrgente.Text, out decimal uHrs) ? uHrs : 0,

                AsnNumber = textNumeroAsn.Text,
                AsnPrefix = textPrefijoAsn.Text,

                // Kitting
                KittingNumber = textNumeroKitting.Text,
                KittingPrefix = textPrefijoKitting.Text,

                // Delivery Order (DO)
                DeliveryOrderNumber = textNumeroOrdenEntrega.Text,
                DeliveryOrderPrefix = textPrefijoOrdenEntrega.Text,
       
                ReciveRequired = true,
            };
        }
        private void ShowResult(ApiResponseDto<string> result)
        {
            MessageBox.Show(
                result.IsSuccess ? result.Data : result.Message,
                result.IsSuccess ? "Éxito" : "Error",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                var request = BuildRequest();

                var result = await SaveClient(request);

                ShowResult(result);

                if (result.IsSuccess)
                    this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error inesperado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void checkAutoPicking_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
