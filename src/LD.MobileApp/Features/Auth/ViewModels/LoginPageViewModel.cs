using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Contracts.User;
using MauiAppLogin;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace MauiAppLogin.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private bool isPasswordVisible = false;

        [ObservableProperty]
        private bool isBusy = false;
        public ICommand TogglePasswordVisibilityCommand => new Command(() => IsPasswordVisible = !IsPasswordVisible);

        public ICommand LoginCommand { get; }
        public ICommand ShowIpConfigCommand { get; }

        private readonly AuthService _authService;
        private readonly ILoaderService _loaderService;
        private readonly EquipmentService _equipmentService;

        private const string DefaultApiUrl = "http://192.168.0.103:8050/api";

        public ILoaderService Loader => _loaderService;

        public LoginViewModel(AuthService authService, ILoaderService loaderService, EquipmentService equipmentService)
        {
            _authService = authService;
            _loaderService = loaderService;
            _equipmentService = equipmentService;
            LoginCommand = new AsyncCommand(Login);
            ShowIpConfigCommand = new AsyncCommand(ShowIpConfigAsync);
            Username = "admin";
            Password = "Pa$$w0rd";
        }

        private async Task ShowIpConfigAsync()
        {
            var currentUrl = Preferences.Default.Get("ApiBaseUrl", DefaultApiUrl);

            var newUrl = await Shell.Current.DisplayPromptAsync(
                "Servidor",
                "URL actual:",
                accept: "Guardar",
                cancel: "Cancelar",
                initialValue: currentUrl,
                maxLength: 120,
                keyboard: Keyboard.Url);

            if (newUrl is null) return;

            newUrl = newUrl.Trim();
            if (string.IsNullOrWhiteSpace(newUrl) || newUrl == currentUrl) return;

            Preferences.Default.Set("ApiBaseUrl", newUrl);
            await Shell.Current.DisplayAlertAsync("Guardado", "La nueva URL se aplicará al próximo inicio de la app.", "OK");
        }

        private async Task Login()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Captura el usuario y contraseña", "OK");
                    return;
                }

                _loaderService.Show("Iniciando sesión...");
                var response = await _authService.LoginAsync(Username, Password);

                if (!response.IsSuccess)
                {
                    await Shell.Current.DisplayAlertAsync("Error", response.Message, "OK");
                    return;
                }
                UserSession.AccessToken = response.Data?.Accesstoken;
                UserSession.RefreshToken = response.Data?.RefreshToken;

                var getMeResponse = await _authService.GetMeAsync();
                if (!getMeResponse.IsSuccess || getMeResponse.Data is null)
                {
                    await Shell.Current.DisplayAlertAsync("Error", getMeResponse.Message, "OK");
                    return;
                }

                UserData.SetUserData(getMeResponse.Data);

                await NavegaSegunEquipoAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
            finally
            {
                _loaderService.Hide();
                IsBusy = false;
            }
        }

        // Consulta el endpoint dedicated para obtener el equipo asignado al usuario actual.
        // Si lo tiene, navega directo al checklist; si no, va al dashboard.
        private async Task NavegaSegunEquipoAsync()
        {
            try
            {
                var response = await _equipmentService.GetAssignedToMeAsync();
                if (response.IsSuccess && response.Data is not null)
                {
                    await Shell.Current.GoToAsync(nameof(ForkliftChecklistPage),
                        new Dictionary<string, object> { { "Equipment", response.Data } });
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[NavegaSegunEquipo] Error al consultar equipo asignado: {ex}");
            }

            await Shell.Current.GoToAsync("//dashboard");
        }
    }


}
