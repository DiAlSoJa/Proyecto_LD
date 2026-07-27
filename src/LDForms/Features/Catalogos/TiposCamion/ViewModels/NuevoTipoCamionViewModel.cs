using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Requests;
using LD.Contracts.TruckType;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Catalogos.TiposCamion.ViewModels;

public partial class NuevoTipoCamionViewModel : ObservableObject
{
    private readonly TruckTypeService _truckTypeService;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }
    public TruckTypeDto? SelectedTruckType { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nuevo tipo de camion";

    public NuevoTipoCamionViewModel(TruckTypeService truckTypeService)
    {
        _truckTypeService = truckTypeService;
    }

    public void SetTruckType(TruckTypeDto truckType)
    {
        SelectedTruckType = truckType;
        HeaderTitle = "Editar tipo de camion";
    }

    public async Task<TruckTypeRequest?> GetTruckTypeAsync()
    {
        var response = await _truckTypeService.GetTruckTypeById(SelectedTruckType?.TruckTypeId ?? 0);

        if (!response.IsSuccess || response.Data is null)
        {
            DialogHelper.ShowError(response.Message ?? "No se pudo cargar el tipo de camion.");
            return null;
        }

        return response.Data;
    }

    public async Task SaveAsync(TruckTypeRequest request)
    {
        try
        {
            var result = SelectedTruckType is not null
                ? await _truckTypeService.UpdateTruckType(SelectedTruckType.TruckTypeId, request)
                : await _truckTypeService.CreateTruckType(request);

            if (result.IsSuccess)
            {
                DialogHelper.ShowSuccess(result.Data ?? "Guardado correctamente.");
                ResponseForm = true;
                RequestClose?.Invoke();
                return;
            }

            DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "Hubo un error al guardar.");
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }
}
