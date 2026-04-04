using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LD.FormsX.Views.Login.ViewModels;

namespace LD.FormsX
{
    public partial class MainWindow : Window
    {
        public MainWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.CloseAction = Close;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión: {version}";
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
    }
}