
using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNewStatus : DraggableForm
    {
        private readonly InventaryStatusService _statusService;
        private InventaryStatusDto? StatusSelected { get; set; }
        private readonly DialogMessageService _dialogService;

        public FrmNewStatus(InventaryStatusService statusService, DialogMessageService dialogService)
        {
            InitializeComponent();
            EnableDrag(panel2);
            EnableDrag(panel1);
            _statusService = statusService;
            _dialogService = dialogService;

        }

        public async void SetInventaryStatus(InventaryStatusDto inventaryStatus)
        {
            StatusSelected = inventaryStatus;
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
                var response = await _statusService.GetStatusById(StatusSelected?.StatusId??"");

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var statusI = response.Data;
                txtStatus.Text = statusI.InventoryStatusIdS; 
                txtNombreStatus.Text = statusI.FullName;
                chkDisponible.Checked = statusI.IsAvailable;
                txtStatus.Enabled = StatusSelected == null; 
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
        private Task<ApiResponseDto<string>> CreateStatus(InventaryStatusRequest request) =>
            _statusService.CreateInventaryStatus(request);

        private Task<ApiResponseDto<string>> EditStatus(string statusId, InventaryStatusRequest request) =>
            _statusService.UpdateInventaryStatus(statusId, request);
        private async Task<ApiResponseDto<string>> SaveStatus(InventaryStatusRequest request)
        {
            return StatusSelected != null
                ? await EditStatus(StatusSelected?.StatusId ?? txtStatus.Text, request)
                : await CreateStatus(request);
        }

        private InventaryStatusRequest BuildRequest()
        {

            return new InventaryStatusRequest
            {
                InventoryStatusIdS = StatusSelected != null? StatusSelected.StatusId:txtStatus.Text,
                FullName = txtNombreStatus.Text.Trim(),
                IsAvailable = chkDisponible.Checked
            };
        }
      
        private void ShowResult(ApiResponseDto<string> result)
        {
            _dialogService.Show(
                 result.IsSuccess ? result.Data??"" : $"Hubo un error: {Environment.NewLine}{result.ErrorMessage??""}",
                  result.IsSuccess ? DialogMessageEnum.Info : DialogMessageEnum.Error
                );
        
               
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                var request = BuildRequest();

                var result = await SaveStatus(request);

                ShowResult(result);

                ResponseForm = result.IsSuccess;
                if (result.IsSuccess)
                    this.Close();
            }
            catch (Exception ex)
            {
                _dialogService.Show($"Hubo un error: {Environment.NewLine}{ex.Message}", DialogMessageEnum.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }
    }
}
