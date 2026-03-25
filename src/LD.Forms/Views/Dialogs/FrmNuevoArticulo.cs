using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.Enums;
using LD.Contracts.Product;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
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
    public partial class FrmNuevoArticulo : DraggableForm
    {

        private readonly ProductService _itemService;
        private LookupService _lookupService;
        private ProductDto? ItemSelected;
        private readonly DialogMessageService _dialogService;
        private bool _cargandoDatos = false;
        private readonly CategoryService _categoryService;

        public FrmNuevoArticulo(ProductService itemService, LookupService lookupService, CategoryService categoryService, DialogMessageService dialogService)
        {
            InitializeComponent();
            _itemService = itemService;
            _dialogService = dialogService;
            _lookupService = lookupService;
            _categoryService = categoryService;
            EnableDrag(panel2);
            EnableDrag(panel1);
        }
        public async void SetItem(ProductDto? item)
        {
            ItemSelected = item;
            await CargarDatosAsync();
        }
        private async Task SetCombos()
        {
            try
            {

                _cargandoDatos = true;

                var clientes = await _lookupService.GetClientLookup();
                if (clientes.IsSuccess)
                {
                    cmbCliente.DataSource = clientes.Data;
                    cmbCliente.DisplayMember = "Value";
                    cmbCliente.ValueMember = "Key";
                    cmbCliente.SelectedIndex = -1;
                }
                //unidades
                var unidades = await _lookupService.GetUnitLookup();

                if (unidades.IsSuccess)
                {
                    cmbUnidadMinima.DataSource = unidades.Data.ToList();
                    cmbUnidadMedia.DataSource = unidades.Data.ToList();
                    cmbUnidadMaxima.DataSource = unidades.Data.ToList();
                    cmbUnidadProduccion.DataSource = unidades.Data.ToList();
                    cmbPaqueteEstandar.DataSource = unidades.Data.ToList();

                    ConfigurarCombo(cmbUnidadMinima);
                    ConfigurarCombo(cmbUnidadMedia);
                    ConfigurarCombo(cmbUnidadMaxima);
                    ConfigurarCombo(cmbUnidadProduccion);
                    ConfigurarCombo(cmbPaqueteEstandar);
                }



                cmbProyecto.DataSource = null;
                cmbCategoria.DataSource = null;
                cmbFamilia.DataSource = null;
                
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
        private void ConfigurarCombo(ComboBox combo)
        {
            combo.DisplayMember = "Value";
            combo.ValueMember = "Key";
            combo.SelectedIndex = -1;
        }


        private async Task SetCombosProjects(string projectSel = "")
        {
            try
            {

                _cargandoDatos = true;
                cmbProyecto.DataSource = null;
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

                cmbCategoria.DataSource = null;
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



        private async Task SetCombosCategories_Fam(string projectSel = "")
        {
            try
            {

                _cargandoDatos = true;
                if (cmbCliente.SelectedValue == null)
                {
                    cmbProyecto.DataSource = null;
                    return;
                }

                if (cmbProyecto.SelectedValue == null)
                {
                    cmbCategoria.DataSource = null;
                    cmbFamilia.DataSource = null;
                    return;
                }

                if (!int.TryParse(cmbCliente.SelectedValue.ToString(), out int clienteId) || clienteId <= 0)
                {
                    cmbProyecto.DataSource = null;
                    return;
                }
                if (!int.TryParse(cmbProyecto.SelectedValue.ToString(), out int proyectoId) || proyectoId <= 0)
                {
                    cmbCategoria.DataSource = null;
                    cmbFamilia.DataSource = null;
                    return;
                }
                //  Categorias
                var categorias = await _lookupService.GetCategoryClientLookup(clienteId, proyectoId);

                if (!categorias.IsSuccess || categorias.Data == null)
                {
                    cmbCategoria.DataSource = null;
                    return;
                }
                cmbCategoria.DisplayMember = "Value";
                cmbCategoria.ValueMember = "Key";
                cmbCategoria.DataSource = categorias.Data;

                if (!string.IsNullOrWhiteSpace(projectSel))
                {
                    cmbCategoria.SelectedValue = projectSel;
                }
                else if (categorias.Data.Count > 1)
                {
                    cmbCategoria.SelectedIndex = -1;
                }
                //  Familias
                var familias = await _lookupService.GetFamilyClientLookup(clienteId, proyectoId);

                if (!familias.IsSuccess || familias.Data == null)
                {
                    cmbFamilia.DataSource = null;
                    return;
                }
                cmbFamilia.DisplayMember = "Value";
                cmbFamilia.ValueMember = "Key";
                cmbFamilia.DataSource = familias.Data;

                if (!string.IsNullOrWhiteSpace(projectSel))
                {
                    cmbFamilia.SelectedValue = projectSel;
                }
                else if (familias.Data.Count > 1)
                {
                    cmbFamilia.SelectedIndex = -1;
                }







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

       





        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            await SetCombos();


        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _itemService.GetItemById(ItemSelected?.ItemId ?? 0);

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var client = response.Data;

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
        private Task<ApiResponseDto<string>> CreateClient(ProductRequest request) =>
        _itemService.CreateItem(request);

        private Task<ApiResponseDto<string>> EditClient(int clientId, ProductRequest request) =>
            _itemService.UpdateItem(clientId, request);
        private async Task<ApiResponseDto<string>> SaveClient(ProductRequest request)
        {
            return ItemSelected != null
                ? await EditClient(ItemSelected?.ItemId ?? 0, request)
                : await CreateClient(request);
        }
        private ProductRequest BuildRequest()
        {
            return new ProductRequest
            {

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

                var result = await SaveClient(request);

                ShowResult(result);

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

        private async void cmbProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoDatos)
                return;
            await SetCombosCategories_Fam();
        }
    }
}
