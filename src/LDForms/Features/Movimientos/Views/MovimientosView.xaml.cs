using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.InventoryMovement;
using LD.FormsX.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LD.FormsX.Movimientos
{
    public partial class MovimientosView : UserControl, INotifyPropertyChanged
    {
        private readonly InventoryMovementService _inventoryMovementService;
        private readonly LookupService _lookupService;
        private bool _loaded;
        private bool _loadingFilters;

        public ObservableCollection<InventoryMovementDto> Movements { get; } = new();
        public ICollectionView MovementsView { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MovimientosView(InventoryMovementService inventoryMovementService, LookupService lookupService)
        {
            InitializeComponent();
            _inventoryMovementService = inventoryMovementService;
            _lookupService = lookupService;
            DataContext = this;

            MovementsView = CollectionViewSource.GetDefaultView(Movements);
            MovementsView.Filter = FilterMovement;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded)
                return;

            _loaded = true;

            ConfigureCombos();
            await LoadClientsAsync();
            await LoadMovementsAsync();
        }

        private void ConfigureCombos()
        {
            cmbCliente.DisplayMemberPath = nameof(DropDownDto.Value);
            cmbCliente.SelectedValuePath = nameof(DropDownDto.Key);

            cmbProyecto.DisplayMemberPath = nameof(DropDownDto.Value);
            cmbProyecto.SelectedValuePath = nameof(DropDownDto.Key);
        }

        private async Task LoadClientsAsync()
        {
            try
            {
                _loadingFilters = true;
                var response = await _lookupService.GetClientLookup();

                cmbCliente.ItemsSource = response.IsSuccess ? response.Data : null;
                cmbCliente.SelectedIndex = -1;
                cmbProyecto.ItemsSource = null;
                cmbProyecto.SelectedIndex = -1;
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

        private async Task LoadProjectsAsync()
        {
            if (cmbCliente.SelectedItem is not DropDownDto selectedClient ||
                !int.TryParse(selectedClient.Key, out var clientId) ||
                clientId <= 0)
            {
                cmbProyecto.ItemsSource = null;
                cmbProyecto.SelectedIndex = -1;
                return;
            }

            try
            {
                _loadingFilters = true;
                var response = await _lookupService.GetProjectClientLookup(clientId);

                cmbProyecto.ItemsSource = response.IsSuccess ? response.Data : null;
                cmbProyecto.SelectedIndex = -1;
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

        private async Task LoadMovementsAsync()
        {
            try
            {
                ShowLoader(true, "Cargando movimientos...");
                txtStatus.Text = "Cargando movimientos...";

                var response = await _inventoryMovementService.GetInventoryMovements();
                if (!response.IsSuccess || response.Data == null)
                {
                    Movements.Clear();
                    txtStatus.Text = response.Message ?? "No se pudieron cargar los movimientos.";
                    return;
                }

                Movements.Clear();
                foreach (var movement in response.Data.OrderByDescending(x => x.Fecha).ThenByDescending(x => x.Hora))
                    Movements.Add(movement);

                RefreshFilters();
            }
            catch (Exception ex)
            {
                Movements.Clear();
                txtStatus.Text = "Ocurrió un error al cargar los movimientos.";
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                ShowLoader(false);
            }
        }

        private bool FilterMovement(object item)
        {
            if (item is not InventoryMovementDto movement)
                return false;

            if (cmbCliente.SelectedItem is DropDownDto selectedClient &&
                !string.IsNullOrWhiteSpace(selectedClient.Value) &&
                !string.Equals(movement.Cliente?.Trim(), selectedClient.Value.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (cmbProyecto.SelectedItem is DropDownDto selectedProject &&
                !string.IsNullOrWhiteSpace(selectedProject.Value) &&
                !string.Equals(movement.Proyecto?.Trim(), selectedProject.Value.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var partNumber = txtNumeroParte?.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(partNumber) &&
                !Contains(movement.PartNumber, partNumber) &&
                !Contains(movement.Description, partNumber))
            {
                return false;
            }

            if (dpFechaInicio.SelectedDate is DateTime startDate && movement.Fecha.Date < startDate.Date)
                return false;

            if (dpFechaFin.SelectedDate is DateTime endDate && movement.Fecha.Date > endDate.Date)
                return false;

            return true;
        }

        private static bool Contains(string? source, string searchText)
        {
            return !string.IsNullOrWhiteSpace(source)
                && source.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }

        private void RefreshFilters()
        {
            MovementsView.Refresh();
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            var count = MovementsView.Cast<object>().Count();
            txtStatus.Text = count > 0
                ? $"{count} movimiento(s) encontrados."
                : "Sin datos para mostrar";
        }

        private void ShowLoader(bool show, string message = "Cargando...")
        {
            TxtLoading.Text = message;
            LoadingOverlay.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            await LoadMovementsAsync();
        }

        private async void cmbCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_loaded || _loadingFilters)
                return;

            await LoadProjectsAsync();
            RefreshFilters();
        }

        private void cmbProyecto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_loaded || _loadingFilters)
                return;

            RefreshFilters();
        }

        private void Filtro_Changed(object sender, TextChangedEventArgs e)
        {
            if (!_loaded)
                return;

            RefreshFilters();
        }

        private void FiltroFecha_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!_loaded)
                return;

            RefreshFilters();
        }
    }
}
