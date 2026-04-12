using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Currency;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Catalogos.Monedas.ViewModels;

public partial class NuevaMonedaViewModel : ObservableObject
{
    private readonly CurrencyService _currencyService;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }
    public CurrencyDto? SelectedCurrency { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nueva moneda";

    public NuevaMonedaViewModel(CurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    public void SetCurrency(CurrencyDto currency)
    {
        SelectedCurrency = currency;
        HeaderTitle = "Editar moneda";
    }

    public async Task<CurrencyRequest?> GetCurrencyAsync()
    {
        var response = await _currencyService.GetCurrencyById(SelectedCurrency?.CurrencyIdS ?? string.Empty);

        if (!response.IsSuccess || response.Data is null)
        {
            DialogHelper.ShowError(response.Message ?? "No se pudo cargar la moneda.");
            return null;
        }

        return response.Data;
    }

    public async Task SaveAsync(CurrencyRequest request, string currencyId)
    {
        try
        {
            var result = SelectedCurrency is not null
                ? await _currencyService.UpdateCurrency(currencyId, request)
                : await _currencyService.CreateCurrency(request);

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
