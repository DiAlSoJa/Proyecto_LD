using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.DTOs.Security;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Almacen.ViewModels;

public partial class CortinasViewModel : ObservableObject
{
    private readonly CortinaService _cortinaService;

    [ObservableProperty]
    private string headerTitle = "Cortinas";

    [ObservableProperty]
    private string statusText = "";

    [ObservableProperty]
    private WarehouseDto? selectedWarehouse;

    [ObservableProperty]
    private CortinaDto? selectedCortina;

    public bool CanExecuteEdit => SelectedCortina is not null;
    public bool CanExecuteDelete => SelectedCortina is not null;

    partial void OnSelectedCortinaChanged(CortinaDto? value)
    {
        OnPropertyChanged(nameof(CanExecuteEdit));
        OnPropertyChanged(nameof(CanExecuteDelete));
    }

    public event Action<List<CortinaDto>>? OnDataLoaded;

    public CortinasViewModel(CortinaService cortinaService)
    {
        _cortinaService = cortinaService;
    }

    public void SetWarehouse(WarehouseDto? warehouse)
    {
        SelectedWarehouse = warehouse;
        HeaderTitle = warehouse is null ? "Cortinas" : $"Cortinas - {warehouse.NombreAlmacen}";
    }

    public async Task CargarDatosAsync()
    {
        try
        {
            var result = await _cortinaService.GetCortinas(SelectedWarehouse?.Id);
            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            StatusText = $"Registros: {result.Data?.Count ?? 0}";
            OnDataLoaded?.Invoke(result.Data ?? []);
            SelectedCortina = null;
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    public async Task<bool> DeleteSelectedAsync()
    {
        if (SelectedCortina is null)
            return false;

        var result = await _cortinaService.DeleteCortina(SelectedCortina.CortinaId);
        if (!result.IsSuccess)
        {
            DialogHelper.ShowError(result.Message);
            return false;
        }

        DialogHelper.ShowSuccess(result.Message);
        return true;
    }
}
