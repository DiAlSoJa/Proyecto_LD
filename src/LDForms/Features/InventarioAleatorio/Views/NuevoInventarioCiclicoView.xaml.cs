using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using LD.FormsX.Model.Lookup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;

namespace LD.FormsX.Views.InventarioAleatorio
{
    public partial class NuevoInventarioCiclicoView : Window
    {
        private readonly LookupService _lookupService;
        private readonly LocationService _locationService;
        private readonly CyclicInventoryService _cyclicInventoryService;
        private readonly DataGridColumnFilterManager _availableGridManager;
        private readonly DataGridColumnFilterManager _selectedGridManager;
        private readonly ObservableCollection<LookupItem> _auditorLookupItems = new();
        private List<LocationDto> _locations = [];
        private List<LocationDto> _warehouseLocations = [];
        private List<LocationDto> _selectedLocations = [];
        private ICollectionView? _availableLocationsView;
        private ICollectionView? _selectedLocationsView;

        public ObservableCollection<LookupItem> AuditorLookupItems => _auditorLookupItems;

        public NuevoInventarioCiclicoView(
            LookupService lookupService,
            LocationService locationService,
            CyclicInventoryService cyclicInventoryService)
        {
            InitializeComponent();

            _lookupService = lookupService;
            _locationService = locationService;
            _cyclicInventoryService = cyclicInventoryService;
            _availableGridManager = new DataGridColumnFilterManager(dgUbicacionesDisponibles);
            _selectedGridManager = new DataGridColumnFilterManager(dgUbicacionesSeleccionadas);

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
                if (string.IsNullOrWhiteSpace(UserData.Id))
                {
                    DialogHelper.ShowWarning("No se pudo identificar al usuario actual.");
                    return;
                }

                var warehousesResult = await _lookupService.GetWarehouseLookupByUser(UserData.Id);
                if (!warehousesResult.IsSuccess)
                {
                    DialogHelper.ShowWarning(warehousesResult.Message);
                    return;
                }

                var auditorsResult = await _lookupService.GetCycleCountAuditorLookupByUser(UserData.Id);
                if (!auditorsResult.IsSuccess)
                {
                    DialogHelper.ShowWarning(auditorsResult.Message);
                    return;
                }

                var warehouses = warehousesResult.Data ?? [];
                var auditors = auditorsResult.Data ?? [];

                cmbAlmacen.ItemsSource = warehouses;
                if (warehouses.Count > 0)
                {
                    cmbAlmacen.SelectedIndex = 0;
                }

                _auditorLookupItems.Clear();
                foreach (var auditor in auditors.Select(ToAuditorLookupItem))
                {
                    _auditorLookupItems.Add(auditor);
                }

                lookupAuditor.ClearSelection();

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

        private void CmbAlmacen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadLocationFilters();
        }

        private void LoadLocationFilters()
        {
            if (!int.TryParse(cmbAlmacen.SelectedValue?.ToString(), out var warehouseId))
            {
                _availableLocationsView = null;
                SetLocationLookup([], [], []);
                dgUbicacionesDisponibles.ItemsSource = null;
                return;
            }

            var warehouseLocations = _locations
                .Where(x => x.Activo && x.WarehouseId == warehouseId)
                .ToList();

            _warehouseLocations = warehouseLocations;
            _selectedLocations = [];

            SetLocationLookup(
                CreateLookup(warehouseLocations.Select(x => x.Rack)),
                CreateLookup(warehouseLocations.Select(x => x.Posicion)),
                CreateLookup(warehouseLocations.Select(x => x.Nivel)));

            ApplyLocationFilters();
            RefreshSelectedLocations();
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

            var availableLocations = filtered
                .Where(x => !_selectedLocations.Any(selected => selected.LocationId == x.LocationId))
                .OrderBy(x => x.Ubicacion)
                .ToList();

            _availableLocationsView = CollectionViewSource.GetDefaultView(availableLocations);
            _availableGridManager.ApplyTo(_availableLocationsView);
            dgUbicacionesDisponibles.ItemsSource = _availableLocationsView;
        }

        private void BtnAgregarUbicacion_Click(object sender, RoutedEventArgs e)
        {
            var selectedLocations = dgUbicacionesDisponibles.SelectedItems
                .OfType<LocationDto>()
                .ToList();

            if (selectedLocations.Count == 0)
            {
                DialogHelper.ShowWarning("Selecciona una ubicacion para agregar.");
                return;
            }

            var selectedLocationIds = _selectedLocations
                .Select(x => x.LocationId)
                .ToHashSet();

            foreach (var selectedLocation in selectedLocations)
            {
                if (selectedLocationIds.Add(selectedLocation.LocationId))
                {
                    _selectedLocations.Add(selectedLocation);
                }
            }

            RefreshSelectedLocations();
            ApplyLocationFilters();
        }

        private void BtnQuitarUbicacion_Click(object sender, RoutedEventArgs e)
        {
            var selectedLocations = dgUbicacionesSeleccionadas.SelectedItems
                .OfType<LocationDto>()
                .ToList();

            if (selectedLocations.Count == 0)
            {
                DialogHelper.ShowWarning("Selecciona una ubicacion para quitar.");
                return;
            }

            var selectedLocationIds = selectedLocations
                .Select(x => x.LocationId)
                .ToHashSet();

            _selectedLocations = _selectedLocations
                .Where(x => !selectedLocationIds.Contains(x.LocationId))
                .ToList();

            RefreshSelectedLocations();
            ApplyLocationFilters();
        }

        private void RefreshSelectedLocations()
        {
            var selectedLocations = _selectedLocations
                .OrderBy(x => x.Ubicacion)
                .ToList();

            _selectedLocationsView = CollectionViewSource.GetDefaultView(selectedLocations);
            _selectedGridManager.ApplyTo(_selectedLocationsView);
            dgUbicacionesSeleccionadas.ItemsSource = _selectedLocationsView;
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!lookupAuditor.TryCommitSelection() || lookupAuditor.SelectedLookupItem is not LookupItem auditorLookup)
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
                    AuditorUserId = auditorLookup.Id?.ToString() ?? string.Empty,
                    AuditorNombre = auditorLookup.Code,
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

        private static LookupItem ToAuditorLookupItem(DropDownDto auditor)
        {
            return new LookupItem
            {
                Id = auditor.Key ?? string.Empty,
                Code = auditor.Value ?? auditor.Key ?? string.Empty,
                Description = auditor.Description ?? auditor.Value ?? string.Empty,
                Data = auditor
            };
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
