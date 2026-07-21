using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.Location;
using LD.FormsX.Features.Ubicaciones.ViewModels;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Ubicaciones;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using LD.Client.Configuration;

namespace LD.FormsX.Views
{
    public partial class UbicacionesView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly LookupService _lookupService;
        private readonly WpfGridFilter<LocationDto> _gridFilter;
        private bool _loaded;
        private bool _loadingWarehouse;
        private int _selectedWarehouseId;

        private UbicacionesViewModel ViewModel => (UbicacionesViewModel)DataContext;

        public UbicacionesView(UbicacionesViewModel viewModel, IServiceProvider serviceProvider, LookupService lookupService)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;
            _lookupService = lookupService;

            _gridFilter = new WpfGridFilter<LocationDto>(dg);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
            });

            cmbAlmacen.ComboBoxElement.SelectionChanged += CmbAlmacen_SelectionChanged;

            viewModel.OnDataLoaded += data =>
            {
                var filteredData = (data ?? [])
                    .Where(x => _selectedWarehouseId > 0 && x.WarehouseId == _selectedWarehouseId)
                    .ToList();
                _gridFilter.SetData(filteredData);
            };
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await LoadWarehousesAsync();

            if (_selectedWarehouseId > 0)
                await ViewModel.CargarDatosAsync();
            else
                _gridFilter.SetData([]);
        }

        private async Task LoadWarehousesAsync()
        {
            try
            {
                _loadingWarehouse = true;
                _selectedWarehouseId = 0;
                ViewModel.SelectedWarehouseId = 0;
                cmbAlmacen.ItemsSource = null;
                cmbAlmacen.SelectedItem = null;
                cmbAlmacen.ComboBoxElement.SelectedIndex = -1;

                if (string.IsNullOrWhiteSpace(UserData.Id))
                {
                    DialogHelper.ShowWarning("No se pudo identificar al usuario para cargar sus almacenes.");
                    return;
                }

                var response = await _lookupService.GetWarehouseLookupByUser(UserData.Id);
                if (response.IsSuccess && response.Data != null)
                {
                    var warehouses = response.Data
                        .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                        .OrderBy(x => x.Value)
                        .ToList();

                    cmbAlmacen.DisplayMemberPath = "Value";
                    cmbAlmacen.SelectedValuePath = "Key";
                    cmbAlmacen.ItemsSource = warehouses;

                    if (warehouses.Count == 1)
                    {
                        cmbAlmacen.ComboBoxElement.SelectedIndex = 0;
                        if (int.TryParse(warehouses[0].Key, out var selectedWarehouseId))
                        {
                            _selectedWarehouseId = selectedWarehouseId;
                            ViewModel.SelectedWarehouseId = selectedWarehouseId;
                        }
                    }
                }
                else
                {
                    DialogHelper.ShowWarning(response.Message ?? "No se pudieron cargar tus almacenes.");
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _loadingWarehouse = false;
            }
        }

        private async void CmbAlmacen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_loaded || _loadingWarehouse)
                return;

            if (sender is not ComboBox combo)
                return;

            if (combo.SelectedValue is not string warehouseKey || !int.TryParse(warehouseKey, out var warehouseId))
            {
                _selectedWarehouseId = 0;
                ViewModel.SelectedWarehouseId = 0;
                _gridFilter.SetData([]);
                return;
            }

            _selectedWarehouseId = warehouseId;
            ViewModel.SelectedWarehouseId = warehouseId;
            await ViewModel.CargarDatosAsync();
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedLocation = _gridFilter.SelectedItem;
        }

        private void dg_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName is nameof(LocationDto.Ocupado) or nameof(LocationDto.Placas))
                e.Cancel = true;
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaUbicacionView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }

        private async void BtnNuevoMasivo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaUbicacionMasivaView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var selectedLocation = ViewModel.SelectedLocation;
            if (selectedLocation is null)
            {
                DialogHelper.ShowWarning("Selecciona una ubicacion para editar.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<NuevaUbicacionView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetLocation(selectedLocation);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }

        private async void BtnEditarMasivo_Click(object sender, RoutedEventArgs e)
        {
            var selectedLocations = dg.SelectedItems.OfType<LocationDto>().ToList();

            if (selectedLocations.Count < 2)
            {
                DialogHelper.ShowWarning("Selecciona al menos dos ubicaciones para editar masivo.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<NuevaUbicacionView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetLocations(selectedLocations);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }
    }
}

