using LD.FormsX.Helpers;
using System.Windows;
using System.Windows.Input;

namespace LDForms;

public partial class StandardLabelQuantityDialog : Window
{
    public int Quantity { get; private set; }

    public StandardLabelQuantityDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => txtCantidad.Focus();
    }

    private void BtnAceptar_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(txtCantidad.Text.Trim(), out var quantity) || quantity <= 0)
        {
            DialogHelper.ShowWarning("Ingresa una cantidad valida mayor a cero.");
            txtCantidad.Focus();
            txtCantidad.SelectAll();
            return;
        }

        Quantity = quantity;
        DialogResult = true;
    }

    private void BtnCancelar_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }
}
