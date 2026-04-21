using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.EquipmentType;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Catalogos.Equipos.ViewModels;

public partial class CatalogoEquiposViewModel : ObservableObject
{
    private readonly EquipmentTypeService _equipmentTypeService;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private EquipmentTypeDto? selectedEquipmentType;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    public event Action<List<EquipmentTypeDto>>? OnDataLoaded;

    public CatalogoEquiposViewModel(EquipmentTypeService equipmentTypeService)
    {
        _equipmentTypeService = equipmentTypeService;
        CanCreate = UserData.HasPermission(PermissionKeys.EquipmentType_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.EquipmentType_Update);
        CanView = UserData.HasPermission(PermissionKeys.EquipmentType_View);
    }

    public async Task CargarDatosAsync()
    {
        try
        {
            var result = await _equipmentTypeService.GetEquipmentTypes();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            SelectedEquipmentType = null;
            StatusText = $"Registros: {result.Data.Count}";
            OnDataLoaded?.Invoke(result.Data);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }
}
