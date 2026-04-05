using LD.Contracts.DTOs.User;
using LD.Contracts.Warehouse;
using LD.FormsX.Features.Usuarios.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.Usuarios
{
    public partial class UsuarioAlmacenView : Window
    {
        private UsuarioAlmacenViewModel ViewModel => (UsuarioAlmacenViewModel)DataContext;

        public UsuarioAlmacenView(UsuarioAlmacenViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = viewModel.ResponseForm;
                Close();
            };
        }

        public void SetUser(GetUserDto user) => ViewModel.SetUser(user);

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAsync();
        }

        private void DgAsignados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedAsignado = dgAsignados.SelectedItem as WarehouseDto;
        }

        private void DgDisponibles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedDisponible = dgDisponibles.SelectedItem as WarehouseDto;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
