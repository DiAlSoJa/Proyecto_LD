using System.Windows;
using System.Windows.Input;
using LD.Contracts.DTOs.KittingFolioCapture;

namespace LD.FormsX.Features.Surtidos.Views;

public partial class CapturaFolioDetalleView : Window
{
    public CapturaFolioDetalleView()
    {
        InitializeComponent();
    }

    public void SetCapture(KittingFolioCaptureDto capture)
    {
        DataContext = capture;
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
