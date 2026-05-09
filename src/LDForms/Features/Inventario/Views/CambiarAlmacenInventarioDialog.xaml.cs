using LD.Client.Services;
using LD.Contracts.AvailableInventory;
using LD.Contracts.DTOs;
using LD.FormsX.Helpers;
using LD.FormsX.Model.Lookup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.Inventario
{
    public partial class CambiarAlmacenInventarioDialog : Window
    {
        private readonly LookupService _lookupService;
        private bool _loadingLocations;

        public int WarehouseId { get; private set; }
        public string UbicacionDestino { get; private set; } = string.Empty;

        public CambiarAlmacenInventarioDialog(
            AvailableInventoryDto inventory,
            IEnumerable<LookupItem> warehouseItems,
            LookupService lookupService)
        {
            InitializeComponent();

            _lookupService = lookupService;
            txtStandardId.Text = $"StandardId: {inventory.StandardIdStr}";
            txtAlmacenActual.Text = $"Almacen actual: {inventory.Almacen}";
            txtUbicacionActual.Text = $"Ubicacion actual: {inventory.Ubicacion}";
            cmbAlmacenDestino.ItemsSource = warehouseItems.OrderBy(x => x.Code).ToList();
        }

        private async void CmbAlmacenDestino_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_loadingLocations)
                return;

            if (cmbAlmacenDestino.SelectedItem is not LookupItem selectedWarehouse ||
                !int.TryParse(selectedWarehouse.Id?.ToString(), out var selectedWarehouseId) ||
                selectedWarehouseId <= 0)
            {
                WarehouseId = 0;
                cmbUbicacionDestino.ItemsSource = null;
                cmbUbicacionDestino.IsEnabled = false;
                return;
            }

            WarehouseId = selectedWarehouseId;
            await LoadLocationsAsync(WarehouseId);
        }

        private async Task LoadLocationsAsync(int warehouseId)
        {
            try
            {
                _loadingLocations = true;
                cmbUbicacionDestino.IsEnabled = false;
                cmbUbicacionDestino.ItemsSource = null;
                cmbUbicacionDestino.Text = string.Empty;

                var response = await _lookupService.GetLocationWarehouseLookup(warehouseId);
                if (!response.IsSuccess || response.Data == null)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudieron cargar las ubicaciones del almacen.");
                    return;
                }

                var locations = response.Data
                    .Select(ToLookupItem)
                    .OrderBy(x => x.Code)
                    .ToList();

                cmbUbicacionDestino.ItemsSource = locations;
                cmbUbicacionDestino.IsEnabled = locations.Any();

                if (!locations.Any())
                    DialogHelper.ShowWarning("El almacen seleccionado no tiene ubicaciones registradas.");
            }
            finally
            {
                _loadingLocations = false;
            }
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (WarehouseId <= 0)
            {
                MessageBox.Show("Selecciona el almacen destino.", "Almacen requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var value = (cmbUbicacionDestino.SelectedValue as string) ?? cmbUbicacionDestino.Text;
            if (string.IsNullOrWhiteSpace(value))
            {
                MessageBox.Show("Selecciona la ubicacion destino.", "Ubicacion requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UbicacionDestino = value.Trim();
            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private static LookupItem ToLookupItem(DropDownDto item)
        {
            return new LookupItem
            {
                Id = int.TryParse(item.Key, out var value) ? value : 0,
                Code = item.Value ?? string.Empty,
                Description = item.Key ?? string.Empty,
                Data = item
            };
        }
    }
}
