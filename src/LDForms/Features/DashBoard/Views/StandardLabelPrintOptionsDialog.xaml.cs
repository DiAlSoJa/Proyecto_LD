using System.Windows;
using System.Windows.Input;

namespace LDForms;

public partial class StandardLabelPrintOptionsDialog : Window
{
    public const string StandardIdOption = "Imprimir Etiquetas StandardId";

    public string? SelectedOption { get; private set; }

    public StandardLabelPrintOptionsDialog()
    {
        InitializeComponent();
    }

    private void BtnAceptar_Click(object sender, RoutedEventArgs e)
    {
        SelectedOption = rbStandardId.IsChecked == true ? StandardIdOption : null;
        DialogResult = SelectedOption != null;
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
