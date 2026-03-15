using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Contracts.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
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
            LoginCommand = new Command(async () => await Login());
        }

        private async Task Login()
        {
            try
            {
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

                await Shell.Current.GoToAsync("//DashboardPage");

                ////UserSession.AccessToken = response.Data?.Accesstoken;
                ////UserSession.RefreshToken = response.Data?.RefreshToken;

                //var getMeResponse = await _authService.GetMeAsync();
                //if (!getMeResponse.IsSuccess || getMeResponse.Data is null)
                //{
                //    //_dialogMessageService.Show(getMeResponse.Message, DialogMessageEnum.Warning);
                //    return;
                //}
                //UserData.SetUserData(getMeResponse.Data);
                //LoginSucceeded?.Invoke(this, EventArgs.Empty);

                // Navegación limpia (elimina login del stack)
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