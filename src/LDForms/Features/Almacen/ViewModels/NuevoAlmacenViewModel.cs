using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Almacen.ViewModels;

public partial class NuevoAlmacenViewModel : ObservableObject
{
    private readonly WarehouseService _warehouseService;

    private int? _editWarehouseId;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nuevo almacén";

    [ObservableProperty]
    private bool isSaving;

    [ObservableProperty]
    private string warehouseName = "";

    [ObservableProperty]
    private string address = "";

    [ObservableProperty]
    private string neighborhood = "";

    [ObservableProperty]
    private string city = "";

    [ObservableProperty]
    private string zipCode = "";

    [ObservableProperty]
    private string capacity = "";

    [ObservableProperty]
    private bool isProduction;

    [ObservableProperty]
    private bool isActive = true;

    public NuevoAlmacenViewModel(WarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    public void SetWarehouse(WarehouseDto warehouse)
    {
        _editWarehouseId = warehouse.Id;
        HeaderTitle = "Editar almacén";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (_editWarehouseId != null)
            await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        try
        {
            var response = await _warehouseService.GetWarehouseById(_editWarehouseId ?? 0);

            if (!response.IsSuccess || response.Data == null)
            {
                DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo cargar el almacén.");
                return;
            }

            var w = response.Data;
            WarehouseName = w.WarehouseName ?? "";
            Address = w.Address ?? "";
            Neighborhood = w.Neighborhood ?? "";
            City = w.City ?? "";
            ZipCode = w.ZipCode ?? "";
            Capacity = w.Capacity?.ToString() ?? "";
            IsProduction = w.IsProduction;
            IsActive = w.IsActive;
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    private WarehouseRequest BuildRequest() => new()
    {
        WarehouseId = _editWarehouseId ?? 0,
        WarehouseName = WarehouseName.Trim(),
        Address = Address.Trim(),
        Neighborhood = Neighborhood.Trim(),
        City = City.Trim(),
        ZipCode = ZipCode.Trim(),
        Capacity = decimal.TryParse(Capacity, out decimal cap) ? cap : null,
        IsProduction = IsProduction,
        IsActive = IsActive
    };

    [RelayCommand]
    public async Task SaveAsync()
    {
        try
        {
            IsSaving = true;
            var request = BuildRequest();

            var result = _editWarehouseId != null
                ? await _warehouseService.UpdateWarehouse(_editWarehouseId.Value, request)
                : await _warehouseService.CreateWarehouse(request);

            if (result.IsSuccess)
            {
                DialogHelper.ShowSuccess(result.Data ?? "Operación realizada correctamente.");
                ResponseForm = true;
                RequestClose?.Invoke();
            }
            else
            {
                DialogHelper.ShowError(result.ErrorMessage ?? "Ocurrió un error al guardar.");
            }
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsSaving = false;
        }
    }
}
