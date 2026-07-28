using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Enums;
using MauiAppLogin.Controls;
using MauiAppLogin.Services;
using MauiAppLogin.Views.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private readonly PatioClientService _patioClientService;
        private readonly ChecklistService _checklistService;
        private readonly WarehouseTaskService _warehouseTaskService;
        private readonly LookupService _lookupService;
        private readonly IDialogService _dialogService;
        private readonly MobileSessionService _sessionService;

        private readonly SignalRService _signalRService;

        private readonly IServiceProvider _serviceProvider;


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
        public ICommand NavigateToValidarEmbarqueCommand { get; }
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

        public DashboardViewModel(
            ApiService apiService,
            PatioClientService patioClientService,
            ChecklistService checklistService,
            WarehouseTaskService warehouseTaskService,
            LookupService lookupService,
            IDialogService dialogService,
            MobileSessionService sessionService,

            SignalRService signalRService,

            IServiceProvider serviceProvider)

        {
            _apiService = apiService;
            _patioClientService = patioClientService;
            _checklistService = checklistService;
            _warehouseTaskService = warehouseTaskService;
            _lookupService = lookupService;
            _dialogService = dialogService;
            _sessionService = sessionService;

            _signalRService = signalRService;

            _serviceProvider = serviceProvider;


            LogoutCommand = new AsyncRelayCommand(Logout);
            NavigateToChangeLocationCommand = new AsyncRelayCommand(NavigateToChangeLocation);
            NavigateToPickingCommand = new AsyncRelayCommand(NavigateToPicking);
            NavigateToValidarEmbarqueCommand = new AsyncRelayCommand(NavigateToValidarEmbarque);
            NavigateToReceptionCommand = new AsyncRelayCommand(NavigateToReception);
            NavigateToTaskManagerCommand = new AsyncRelayCommand(NavigateToTaskManager);
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


        public void OnNavigatedTo()
        {
            _signalRService.NotificationReceived += HandleSignalRNotification;
        }

        public void OnNavigatedFrom()
        {
            _signalRService.NotificationReceived -= HandleSignalRNotification;
        }

        private void HandleSignalRNotification(LD.Contracts.SignalR.HubNotification notification)
        {
            if (notification.Type != "task_assigned") return;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                OperationalTasksPendingCount++;
                await _dialogService.ShowSuccessAsync("Nueva tarea asignada", notification.Title);
            });
        }

        private static Shell? GetActiveShell()
            => Shell.Current ?? Application.Current?.MainPage as Shell;

        private static Page? GetActivePage()
            => Shell.Current?.CurrentPage ?? Application.Current?.MainPage;

        private async Task NavigateAsync(string route)
        {
            if (route == nameof(InventoryList))
            {
                await OpenInventoryListAsync();
                return;
            }

            var shell = GetActiveShell();
            if (shell is null)
            {
                await _dialogService.ShowErrorAsync(
                    "Error",
                    "No se pudo abrir la pantalla porque la navegación de Shell no está disponible.");
                return;
            }

            await MainThread.InvokeOnMainThreadAsync(() => shell.GoToAsync(route));
        }

        private async Task NavigateAsync(string route, IDictionary<string, object> parameters)
        {
            var shell = GetActiveShell();
            if (shell is null)
            {
                await _dialogService.ShowErrorAsync(
                    "Error",
                    "No se pudo abrir la pantalla porque la navegación de Shell no está disponible.");
                return;
            }

            await MainThread.InvokeOnMainThreadAsync(() => shell.GoToAsync(route, parameters));
        }

        private async Task OpenInventoryListAsync()
        {
            var shell = GetActiveShell();
            if (shell is null)
            {
                await _dialogService.ShowErrorAsync(
                    "Error",
                    "No se pudo abrir el listado porque no hay una pagina activa.");
                return;
            }

            var inventoryPage = new NavigationPage(_serviceProvider.GetRequiredService<InventoryList>())
            {
                BarBackgroundColor = Color.FromArgb("#1F3A5F"),
                BarTextColor = Colors.White
            };

            await MainThread.InvokeOnMainThreadAsync(() => shell.Navigation.PushModalAsync(inventoryPage));

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
                var operationalTasksResponse = await _warehouseTaskService.GetTasksAsync(soloPendientes: true, selectedWarehouseId);
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
            IsBusy = true;
            try
            {

                bool tieneTarea = false;
                if (CanViewWarehouseTasks)
                {
                    try
                    {
                        _dialogService.ShowBlocking("Verificando", "Comprobando tarea asignada...");
                        var taskResponse = await _warehouseTaskService.GetMyAssignedTaskAsync();
                        tieneTarea = taskResponse.IsSuccess && taskResponse.Data != null;
                    }
                    catch { }
                    finally
                    {
                        _dialogService.HideBlocking();
                    }
                }

                var dialog = new LogoutDialog(tieneTarea);
                var page = GetActivePage();
                if (page is null)
                {
                    await _dialogService.ShowErrorAsync(
                        "Error",
                        "No se pudo mostrar el diálogo de cierre de sesión.");
                    return;
                }

                var result = await page.ShowPopupAsync(dialog);

                if (result is not true) return;
                await _sessionService.ClearAsync();
                await NavigateAsync("//login");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavigateToChangeLocation()
        {
            var parameters = new Dictionary<string, object> { { "TextInformation", "" } };
            await NavigateAsync("ChangeLocationPage", parameters);
        }

        private async Task NavigateToPicking()
        {
            await NavigateAsync("PickingPage");
        }

        private async Task NavigateToValidarEmbarque()
        {
            await NavigateAsync(nameof(ValidarEmbarquePage));
        }

        private async Task NavigateToReception()
        {
            await NavigateAsync("ReceptionPage");
        }

        private async Task NavigateToTaskManager()
        {
            await NavigateAsync(nameof(TaskWaitingPage));
        }

        private async Task NavigateToTaskManagerSecurity(string? textInfo)
        {
            await NavigateAsync(nameof(TaskSecurityPage));
        }

        private async Task NavigateToTaskList()
        {
            await NavigateAsync("TaskList");
        }

        private async Task NavigateToCaseta()
        {
            var opciones = new[] { "Carga", "Descarga" };
            var popup = new OptionPopup(
                "Caseta",
                opciones,
                OptionPopupPresentation.Separated);

            var page = GetActivePage();
            if (page is null)
            {
                await _dialogService.ShowErrorAsync(
                    "Error",
                    "No se pudo mostrar el selector de caseta.");
                return;
            }

            page.ShowPopup(popup);

            var seleccion = await popup.Result;

            if (string.IsNullOrWhiteSpace(seleccion)) return;

            await NavigateAsync($"RegisterLicense?tipo={Uri.EscapeDataString(seleccion)}");
        }

        private async Task NavigateToMovement()
        {
            await NavigateAsync("MovementPage");
        }

        private async Task NavigateToDamageReport()
        {
            await NavigateAsync("DamageReportPage");
        }

        private async Task NavigateToOperations()
        {
            await NavigateAsync("WarehouseOperations");
        }

        private async Task NavigateToAudit()
        {
            var parameters = new Dictionary<string, object>
            {
                { "TextInformation", "Auditar Ubicación: DC01A" }
            };
            await NavigateAsync("ChangeLocationPage", parameters);
        }

        private async Task NavigateToInventoryList()
        {
            await OpenInventoryListAsync();
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

                    await NavigateAsync(nameof(ForkliftChecklistPage),
                        new Dictionary<string, object>
                        {
                            ["Equipment"]   = status.Equipment!,
                            ["IsMandatory"] = false
                        });
                    return;
                }

                // No ha hecho checklist hoy → modo obligatorio
                await NavigateAsync(nameof(ForkliftChecklistPage),
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
            await NavigateAsync(nameof(PatioPendientesPage));
        }

    }
}
