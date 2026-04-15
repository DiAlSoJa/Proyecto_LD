using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Currency;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Catalogos.Monedas.ViewModels;

public partial class CatalogoMonedasViewModel : ObservableObject
{
    private readonly CurrencyService _currencyService;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private CurrencyDto? selectedCurrency;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    public event Action<List<CurrencyDto>>? OnDataLoaded;

    public CatalogoMonedasViewModel(CurrencyService currencyService)
    {
        _currencyService = currencyService;
        CanCreate = UserData.HasPermission(PermissionKeys.Currency_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.Currency_Update);
        CanView = UserData.HasPermission(PermissionKeys.Currency_View);
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
