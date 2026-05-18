using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Enums;
using MauiAppLogin.Controls;
using MauiAppLogin.Views.Controls;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private readonly PatioClientService _patioClientService;
        private readonly ChecklistService _checklistService;
        private readonly OperationalTaskService _operationalTaskService;
        private readonly LookupService _lookupService;
        private readonly IDialogService _dialogService;

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
        private bool canViewPatioPendientes;
        [ObservableProperty]
        private bool canViewSecurityTasks;

        [ObservableProperty]
        private bool canAudit;
        [ObservableProperty]
        private bool canViewInventoryList;
        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private int tareasPendientesCount;

        [ObservableProperty]
        private bool hasTareasPendientes;

        private int operationalTasksPendingCount;
        public int OperationalTasksPendingCount
        {
            get => operationalTasksPendingCount;
            set
            {
                if (SetProperty(ref operationalTasksPendingCount, value))
                {
                    HasOperationalTasksPending = value > 0;
                    OnPropertyChanged(nameof(OperationalTasksHeaderText));
                }
            }
        }

        private bool hasOperationalTasksPending;
        public bool HasOperationalTasksPending
        {
            get => hasOperationalTasksPending;
            set => SetProperty(ref hasOperationalTasksPending, value);
        }

        public string OperationalTasksHeaderText => $"Tareas ({OperationalTasksPendingCount})";

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
        public ICommand NavigateToPatioPendientesCommand { get; }

        public DashboardViewModel(ApiService apiService, PatioClientService patioClientService, ChecklistService checklistService, OperationalTaskService operationalTaskService, LookupService lookupService, IDialogService dialogService)
        {
            _apiService = apiService;
            _patioClientService = patioClientService;
            _checklistService = checklistService;
            _operationalTaskService = operationalTaskService;
            _lookupService = lookupService;
            _dialogService = dialogService;

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
            NavigateToPatioPendientesCommand = new AsyncRelayCommand(NavigateToPatioPendientes);

            LoadPermissions();
        }

        private void LoadPermissions()
        {
            Username = UserData.UserName ?? string.Empty;

            ShowAlmacenista = UserData.HasModule((int)Module_e.WarehouseStaff);
            ShowSecurity = UserData.HasModule((int)Module_e.Security);
            ShowConsultas = UserData.HasModule((int)Module_e.Queries);
            ShowDamageReport = UserData.HasModule((int)Module_e.DamageReport);
            ShowOperations = UserData.HasModule((int)Module_e.Operations)
                || UserData.HasModule((int)Module_e.YardControl);
            ShowInventory = UserData.HasModule((int)Module_e.Inventory);
            ShowChecklist = UserData.HasModule((int)Module_e.ForkliftChecklist);

            CanChangeLocation = UserData.HasPermission(PermissionKeys.WarehouseStaff_LocationChange_Execute);
            CanSupply = UserData.HasPermission(PermissionKeys.WarehouseStaff_Supply_Execute);
            CanViewAsn = UserData.HasPermission(PermissionKeys.WarehouseStaff_Asn_View);
            CanViewWarehouseTasks = UserData.HasPermission(PermissionKeys.WarehouseStaff_Tasks_View);

            CanRegisterVehicle = UserData.HasPermission(PermissionKeys.Vehicle_Create);
            CanViewPatioPendientes = UserData.HasPermission(PermissionKeys.Vehicle_View)
                || UserData.HasPermission(PermissionKeys.YardControl_View);
            CanViewSecurityTasks = UserData.HasPermission(PermissionKeys.Security_Tasks_View);

            CanAudit = UserData.HasPermission(PermissionKeys.Inventory_Audit_View);
            CanViewInventoryList = UserData.HasPermission(PermissionKeys.Inventory_List_View);
        }

        public async Task CargarTareasPendientesAsync()
        {
            try
            {
                var warehouseResponse = string.IsNullOrWhiteSpace(UserData.Id)
                    ? null
                    : await _lookupService.GetWarehouseLookupByUser(UserData.Id);
                var warehouseIds = (warehouseResponse?.Data ?? new())
                    .Select(x => int.TryParse(x.Key, out var warehouseId) ? warehouseId : (int?)null)
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .ToHashSet();

                if (warehouseResponse?.IsSuccess == true && warehouseIds.Count == 0)
                {
                    OperationalTasksPendingCount = 0;
                    return;
                }

                var selectedWarehouseId = warehouseIds.Count == 1 ? warehouseIds.First() : (int?)null;
                var operationalTasksResponse = await _operationalTaskService.GetTasks(soloPendientes: true, selectedWarehouseId);
                if (operationalTasksResponse.IsSuccess && operationalTasksResponse.Data != null)
                {
                    OperationalTasksPendingCount = selectedWarehouseId.HasValue || warehouseIds.Count == 0
                        ? operationalTasksResponse.Data.Count
                        : operationalTasksResponse.Data.Count(x => x.WarehouseId.HasValue && warehouseIds.Contains(x.WarehouseId.Value));
                }
            }
            catch { }

            if (!CanViewSecurityTasks) return;
            try
            {
                var response = await _patioClientService.GetTasksAsync(soloPendientes: true);
                if (response.IsSuccess && response.Data != null)
                {
                    TareasPendientesCount = response.Data.Count;
                    HasTareasPendientes = TareasPendientesCount > 0;
                }
            }
            catch { }
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
            await Shell.Current.GoToAsync(nameof(TaskSecurityPage));
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

            await Shell.Current.GoToAsync($"RegisterLicense?tipo={Uri.EscapeDataString(seleccion)}");
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
            try
            {
                var response = await _checklistService.GetDailyStatusAsync();

                if (!response.IsSuccess || response.Data is null)
                {
                    await _dialogService.ShowErrorAsync(
                        "Error",
                        "No se pudo verificar el estado del checklist. Intenta de nuevo.");
                    return;
                }

                var status = response.Data;

                // Sin equipo asignado → aviso y queda en dashboard
                if (!status.HasAssignedEquipment)
                {
                    await _dialogService.ShowInfoAsync(
                        "Sin equipo asignado",
                        "No tienes un equipo asignado. Contacta a tu supervisor.");
                    return;
                }

                // Ya completó checklist hoy → pregunta si registra otro
                if (status.HasCompletedToday)
                {
                    var hora = status.LastChecklistAt?.ToLocalTime().ToString("HH:mm") ?? "hoy";
                    var registrarOtro = await _dialogService.ShowWarningAsync(
                        "Ya registraste un checklist hoy",
                        $"El último fue registrado a las {hora}. ¿Deseas registrar uno adicional?");

                    if (!registrarOtro) return;

                    await Shell.Current.GoToAsync(nameof(ForkliftChecklistPage),
                        new Dictionary<string, object>
                        {
                            ["Equipment"]   = status.Equipment!,
                            ["IsMandatory"] = false
                        });
                    return;
                }

                // No ha hecho checklist hoy → modo obligatorio
                await Shell.Current.GoToAsync(nameof(ForkliftChecklistPage),
                    new Dictionary<string, object>
                    {
                        ["Equipment"]   = status.Equipment!,
                        ["IsMandatory"] = true
                    });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NavigateToChecklist] Error: {ex}");
                await _dialogService.ShowErrorAsync(
                    "Sin conexión",
                    "No se pudo verificar el estado del checklist. Verifica tu conexión e intenta de nuevo.");
            }
        }

        private async Task NavigateToPatioPendientes()
        {
            await Shell.Current.GoToAsync(nameof(PatioPendientesPage));
        }
    }
}
