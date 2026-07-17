using LD.Client.Services;
using LD.Contracts.DTOs.Security;
using LD.FormsX.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace LD.FormsX.Views.Dialogs
{
    public partial class BuscarVehiculoView : Window, INotifyPropertyChanged
    {
        public enum VehicleSearchMode
        {
            Patio,
            AsnDescarga
        }

        private readonly PatioClientService _patioClientService;
        private readonly DataGridColumnFilterManager _columnFilterManager;
        private string _statusMessage = "Cargando registros de seguridad...";

        public VehicleSearchMode SearchMode { get; set; } = VehicleSearchMode.Patio;
        public ObservableCollection<SecurityRegistrationDto> Vehicles { get; } = new();
        public ICollectionView VehiclesView { get; }

        public SecurityRegistrationDto? SelectedVehicle => dgVehiculos.SelectedItem as SecurityRegistrationDto;

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusMessage)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public BuscarVehiculoView(PatioClientService patioClientService)
        {
            InitializeComponent();
            _patioClientService = patioClientService;

            VehiclesView = CollectionViewSource.GetDefaultView(Vehicles);
            VehiclesView.Filter = FilterVehicle;
            dgVehiculos.ItemsSource = VehiclesView;
            DataContext = this;

            DataGridFilterStyler.Apply(dgVehiculos);
            _columnFilterManager = new DataGridColumnFilterManager(dgVehiculos);
            _columnFilterManager.ApplyTo(VehiclesView);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbFiltroHoras.SelectedIndex = 0;
            await LoadVehiclesAsync();
        }

        private async System.Threading.Tasks.Task LoadVehiclesAsync()
        {
            try
            {
                var response = SearchMode == VehicleSearchMode.AsnDescarga
                    ? await _patioClientService.GetVehiculosDescargaAsync()
                    : await _patioClientService.GetVehiculosSinSalidaAsync();

                if (!response.IsSuccess || response.Data == null)
                {
                    StatusMessage = response.ErrorMessage
                        ?? response.Message
                        ?? $"No se pudieron cargar los registros de seguridad. Codigo: {response.Code}";
                    return;
                }

                var (hours, days) = GetSelectedCreatedAtFilter();
                var now = DateTime.UtcNow;
                var filteredVehicles = response.Data.AsEnumerable();

                if (SearchMode == VehicleSearchMode.AsnDescarga)
                {
                    filteredVehicles = filteredVehicles.Where(vehicle =>
                        string.Equals(vehicle.Tipo?.Trim(), "Descarga", StringComparison.OrdinalIgnoreCase));
                }

                if (days.HasValue)
                {
                    var from = now.AddDays(-days.Value);
                    filteredVehicles = filteredVehicles.Where(vehicle =>
                        vehicle.CreatedAt >= from && vehicle.CreatedAt <= now);
                }
                else if (hours.HasValue)
                {
                    var from = now.AddHours(-hours.Value);
                    filteredVehicles = filteredVehicles.Where(vehicle =>
                        vehicle.CreatedAt >= from && vehicle.CreatedAt <= now);
                }

                Vehicles.Clear();
                foreach (var vehicle in filteredVehicles.OrderByDescending(x => x.CreatedAt))
                    Vehicles.Add(vehicle);

                VehiclesView.Refresh();
                StatusMessage = VehiclesView.Cast<object>().Any()
                    ? $"{VehiclesView.Cast<object>().Count()} registro(s) disponibles."
                    : SearchMode == VehicleSearchMode.AsnDescarga
                        ? "No se encontraron vehiculos de descarga."
                        : "No se encontraron registros de seguridad.";
            }
            catch (Exception ex)
            {
                StatusMessage = "Ocurrio un error al cargar los registros de seguridad.";
                DialogHelper.ShowError(ex.Message);
            }
        }

        private bool FilterVehicle(object item)
        {
            if (item is not SecurityRegistrationDto vehicle)
                return false;

            var searchText = txtBuscar?.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(searchText))
                return true;

            return Contains(vehicle.Placa, searchText)
                || Contains(vehicle.Nombre, searchText)
                || Contains(vehicle.Tipo, searchText)
                || Contains(vehicle.TipoVehiculo, searchText)
                || Contains(vehicle.Linea, searchText)
                || Contains(vehicle.Numero, searchText)
                || Contains(vehicle.Licencia, searchText);
        }

        private (int? Hours, int? Days) GetSelectedCreatedAtFilter()
        {
            var selectedValue = (cmbFiltroHoras?.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? string.Empty;

            return selectedValue switch
            {
                "all"  => (null, null),
                "24h"  => (24, null),
                "3d"   => (null, 3),
                "5d"   => (null, 5),
                "7d"   => (null, 7),
                "30d"  => (null, 30),
                _      => (null, null)
            };
        }

        private static bool Contains(string? source, string searchText)
        {
            return !string.IsNullOrWhiteSpace(source)
                && source.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            VehiclesView.Refresh();
            UpdateStatusMessage();
        }

        private async void cmbFiltroHoras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            await LoadVehiclesAsync();
        }

        private void UpdateStatusMessage()
        {
            StatusMessage = VehiclesView.Cast<object>().Any()
                ? $"{VehiclesView.Cast<object>().Count()} registro(s) encontrados."
                : "No se encontraron registros con ese filtro.";
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            ConfirmSelection();
        }

        private void dgVehiculos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedVehicle != null)
                ConfirmSelection();
        }

        private void dgVehiculos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;
            ConfirmSelection();
        }

        private void ConfirmSelection()
        {
            if (SelectedVehicle == null)
            {
                DialogHelper.ShowWarning("Selecciona un registro de seguridad.");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
