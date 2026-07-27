using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.TruckType;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Catalogos.TiposCamion.ViewModels;

public partial class CatalogoTiposCamionViewModel : ObservableObject
{
    private readonly TruckTypeService _truckTypeService;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private TruckTypeDto? selectedTruckType;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    [ObservableProperty]
    private bool canDelete;

    public event Action<List<TruckTypeDto>>? OnDataLoaded;

    public CatalogoTiposCamionViewModel(TruckTypeService truckTypeService)
    {
        _truckTypeService = truckTypeService;

        CanCreate = UserData.HasPermission(PermissionKeys.Catalog_View);
        CanEdit = UserData.HasPermission(PermissionKeys.Catalog_View);
        CanView = UserData.HasPermission(PermissionKeys.Catalog_View);
        CanDelete = UserData.HasPermission(PermissionKeys.Catalog_View);
    }

    public async Task CargarDatosAsync()
    {
        try
        {
            var result = await _truckTypeService.GetTruckTypes();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            SelectedTruckType = null;
            StatusText = $"Registros: {result.Data.Count}";
            OnDataLoaded?.Invoke(result.Data);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    public async Task<bool> DeleteSelectedAsync()
    {
        if (SelectedTruckType is null)
            return false;

        var result = await _truckTypeService.DeleteTruckType(SelectedTruckType.TruckTypeId);

        if (!result.IsSuccess)
        {
            DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "Hubo un error al eliminar el tipo de camion.");
            return false;
        }

        DialogHelper.ShowSuccess(result.Data ?? "Tipo de camion eliminado correctamente.");
        return true;
    }
}
