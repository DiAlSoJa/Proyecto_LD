using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Units;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Catalogos.Unidades.ViewModels;

public partial class CatalogoUnidadesViewModel : ObservableObject
{
    private readonly UnitService _unitService;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private UnitDto? selectedUnit;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    public event Action<List<UnitDto>>? OnDataLoaded;

    public CatalogoUnidadesViewModel(UnitService unitService)
    {
        _unitService = unitService;
        CanCreate = UserData.HasPermission(PermissionKeys.Unit_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.Unit_Update);
        CanView = UserData.HasPermission(PermissionKeys.Unit_View);
    }

    public async Task CargarDatosAsync()
    {
        try
        {
            var result = await _unitService.GetUnits();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            SelectedUnit = null;
            StatusText = $"Registros: {result.Data.Count}";
            OnDataLoaded?.Invoke(result.Data);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }
}
