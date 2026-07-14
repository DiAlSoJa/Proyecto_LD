using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.DTOs.Security;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Almacen.ViewModels;

public partial class NuevaCortinaViewModel : ObservableObject
{
    private readonly CortinaService _cortinaService;
    private int? _editCortinaId;

    public Action? RequestClose { get; set; }

    [ObservableProperty]
    private string headerTitle = "Nueva cortina";

    [ObservableProperty]
    private string numero = "";

    [ObservableProperty]
    private string descripcion = "";

    [ObservableProperty]
    private bool estaDisponible = true;

    [ObservableProperty]
    private int warehouseId;

    [ObservableProperty]
    private string warehouseName = "";

    public NuevaCortinaViewModel(CortinaService cortinaService)
    {
        _cortinaService = cortinaService;
    }

    public void SetWarehouse(WarehouseDto? warehouse)
    {
        WarehouseId = warehouse?.Id ?? 0;
        WarehouseName = warehouse?.NombreAlmacen ?? "";
        UpdateHeaderTitle();
    }

    public void SetCortina(CortinaDto? cortina)
    {
        if (cortina is null)
            return;

        _editCortinaId = cortina.CortinaId;
        HeaderTitle = "Editar cortina";
        Numero = cortina.Numero;
        Descripcion = cortina.Descripcion;
        EstaDisponible = cortina.EstaDisponible;
        WarehouseId = cortina.WarehouseId;
        UpdateHeaderTitle();
    }

    [RelayCommand]
    public Task LoadAsync() => Task.CompletedTask;

    [RelayCommand]
    public async Task SaveAsync()
    {
        try
        {
            var request = new CortinaRequest
            {
                CortinaId = _editCortinaId,
                Numero = Numero.Trim(),
                Descripcion = Descripcion.Trim(),
                EstaDisponible = EstaDisponible,
                WarehouseId = WarehouseId
            };

            var result = _editCortinaId.HasValue
                ? await _cortinaService.UpdateCortina(_editCortinaId.Value, request)
                : await _cortinaService.CreateCortina(request);

            if (result.IsSuccess)
            {
                DialogHelper.ShowSuccess(result.Message);
                RequestClose?.Invoke();
            }
            else
            {
                DialogHelper.ShowError(result.Message);
            }
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    private void UpdateHeaderTitle()
    {
        var action = _editCortinaId.HasValue ? "Editar cortina" : "Nueva cortina";
        HeaderTitle = string.IsNullOrWhiteSpace(WarehouseName)
            ? action
            : $"{action} - {WarehouseName}";
    }
}
