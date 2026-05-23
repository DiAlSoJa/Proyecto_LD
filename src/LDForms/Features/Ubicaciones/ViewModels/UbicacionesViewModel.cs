using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Location;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Ubicaciones.ViewModels;

public partial class UbicacionesViewModel : ObservableObject
{
    private readonly LocationService _locationService;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string loadingMessage = "Trayendo ubicaciones...";

    [ObservableProperty]
    private string statusText = "";

    [ObservableProperty]
    private LocationDto? selectedLocation;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    [ObservableProperty]
    private int selectedWarehouseId;

    public bool CanExecuteEdit => CanEdit && SelectedLocation is not null;

    partial void OnSelectedLocationChanged(LocationDto? value) =>
        OnPropertyChanged(nameof(CanExecuteEdit));

    public event Action<List<LocationDto>>? OnDataLoaded;

    public UbicacionesViewModel(LocationService locationService)
    {
        _locationService = locationService;

        CanCreate = UserData.HasPermission(PermissionKeys.Location_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.Location_Update);
        CanView = UserData.HasPermission(PermissionKeys.Location_View);
    }

    [RelayCommand]
    public async Task CargarDatosAsync()
    {
        if (!CanView) return;

        try
        {
            IsLoading = true;
            LoadingMessage = "Trayendo ubicaciones...";

            if (SelectedWarehouseId <= 0)
            {
                SelectedLocation = null;
                StatusText = "Selecciona un almacén para consultar.";
                OnDataLoaded?.Invoke([]);
                return;
            }

            var result = await _locationService.GetLocations();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            var filteredData = result.Data?.Where(x => x.WarehouseId == SelectedWarehouseId).ToList() ?? [];

            SelectedLocation = null;
            StatusText = $"Registros: {filteredData.Count}";
            OnDataLoaded?.Invoke(filteredData);
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
