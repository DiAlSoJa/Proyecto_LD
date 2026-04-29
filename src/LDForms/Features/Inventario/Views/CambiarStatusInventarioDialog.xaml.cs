using LD.Contracts.AvailableInventory;
using LD.FormsX.Model.Lookup;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Inventario
{
    public partial class CambiarStatusInventarioDialog : Window
    {
        public string StatusDestino { get; private set; } = string.Empty;

        public CambiarStatusInventarioDialog(
            AvailableInventoryDto inventory,
            IEnumerable<LookupItem> statusItems)
        {
            InitializeComponent();

            txtStandardId.Text = $"StandardId: {inventory.StandardIdStr}";
            txtStatusActual.Text = $"Status actual: {inventory.StatusId}";
            cmbStatusDestino.ItemsSource = statusItems.OrderBy(x => x.Code).ToList();
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            var value = (cmbStatusDestino.SelectedValue as string) ?? cmbStatusDestino.Text;
            if (string.IsNullOrWhiteSpace(value))
            {
                MessageBox.Show("Selecciona el status destino.", "Status requerido", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            StatusDestino = value.Trim();
            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
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
