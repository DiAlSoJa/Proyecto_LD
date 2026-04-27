using LD.Contracts.AvailableInventory;
using LD.FormsX.Model.Lookup;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace LD.FormsX.Views.Inventario
{
    public partial class CambiarUbicacionInventarioDialog : Window
    {
        public string UbicacionDestino { get; private set; } = string.Empty;

        public CambiarUbicacionInventarioDialog(
            AvailableInventoryDto inventory,
            IEnumerable<LookupItem> locationItems)
        {
            InitializeComponent();

            txtStandardId.Text = $"StandardId: {inventory.StandardIdStr}";
            txtUbicacionActual.Text = $"Ubicacion actual: {inventory.Ubicacion}";
            cmbUbicacionDestino.ItemsSource = locationItems.OrderBy(x => x.Code).ToList();
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            var value = (cmbUbicacionDestino.SelectedValue as string) ?? cmbUbicacionDestino.Text;
            if (string.IsNullOrWhiteSpace(value))
            {
                MessageBox.Show("Selecciona la ubicacion destino.", "Ubicacion requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UbicacionDestino = value.Trim();
            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
