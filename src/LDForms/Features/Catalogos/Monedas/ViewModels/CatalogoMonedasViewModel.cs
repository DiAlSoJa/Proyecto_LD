using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Currency;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Catalogos.Monedas.ViewModels;

public partial class CatalogoMonedasViewModel : ObservableObject
{
    private readonly CurrencyService _currencyService;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private CurrencyDto? selectedCurrency;

    public event Action<List<CurrencyDto>>? OnDataLoaded;

    public CatalogoMonedasViewModel(CurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    public async Task CargarDatosAsync()
    {
        try
        {
            var result = await _currencyService.GetCurrency();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            SelectedCurrency = null;
            StatusText = $"Registros: {result.Data.Count}";
            OnDataLoaded?.Invoke(result.Data);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }
}
