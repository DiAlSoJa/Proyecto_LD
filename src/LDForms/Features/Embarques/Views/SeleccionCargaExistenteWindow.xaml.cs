using LD.Contracts.Constants;
using LD.Contracts.Kitting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Features.Embarques.Views
{
    public partial class SeleccionCargaExistenteWindow : Window
    {
        private readonly List<DeliveryOrderOption> _allItems = new();

        public ObservableCollection<DeliveryOrderOption> FilteredItems { get; } = new();

        public string? SelectedDeliveryOrderCode { get; private set; }

        public SeleccionCargaExistenteWindow(
            IEnumerable<KittingDto> items,
            string clientText,
            string projectText)
        {
            InitializeComponent();
            DataContext = this;

            TxtContext.Text = $"Cliente: {clientText} | Proyecto: {projectText}";

            _allItems = BuildOptions(items, clientText, projectText);
            ApplyFilter();

            Loaded += SeleccionCargaExistenteWindow_Loaded;
        }

        private void SeleccionCargaExistenteWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtBuscar.Focus();
            txtBuscar.SelectAll();

            if (dgOpciones.Items.Count > 0)
            {
                dgOpciones.SelectedIndex = 0;
                BtnAceptar.IsEnabled = true;
            }
        }

        private static List<DeliveryOrderOption> BuildOptions(
            IEnumerable<KittingDto> items,
            string clientText,
            string projectText)
        {
            var filtered = items
                .Where(x => KittingStatusNames.IsLoading(x.Status))
                .Where(x => !string.IsNullOrWhiteSpace(x.DeliveryOrderCode))
                .Where(x => string.Equals(x.Client?.Trim(), clientText.Trim(), StringComparison.OrdinalIgnoreCase))
                .Where(x => string.Equals(x.Project?.Trim(), projectText.Trim(), StringComparison.OrdinalIgnoreCase));

            return filtered
                .GroupBy(x => x.DeliveryOrderCode!.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(group =>
                {
                    var first = group.First();
                    return new DeliveryOrderOption
                    {
                        DeliveryOrderCode = first.DeliveryOrderCode?.Trim() ?? string.Empty,
                        KittingCode = first.KittingCode?.Trim() ?? string.Empty,
                        KittingsCount = group.Count(),
                        Client = first.Client?.Trim() ?? string.Empty,
                        Project = first.Project?.Trim() ?? string.Empty,
                        Status = KittingStatusNames.Display(first.Status)
                    };
                })
                .OrderBy(x => x.DeliveryOrderCode)
                .ThenBy(x => x.KittingCode)
                .ToList();
        }

        private void ApplyFilter()
        {
            var searchText = txtBuscar.Text?.Trim() ?? string.Empty;

            IEnumerable<DeliveryOrderOption> filtered = _allItems;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = filtered.Where(x =>
                    Contains(x.DeliveryOrderCode, searchText) ||
                    Contains(x.KittingCode, searchText) ||
                    Contains(x.Client, searchText) ||
                    Contains(x.Project, searchText) ||
                    Contains(x.Status, searchText));
            }

            FilteredItems.Clear();
            foreach (var item in filtered)
                FilteredItems.Add(item);

            if (FilteredItems.Count == 0)
            {
                SelectedDeliveryOrderCode = null;
                BtnAceptar.IsEnabled = false;
                return;
            }

            if (dgOpciones.SelectedItem is null)
                dgOpciones.SelectedIndex = 0;
        }

        private static bool Contains(string? value, string searchText) =>
            !string.IsNullOrWhiteSpace(value) &&
            value.Contains(searchText, StringComparison.OrdinalIgnoreCase);

        private void ConfirmSelection()
        {
            if (dgOpciones.SelectedItem is not DeliveryOrderOption selected)
                return;

            SelectedDeliveryOrderCode = selected.DeliveryOrderCode;
            DialogResult = true;
            Close();
        }

        private void txtBuscar_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void dgOpciones_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            BtnAceptar.IsEnabled = dgOpciones.SelectedItem is DeliveryOrderOption;
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

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            ConfirmSelection();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void MainBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
                e.Handled = true;
            }
        }

        public sealed class DeliveryOrderOption
        {
            public string DeliveryOrderCode { get; set; } = string.Empty;
            public string KittingCode { get; set; } = string.Empty;
            public int KittingsCount { get; set; }
            public string Client { get; set; } = string.Empty;
            public string Project { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
        }
    }
}
