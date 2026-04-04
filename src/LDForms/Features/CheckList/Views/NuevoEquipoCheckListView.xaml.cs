using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.CheckList
{
    public partial class NuevoEquipoCheckListView : Window
    {
        public NuevoEquipoCheckListView()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnNuevoProveedor_Click(object sender, RoutedEventArgs e)
        {
            // Abrir diálogo de proveedor
        }
    }
}