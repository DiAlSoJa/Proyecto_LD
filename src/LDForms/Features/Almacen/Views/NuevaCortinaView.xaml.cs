using LD.Contracts.DTOs.Security;
using LD.Contracts.Warehouse;
using LD.FormsX.Features.Almacen.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Almacen;

public partial class NuevaCortinaView : Window
{
    private NuevaCortinaViewModel ViewModel => (NuevaCortinaViewModel)DataContext;

    public NuevaCortinaView(NuevaCortinaViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.RequestClose = () => { DialogResult = true; Close(); };
    }

    public void SetWarehouse(WarehouseDto? warehouse) => ViewModel.SetWarehouse(warehouse);
    public void SetCortina(CortinaDto? cortina) => ViewModel.SetCortina(cortina);

    private async void Window_Loaded(object sender, RoutedEventArgs e) => await ViewModel.LoadAsync();
    private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();
    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); }
}
