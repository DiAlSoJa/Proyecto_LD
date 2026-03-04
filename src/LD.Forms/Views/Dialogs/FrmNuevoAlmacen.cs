using LD.Contracts.Client;
using LD.Contracts.Enums;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.Warehouse;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevoAlmacen : DraggableForm
    {
        private WarehouseDto? WarehouseSelected;
        private WarehouseService _warehouseService;
        private readonly DialogMessageService _dialogService;
        public FrmNuevoAlmacen(WarehouseService warehouseService,DialogMessageService dialogMessageService)
        {
            InitializeComponent();
            EnableDrag(panel1);
            EnableDrag(panel2);

            _warehouseService = warehouseService;
            _dialogService = dialogMessageService;
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
                var warehouse = response.Data;
                txtWarehouseName.Text = warehouse.WarehouseName;
                txtAddress.Text = warehouse.Address;
                txtcolonia.Text = warehouse.Neighborhood;
                txtCity.Text = warehouse.City;
                txtZipCode.Text = warehouse.ZipCode;
                txtCapacity.Text = warehouse.Capacity.ToString();
                isProduction.Checked = warehouse.IsProduction;
                isActive.Checked = warehouse.IsActive;

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

        private async Task<ApiResponseDto<string>> CreateClient(WarehouseRequest request) =>
            await _warehouseService.CreateWarehouse(request);

        private async Task<ApiResponseDto<string>> EditClient(int warehouseId, WarehouseRequest request) =>
            await _warehouseService.UpdateWarehouse(warehouseId, request);
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
                WarehouseId = WarehouseSelected != null ? WarehouseSelected.Id : 0,
                WarehouseName = txtWarehouseName.Text,
                Address = txtAddress.Text,
                Neighborhood = txtcolonia.Text,
                City = txtCity.Text,
                ZipCode = txtZipCode.Text,
                Capacity = decimal.TryParse(txtCapacity.Text, out decimal capacity) ? capacity : null,
                IsProduction = isProduction.Checked,
                IsActive = isActive.Checked

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
