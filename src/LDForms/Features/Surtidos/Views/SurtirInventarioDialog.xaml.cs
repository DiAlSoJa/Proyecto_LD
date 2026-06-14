using LD.Client.Services;
using LD.Contracts.AvailableInventory;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace LD.FormsX.Features.Surtidos.Views
{
    public partial class SurtirInventarioDialog : Window, INotifyPropertyChanged
    {
        private const string DisponibleStatus = "Disponible";
        private const string SurtidoStatus = "Surtido";

        private readonly AvailableInventoryService _availableInventoryService;
        private readonly ObservableCollection<AvailableInventoryDto> _inventories = new();
        private readonly DataGridColumnFilterManager _columnFilterManager;
        private readonly ICollectionView _inventoriesView;
        private List<AvailableInventoryDto> _allInventories = new();
        private bool _loading;
        private bool _soloEstatusDisponible = true;
        private string _clientFilter = string.Empty;
        private string _projectFilter = string.Empty;
        private string _partFilter = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public IReadOnlyList<AvailableInventoryDto> SelectedInventories { get; private set; } = Array.Empty<AvailableInventoryDto>();

        public string ClientFilter
        {
            get => _clientFilter;
            set
            {
                if (_clientFilter == value)
                    return;

                _clientFilter = value;
                OnPropertyChanged(nameof(ClientFilter));
            }
        }

        public string ProjectFilter
        {
            get => _projectFilter;
            set
            {
                if (_projectFilter == value)
                    return;

                _projectFilter = value;
                OnPropertyChanged(nameof(ProjectFilter));
            }
        }

        public string PartFilter
        {
            get => _partFilter;
            set
            {
                if (_partFilter == value)
                    return;

                _partFilter = value;
                OnPropertyChanged(nameof(PartFilter));
            }
        }

        public bool SoloEstatusDisponible
        {
            get => _soloEstatusDisponible;
            set
            {
                if (_soloEstatusDisponible == value)
                    return;

                _soloEstatusDisponible = value;
                OnPropertyChanged(nameof(SoloEstatusDisponible));
                ApplyFilter();
            }
        }

        public SurtirInventarioDialog(
            AvailableInventoryService availableInventoryService,
            string? initialPartNumber,
            string? initialClient,
            string? initialProject)
        {
            InitializeComponent();
            DataContext = this;

            _availableInventoryService = availableInventoryService;
            DataGridFilterStyler.Apply(dgInventario);
            _columnFilterManager = new DataGridColumnFilterManager(dgInventario);
            _inventoriesView = CollectionViewSource.GetDefaultView(_inventories);
            _inventoriesView.Filter = FilterInventory;
            _columnFilterManager.ApplyTo(_inventoriesView);
            dgInventario.ItemsSource = _inventoriesView;

            PartFilter = initialPartNumber?.Trim() ?? string.Empty;
            ClientFilter = initialClient?.Trim() ?? string.Empty;
            ProjectFilter = initialProject?.Trim() ?? string.Empty;

            txtParte.Text = PartFilter;
            txtCliente.Text = ClientFilter;
            txtProyecto.Text = ProjectFilter;

            Loaded += SurtirInventarioDialog_Loaded;
        }

        private async void SurtirInventarioDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadInventoriesAsync();
            ApplyFilter();
        }

        private async Task LoadInventoriesAsync()
        {
            try
            {
                SetLoading(true, "Cargando inventario...");

                var response = await _availableInventoryService.GetAvailableInventories();
                if (!response.IsSuccess || response.Data == null)
                {
                    _allInventories = new List<AvailableInventoryDto>();
                    txtStatus.Text = response.Message ?? "No se pudo cargar el inventario.";
                    return;
                }

                _allInventories = response.Data
                    .Where(x => x.Qty.GetValueOrDefault() > 0 || IsInventoryStatus(x, DisponibleStatus) || IsInventoryStatus(x, SurtidoStatus))
                    .OrderByDescending(x => x.Fecha)
                    .ThenByDescending(x => x.Hora)
                    .ToList();

                RebuildInventoryCollection();
                txtStatus.Text = $"{_allInventories.Count} registro(s) cargados.";
            }
            catch (Exception ex)
            {
                _allInventories = new List<AvailableInventoryDto>();
                RebuildInventoryCollection();
                txtStatus.Text = "Ocurrio un error al cargar el inventario.";
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void ApplyFilter()
        {
            _inventoriesView.Refresh();

            var count = _inventoriesView.Cast<object>().Count();
            txtStatus.Text = count > 0
                ? $"{count} registro(s) encontrados."
                : "Sin resultados para los filtros actuales.";
        }

        private void Filtro_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == txtCliente)
                ClientFilter = txtCliente.Text?.Trim() ?? string.Empty;
            else if (sender == txtProyecto)
                ProjectFilter = txtProyecto.Text?.Trim() ?? string.Empty;
            else if (sender == txtParte)
                PartFilter = txtParte.Text?.Trim() ?? string.Empty;

            ApplyFilter();
        }

        private void ChkSoloEstatusDisponible_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
                SoloEstatusDisponible = checkBox.IsChecked ?? false;
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgInventario.SelectedItems
                .OfType<AvailableInventoryDto>()
                .ToList();

            if (!selected.Any())
            {
                DialogHelper.ShowWarning("Selecciona al menos un registro de inventario.");
                return;
            }

            if (selected.Any(x => string.IsNullOrWhiteSpace(GetStandardId(x))))
            {
                DialogHelper.ShowWarning("Todos los registros seleccionados deben tener StandardId.");
                return;
            }

            if (selected.Any(x => !IsInventoryStatus(x, DisponibleStatus)))
            {
                DialogHelper.ShowWarning("Solo puedes agregar registros con estatus disponible.");
                return;
            }

            SelectedInventories = selected;
            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void DgInventario_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BtnAceptar_Click(sender, new RoutedEventArgs());
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private static bool Contains(string? source, string search)
        {
            return !string.IsNullOrWhiteSpace(source) &&
                   source.Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        private bool FilterInventory(object item)
        {
            if (item is not AvailableInventoryDto inventory)
                return false;

            if (SoloEstatusDisponible)
            {
                if (!IsInventoryStatus(inventory, DisponibleStatus))
                    return false;
            }
            else if (!IsInventoryStatus(inventory, DisponibleStatus) &&
                     !IsInventoryStatus(inventory, SurtidoStatus))
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(ClientFilter) && !Contains(inventory.Cliente, ClientFilter))
                return false;

            if (!string.IsNullOrWhiteSpace(ProjectFilter) && !Contains(inventory.Proyecto, ProjectFilter))
                return false;

            if (!string.IsNullOrWhiteSpace(PartFilter) &&
                !Contains(inventory.PartNumber, PartFilter) &&
                !Contains(inventory.Description, PartFilter))
            {
                return false;
            }

            return true;
        }

        private void RebuildInventoryCollection()
        {
            _inventories.Clear();
            foreach (var inventory in _allInventories)
                _inventories.Add(inventory);

            _inventoriesView.Refresh();
        }

        private static string? GetStandardId(AvailableInventoryDto inventory)
        {
            if (inventory.StandardId.HasValue && inventory.StandardId.Value > 0)
                return inventory.StandardId.Value.ToString();

            return string.IsNullOrWhiteSpace(inventory.StandardIdStr)
                ? null
                : inventory.StandardIdStr.Trim();
        }

        private static bool IsInventoryStatus(AvailableInventoryDto inventory, string expectedStatus)
        {
            var availableStatus = inventory.AvailableStatus?.Trim();
            if (!string.IsNullOrWhiteSpace(availableStatus))
                return string.Equals(availableStatus, expectedStatus, StringComparison.OrdinalIgnoreCase);

            var statusId = inventory.StatusId?.Trim();
            return string.Equals(statusId, expectedStatus, StringComparison.OrdinalIgnoreCase);
        }

        private void SetLoading(bool loading, string message = "")
        {
            _loading = loading;
            loadingOverlay.Visibility = loading ? Visibility.Visible : Visibility.Collapsed;
            if (!string.IsNullOrWhiteSpace(message))
                txtLoading.Text = message;
            btnAceptar.IsEnabled = !loading;
            btnCancelar.IsEnabled = !loading;
            txtCliente.IsEnabled = !loading;
            txtProyecto.IsEnabled = !loading;
            txtParte.IsEnabled = !loading;
            dgInventario.IsEnabled = !loading;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
