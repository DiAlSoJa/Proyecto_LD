using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Requests;
using LD.Contracts.Units;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Catalogos.Unidades.ViewModels;

public partial class NuevaUnidadViewModel : ObservableObject
{
    private readonly UnitService _unitService;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }
    public UnitDto? SelectedUnit { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nueva unidad";

    public NuevaUnidadViewModel(UnitService unitService)
    {
        _unitService = unitService;
    }

    public void SetUnit(UnitDto unit)
    {
        SelectedUnit = unit;
        HeaderTitle = "Editar unidad";
    }

    public async Task<UnitRequest?> GetUnitAsync()
    {
        var response = await _unitService.GetUnitById(SelectedUnit?.Unidad ?? string.Empty);

        if (!response.IsSuccess || response.Data is null)
        {
            DialogHelper.ShowError(response.Message ?? "No se pudo cargar la unidad.");
            return null;
        }

        return response.Data;
    }

    public async Task SaveAsync(UnitRequest request, string unitId)
    {
        try
        {
            var result = SelectedUnit is not null
                ? await _unitService.UpdateUnit(unitId, request)
                : await _unitService.CreateUnit(request);

            if (result.IsSuccess)
            {
                DialogHelper.ShowSuccess(result.Data ?? "Guardado correctamente.");
                ResponseForm = true;
                RequestClose?.Invoke();
            }
            else
            {
                DialogHelper.ShowError(result.ErrorMessage ?? "Hubo un error al guardar.");
            }
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }
}
