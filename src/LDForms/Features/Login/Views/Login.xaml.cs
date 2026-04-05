using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LD.FormsX.Views.Login.ViewModels;

namespace LD.FormsX
{
    public partial class MainWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        public MainWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
            viewModel.CloseAction = Close;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.IsPasswordVisible) && !_viewModel.IsPasswordVisible)
                txtPassword.Password = _viewModel.Password ?? string.Empty;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = _viewModel.Password ?? string.Empty;
            txtUsuario.Focus();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtPassword.Focus();
        }
    }
}