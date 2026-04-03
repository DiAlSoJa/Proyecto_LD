using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.FormsX.Helpers;
using LDForms;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX
{
    public partial class MainWindow : Window
    {
        private readonly AuthService _authService;
        private readonly IServiceProvider _serviceProvider;

        private bool bloqueo;
        private bool validacionForzoza;
        private bool respuesta;

        public event EventHandler? LoginSucceeded;

        public MainWindow(AuthService authService, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _authService = authService;
            _serviceProvider = serviceProvider;
        }

        public string Usuario => txtUsuario.Text.Trim();
        public string Password => txtPassword.Password;

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            await EjecutarLoginAsync();
        }

        private async void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                await EjecutarLoginAsync();
            }
        }

        private async Task EjecutarLoginAsync()
        {
            try
            {
                btnLogin.IsEnabled = false;
                LoadingOverlay.Visibility = Visibility.Visible;
                lblMensaje.Text = string.Empty;

                await ValidaAsync();
            }
            finally
            {
                btnLogin.IsEnabled = true;
                LoadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private async Task ValidaAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
                {
                    DialogHelper.ShowWarning("Ingrese usuario y contraseña");
                    return;
                }

                var response = await _authService.LoginAsync(Usuario, Password);
                if (!response.IsSuccess)
                {
                    DialogHelper.ShowWarning(response.Message);
                    return;
                }

                UserSession.AccessToken = response.Data?.Accesstoken;
                UserSession.RefreshToken = response.Data?.RefreshToken;

                var getMeResponse = await _authService.GetMeAsync();
                if (!getMeResponse.IsSuccess || getMeResponse.Data is null)
                {
                    DialogHelper.ShowWarning(getMeResponse.Message);
                    return;
                }

                UserData.SetUserData(getMeResponse.Data);

                LoginSucceeded?.Invoke(this, EventArgs.Empty);

                var dashboard = _serviceProvider.GetRequiredService<DashBoard>();
                dashboard.Show();
                Close();
            }
            catch (Exception)
            {
                DialogHelper.ShowError("Hubo un error inesperado");
                SetRespuesta(false);
            }
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
            if (bloqueo)
            {
                SetRespuesta(false);

                if (!validacionForzoza)
                {
                    Close();
                }
                else
                {
                    var result = MessageBox.Show(
                        "¿Está seguro de cerrar el sistema?",
                        "Confirmación",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        SetRespuesta(true);
                        Application.Current.Shutdown();
                    }
                }
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void SetRespuesta(bool value)
        {
            respuesta = value;
        }

        public bool GetRespuesta()
        {
            return respuesta;
        }
    }
}