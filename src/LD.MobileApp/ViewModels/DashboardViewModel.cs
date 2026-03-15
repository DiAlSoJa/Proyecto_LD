using CommunityToolkit.Mvvm.Input;
using LD.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using LD.Client.Services;

namespace MauiAppLogin.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        public DashboardViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Logout = new AsyncRelayCommand(LogoutHandle);
        }
        public ICommand Logout;


        public async Task LogoutHandle()
        {
            bool confirmar = await Shell.Current.DisplayAlertAsync(
                "Cerrar sesión",
                "¿Seguro que quieres cerrar sesión?",
                "Sí",
                "No");

            if (!confirmar)
                return;

            _apiService.ClearToken();
            await Shell.Current.GoToAsync("login");
        }
    }
}
        