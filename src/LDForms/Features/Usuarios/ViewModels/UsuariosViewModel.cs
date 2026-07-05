using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.Realtime;
using LD.Contracts.DTOs.User;
using LD.Contracts.User;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Usuarios.ViewModels;

public partial class UsuariosViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly WarehouseTaskService _warehouseTaskService;

    private List<GetUserDto> _allUsers = [];

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string loadingMessage = "Trayendo usuarios...";

    [ObservableProperty]
    private string statusText = "";

    [ObservableProperty]
    private GetUserDto? selectedUser;

    [ObservableProperty]
    private List<PermissionDto> permissions = [];

    [ObservableProperty]
    private List<WarehouseDto> warehouses = [];

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    [ObservableProperty]
    private bool canManageWarehouses;

    // ── Usuarios conectados (tab "Usuarios conectados") ───────────────────────
    [ObservableProperty]
    private bool isLoadingConectados;

    [ObservableProperty]
    private string conectadosStatusText = "";

    [ObservableProperty]
    private bool canViewConectados;

    public ObservableCollection<ConnectedUserDto> ConnectedUsers { get; } = [];
    public ObservableCollection<ConnectedUserTaskRow> ConnectedAssignedTasks { get; } = [];

    public bool CanExecuteEdit => CanEdit && SelectedUser is not null;
    public bool CanExecuteManageWarehouses => CanManageWarehouses && SelectedUser is not null;

    partial void OnSelectedUserChanged(GetUserDto? value)
    {
        OnPropertyChanged(nameof(CanExecuteEdit));
        OnPropertyChanged(nameof(CanExecuteManageWarehouses));
    }

    public event Action<List<UserDto>>? OnDataLoaded;

    public UsuariosViewModel(UserService userService, WarehouseTaskService warehouseTaskService)
    {
        _userService = userService;
        _warehouseTaskService = warehouseTaskService;

        CanCreate = UserData.HasPermission(PermissionKeys.User_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.User_Update);
        CanView = UserData.HasPermission(PermissionKeys.User_View);
        CanManageWarehouses = UserData.HasPermission(PermissionKeys.User_Update);
        CanViewConectados = UserData.HasPermission(PermissionKeys.WarehouseTask_View);
    }

    [RelayCommand]
    public async Task CargarConectadosAsync()
    {
        if (!CanViewConectados) return;

        try
        {
            IsLoadingConectados = true;

            var result = await _warehouseTaskService.GetConnectedUsersAsync();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            var conectados = result.Data ?? [];

            ConnectedUsers.Clear();
            ConnectedAssignedTasks.Clear();

            foreach (var usuario in conectados)
            {
                ConnectedUsers.Add(usuario);

                foreach (var tarea in usuario.AssignedTasks)
                {
                    ConnectedAssignedTasks.Add(new ConnectedUserTaskRow
                    {
                        UserName      = usuario.FullName ?? usuario.UserName,
                        Folio         = tarea.WarehouseTaskId,
                        TaskName      = tarea.Name,
                        WarehouseName = tarea.WarehouseName,
                        Status        = tarea.Status,
                        AssignedAt    = tarea.AssignedAt
                    });
                }
            }

            ConectadosStatusText = $"Conectados: {ConnectedUsers.Count}";
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsLoadingConectados = false;
        }
    }

    public void SelectUserByDto(UserDto? userDto)
    {
        SelectedUser = _allUsers.FirstOrDefault(x => x.User?.Id == userDto?.Id);
        Permissions = SelectedUser?.Permissions ?? [];
        Warehouses = SelectedUser?.Warehouse ?? [];
    }

    public void SelectUserById(string? userId)
    {
        SelectedUser = _allUsers.FirstOrDefault(x => x.User?.Id == userId);
        Permissions = SelectedUser?.Permissions ?? [];
        Warehouses = SelectedUser?.Warehouse ?? [];
    }

    [RelayCommand]
    public async Task CargarDatosAsync()
    {
        await LoadUsersAsync();
    }

    public async Task CargarDatosConSeleccionAsync(string? userId)
    {
        await LoadUsersAsync(userId);
    }

    private async Task LoadUsersAsync(string? userIdToSelect = null)
    {
        if (!CanView) return;

        try
        {
            IsLoading = true;
            LoadingMessage = "Trayendo usuarios...";

            var result = await _userService.GetUsers();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _allUsers = result.Data ?? [];

            var users = _allUsers
                .Where(x => x.User != null)
                .Select(x => x.User!)
                .ToList();

            StatusText = $"Registros: {users.Count}";
            OnDataLoaded?.Invoke(users);

            if (!string.IsNullOrWhiteSpace(userIdToSelect))
            {
                SelectUserById(userIdToSelect);
            }
            else
            {
                SelectedUser = null;
                Permissions = [];
                Warehouses = [];
            }
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
