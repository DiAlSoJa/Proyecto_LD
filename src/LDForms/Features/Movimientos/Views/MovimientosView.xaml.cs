using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.InventoryMovement;
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

namespace LD.FormsX.Movimientos
{
    public partial class MovimientosView : UserControl, INotifyPropertyChanged
    {
        private readonly InventoryMovementService _inventoryMovementService;
        private readonly LookupService _lookupService;
        private readonly DataGridColumnFilterManager _columnFilterManager;
        private List<UserProjectClientDto> _userProjectClients = new();
        private bool _loaded;
        private bool _loadingFilters;
        private int _selectedClientId;
        private int _selectedProjectId;
        private string _selectedClientText = string.Empty;
        private string _selectedProjectText = string.Empty;

        public ObservableCollection<InventoryMovementDto> Movements { get; } = new();
        public ObservableCollection<LookupItem> ClientLookupItems { get; } = new();
        public ObservableCollection<LookupItem> ProjectLookupItems { get; } = new();
        public ICollectionView MovementsView { get; }

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

        public MovimientosView(InventoryMovementService inventoryMovementService, LookupService lookupService)
        {
            InitializeComponent();
            _inventoryMovementService = inventoryMovementService;
            _lookupService = lookupService;
            DataContext = this;

            MovementsView = CollectionViewSource.GetDefaultView(Movements);
            MovementsView.Filter = FilterMovement;
            DataGridFilterStyler.Apply(dg);
            _columnFilterManager = new DataGridColumnFilterManager(dg);
            _columnFilterManager.ApplyTo(MovementsView);
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
                foreach (var movement in response.Data.OrderByDescending(x => x.MovementId))
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

            if (!string.IsNullOrWhiteSpace(SelectedClientText) &&
                !string.Equals(movement.Cliente?.Trim(), SelectedClientText.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(SelectedProjectText) &&
                !string.Equals(movement.Proyecto?.Trim(), SelectedProjectText.Trim(), StringComparison.OrdinalIgnoreCase))
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

            var standardId = txtStandardId?.Text?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(standardId) &&
                !Contains(movement.StandardIdStr, standardId) &&
                !Contains(movement.StandardId?.ToString(), standardId))
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

            await LoadMovementsAsync();
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
