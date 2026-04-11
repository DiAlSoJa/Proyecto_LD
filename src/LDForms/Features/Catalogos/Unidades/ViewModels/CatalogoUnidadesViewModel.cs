using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Units;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Catalogos.Unidades.ViewModels;

public partial class CatalogoUnidadesViewModel : ObservableObject
{
    private readonly UnitService _unitService;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private UnitDto? selectedUnit;

    public event Action<List<UnitDto>>? OnDataLoaded;

    public CatalogoUnidadesViewModel(UnitService unitService)
    {
        _unitService = unitService;
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
