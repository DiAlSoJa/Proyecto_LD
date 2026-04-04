using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.FormsX.Helpers;
using LDForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LD.FormsX.Views.Login.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ILogger<LoginViewModel> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly AuthService _authService;
         
        [ObservableProperty]
        private string usuario;
        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool isLoading;
        public ICommand LoginCommand { get; }

        public LoginViewModel(AuthService authService, IServiceProvider serviceProvider, ILogger<LoginViewModel> logger)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;
            _logger = logger;

            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
            {
                _logger.LogWarning("Intento de login con campos vacíos");
                DialogHelper.ShowWarning("Ingrese usuario y contraseña");
                return;
            }

            try
            {
                IsLoading = true;
                _logger.LogInformation("Iniciando login para el usuario: {Usuario}", Usuario);

                var response = await _authService.LoginAsync(Usuario, Password);

                if (!response.IsSuccess)
                {
                    _logger.LogWarning("Login fallido para {Usuario}: {Mensaje}", Usuario, response.Message);
                    DialogHelper.ShowWarning(response.Message);
                    return;
                }

                _logger.LogInformation("Login exitoso para {Usuario}", Usuario);
                UserSession.AccessToken = response.Data?.Accesstoken;
                UserSession.RefreshToken = response.Data?.RefreshToken;

                var getMe = await _authService.GetMeAsync();

                if (!getMe.IsSuccess || getMe.Data == null)
                {
                    _logger.LogWarning("Error al obtener datos del usuario: {Mensaje}", getMe.Message);
                    DialogHelper.ShowWarning(getMe.Message);
                    return;
                }

                _logger.LogInformation("Datos del usuario obtenidos correctamente [getme]");
                UserData.SetUserData(getMe.Data);

                var dashboard = _serviceProvider.GetRequiredService<DashBoard>();
                dashboard.Show();

                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante el login del usuario: {Usuario}", Usuario);
                DialogHelper.ShowError("Error inesperado");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public Action? CloseAction { get; set; }
    }
}
