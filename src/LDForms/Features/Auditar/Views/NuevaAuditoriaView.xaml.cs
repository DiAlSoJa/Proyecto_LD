using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LD.Client.Services;
using LD.Contracts.DTOs.LoadMapping;
using LD.FormsX.Helpers;

namespace LD.FormsX.Views.Auditar
{
    /// <summary>
    /// Lógica de interacción para NuevaAuditoriaView.xaml
    /// </summary>
    public partial class NuevaAuditoriaView : Window
    {
        private readonly LoadMappingService _loadMappingService;
        private readonly List<LoadMappingAvailableOrderDto> _allItems = new();

        public ObservableCollection<LoadMappingAvailableOrderDto> FilteredItems { get; } = new();
        public LoadMappingAvailableOrderDto? SelectedOrder { get; private set; }

        public NuevaAuditoriaView(LoadMappingService loadMappingService)
        {
            InitializeComponent();
            _loadMappingService = loadMappingService;
            DataContext = this;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrdersAsync();
        }

        private async System.Threading.Tasks.Task LoadOrdersAsync()
        {
            try
            {
                SetLoadingState(true, "Cargando ordenes...");
                _allItems.Clear();
                FilteredItems.Clear();
                SelectedOrder = null;
                TxtContexto.Text = "Solo se muestran ordenes en Cargando de tus almacenes.";

                var response = await _loadMappingService.GetLoadingOrders();
                if (!response.IsSuccess)
                {
                    TxtContexto.Text = response.Message ?? "No se pudieron cargar las ordenes.";
                    DialogHelper.ShowWarning(response.Message ?? "No se pudieron cargar las ordenes.");
                    return;
                }

                if (response.Data is null || response.Data.Count == 0)
                {
                    TxtContexto.Text = "No se encontraron ordenes de entrega en Cargando para tus almacenes.";
                    ApplyFilter();
                    return;
                }

                _allItems.AddRange(response.Data);
                TxtContexto.Text = "Selecciona una orden de entrega en Cargando para crear el mapeo.";
                ApplyFilter();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void ApplyFilter()
        {
            var searchText = txtBuscar.Text?.Trim() ?? string.Empty;

            IEnumerable<LoadMappingAvailableOrderDto> filtered = _allItems;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = filtered.Where(x =>
                    Contains(x.OrdenEntrega, searchText) ||
                    Contains(x.Cliente, searchText) ||
                    Contains(x.Proyecto, searchText) ||
                    Contains(x.Almacen, searchText) ||
                    Contains(x.Status, searchText) ||
                    Contains(x.KittingsCount.ToString(), searchText));
            }

            FilteredItems.Clear();
            foreach (var item in filtered
                         .OrderBy(x => x.OrdenEntrega)
                         .ThenBy(x => x.Cliente)
                         .ThenBy(x => x.Proyecto))
            {
                FilteredItems.Add(item);
            }

            if (FilteredItems.Count == 0)
            {
                SelectedOrder = null;
                BtnAceptar.IsEnabled = false;
                return;
            }

            dgOpciones.SelectedIndex = 0;
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            var selected = SelectedOrder ?? dgOpciones.SelectedItem as LoadMappingAvailableOrderDto;
            if (selected is null)
            {
                DialogHelper.ShowWarning("Selecciona una orden de entrega.");
                return;
            }

            SelectedOrder = selected;
            DialogResult = true;
            Close();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            ApplyFilter();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void dgOpciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedOrder = dgOpciones.SelectedItem as LoadMappingAvailableOrderDto;
            BtnAceptar.IsEnabled = SelectedOrder is not null;
        }

        private void dgOpciones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ConfirmSelection();
        }

        private void dgOpciones_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConfirmSelection();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                e.Handled = true;
            }
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                dgOpciones.Focus();
                if (dgOpciones.Items.Count > 0 && dgOpciones.SelectedIndex < 0)
                    dgOpciones.SelectedIndex = 0;

                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                ConfirmSelection();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                e.Handled = true;
            }
        }

        private void ConfirmSelection()
        {
            if (dgOpciones.SelectedItem is not LoadMappingAvailableOrderDto selected)
                return;

            SelectedOrder = selected;
            DialogResult = true;
            Close();
        }

        private void SetLoadingState(bool isLoading, string message = "Cargando ordenes...")
        {
            LoadingOverlay.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            TxtLoading.Text = message;
            txtBuscar.IsEnabled = !isLoading;
            dgOpciones.IsEnabled = !isLoading;
            BtnAceptar.IsEnabled = !isLoading && SelectedOrder is not null;
        }

        private static bool Contains(string? value, string searchText) =>
            !string.IsNullOrWhiteSpace(value) &&
            value.Contains(searchText, StringComparison.OrdinalIgnoreCase);

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                e.Handled = true;
            }
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
