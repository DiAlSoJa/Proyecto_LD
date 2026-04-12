using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Enums;
using MauiAppLogin.Views.Controls;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private bool showAlmacenista;
        [ObservableProperty]
        private bool showSecurity;
        [ObservableProperty]
        private bool showConsultas;
        [ObservableProperty]
        private bool showDamageReport;
        [ObservableProperty]
        private bool showOperations;
        [ObservableProperty]
        private bool showInventory;
        [ObservableProperty]
        private bool showChecklist;

        [ObservableProperty]
        private bool canChangeLocation;
        [ObservableProperty]
        private bool canSupply;
        [ObservableProperty]
        private bool canViewAsn;
        [ObservableProperty]
        private bool canViewWarehouseTasks;

        [ObservableProperty]
        private bool canRegisterVehicle;
        [ObservableProperty]
        private bool canViewSecurityTasks;

        [ObservableProperty]
        private bool canAudit;
        [ObservableProperty]
        private bool canViewInventoryList;
        [ObservableProperty]
        private bool isBusy;

        public ICommand LogoutCommand { get; }
        public ICommand NavigateToChangeLocationCommand { get; }
        public ICommand NavigateToPickingCommand { get; }
        public ICommand NavigateToReceptionCommand { get; }
        public ICommand NavigateToTaskManagerCommand { get; }
        public ICommand NavigateToTaskManagerSecurityCommand { get; }
        public ICommand NavigateToTaskListCommand { get; }
        public ICommand NavigateToCasetaCommand { get; }
        public ICommand NavigateToMovementCommand { get; }
        public ICommand NavigateToDamageReportCommand { get; }
        public ICommand NavigateToOperationsCommand { get; }
        public ICommand NavigateToAuditCommand { get; }
        public ICommand NavigateToInventoryListCommand { get; }
        public ICommand NavigateToChecklistCommand { get; }

        public DashboardViewModel(ApiService apiService)
        {
            _apiService = apiService;

            LogoutCommand = new AsyncRelayCommand(Logout);
            NavigateToChangeLocationCommand = new AsyncRelayCommand(NavigateToChangeLocation);
            NavigateToPickingCommand = new AsyncRelayCommand(NavigateToPicking);
            NavigateToReceptionCommand = new AsyncRelayCommand(NavigateToReception);
            NavigateToTaskManagerCommand = new AsyncRelayCommand<string?>(NavigateToTaskManager);
            NavigateToTaskManagerSecurityCommand = new AsyncRelayCommand<string?>(NavigateToTaskManagerSecurity);
            NavigateToTaskListCommand = new AsyncRelayCommand(NavigateToTaskList);
            NavigateToCasetaCommand = new AsyncRelayCommand(NavigateToCaseta);
            NavigateToMovementCommand = new AsyncRelayCommand(NavigateToMovement);
            NavigateToDamageReportCommand = new AsyncRelayCommand(NavigateToDamageReport);
            NavigateToOperationsCommand = new AsyncRelayCommand(NavigateToOperations);
            NavigateToAuditCommand = new AsyncRelayCommand(NavigateToAudit);
            NavigateToInventoryListCommand = new AsyncRelayCommand(NavigateToInventoryList);
            NavigateToChecklistCommand = new AsyncRelayCommand(NavigateToChecklist);

            LoadPermissions();
        }

        private void LoadPermissions()
        {
            Username = UserData.UserName ?? string.Empty;

            ShowAlmacenista = UserData.HasModule((int)Module_e.WarehouseStaff);
            ShowSecurity = UserData.HasModule((int)Module_e.Security);
            ShowConsultas = UserData.HasModule((int)Module_e.Queries);
            ShowDamageReport = UserData.HasModule((int)Module_e.DamageReport);
            ShowOperations = UserData.HasModule((int)Module_e.Operations);
            ShowInventory = UserData.HasModule((int)Module_e.Inventory);
            ShowChecklist = UserData.HasModule((int)Module_e.ForkliftChecklist);

            CanChangeLocation = UserData.HasPermission(PermissionKeys.WarehouseStaff_LocationChange_Execute);
            CanSupply = UserData.HasPermission(PermissionKeys.WarehouseStaff_Supply_Execute);
            CanViewAsn = UserData.HasPermission(PermissionKeys.WarehouseStaff_Asn_View);
            CanViewWarehouseTasks = UserData.HasPermission(PermissionKeys.WarehouseStaff_Tasks_View);

            CanRegisterVehicle = UserData.HasPermission(PermissionKeys.Vehicle_Create);
            CanViewSecurityTasks = UserData.HasPermission(PermissionKeys.Security_Tasks_View);

            CanAudit = UserData.HasPermission(PermissionKeys.Inventory_Audit_View);
            CanViewInventoryList = UserData.HasPermission(PermissionKeys.Inventory_List_View);
        }

        private async Task Logout()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                bool confirmar = await Application.Current!.MainPage!.DisplayAlertAsync(
                    "Cerrar sesión",
                    "¿Seguro que quieres cerrar sesión?",
                    "Sí",
                    "No");
                if (!confirmar) return;
                _apiService.ClearToken();
                await Shell.Current.GoToAsync("//login");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateToChangeLocation()
        {
            var parameters = new Dictionary<string, object> { { "TextInformation", "" } };
            await Shell.Current.GoToAsync("ChangeLocationPage", parameters);
        }

        private async Task NavigateToPicking()
        {
            await Shell.Current.GoToAsync("PickingPage");
        }

        private async Task NavigateToReception()
        {
            await Shell.Current.GoToAsync("ReceptionPage");
        }

        private async Task NavigateToTaskManager(string? textInfo)
        {
            var parameters = new Dictionary<string, object>
            {
                { "TextInformation", textInfo ?? "" }
            };
            await Shell.Current.GoToAsync("ChangeLocationPage", parameters);
        }

        private async Task NavigateToTaskManagerSecurity(string? textInfo)
        {
            var parameters = new Dictionary<string, object>
            {
                { "TextInformation", textInfo ?? "" }
            };
            await Shell.Current.GoToAsync("TaskSecurity", parameters);
        }

        private async Task NavigateToTaskList()
        {
            await Shell.Current.GoToAsync("TaskList");
        }

        private async Task NavigateToCaseta()
        {
            var opciones = new[] { "Carga", "Descarga" };
            var popup = new OptionPopup("Caseta", opciones);
            Application.Current!.MainPage!.ShowPopup(popup);
            var seleccion = await popup.Result;
            if (string.IsNullOrWhiteSpace(seleccion)) return;
            switch (seleccion)
            {
                case "Carga":
                case "Descarga":
                    await Shell.Current.GoToAsync("RegisterLicense");
                    break;
            }
        }

        private async Task NavigateToMovement()
        {
            await Shell.Current.GoToAsync("MovementPage");
        }

        private async Task NavigateToDamageReport()
        {
            await Shell.Current.GoToAsync("DamageReportPage");
        }

        private async Task NavigateToOperations()
        {
            await Shell.Current.GoToAsync("WarehouseOperations");
        }

        private async Task NavigateToAudit()
        {
            var parameters = new Dictionary<string, object>
            {
                { "TextInformation", "Auditar Ubicación: DC01A" }
            };
            await Shell.Current.GoToAsync("ChangeLocationPage", parameters);
        }

        private async Task NavigateToInventoryList()
        {
            await Shell.Current.GoToAsync("InventoryList");
        }

        private async Task NavigateToChecklist()
        {
            await Shell.Current.GoToAsync("ForkliftChecklistPage");
        }
    }
}