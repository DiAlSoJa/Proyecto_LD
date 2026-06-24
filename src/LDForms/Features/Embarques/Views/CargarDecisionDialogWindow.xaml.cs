using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Features.Embarques.Views
{
    public enum CargarDecision
    {
        Cancelar = 0,
        NuevaCarga = 1,
        CargaExistente = 2
    }

    public partial class CargarDecisionDialogWindow : Window
    {
        public CargarDecision Decision { get; private set; } = CargarDecision.Cancelar;

        public CargarDecisionDialogWindow()
        {
            InitializeComponent();
        }

        private void BtnNuevaCarga_Click(object sender, RoutedEventArgs e)
        {
            Decision = CargarDecision.NuevaCarga;
            DialogResult = true;
            Close();
        }

        private void BtnCargaExistente_Click(object sender, RoutedEventArgs e)
        {
            Decision = CargarDecision.CargaExistente;
            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Decision = CargarDecision.Cancelar;
            DialogResult = false;
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Decision = CargarDecision.Cancelar;
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
                Decision = CargarDecision.Cancelar;
                DialogResult = false;
                Close();
            }
        }
    }
}
