using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Usuarios.ViewModels;

public partial class UsuarioAlmacenViewModel : ObservableObject
{
    private readonly WarehouseService _warehouseService;
    private readonly UserService _userService;

    private GetUserDto? _selectedUser;

    private List<WarehouseDto> _allWarehouses = [];
    private List<WarehouseDto> _addedWarehouses = [];
    private List<WarehouseDto> _availableWarehouses = [];

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Almacenes del usuario";

    [ObservableProperty]
    private List<WarehouseDto> disponibles = [];

    [ObservableProperty]
    private List<WarehouseDto> asignados = [];

    [ObservableProperty]
    private WarehouseDto? selectedDisponible;

    [ObservableProperty]
    private WarehouseDto? selectedAsignado;

    public UsuarioAlmacenViewModel(WarehouseService warehouseService, UserService userService)
    {
        _warehouseService = warehouseService;
        _userService = userService;
    }

    public void SetUser(GetUserDto user)
    {
        _selectedUser = user;
        if (user.User?.Nombre is not null)
            HeaderTitle = $"Almacenes — {user.User.Nombre}";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        try
        {
            var response = await _warehouseService.GetWarehouses();

            if (!response.IsSuccess)
            {
                DialogHelper.ShowWarning(response.Message);
                return;
            }

            _allWarehouses = response.Data ?? [];

            var assignedIds = _selectedUser?.Warehouse?
                .Select(w => w.Id)
                .ToHashSet() ?? [];

            _addedWarehouses = _allWarehouses.Where(w => assignedIds.Contains(w.Id)).ToList();
            _availableWarehouses = _allWarehouses.Where(w => !assignedIds.Contains(w.Id)).ToList();

            RefreshLists();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    private void RefreshLists()
    {
        Disponibles = _availableWarehouses.ToList();
        Asignados = _addedWarehouses.ToList();
    }

    private async Task SaveAsync()
    {
        if (_selectedUser?.User?.Id is null) return;

        var request = new UserWarehouseRequest
        {
            WarehouseIds = _addedWarehouses.Select(w => w.Id).ToList()
        };

        var result = await _userService.AssignWarehouses(_selectedUser.User.Id, request);

        if (!result.IsSuccess)
            DialogHelper.ShowError(result.Message);
        else
            ResponseForm = true;
    }

    [RelayCommand]
    public async Task AgregarAsync()
    {
        if (SelectedDisponible is null) return;

        _addedWarehouses.Add(SelectedDisponible);
        _availableWarehouses.Remove(SelectedDisponible);
        SelectedDisponible = null;

        RefreshLists();
        await SaveAsync();
    }

    [RelayCommand]
    public async Task QuitarAsync()
    {
        if (SelectedAsignado is null) return;

        _availableWarehouses.Add(SelectedAsignado);
        _addedWarehouses.Remove(SelectedAsignado);
        SelectedAsignado = null;

        RefreshLists();
        await SaveAsync();
    }
}
