using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels
{
    public partial class DashboardViewModel : OriginViewModel
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string username = string.Empty;

        public ICommand LogoutCommand { get; }
        public ICommand OpenModuleCommand { get; }

        public DashboardViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LogoutCommand = new AsyncRelayCommand(() => LogoutHandle());
            OpenModuleCommand = new AsyncRelayCommand<string>(m => OpenModule(m));
        }

        private async Task OpenModule(string? modulo)
        {
            if (string.IsNullOrEmpty(modulo)) return;

            var opciones = modulo switch
            {
                "Caseta"           => new[] { "Carga", "Descarga" },
                "Cortinas"         => new[] { "Abrir Descarga", "Cerrar Descarga", "", "Abrir Carga", "Cerrar Carga" },
                "Validar Acople"   => new[] { "Cortina", "Patio" },
                "Validar Desacople"=> new[] { "Cortina", "Patio" },
                _                  => new[] { "Nuevo", "Consultar", "Historial" }
            };

            var popup = new OptionPopup(modulo, opciones);
            Application.Current!.MainPage!.ShowPopup(popup);

            var seleccion = await popup.Result;
            if (string.IsNullOrWhiteSpace(seleccion))
                return;

            switch (seleccion)
            {
                case "Carga":
                case "Descarga":
                    await Shell.Current.GoToAsync("RegisterLicense");
                    break;
            }
        }

        public async Task LogoutHandle()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                bool confirmar = await Application.Current!.MainPage!.DisplayAlert(
                    "Cerrar sesión",
                    "¿Seguro que quieres cerrar sesión?",
                    "Sí",
                    "No");

                if (!confirmar)
                    return;

                _apiService.ClearToken();
                await Shell.Current.GoToAsync("//login");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}