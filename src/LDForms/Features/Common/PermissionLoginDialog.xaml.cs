using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.DTOs.Auth;

namespace LD.FormsX.Features.Common
{
    public partial class PermissionLoginDialog : Window
    {
        private static readonly Brush InfoBackground = new SolidColorBrush(Color.FromRgb(248, 250, 252));
        private static readonly Brush InfoBorder = new SolidColorBrush(Color.FromRgb(229, 231, 235));
        private static readonly Brush InfoForeground = new SolidColorBrush(Color.FromRgb(71, 85, 105));
        private static readonly Brush ErrorBackground = new SolidColorBrush(Color.FromRgb(254, 242, 242));
        private static readonly Brush ErrorBorder = new SolidColorBrush(Color.FromRgb(239, 68, 68));
        private static readonly Brush ErrorForeground = new SolidColorBrush(Color.FromRgb(153, 27, 27));
        private static readonly Brush SuccessBackground = new SolidColorBrush(Color.FromRgb(240, 253, 244));
        private static readonly Brush SuccessBorder = new SolidColorBrush(Color.FromRgb(34, 197, 94));
        private static readonly Brush SuccessForeground = new SolidColorBrush(Color.FromRgb(22, 101, 52));

        private readonly AuthService _authService;
        private string _requiredPermissionKey = string.Empty;
        private bool _isAuthorizing;
        private bool _keepAuthorizedTokenForCaller;

        public PermissionLoginDialog(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        public string? AuthorizedAccessToken { get; private set; }

        public void Configure(string requiredPermissionKey, string title, string subtitle)
        {
            _requiredPermissionKey = requiredPermissionKey?.Trim() ?? string.Empty;
            txtTitle.Text = string.IsNullOrWhiteSpace(title) ? "Autorizar accion" : title;
            txtSubtitle.Text = string.IsNullOrWhiteSpace(subtitle)
                ? "Ingresa credenciales con permiso suficiente."
                : subtitle;
        }

        public void RestoreOriginalSessionToken()
        {
            if (string.IsNullOrWhiteSpace(UserSession.AccessToken))
                _authService._api.ClearToken();
            else
                _authService._api.SetBearerToken(UserSession.AccessToken);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsuario.Focus();
            SetInfoMessage("Listo para validar permisos.");
        }

        private async void BtnAutorizar_Click(object sender, RoutedEventArgs e)
        {
            await AuthorizeAsync();
        }

        private async Task AuthorizeAsync()
        {
            if (_isAuthorizing)
                return;

            var username = txtUsuario.Text.Trim();
            var password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                SetErrorMessage("Ingresa usuario y contrasena.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_requiredPermissionKey))
            {
                SetErrorMessage("No se configuro el permiso requerido.");
                return;
            }

            try
            {
                _isAuthorizing = true;
                SetBusyState(true);
                SetInfoMessage("Validando credenciales...");

                var loginResponse = await _authService.LoginAsync(username, password);
                if (!loginResponse.IsSuccess || string.IsNullOrWhiteSpace(loginResponse.Data?.Accesstoken))
                {
                    RestoreOriginalSessionToken();
                    SetErrorMessage(loginResponse.ErrorMessage ?? loginResponse.Message ?? "Credenciales invalidas.");
                    return;
                }

                var getMeResponse = await _authService.GetMeAsync();
                if (!getMeResponse.IsSuccess || getMeResponse.Data == null)
                {
                    RestoreOriginalSessionToken();
                    SetErrorMessage(getMeResponse.ErrorMessage ?? getMeResponse.Message ?? "No se pudieron consultar los permisos.");
                    return;
                }

                if (!HasPermission(getMeResponse.Data.Authorization, _requiredPermissionKey))
                {
                    RestoreOriginalSessionToken();
                    SetErrorMessage("El usuario no tiene permiso para esta accion.");
                    return;
                }

                AuthorizedAccessToken = loginResponse.Data.Accesstoken;
                _keepAuthorizedTokenForCaller = true;
                SetSuccessMessage("Permiso autorizado.");
                DialogResult = true;
            }
            catch (Exception ex)
            {
                RestoreOriginalSessionToken();
                SetErrorMessage(ex.Message);
            }
            finally
            {
                _isAuthorizing = false;
                SetBusyState(false);
            }
        }

        private static bool HasPermission(AuthorizationDto? authorization, string key)
        {
            return authorization?.Modules?
                .SelectMany(module => module.Permissions
                    .Concat(module.SubModules?.SelectMany(subModule => subModule.Permissions) ?? []))
                .Any(permission => string.Equals(permission.Key, key, StringComparison.OrdinalIgnoreCase)) ?? false;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            RestoreOriginalSessionToken();
            DialogResult = false;
        }

        private async void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;
            await AuthorizeAsync();
        }

        private void TxtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;
            txtPassword.Focus();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_keepAuthorizedTokenForCaller)
                RestoreOriginalSessionToken();
        }

        private void SetBusyState(bool isBusy)
        {
            btnAutorizar.IsEnabled = !isBusy;
            txtUsuario.IsEnabled = !isBusy;
            txtPassword.IsEnabled = !isBusy;
        }

        private void SetInfoMessage(string message)
        {
            SetMessage(message, InfoBackground, InfoBorder, InfoForeground);
        }

        private void SetErrorMessage(string message)
        {
            SetMessage(message, ErrorBackground, ErrorBorder, ErrorForeground);
        }

        private void SetSuccessMessage(string message)
        {
            SetMessage(message, SuccessBackground, SuccessBorder, SuccessForeground);
        }

        private void SetMessage(string message, Brush background, Brush border, Brush foreground)
        {
            messagePanel.Background = background;
            messagePanel.BorderBrush = border;
            txtMessage.Foreground = foreground;
            txtMessage.Text = message;
        }
    }
}
