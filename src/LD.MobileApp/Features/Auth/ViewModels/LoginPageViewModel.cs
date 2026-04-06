using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Contracts.User;
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

        private readonly AuthService _authService;
        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new AsyncCommand(Login);
            Username = "admin";
            Password = "Pa$$w0rd";
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