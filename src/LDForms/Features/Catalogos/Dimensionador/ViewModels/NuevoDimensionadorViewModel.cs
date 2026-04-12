using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Dimensioner;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Catalogos.Dimensionador.ViewModels;

public partial class NuevoDimensionadorViewModel : ObservableObject
{
    private readonly DimensionerService _dimensionerService;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }
    public DimensionerDto? SelectedDimensioner { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nuevo dimensionador";

    public NuevoDimensionadorViewModel(DimensionerService dimensionerService)
    {
        _dimensionerService = dimensionerService;
    }

    public void SetDimensioner(DimensionerDto dimensioner)
    {
        SelectedDimensioner = dimensioner;
        HeaderTitle = "Editar dimensionador";
    }

    public async Task<DimensionerRequest?> GetDimensionerAsync()
    {
        var response = await _dimensionerService.GetDimensionerById(SelectedDimensioner?.DimensionerId ?? string.Empty);

        if (!response.IsSuccess || response.Data is null)
        {
            DialogHelper.ShowError(response.Message ?? "No se pudo cargar la dimensión.");
            return null;
        }

        return response.Data;
    }

    public async Task SaveAsync(DimensionerRequest request, string dimensionerId)
    {
        try
        {
            var result = SelectedDimensioner is not null
                ? await _dimensionerService.UpdateDimensioner(dimensionerId, request)
                : await _dimensionerService.CreateDimensioner(request);

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

    public decimal ParseDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;

        if (decimal.TryParse(value.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;

        if (decimal.TryParse(value.Trim(), NumberStyles.Any, new CultureInfo("es-MX"), out result))
            return result;

        throw new Exception($"El valor '{value}' no es numérico válido.");
    }
}
