using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.DTOs.User;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.InventarioAleatorio
{
    public partial class NuevoInventarioCiclicoView : Window
    {
        private readonly UserService _userService;
        private readonly LocationService _locationService;
        private readonly CyclicInventoryService _cyclicInventoryService;
        private List<LocationDto> _locations = [];
        private List<LocationDto> _warehouseLocations = [];
        private List<LocationDto> _selectedLocations = [];

        public NuevoInventarioCiclicoView(
            UserService userService,
            LocationService locationService,
            CyclicInventoryService cyclicInventoryService)
        {
            InitializeComponent();

            _userService = userService;
            _locationService = locationService;
            _cyclicInventoryService = cyclicInventoryService;

            Loaded += NuevoInventarioCiclicoView_Loaded;
        }

        private async void NuevoInventarioCiclicoView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadLookupsAsync();
        }

        private async Task LoadLookupsAsync()
        {
            try
            {
                var usersResult = await _userService.GetUsers();
                if (!usersResult.IsSuccess)
                {
                    DialogHelper.ShowWarning(usersResult.Message);
                    return;
                }

                var users = usersResult.Data ?? [];
                var currentUser = users.FirstOrDefault(x => x.User?.Id == UserData.Id);
                var currentWarehouseIds = currentUser?.Warehouse?
                    .Where(x => x.Activo)
                    .Select(x => x.Id)
                    .Distinct()
                    .ToHashSet() ?? [];

                var warehouses = currentUser?.Warehouse?
                    .Where(x => x.Activo)
                    .OrderBy(x => x.NombreAlmacen)
                    .Select(x => new DropDownDto
                    {
                        Key = x.Id.ToString(),
                        Value = x.NombreAlmacen
                    })
                    .ToList() ?? [];

                cmbAlmacen.ItemsSource = warehouses;
                if (warehouses.Count > 0)
                {
                    cmbAlmacen.SelectedIndex = 0;
                }

                cmbAuditor.ItemsSource = BuildAuditorLookup(users, currentWarehouseIds);

                var locationsResult = await _locationService.GetLocations();
                if (!locationsResult.IsSuccess)
                {
                    DialogHelper.ShowWarning(locationsResult.Message);
                    return;
                }

                _locations = locationsResult.Data ?? [];
                LoadLocationFilters();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private static List<DropDownDto> BuildAuditorLookup(
            List<GetUserDto> users,
            HashSet<int> currentWarehouseIds)
        {
            if (currentWarehouseIds.Count == 0)
            {
                return [];
            }

            return users
                .Where(x => x.User?.Activo == true)
                .Where(x => x.Warehouse?.Any(w => currentWarehouseIds.Contains(w.Id)) == true)
                .OrderBy(x => x.User?.Nombre ?? x.User?.UserName)
                .Select(x => new DropDownDto
                {
                    Key = x.User?.Id,
                    Value = string.IsNullOrWhiteSpace(x.User?.Nombre)
                        ? x.User?.UserName
                        : x.User?.Nombre
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.Key) && !string.IsNullOrWhiteSpace(x.Value))
                .ToList();
        }

        private void CmbAlmacen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadLocationFilters();
        }

        private void LoadLocationFilters()
        {
            if (!int.TryParse(cmbAlmacen.SelectedValue?.ToString(), out var warehouseId))
            {
                SetLocationLookup([], [], []);
                return;
            }

            var warehouseLocations = _locations
                .Where(x => x.Activo && x.WarehouseId == warehouseId)
                .ToList();

            _warehouseLocations = warehouseLocations;
            _selectedLocations = [];
            dgUbicacionesSeleccionadas.ItemsSource = _selectedLocations;

            SetLocationLookup(
                CreateLookup(warehouseLocations.Select(x => x.Rack)),
                CreateLookup(warehouseLocations.Select(x => x.Posicion)),
                CreateLookup(warehouseLocations.Select(x => x.Nivel)));

            ApplyLocationFilters();
        }

        private void SetLocationLookup(
            List<DropDownDto> racks,
            List<DropDownDto> posiciones,
            List<DropDownDto> niveles)
        {
            cmbRack.ItemsSource = racks;
            cmbRack.SelectedIndex = racks.Count > 0 ? 0 : -1;

            cmbPosicion.ItemsSource = posiciones;
            cmbPosicion.SelectedIndex = posiciones.Count > 0 ? 0 : -1;

            cmbNivel.ItemsSource = niveles;
            cmbNivel.SelectedIndex = niveles.Count > 0 ? 0 : -1;

            UpdateRackFilterState();
        }

        private void Filtro_Changed(object sender, RoutedEventArgs e)
        {
            UpdateRackFilterState();
            ApplyLocationFilters();
        }

        private void UpdateRackFilterState()
        {
            var rackSelected = rbRack?.IsChecked == true;

            if (cmbRack is null || cmbPosicion is null || cmbNivel is null)
            {
                return;
            }

            cmbRack.IsEnabled = rackSelected;
            cmbPosicion.IsEnabled = rackSelected;
            cmbNivel.IsEnabled = rackSelected;

            if (!rackSelected)
            {
                cmbRack.SelectedIndex = -1;
                cmbPosicion.SelectedIndex = -1;
                cmbNivel.SelectedIndex = -1;
            }
        }

        private void ApplyLocationFilters()
        {
            if (dgUbicacionesDisponibles is null)
            {
                return;
            }

            IEnumerable<LocationDto> filtered = _warehouseLocations;

            if (rbNoRack?.IsChecked == true)
            {
                filtered = filtered.Where(x => !x.EsRack);
            }
            else if (rbRack?.IsChecked == true)
            {
                filtered = filtered.Where(x => x.EsRack);

                var rack = cmbRack.SelectedValue?.ToString();
                if (IsSpecificFilterValue(rack))
                {
                    filtered = filtered.Where(x => string.Equals(x.Rack, rack, StringComparison.OrdinalIgnoreCase));
                }

                var posicion = cmbPosicion.SelectedValue?.ToString();
                if (IsSpecificFilterValue(posicion))
                {
                    filtered = filtered.Where(x => string.Equals(x.Posicion, posicion, StringComparison.OrdinalIgnoreCase));
                }

                var nivel = cmbNivel.SelectedValue?.ToString();
                if (IsSpecificFilterValue(nivel))
                {
                    filtered = filtered.Where(x => string.Equals(x.Nivel, nivel, StringComparison.OrdinalIgnoreCase));
                }
            }

            var ubicacion = txtUbicacion?.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(ubicacion))
            {
                filtered = filtered.Where(x => x.Ubicacion.Contains(ubicacion, StringComparison.OrdinalIgnoreCase));
            }

            dgUbicacionesDisponibles.ItemsSource = filtered
                .Where(x => !_selectedLocations.Any(selected => selected.LocationId == x.LocationId))
                .OrderBy(x => x.Ubicacion)
                .ToList();
        }

        private void BtnAgregarUbicacion_Click(object sender, RoutedEventArgs e)
        {
            if (dgUbicacionesDisponibles.SelectedItem is not LocationDto selectedLocation)
            {
                DialogHelper.ShowWarning("Selecciona una ubicacion para agregar.");
                return;
            }

            if (_selectedLocations.Any(x => x.LocationId == selectedLocation.LocationId))
            {
                return;
            }

            _selectedLocations.Add(selectedLocation);
            RefreshSelectedLocations();
            ApplyLocationFilters();
        }

        private void BtnQuitarUbicacion_Click(object sender, RoutedEventArgs e)
        {
            if (dgUbicacionesSeleccionadas.SelectedItem is not LocationDto selectedLocation)
            {
                DialogHelper.ShowWarning("Selecciona una ubicacion para quitar.");
                return;
            }

            _selectedLocations = _selectedLocations
                .Where(x => x.LocationId != selectedLocation.LocationId)
                .ToList();

            RefreshSelectedLocations();
            ApplyLocationFilters();
        }

        private void RefreshSelectedLocations()
        {
            dgUbicacionesSeleccionadas.ItemsSource = null;
            dgUbicacionesSeleccionadas.ItemsSource = _selectedLocations
                .OrderBy(x => x.Ubicacion)
                .ToList();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAuditor.SelectedValue is null)
            {
                DialogHelper.ShowWarning("Selecciona un auditor.");
                return;
            }

            if (!int.TryParse(cmbAlmacen.SelectedValue?.ToString(), out var warehouseId))
            {
                DialogHelper.ShowWarning("Selecciona un almacen.");
                return;
            }

            if (_selectedLocations.Count == 0)
            {
                DialogHelper.ShowWarning("Agrega al menos una ubicacion.");
                return;
            }

            try
            {
                var request = new InventarioCiclicoRequest
                {
                    Fecha = DateTime.Today,
                    AuditorUserId = cmbAuditor.SelectedValue.ToString() ?? string.Empty,
                    AuditorNombre = (cmbAuditor.SelectedItem as DropDownDto)?.Value,
                    WarehouseId = warehouseId,
                    Estatus = "Abierto",
                    LocationIds = _selectedLocations.Select(x => x.LocationId).Distinct().ToList()
                };

                var result = await _cyclicInventoryService.CreateCyclicInventory(request);
                if (!result.IsSuccess)
                {
                    DialogHelper.ShowWarning(result.Message);
                    return;
                }

                DialogHelper.ShowSuccess(result.Message);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private static List<DropDownDto> CreateLookup(IEnumerable<string?> values)
        {
            var lookup = values
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .Select(x => new DropDownDto { Key = x, Value = x })
                .ToList();

            lookup.Insert(0, new DropDownDto { Key = "", Value = "Todos" });

            return lookup;
        }

        private static bool IsSpecificFilterValue(string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
