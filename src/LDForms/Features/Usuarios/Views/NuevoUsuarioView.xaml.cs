using LD.Contracts.DTOs.User;
using LD.FormsX.Features.Usuarios.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Usuarios
{
    public partial class NuevoUsuarioView : Window
    {
        private NuevoUsuarioViewModel ViewModel => (NuevoUsuarioViewModel)DataContext;

        public NuevoUsuarioView(NuevoUsuarioViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };
        }

        public void SetUser(GetUserDto user) => ViewModel.SetUser(user);

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAsync();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveAsync(txtPassword.Password, txtConfirmPassword.Password);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
