using LD.Client.Services;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.CheckList
{
    public partial class NuevoProveedorCheckListView : Window
    {
        private readonly EquipmentSupplierService _equipmentSupplierService;

        public string CreatedSupplierName { get; private set; } = string.Empty;

        public NuevoProveedorCheckListView(EquipmentSupplierService equipmentSupplierService)
        {
            InitializeComponent();
            _equipmentSupplierService = equipmentSupplierService;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var supplierName = txtNombreProveedor.Text.Trim();
                if (string.IsNullOrWhiteSpace(supplierName))
                {
                    DialogHelper.ShowWarning("Captura el nombre del proveedor.");
                    return;
                }

                var response = await _equipmentSupplierService.CreateEquipmentSupplier(new EquipmentSupplierRequest
                {
                    EquipmentSupplierName = supplierName
                });

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo guardar el proveedor.");
                    return;
                }

                CreatedSupplierName = supplierName;
                DialogHelper.ShowSuccess(response.Message ?? "Proveedor guardado correctamente.");
                DialogResult = true;
                Close();
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
    }
}
