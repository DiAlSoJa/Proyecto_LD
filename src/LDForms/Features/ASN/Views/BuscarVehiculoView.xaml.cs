using LD.Client.Services;
using LD.Contracts.Vehicle;
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
        private readonly VehicleService _vehicleService;
        private readonly DataGridColumnFilterManager _columnFilterManager;
        private string _statusMessage = "Cargando vehículos...";

        public ObservableCollection<VehicleDto> Vehicles { get; } = new();
        public ICollectionView VehiclesView { get; }

        public VehicleDto? SelectedVehicle => dgVehiculos.SelectedItem as VehicleDto;

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

        public BuscarVehiculoView(VehicleService vehicleService)
        {
            InitializeComponent();
            _vehicleService = vehicleService;
            DataContext = this;

            VehiclesView = CollectionViewSource.GetDefaultView(Vehicles);
            VehiclesView.Filter = FilterVehicle;
            DataGridFilterStyler.Apply(dgVehiculos);
            _columnFilterManager = new DataGridColumnFilterManager(dgVehiculos);
            _columnFilterManager.ApplyTo(VehiclesView);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbFiltroHoras.SelectedIndex = 1;
            await LoadVehiclesAsync();
        }

        private async System.Threading.Tasks.Task LoadVehiclesAsync()
        {
            try
            {
                var response = await _vehicleService.GetVehicles();

                if (!response.IsSuccess || response.Data == null)
                {
                    StatusMessage = response.Message ?? "No se pudieron cargar los vehículos.";
                    return;
                }

                Vehicles.Clear();
                foreach (var vehicle in response.Data.OrderByDescending(x => x.CreatedAt))
                    Vehicles.Add(vehicle);

                VehiclesView.Refresh();
                StatusMessage = VehiclesView.Cast<object>().Any()
                    ? $"{VehiclesView.Cast<object>().Count()} vehículo(s) disponibles."
                    : "No se encontraron vehículos.";
            }
            catch (Exception ex)
            {
                StatusMessage = "Ocurrió un error al cargar los vehículos.";
                DialogHelper.ShowError(ex.Message);
            }
        }

        private bool FilterVehicle(object item)
        {
            if (item is not VehicleDto vehicle)
                return false;

            if (!MatchesHourFilter(vehicle))
                return false;

            var searchText = txtBuscar?.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(searchText))
                return true;

            return Contains(vehicle.Placas, searchText)
                || Contains(vehicle.Nombre, searchText)
                || Contains(vehicle.Tipo, searchText)
                || Contains(vehicle.NumeroVehiculo, searchText);
        }

        private bool MatchesHourFilter(VehicleDto vehicle)
        {
            if (cmbFiltroHoras?.SelectedIndex <= 0)
                return true;

            if (vehicle.CreatedAt == default)
                return true;

            DateTime now = DateTime.Now;
            DateTime limit = cmbFiltroHoras.SelectedIndex switch
            {
                1 => now.AddHours(-24),
                2 => now.AddDays(-7),
                3 => now.AddDays(-30),
                _ => DateTime.MinValue
            };

            return vehicle.CreatedAt >= limit;
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

        private void cmbFiltroHoras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            VehiclesView.Refresh();
            UpdateStatusMessage();
        }

        private void UpdateStatusMessage()
        {
            StatusMessage = VehiclesView.Cast<object>().Any()
                ? $"{VehiclesView.Cast<object>().Count()} vehículo(s) encontrados."
                : "No se encontraron vehículos con ese filtro.";
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
                DialogHelper.ShowWarning("Selecciona un vehículo.");
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
