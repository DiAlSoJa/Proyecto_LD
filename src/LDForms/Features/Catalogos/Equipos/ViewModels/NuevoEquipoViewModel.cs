using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.EquipmentType;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Catalogos.Equipos.ViewModels;

public partial class NuevoEquipoViewModel : ObservableObject
{
    private readonly EquipmentTypeService _equipmentTypeService;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }
    public EquipmentTypeDto? SelectedEquipmentType { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nuevo equipo";

    public NuevoEquipoViewModel(EquipmentTypeService equipmentTypeService)
    {
        _equipmentTypeService = equipmentTypeService;
    }

    public void SetEquipmentType(EquipmentTypeDto equipmentType)
    {
        SelectedEquipmentType = equipmentType;
        HeaderTitle = "Editar equipo";
    }

    public async Task<EquipmentTypeRequest?> GetEquipmentTypeAsync()
    {
        var response = await _equipmentTypeService.GetEquipmentTypeById(SelectedEquipmentType?.EquipmentTypeId ?? 0);

        if (!response.IsSuccess || response.Data is null)
        {
            DialogHelper.ShowError(response.Message ?? "No se pudo cargar el equipo.");
            return null;
        }

        return response.Data;
    }

    public async Task SaveAsync(EquipmentTypeRequest request, int equipmentTypeId)
    {
        try
        {
            var result = SelectedEquipmentType is not null
                ? await _equipmentTypeService.UpdateEquipmentType(equipmentTypeId, request)
                : await _equipmentTypeService.CreateEquipmentType(request);

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
