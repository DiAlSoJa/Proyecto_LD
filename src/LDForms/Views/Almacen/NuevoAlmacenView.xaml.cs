using LD.Client.Services;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Almacen
{
    public partial class NuevoAlmacenView : Window
    {
        private WarehouseDto? WarehouseSelected;
        private readonly WarehouseService _warehouseService;

        public bool ResponseForm { get; private set; }

        public NuevoAlmacenView(WarehouseService warehouseService)
        {
            InitializeComponent();
            _warehouseService = warehouseService;
        }

        public async void SetWarehouse(WarehouseDto? warehouse)
        {
            WarehouseSelected = warehouse;
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _warehouseService.GetWarehouseById(WarehouseSelected?.Id ?? 0);

                if (!response.IsSuccess || response.Data == null)
                {
                    DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo cargar el almacén.");
                    return;
                }

                var warehouse = response.Data;
                txtWarehouseName.Text = warehouse.WarehouseName;
                txtAddress.Text = warehouse.Address;
                txtcolonia.Text = warehouse.Neighborhood;
                txtCity.Text = warehouse.City;
                txtZipCode.Text = warehouse.ZipCode;
                txtCapacity.Text = warehouse.Capacity?.ToString() ?? string.Empty;
                isProduction.IsChecked = warehouse.IsProduction;
                isActive.IsChecked = warehouse.IsActive;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private WarehouseRequest BuildRequest()
        {
            return new WarehouseRequest
            {
                WarehouseId = WarehouseSelected != null ? WarehouseSelected.Id : 0,
                WarehouseName = txtWarehouseName.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Neighborhood = txtcolonia.Text.Trim(),
                City = txtCity.Text.Trim(),
                ZipCode = txtZipCode.Text.Trim(),
                Capacity = decimal.TryParse(txtCapacity.Text, out decimal capacity) ? capacity : null,
                IsProduction = isProduction.IsChecked ?? false,
                IsActive = isActive.IsChecked ?? false
            };
        }

        private async Task<ApiResponseDto<string>> CreateWarehouse(WarehouseRequest request) =>
            await _warehouseService.CreateWarehouse(request);

        private async Task<ApiResponseDto<string>> EditWarehouse(int warehouseId, WarehouseRequest request) =>
            await _warehouseService.UpdateWarehouse(warehouseId, request);

        private async Task<ApiResponseDto<string>> SaveWarehouse(WarehouseRequest request)
        {
            return WarehouseSelected != null
                ? await EditWarehouse(WarehouseSelected.Id, request)
                : await CreateWarehouse(request);
        }

        private void ShowResult(ApiResponseDto<string> result)
        {
            if (result.IsSuccess)
                DialogHelper.ShowSuccess(result.Data ?? "Operación realizada correctamente.");
            else
                DialogHelper.ShowError(result.ErrorMessage ?? "Ocurrió un error al guardar.");
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                var result = await SaveWarehouse(request);

                ShowResult(result);
                ResponseForm = result.IsSuccess;

                if (result.IsSuccess)
                {
                    this.DialogResult = true;
                    Close();
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

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}