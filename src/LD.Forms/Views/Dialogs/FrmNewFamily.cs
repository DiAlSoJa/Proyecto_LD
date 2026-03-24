
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using LD.Client.Services;
using LD.Contracts.Category;
using LD.Contracts.Currency;
using LD.Contracts.DTOs.Family;
using LD.Contracts.Enums;
using LD.Contracts.Project;
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
    public partial class FrmNewFamily : DraggableForm
    {
        private readonly FamilyService _familyService;
        private LookupService _lookupService;
        private FamilyDto? FamilySelect { get; set; }
        private readonly DialogMessageService _dialogService;
        private bool _cargandoDatos = false;


        public FrmNewFamily(FamilyService categoryService, LookupService lookupService, DialogMessageService dialogService)
        {
            InitializeComponent();
            EnableDrag(panel2);
            EnableDrag(panel1);
            _familyService = categoryService;
            _dialogService = dialogService;
            _lookupService = lookupService;

        }

        public async void SetFamily(FamilyDto unitS)
        {
            FamilySelect = unitS;
            //await CargarDatosAsync();
        }



        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);


            await SetCombos();
            if (FamilySelect != null)
                await CargarDatosAsync();

        }
        private async Task SetCombos()
        {
            var clientes = await _lookupService.GetClientLookup();


            if (clientes.IsSuccess)
            {

                cmbCliente.DataSource = clientes.Data;
                cmbCliente.DisplayMember = "Value";
                cmbCliente.ValueMember = "Key";
                cmbCliente.SelectedIndex = -1;


            }




        }


        private async Task SetCombosProjects(string projectSel = "")
        {
            if (cmbCliente.SelectedValue == null)
            {
                cmbProyecto.DataSource = null;
                return;
            }

            if (!int.TryParse(cmbCliente.SelectedValue.ToString(), out int clienteId) || clienteId <= 0)
            {
                cmbProyecto.DataSource = null;
                return;
            }

            var proyectos = await _lookupService.GetProjectClientLookup(clienteId);

            if (!proyectos.IsSuccess || proyectos.Data == null)
            {
                cmbProyecto.DataSource = null;
                return;
            }

            cmbProyecto.DisplayMember = "Value";
            cmbProyecto.ValueMember = "Key";
            cmbProyecto.DataSource = proyectos.Data;

            if (!string.IsNullOrWhiteSpace(projectSel))
            {
                cmbProyecto.SelectedValue = projectSel;
            }
            else if (proyectos.Data.Count > 1)
            {
                cmbProyecto.SelectedIndex = -1;
            }
        }



        private async Task CargarDatosAsync()
        {
            try
            {
                _cargandoDatos = true;

                var response = await _familyService.GetFamilyById(FamilySelect?.FamiliaId ?? 0);

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }

                var unitI = response.Data;

                txtNombre.Text = unitI.FamilyName;
                cmbCliente.SelectedValue = unitI.ClientId.ToString();

                await SetCombosProjects(unitI.ProjectId.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _cargandoDatos = false;
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
        private Task<ApiResponseDto<string>> CreateFamily(FamilyRequest request) =>
            _familyService.CreateFamily(request);

        private Task<ApiResponseDto<string>> EditFamily(int familyId, FamilyRequest request) =>
            _familyService.UpdateFamily(familyId, request);
        private async Task<ApiResponseDto<string>> SaveFamily(FamilyRequest request)
        {
            return FamilySelect != null
                ? await EditFamily(FamilySelect?.FamiliaId ?? 0, request)
                : await CreateFamily(request);
        }

        private FamilyRequest BuildRequest()
        {

            return new FamilyRequest
            {                
                FamilyName = txtNombre.Text.Trim(),             
                ClientId = int.TryParse(cmbCliente.SelectedValue?.ToString(), out int clienteId) ? clienteId : 0,
                ProjectId = int.TryParse(cmbProyecto.SelectedValue?.ToString(), out int projectId) ? projectId : 0,

            };
        }

        private void ShowResult(ApiResponseDto<string> result)
        {
            _dialogService.Show(
                 result.IsSuccess ? result.Data ?? "" : $"Hubo un error: {Environment.NewLine}{result.ErrorMessage ?? ""}",
                  result.IsSuccess ? DialogMessageEnum.Info : DialogMessageEnum.Error
                );


        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                var request = BuildRequest();

                var result = await SaveFamily(request);

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

        private async void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoDatos)
                return;

            await SetCombosProjects();
        }

        private async void cmbCliente_SelectionChangeCommitted(object sender, EventArgs e)
        {
          
        }
    }
}
