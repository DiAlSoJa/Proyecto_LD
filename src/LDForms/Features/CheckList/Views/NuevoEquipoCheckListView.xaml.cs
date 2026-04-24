using LD.Client.Services;
using LD.Contracts.Equipment;
using LD.Contracts.EquipmentSupplier;
using LD.Contracts.EquipmentType;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.CheckList
{
    public partial class NuevoEquipoCheckListView : Window
    {
        private readonly EquipmentService _equipmentService;
        private readonly EquipmentTypeService _equipmentTypeService;
        private readonly EquipmentSupplierService _equipmentSupplierService;
        private readonly IServiceProvider _serviceProvider;
        private EquipmentDto? _selectedEquipment;

        public NuevoEquipoCheckListView(
            EquipmentService equipmentService,
            EquipmentTypeService equipmentTypeService,
            EquipmentSupplierService equipmentSupplierService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _equipmentService = equipmentService;
            _equipmentTypeService = equipmentTypeService;
            _equipmentSupplierService = equipmentSupplierService;
            _serviceProvider = serviceProvider;

            Loaded += NuevoEquipoCheckListView_Loaded;
            btnSave.Click += BtnGuardar_Click;
            button2.Click += BtnCerrar_Click;
            btnNuevo.Click += BtnNuevoProveedor_Click;
        }

        public void SetEquipment(EquipmentDto equipment)
        {
            _selectedEquipment = equipment;
            Title = "Editar Equipo";
        }

        private async void NuevoEquipoCheckListView_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarTiposAsync();
            await CargarProveedoresAsync();

            if (_selectedEquipment is not null)
            {
                await CargarEquipoAsync();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                if (cmbTipo.SelectedValue is not int equipmentTypeId || equipmentTypeId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un tipo de equipo.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNoEquipo.Text))
                {
                    DialogHelper.ShowWarning("Captura el No. de Equipo.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSerie.Text))
                {
                    DialogHelper.ShowWarning("Captura la serie.");
                    return;
                }

                var request = BuildRequest(equipmentTypeId);
                var response = _selectedEquipment is null
                    ? await _equipmentService.CreateEquipment(request)
                    : await _equipmentService.UpdateEquipment(_selectedEquipment.EquipmentId, request);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo guardar el equipo.");
                    return;
                }

                DialogHelper.ShowSuccess(response.Message ?? "Equipo guardado correctamente.");
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

        private EquipmentRequest BuildRequest(int equipmentTypeId)
        {
            decimal? hourmeter = null;
            var hourmeterText = txtHorometro.Text.Trim();

            if (decimal.TryParse(hourmeterText, NumberStyles.Any, CultureInfo.CurrentCulture, out var parsedHourmeter) ||
                decimal.TryParse(hourmeterText, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedHourmeter))
            {
                hourmeter = parsedHourmeter;
            }

            return new EquipmentRequest
            {
                EquipmentId = _selectedEquipment?.EquipmentId ?? 0,
                EquipmentName = txtNoEquipo.Text.Trim(),
                SerialNumber = txtSerie.Text.Trim(),
                Brand = txtMarca.Text.Trim(),
                Hourmeter = hourmeter,
                IsOperative = rbSi.IsChecked == true,
                EquipmentTypeId = equipmentTypeId,
                EquipmentSupplierId = cmbProveedor.SelectedValue is int supplierId
                    ? supplierId
                    : _selectedEquipment?.EquipmentSupplierId ?? 0,
                WarehouseId = 0,
                Turn1 = _selectedEquipment?.Turno1 ?? string.Empty,
                Turn2 = _selectedEquipment?.Turno2 ?? string.Empty,
                Turn3 = _selectedEquipment?.Turno3 ?? string.Empty,
                ImagePathLeft = _selectedEquipment?.ImagePathLeft ?? string.Empty,
                ImagePathRight = _selectedEquipment?.ImagePathRight ?? string.Empty
            };
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtnNuevoProveedor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoProveedorCheckListView>();
            dialog.Owner = this;

            var result = dialog.ShowDialog();
            if (result == true)
            {
                await CargarProveedoresAsync(dialog.CreatedSupplierName);
            }
        }

        private async Task CargarTiposAsync()
        {
            var response = await _equipmentTypeService.GetEquipmentTypes();
            if (!response.IsSuccess || response.Data is null)
            {
                DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudieron cargar los tipos de equipo.");
                return;
            }

            cmbTipo.ItemsSource = response.Data;
            cmbTipo.DisplayMemberPath = nameof(EquipmentTypeDto.EquipmentName);
            cmbTipo.SelectedValuePath = nameof(EquipmentTypeDto.EquipmentTypeId);

            if (cmbTipo.SelectedIndex < 0 && response.Data.Count > 0)
            {
                cmbTipo.SelectedIndex = 0;
            }
        }

        private async Task CargarProveedoresAsync(string? supplierNameToSelect = null)
        {
            var response = await _equipmentSupplierService.GetEquipmentSuppliers();
            if (!response.IsSuccess || response.Data is null)
            {
                cmbProveedor.ItemsSource = null;
                DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudieron cargar los proveedores.");
                return;
            }

            var suppliers = response.Data
                .OrderBy(x => x.EquipmentSupplierName)
                .ToList();

            cmbProveedor.ItemsSource = suppliers;
            cmbProveedor.DisplayMemberPath = nameof(EquipmentSupplierDto.EquipmentSupplierName);
            cmbProveedor.SelectedValuePath = nameof(EquipmentSupplierDto.EquipmentSupplierId);

            if (!string.IsNullOrWhiteSpace(supplierNameToSelect))
            {
                var createdSupplier = suppliers.LastOrDefault(x =>
                    string.Equals(x.EquipmentSupplierName, supplierNameToSelect, StringComparison.OrdinalIgnoreCase));

                if (createdSupplier is not null)
                {
                    cmbProveedor.SelectedValue = createdSupplier.EquipmentSupplierId;
                    return;
                }
            }

            if (_selectedEquipment?.EquipmentSupplierId > 0)
            {
                cmbProveedor.SelectedValue = _selectedEquipment.EquipmentSupplierId;
                return;
            }

            if (cmbProveedor.SelectedIndex < 0 && suppliers.Count > 0)
            {
                cmbProveedor.SelectedIndex = 0;
            }
        }

        private async Task CargarEquipoAsync()
        {
            var response = await _equipmentService.GetEquipmentById(_selectedEquipment!.EquipmentId);
            if (!response.IsSuccess || response.Data is null)
            {
                DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo cargar el equipo.");
                return;
            }

            var equipment = response.Data;
            cmbTipo.SelectedValue = equipment.EquipmentTypeId;
            txtNoEquipo.Text = equipment.EquipmentName;
            txtSerie.Text = equipment.SerialNumber;
            txtMarca.Text = equipment.Brand ?? string.Empty;
            txtHorometro.Text = equipment.Hourmeter?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
            rbSi.IsChecked = equipment.IsOperative;
            rbNo.IsChecked = !equipment.IsOperative;
            cmbProveedor.SelectedValue = equipment.EquipmentSupplierId;
        }
    }
}
