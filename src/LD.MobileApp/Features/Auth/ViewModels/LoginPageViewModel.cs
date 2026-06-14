using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Contracts.User;
using MauiAppLogin;
using MauiAppLogin.Controls;
using MauiAppLogin.Services;
using MauiAppLogin.Views.Controls;
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
        private readonly IDialogService _dialogService;
        private readonly ChecklistService _checklistService;
        private readonly MobileSessionService _sessionService;

        private const string DefaultApiUrl = "https://ld-api-prod-gzcccygmfnb7gkdz.mexicocentral-01.azurewebsites.net/api";
        private const string ConfigurationPassword = "Pa$$w0rd";

        public ILoaderService Loader => _loaderService;

        public LoginViewModel(
            AuthService authService,
            ILoaderService loaderService,
            IDialogService dialogService,
            ChecklistService checklistService,
            MobileSessionService sessionService)
        {
            _authService = authService;
            _loaderService = loaderService;
            _dialogService = dialogService;
            _checklistService = checklistService;
            _sessionService = sessionService;
            LoginCommand = new AsyncCommand(Login);
            ShowIpConfigCommand = new AsyncCommand(ShowIpConfigAsync);
            Username = "admin";
            Password = "Pa$$w0rd";
        }

        private async Task ShowIpConfigAsync()
        {
            var enteredPassword = await PromptForConfigurationPasswordAsync();
            if (enteredPassword is null) return;

            if (!string.Equals(enteredPassword.Trim(), ConfigurationPassword, StringComparison.Ordinal))
            {
                await _dialogService.ShowErrorAsync("Error", "Contraseña incorrecta.");
                return;
            }

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
            await _dialogService.ShowSuccessAsync("Guardado", "La nueva URL se aplicará al próximo inicio de la app.");
        }

        private static async Task<string?> PromptForConfigurationPasswordAsync()
        {
            var page = Shell.Current.CurrentPage ?? Application.Current?.MainPage;
            if (page is null)
                return null;

            var popup = new PasswordPromptPopup(
                "Acceso restringido",
                "Ingresa la contraseña para cambiar la URL:");

            var result = await page.ShowPopupAsync(popup);
            return result as string;
        }

        // Llamado desde LoginPage.OnAppearing para restaurar sesión en cold start.
        // Si el token es válido (o se renueva silenciosamente), navega directamente al inicio.
        public async Task TryAutoLoginAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                var restored = await _sessionService.TryRestoreFullSessionAsync();
                if (!restored) return;
                await NavegaAlInicioAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TryAutoLogin] {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Login()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    await _dialogService.ShowErrorAsync("Error", "Captura el usuario y contraseña");
                    return;
                }

                _loaderService.Show("Iniciando sesión...");
                var response = await _authService.LoginAsync(Username, Password);

                if (!response.IsSuccess)
                {
                    await _dialogService.ShowErrorAsync("Error", response.Message);
                    return;
                }

                UserSession.AccessToken = response.Data?.Accesstoken;
                UserSession.RefreshToken = response.Data?.RefreshToken;

                // Persistir tokens en SecureStorage para restaurar la sesión al reabrir la app
                var expiry = DateTime.UtcNow.AddHours(8);
                await _sessionService.PersistAsync(
                    response.Data!.Accesstoken!,
                    response.Data.RefreshToken!,
                    expiry);

                var getMeResponse = await _authService.GetMeAsync();
                if (!getMeResponse.IsSuccess || getMeResponse.Data is null)
                {
                    await _dialogService.ShowErrorAsync("Error", getMeResponse.Message);
                    return;
                }

                UserData.SetUserData(getMeResponse.Data);

                await NavegaAlInicioAsync();
            }
            catch (Exception ex)
            {
                _loaderService.Hide();
                await _dialogService.ShowErrorAsync("Error",$"No se pudo iniciar sesion {Environment.NewLine} Revise su conexion o contacte con supervisor");
            }
            finally
            {
                _loaderService.Hide();
                IsBusy = false;
            }
        }

        // Después del login: si tiene equipo y no hizo checklist hoy -> checklist obligatorio.
        // Cualquier otro caso (sin equipo, ya hizo checklist, error de red) -> dashboard.
        private async Task NavegaAlInicioAsync()
        {
            try
            {
                var response = await _checklistService.GetDailyStatusAsync();

                if (response.IsSuccess
                    && response.Data is { HasAssignedEquipment: true, HasCompletedToday: false })
                {
                    await Shell.Current.GoToAsync(nameof(ForkliftChecklistPage),
                        new Dictionary<string, object>
                        {
                            ["Equipment"] = response.Data.Equipment!,
                            ["IsMandatory"] = true
                        });
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NavegaAlInicio] {ex}");
            }

            await Shell.Current.GoToAsync("//dashboard");
        }

        /* Referencia del flujo anterior - Sprint 3 Hotfix (2026-05-14)
         * La verificación de equipo ya no ocurre en el login.
         * Se mantiene comentado como referencia del flujo anterior.
         *
         * private async Task NavegaSegunEquipoAsync()
         * {
         *     try
         *     {
         *         var response = await _checklistService.GetDailyStatusAsync();
         *         if (!response.IsSuccess || response.Data is null)
         *         {
         *             await Shell.Current.GoToAsync(nameof(NoEquipmentPage));
         *             return;
         *         }
         *         var status = response.Data;
         *         if (!status.HasAssignedEquipment)
         *         {
         *             await Shell.Current.GoToAsync(nameof(NoEquipmentPage));
         *             return;
         *         }
         *         if (!status.HasCompletedToday)
         *         {
         *             await Shell.Current.GoToAsync(nameof(ForkliftChecklistPage),
         *                 new Dictionary<string, object>
         *                 {
         *                     { "Equipment",   status.Equipment! },
         *                     { "IsMandatory", true }
         *                 });
         *             return;
         *         }
         *         if (status.LastChecklistAt.HasValue)
         *             Preferences.Default.Set("ChecklistCompletedAt",
         *                 status.LastChecklistAt.Value.ToString("o"));
         *         await Shell.Current.GoToAsync("//dashboard");
         *     }
         *     catch (Exception ex)
         *     {
         *         System.Diagnostics.Debug.WriteLine($"[NavegaSegunEquipo] {ex}");
         *         await Shell.Current.GoToAsync(nameof(NoEquipmentPage));
         *     }
         * }
         */
    }
}
