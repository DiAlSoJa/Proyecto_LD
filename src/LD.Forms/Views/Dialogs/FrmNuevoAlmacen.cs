using LD.Contracts.Client;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.Forms.Classes.DTOs;
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
    public partial class FrmNuevoAlmacen : DraggableForm
    {
        private WarehouseDto? WarehouseSelected;
        private WarehouseService? _warehouseService;

        public FrmNuevoAlmacen(WarehouseService warehouseService)
        {
            InitializeComponent();
            _warehouseService = warehouseService;
            EnableDrag(panel1);
            EnableDrag(panel2);

        }
        public async void SetWarehouse(WarehouseDto? warehouse)
        {
            WarehouseSelected = warehouse;
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
                var response = await _warehouseService.GetWarehouseById(WarehouseSelected?.Id ?? 0);

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

        private Task<ApiResponseDto<string>> CreateClient(WarehouseRequest request) =>
         _warehouseService.CreateWarehouse(request);

        private Task<ApiResponseDto<string>> EditClient(int clientId, WarehouseRequest request) =>
            _warehouseService.UpdateWarehouse(clientId, request);
        private async Task<ApiResponseDto<string>> SaveClient(WarehouseRequest request)
        {
            return WarehouseSelected != null
                ? await EditClient(WarehouseSelected?.Id ?? 0, request)
                : await CreateClient(request);
        }
        private WarehouseRequest BuildRequest()
        {
            return new WarehouseRequest
            {
                WarehouseName = txtWarehouseName.Text,
                Address = txtAddress.Text,
                Neighborhood = txtcolonia.Text,
                City = txtCity.Text,
                ZipCode = txtZipCode.Text,
                Capacity = int.TryParse(txtCapacity.Text, out int capacity) ? capacity : null,
                IsProduction = isProduction.Checked,
                IsActive = isActive.Checked

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
    }
}
