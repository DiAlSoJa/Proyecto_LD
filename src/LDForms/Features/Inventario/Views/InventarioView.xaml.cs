using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.AvailableInventory;
using LD.Contracts.DTOs;
using LD.Contracts.InventaryStatus;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using LD.FormsX.Model.Lookup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LD.FormsX.Views.Inventario
{
    public partial class InventarioView : UserControl, INotifyPropertyChanged
    {
        private readonly AvailableInventoryService _availableInventoryService;
        private readonly LookupService _lookupService;
        private readonly InventaryStatusService _inventaryStatusService;
        private readonly DataGridColumnFilterManager _columnFilterManager;
        private List<UserProjectClientDto> _userProjectClients = new();
        private bool _loaded;
        private bool _loadingFilters;
        private int _selectedClientId;
        private int _selectedProjectId;
        private string _selectedClientText = string.Empty;
        private string _selectedProjectText = string.Empty;

        public ObservableCollection<AvailableInventoryDto> AvailableInventories { get; } = new();
        public ObservableCollection<LookupItem> ClientLookupItems { get; } = new();
        public ObservableCollection<LookupItem> ProjectLookupItems { get; } = new();
        public ObservableCollection<LookupItem> WarehouseLookupItems { get; } = new();
        public ObservableCollection<LookupItem> LocationLookupItems { get; } = new();
        public ObservableCollection<LookupItem> StatusLookupItems { get; } = new();
        public ICollectionView AvailableInventoriesView { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int SelectedClientId
        {
            get => _selectedClientId;
            set
            {
                if (_selectedClientId == value)
                    return;

                _selectedClientId = value;
                OnPropertyChanged(nameof(SelectedClientId));
            }
        }

        public int SelectedProjectId
        {
            get => _selectedProjectId;
            set
            {
                if (_selectedProjectId == value)
                    return;

                _selectedProjectId = value;
                OnPropertyChanged(nameof(SelectedProjectId));
            }
        }

        public string SelectedClientText
        {
            get => _selectedClientText;
            set
            {
                if (_selectedClientText == value)
                    return;

                _selectedClientText = value;
                OnPropertyChanged(nameof(SelectedClientText));
            }
        }

        public string SelectedProjectText
        {
            get => _selectedProjectText;
            set
            {
                if (_selectedProjectText == value)
                    return;

                _selectedProjectText = value;
                OnPropertyChanged(nameof(SelectedProjectText));
            }
        }

        public InventarioView(
            AvailableInventoryService availableInventoryService,
            LookupService lookupService,
            InventaryStatusService inventaryStatusService)
        {
            InitializeComponent();
            _availableInventoryService = availableInventoryService;
            _lookupService = lookupService;
            _inventaryStatusService = inventaryStatusService;
            DataContext = this;

            AvailableInventoriesView = CollectionViewSource.GetDefaultView(AvailableInventories);
            AvailableInventoriesView.Filter = FilterInventory;
            DataGridFilterStyler.Apply(dg);
            _columnFilterManager = new DataGridColumnFilterManager(dg);
            _columnFilterManager.ApplyTo(AvailableInventoriesView);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded)
                return;

            _loaded = true;

            await LoadClientsAsync();
        }

        private async Task LoadClientsAsync()
        {
            try
            {
                _loadingFilters = true;
                ClientLookupItems.Clear();
                ProjectLookupItems.Clear();

                if (string.IsNullOrWhiteSpace(UserData.Id))
                {
                    DialogHelper.ShowWarning("No se pudo identificar el usuario actual para cargar clientes y proyectos.");
                    return;
                }

                var response = await _lookupService.GetProjectClientsByUserWarehouses(UserData.Id);
                if (response.IsSuccess && response.Data != null)
                {
                    _userProjectClients = response.Data;
                    var clientes = _userProjectClients
                        .GroupBy(x => x.ClientId)
                        .Select(group => new DropDownDto
                        {
                            Key = group.Key.ToString(),
                            Value = group.First().Client
                        })
                        .OrderBy(x => x.Value);

                    foreach (var item in clientes.Select(ToLookupItem))
                        ClientLookupItems.Add(item);
                }
                else
                {
                    _userProjectClients.Clear();
                    DialogHelper.ShowWarning(response.Message ?? "No se pudieron cargar clientes y proyectos del usuario.");
                }

                ClearProjectSelection();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _loadingFilters = false;
            }
        }

        private Task LoadProjectsAsync()
        {
            if (SelectedClientId <= 0)
            {
                ProjectLookupItems.Clear();
                ClearProjectSelection();
                return Task.CompletedTask;
            }

            try
            {
                _loadingFilters = true;
                ProjectLookupItems.Clear();
                var proyectos = _userProjectClients
                    .Where(x => x.ClientId == SelectedClientId)
                    .GroupBy(x => x.ProjectId)
                    .Select(group => new DropDownDto
                    {
                        Key = group.Key.ToString(),
                        Value = group.First().Project
                    })
                    .OrderBy(x => x.Value);

                foreach (var item in proyectos.Select(ToLookupItem))
                    ProjectLookupItems.Add(item);

                ClearProjectSelection();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _loadingFilters = false;
            }

            return Task.CompletedTask;
        }

        private async Task LoadLocationsAsync()
        {
            try
            {
                var response = await _lookupService.GetLocationLookup();

                LocationLookupItems.Clear();
                if (response.IsSuccess && response.Data != null)
                {
                    foreach (var item in response.Data.Select(ToLookupItem).OrderBy(x => x.Code))
                        LocationLookupItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task LoadWarehousesAsync()
        {
            try
            {
                var response = await _lookupService.GetWarehouseLookup();

                WarehouseLookupItems.Clear();
                if (response.IsSuccess && response.Data != null)
                {
                    foreach (var item in response.Data.Select(ToLookupItem).OrderBy(x => x.Code))
                        WarehouseLookupItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task LoadStatusesAsync()
        {
            try
            {
                var response = await _inventaryStatusService.GetInventaryStatus();

                StatusLookupItems.Clear();
                if (response.IsSuccess && response.Data != null)
                {
                    foreach (var item in response.Data.Select(ToLookupItem).OrderBy(x => x.Code))
                        StatusLookupItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task LoadAvailableInventoriesAsync()
        {
            try
            {
                ShowLoader(true, "Cargando inventario disponible...");
                txtStatus.Text = "Cargando inventario disponible...";

                var response = await _availableInventoryService.GetAvailableInventories();
                if (!response.IsSuccess || response.Data == null)
                {
                    AvailableInventories.Clear();
                    txtStatus.Text = response.Message ?? "No se pudo cargar el inventario disponible.";
                    return;
                }

                AvailableInventories.Clear();
                foreach (var inventory in response.Data.OrderByDescending(x => x.Fecha).ThenByDescending(x => x.Hora))
                    AvailableInventories.Add(inventory);

                RefreshFilters();
            }
            catch (Exception ex)
            {
                AvailableInventories.Clear();
                txtStatus.Text = "Ocurrió un error al cargar el inventario disponible.";
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                ShowLoader(false);
            }
        }

        private bool FilterInventory(object item)
        {
            if (item is not AvailableInventoryDto inventory)
                return false;

            if (IsHiddenAvailableStatus(inventory.AvailableStatus))
                return false;

            if (!string.IsNullOrWhiteSpace(SelectedClientText) &&
                !string.Equals(inventory.Cliente?.Trim(), SelectedClientText.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(SelectedProjectText) &&
                !string.Equals(inventory.Proyecto?.Trim(), SelectedProjectText.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var partNumber = txtNumeroParte?.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(partNumber) &&
                !Contains(inventory.PartNumber, partNumber) &&
                !Contains(inventory.Description, partNumber))
            {
                return false;
            }

            var standardId = txtStandardId?.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(standardId) &&
                !Contains(inventory.SD, standardId) &&
                !Contains(inventory.StandardIdStr, standardId) &&
                !Contains(inventory.StandardId?.ToString(), standardId))
            {
                return false;
            }

            return true;
        }

        private static bool IsHiddenAvailableStatus(string? availableStatus) =>
            string.Equals(availableStatus?.Trim(), "Salida", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(availableStatus?.Trim(), "Surtido", StringComparison.OrdinalIgnoreCase);

        private static bool Contains(string? source, string searchText)
        {
            return !string.IsNullOrWhiteSpace(source)
                && source.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }

        private void RefreshFilters()
        {
            AvailableInventoriesView.Refresh();
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            var count = AvailableInventoriesView.Cast<object>().Count();
            txtStatus.Text = count > 0
                ? $"{count} registro(s) encontrados."
                : "Sin datos para mostrar";
        }

        private void ShowLoader(bool show, string message = "Cargando...")
        {
            TxtLoading.Text = message;
            LoadingOverlay.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            await LoadAvailableInventoriesAsync();
        }

        private async void BtnCambiarUbicacion_Click(object sender, RoutedEventArgs e)
        {
            var selectedInventories = dg.SelectedItems
                .OfType<AvailableInventoryDto>()
                .ToList();

            if (!selectedInventories.Any())
            {
                DialogHelper.ShowWarning("Selecciona al menos un registro de inventario.");
                return;
            }

            var standardIds = selectedInventories
                .Select(GetStandardId)
                .Where(x => x.HasValue && x.Value > 0)
                .Select(x => x!.Value)
                .Distinct()
                .ToList();

            if (standardIds.Count != selectedInventories.Count)
            {
                DialogHelper.ShowWarning("Uno o mas registros seleccionados no tienen StandardId.");
                return;
            }

            if (!LocationLookupItems.Any())
                await LoadLocationsAsync();

            var dialog = new CambiarUbicacionInventarioDialog(selectedInventories.First(), LocationLookupItems);
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(
                dialog,
                LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                ShowLoader(true, "Cambiando ubicacion...");
                var response = await _availableInventoryService.ChangeLocation(standardIds, dialog.UbicacionDestino);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cambiar la ubicacion.");
                    return;
                }

                DialogHelper.ShowSuccess(response.Message ?? "Ubicacion actualizada correctamente.");
                await LoadAvailableInventoriesAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                ShowLoader(false);
            }
        }

        private static int? GetStandardId(AvailableInventoryDto inventory)
        {
            if (inventory.StandardId.HasValue)
                return inventory.StandardId.Value;

            return int.TryParse(inventory.StandardIdStr, out var parsedStandardId)
                ? parsedStandardId
                : null;
        }

        private async void BtnCambiarStatus_Click(object sender, RoutedEventArgs e)
        {
            var selectedInventories = dg.SelectedItems
                .OfType<AvailableInventoryDto>()
                .ToList();

            if (!selectedInventories.Any())
            {
                DialogHelper.ShowWarning("Selecciona al menos un registro de inventario.");
                return;
            }

            var standardIds = selectedInventories
                .Select(GetStandardId)
                .Where(x => x.HasValue && x.Value > 0)
                .Select(x => x!.Value)
                .Distinct()
                .ToList();

            if (standardIds.Count != selectedInventories.Count)
            {
                DialogHelper.ShowWarning("Uno o mas registros seleccionados no tienen StandardId.");
                return;
            }

            if (!StatusLookupItems.Any())
                await LoadStatusesAsync();

            var dialog = new CambiarStatusInventarioDialog(selectedInventories.First(), StatusLookupItems);
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(
                dialog,
                LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                ShowLoader(true, "Cambiando status...");
                var response = await _availableInventoryService.ChangeStatus(standardIds, dialog.StatusDestino);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cambiar el status.");
                    return;
                }

                DialogHelper.ShowSuccess(response.Message ?? "Status actualizado correctamente.");
                await LoadAvailableInventoriesAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                ShowLoader(false);
            }
        }

        private async void BtnCambiarAlmacen_Click(object sender, RoutedEventArgs e)
        {
            var selectedInventories = dg.SelectedItems
                .OfType<AvailableInventoryDto>()
                .ToList();

            if (!selectedInventories.Any())
            {
                DialogHelper.ShowWarning("Selecciona al menos un registro de inventario.");
                return;
            }

            var standardIds = selectedInventories
                .Select(GetStandardId)
                .Where(x => x.HasValue && x.Value > 0)
                .Select(x => x!.Value)
                .Distinct()
                .ToList();

            if (standardIds.Count != selectedInventories.Count)
            {
                DialogHelper.ShowWarning("Uno o mas registros seleccionados no tienen StandardId.");
                return;
            }

            if (!WarehouseLookupItems.Any())
                await LoadWarehousesAsync();

            var dialog = new CambiarAlmacenInventarioDialog(selectedInventories.First(), WarehouseLookupItems, _lookupService);
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(
                dialog,
                LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                ShowLoader(true, "Cambiando almacen...");
                var response = await _availableInventoryService.ChangeWarehouse(
                    standardIds,
                    dialog.WarehouseId,
                    dialog.UbicacionDestino);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cambiar el almacen.");
                    return;
                }

                DialogHelper.ShowSuccess(response.Message ?? "Almacen actualizado correctamente.");
                await LoadAvailableInventoriesAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                ShowLoader(false);
            }
        }

        private async void LookupCliente_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            if (!_loaded || _loadingFilters)
                return;

            if (sender is not InlineLookupEditor editor || editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not DropDownDto selectedClient)
                return;

            SelectedClientId = int.TryParse(selectedClient.Key, out var clientId) ? clientId : 0;
            SelectedClientText = selectedClient.Value ?? string.Empty;

            await LoadProjectsAsync();
            RefreshFilters();
        }

        private async void LookupProyecto_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            if (!_loaded || _loadingFilters)
                return;

            if (sender is not InlineLookupEditor editor || editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not DropDownDto selectedProject)
                return;

            SelectedProjectId = int.TryParse(selectedProject.Key, out var projectId) ? projectId : 0;
            SelectedProjectText = selectedProject.Value ?? string.Empty;

            await LoadAvailableInventoriesAsync();
        }

        private void Filtro_Changed(object sender, TextChangedEventArgs e)
        {
            if (!_loaded)
                return;

            RefreshFilters();
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

        private static LookupItem ToLookupItem(InventaryStatusDto item)
        {
            return new LookupItem
            {
                Id = 0,
                Code = item.StatusId ?? string.Empty,
                Description = item.Descripcion ?? string.Empty,
                Data = item
            };
        }

        private void ClearProjectSelection()
        {
            SelectedProjectId = 0;
            SelectedProjectText = string.Empty;
            lookupProyecto?.ClearSelection();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
