using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Contracts.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MvvmHelpers.Commands;

namespace MauiAppLogin.ViewModels
{
    public partial class LoginViewModel : OriginViewModel
    {
        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;

        public ICommand LoginCommand { get; }

        private readonly AuthService _authService;
        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new AsyncCommand(Login);
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

                var result = await _authService.LoginAsync(Username, Password);

                if (!result.IsSuccess)
                {
                    await Shell.Current.DisplayAlertAsync("Error", result.Message, "OK");
                    return;
                }

                 //🔹 Guardar tokens si quieres
                 //UserSession.AccessToken = result.Data?.AccessToken;

                await Shell.Current.GoToAsync("//dashboard");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }


}