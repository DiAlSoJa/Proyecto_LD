
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.Units;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNewUnit : DraggableForm
    {
        private readonly UnitService _unitService;
        private UnitDto? UnitSelected { get; set; }
        private readonly DialogMessageService _dialogService;

        public FrmNewUnit(UnitService unitService, DialogMessageService dialogService)
        {
            InitializeComponent();
            EnableDrag(panel2);
            EnableDrag(panel1);
            _unitService = unitService;
            _dialogService = dialogService;

        }

        public async void SetUnit(UnitDto unitS)
        {
            UnitSelected = unitS;
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
                var response = await _unitService.GetUnitById(UnitSelected?.Unidad??"");

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var unitI = response.Data;
                txtId.Text = unitI.UnitIdS; 
                txtNombre.Text = unitI.Description;                
                txtId.Enabled = UnitSelected == null; 
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
        private Task<ApiResponseDto<string>> CreateStatus(UnitRequest request) =>
            _unitService.CreateUnit(request);

        private Task<ApiResponseDto<string>> EditStatus(string statusId, UnitRequest request) =>
            _unitService.UpdateUnit(statusId, request);
        private async Task<ApiResponseDto<string>> SaveStatus(UnitRequest request)
        {
            return UnitSelected != null
                ? await EditStatus(UnitSelected?.Unidad ?? txtId.Text, request)
                : await CreateStatus(request);
        }

        private UnitRequest BuildRequest()
        {

            return new UnitRequest
            {
                UnitIdS = UnitSelected != null? UnitSelected.Unidad:txtId.Text,
                Description = txtNombre.Text.Trim(),                
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
