using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Location;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
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

            var result = await _locationService.GetLocations();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            SelectedLocation = null;
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
