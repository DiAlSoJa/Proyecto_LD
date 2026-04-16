using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Dimensioner;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Catalogos.Dimensionador.ViewModels;

public partial class CatalogoDimensionadorViewModel : ObservableObject
{
    private readonly DimensionerService _dimensionerService;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private DimensionerDto? selectedDimensioner;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    public event Action<List<DimensionerDto>>? OnDataLoaded;

    public CatalogoDimensionadorViewModel(DimensionerService dimensionerService)
    {
        _dimensionerService = dimensionerService;
        CanCreate = UserData.HasPermission(PermissionKeys.Dimensioner_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.Dimensioner_Update);
        CanView = UserData.HasPermission(PermissionKeys.Dimensioner_View);

    }

    public async Task CargarDatosAsync()
    {
        try
        {
            var result = await _dimensionerService.GetDimensioners();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            SelectedDimensioner = null;
            StatusText = $"Registros: {result.Data.Count}";
            OnDataLoaded?.Invoke(result.Data);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }
}
