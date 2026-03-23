
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
    public partial class FrmNewCategory : DraggableForm
    {
        private readonly CategoryService _categoryService;
        private LookupService _lookupService;
        private CategoryDto? CategorySelect { get; set; }
        private readonly DialogMessageService _dialogService;


        public FrmNewCategory(CategoryService categoryService, LookupService lookupService, DialogMessageService dialogService)
        {
            InitializeComponent();
            EnableDrag(panel2);
            EnableDrag(panel1);
            _categoryService = categoryService;
            _dialogService = dialogService;
            _lookupService = lookupService;

        }

        public async void SetCategory(CategoryDto unitS)
        {
            CategorySelect = unitS;
            await CargarDatosAsync();
        }



        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);


            await SetCombos();
            if (CategorySelect != null)
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



        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _categoryService.GetCategoryById(CategorySelect?.Categoria ?? "");

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var unitI = response.Data;
                txtId.Text = unitI.CategoryName;
                txtNombre.Text = unitI.Description;
                txtId.Enabled = CategorySelect == null;
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
        private Task<ApiResponseDto<string>> CreateCategory(CategoryRequest request) =>
            _categoryService.CreateCategory(request);

        private Task<ApiResponseDto<string>> EditCategory(string categoryId, CategoryRequest request) =>
            _categoryService.UpdateCategory(categoryId, request);
        private async Task<ApiResponseDto<string>> SaveCategory(CategoryRequest request)
        {
            return CategorySelect != null
                ? await EditCategory(CategorySelect?.Categoria ?? txtId.Text, request)
                : await CreateCategory(request);
        }

        private CategoryRequest BuildRequest()
        {

            return new CategoryRequest
            {
                CategoryName = CategorySelect != null ? CategorySelect.Categoria : txtId.Text,
                Description = txtNombre.Text.Trim(),
                Frecuency = int.TryParse(txtFrecuencia.Text, out int frec) ? frec : null,
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

                var result = await SaveCategory(request);

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
            if (cmbCliente.SelectedValue == null)
                return;

            if (!int.TryParse(cmbCliente.SelectedValue.ToString(), out int clienteId) || clienteId <= 0)
                return;

            var proyectos = await _lookupService.GetProjectClientLookup(clienteId);

            if (proyectos.IsSuccess)
            {
                cmbProyecto.DataSource = proyectos.Data;
                cmbProyecto.DisplayMember = "Value";
                cmbProyecto.ValueMember = "Key";

                if (proyectos.Data.Count > 1)
                {
                    cmbProyecto.SelectedIndex = -1;
                }
            }




        }

        private async void cmbCliente_SelectionChangeCommitted(object sender, EventArgs e)
        {
          
        }
    }
}
