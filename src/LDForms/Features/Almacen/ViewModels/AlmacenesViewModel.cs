using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Almacen.ViewModels;

public partial class AlmacenesViewModel : ObservableObject
{
    private readonly WarehouseService _warehouseService;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string loadingMessage = "Trayendo almacenes...";

    [ObservableProperty]
    private string statusText = "";

    [ObservableProperty]
    private WarehouseDto? selectedWarehouse;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    public event Action<List<WarehouseDto>>? OnDataLoaded;

    public AlmacenesViewModel(WarehouseService warehouseService)
    {
        _warehouseService = warehouseService;

        CanCreate = UserData.HasPermission(PermissionKeys.Warehouse_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.Warehouse_Update);
        CanView = UserData.HasPermission(PermissionKeys.Warehouse_View);
    }

    [RelayCommand]
    public async Task CargarDatosAsync()
    {
        if (!CanView) return;

        try
        {
            IsLoading = true;
            LoadingMessage = "Trayendo almacenes...";

            var result = await _warehouseService.GetWarehouses();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            SelectedWarehouse = null;
            StatusText = $"Registros: {result.Data?.Count ?? 0}";
            OnDataLoaded?.Invoke(result.Data ?? []);
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
