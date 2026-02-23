using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiAppLogin.ViewModels
{
    public partial class LoginViewModel : BindableObject
    {
        private string email;
        private string password;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login());
        }

        private async Task Login()
        {
            // Validación básica
            if (string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ingrese correo y contraseña",
                    "OK");
                return;
            }

            // Navegación limpia (elimina login del stack)
            await Shell.Current.GoToAsync("//dashboard");
        }
    }
}