using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.Family;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Catalogos.Familias.ViewModels;

public partial class CatalogoFamiliasViewModel : ObservableObject
{
    private readonly FamilyService _familyService;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private FamilyDto? selectedFamily;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    public event Action<List<FamilyDto>>? OnDataLoaded;

    public CatalogoFamiliasViewModel(FamilyService familyService)
    {
        _familyService = familyService;
        CanCreate = UserData.HasPermission(PermissionKeys.Family_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.Family_Update);
        CanView = UserData.HasPermission(PermissionKeys.Family_View);
    }

    public async Task CargarDatosAsync()
    {
        try
        {
            var result = await _familyService.GetFamily();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            SelectedFamily = null;
            StatusText = $"Registros: {result.Data.Count}";
            OnDataLoaded?.Invoke(result.Data);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }
}
