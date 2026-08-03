using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Ubicaciones.ViewModels;

public partial class NuevaUbicacionViewModel : ObservableObject
{
    private readonly LocationService _locationService;
    private readonly LookupService _lookupService;

    private int? _editLocationId;
    private List<LocationDto> _bulkLocations = [];

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nueva ubicacion";

    [ObservableProperty]
    private bool isSaving;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEditFields))]
    private bool isEditing;

    public bool CanEditFields => !IsEditing;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowIdentityFields))]
    private bool isBulkEdit;

    public bool ShowIdentityFields => !IsBulkEdit;

    // Combos
    [ObservableProperty]
    private List<DropDownDto> warehousesSource = [];

    [ObservableProperty]
    private string? selectedWarehouseId;

    // Datos principales
    [ObservableProperty]
    private string locationName = "";

    [ObservableProperty]
    private string aisle = "";

    [ObservableProperty]
    private bool isActive = true;

    [ObservableProperty]
    private bool isFiscal;

    [ObservableProperty]
    private bool hasControlledTemperature;

    // Dimensiones
    [ObservableProperty]
    private string height = "";

    [ObservableProperty]
    private string width = "";

    [ObservableProperty]
    private string depth = "";

    // Tipo
    [ObservableProperty]
    private bool isRack = true;

    [ObservableProperty]
    private bool isCompartidoType;

    // Sub-tipo
    [ObservableProperty]
    private bool isGeneral = true;

    [ObservableProperty]
    private bool isCompartido;

    [ObservableProperty]
    private bool isReciboYEmbarque;

    [ObservableProperty]
    private bool isCuarentena;

    [ObservableProperty]
    private bool isEmbarque;

    // Tamaño
    [ObservableProperty]
    private bool isDoble;

    [ObservableProperty]
    private bool isSencillo = true;

    // Extras
    [ObservableProperty]
    private bool hasPaso;

    [ObservableProperty]
    private bool hasCortina;

    [ObservableProperty]
    private bool ocupado;

    [ObservableProperty]
    private string placas = "";

    public NuevaUbicacionViewModel(LocationService locationService, LookupService lookupService)
    {
        _locationService = locationService;
        _lookupService = lookupService;
    }

    public void SetLocation(LocationDto location)
    {
        _bulkLocations = [];
        _editLocationId = location.LocationId;
        HeaderTitle = "Editar ubicacion";
        IsEditing = true;
        IsBulkEdit = false;
        SelectedWarehouseId = location.WarehouseId.ToString();
        LocationName = location.Ubicacion ?? "";
        Aisle = location.Aisle ?? "";
        ResponseForm = false;
        IsSaving = false;
    }

    public void SetLocations(IEnumerable<LocationDto> locations)
    {
        _bulkLocations = locations?.ToList() ?? [];
        if (_bulkLocations.Count == 0)
            return;

        _editLocationId = _bulkLocations[0].LocationId;
        HeaderTitle = $"Editar masivo ({_bulkLocations.Count})";
        IsEditing = true;
        IsBulkEdit = true;
        SelectedWarehouseId = _bulkLocations[0].WarehouseId.ToString();
        LocationName = _bulkLocations[0].Ubicacion ?? "";
        Aisle = _bulkLocations[0].Aisle ?? "";
        ResponseForm = false;
        IsSaving = false;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        await CargarCombosAsync();
        if (_editLocationId != null)
            await CargarDatosAsync();
    }

    private async Task CargarCombosAsync()
    {
        try
        {
            var response = await _lookupService.GetWarehouseLookup();

            if (response.IsFailure || response.Data == null)
            {
                DialogHelper.ShowError(response.Message ?? "No se pudieron cargar los almacenes.");
                return;
            }

            WarehousesSource = response.Data;
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError($"Ocurrio un error al cargar los almacenes: {ex.Message}");
        }
    }

    private async Task CargarDatosAsync()
    {
        try
        {
            var response = await _locationService.GetLocationById(_editLocationId ?? 0);

            if (!response.IsSuccess || response.Data == null)
            {
                DialogHelper.ShowError(response.Message ?? "No se pudo cargar la ubicacion.");
                return;
            }

            var l = response.Data;

            SelectedWarehouseId = l.WarehouseId.ToString();
            LocationName = l.LocationName ?? "";
            Aisle = l.Aisle ?? "";

            IsActive = l.IsActive;
            IsFiscal = l.IsFiscal;
            HasControlledTemperature = l.HasControlledTemperature;

            Height = l.Height?.ToString() ?? "";
            Width = l.Width?.ToString() ?? "";
            Depth = l.Depth?.ToString() ?? "";

            IsRack = l.IsRack;
            IsCompartidoType = l.IsCompartidoType;

            IsGeneral = l.IsGeneral;
            IsCuarentena = l.IsCuarentena;
            IsEmbarque = l.IsEmbarque;
            IsCompartido = l.IsCompartido;
            IsReciboYEmbarque = l.IsReciboYEmbarque;

            IsDoble = l.IsDoble;
            IsSencillo = l.IsSencillo;

            HasPaso = l.HasPaso;
            HasCortina = l.HasCortina;
            Ocupado = l.Ocupado;
            Placas = l.Placas ?? "";
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError($"Ocurrio un error al cargar la informacion: {ex.Message}");
        }
    }

    private LocationRequest BuildRequest(LocationDto? sourceLocation = null)
    {
        var name = IsBulkEdit && sourceLocation is not null
            ? (sourceLocation.Ubicacion ?? string.Empty).Trim()
            : LocationName.Trim();

        var warehouseId = IsBulkEdit && sourceLocation is not null
            ? sourceLocation.WarehouseId
            : int.TryParse(SelectedWarehouseId, out int wId) ? wId : 0;

        string level = string.Empty;
        string position = string.Empty;
        string rack = string.Empty;

        if (!string.IsNullOrWhiteSpace(name))
        {
            if (name.Length >= 1)
                level = name.Substring(name.Length - 1, 1);

            if (name.Length >= 3)
                position = name.Substring(name.Length - 3, 2);

            if (name.Length > 3)
                rack = name.Substring(0, name.Length - 3);
            else
                rack = name;
        }

        return new LocationRequest
        {
            LocationId = sourceLocation?.LocationId ?? _editLocationId ?? 0,
            WarehouseId = warehouseId,
            LocationName = name,
            Aisle = string.IsNullOrWhiteSpace(Aisle) ? null : Aisle.Trim(),

            IsActive = IsActive,
            IsFiscal = IsFiscal,
            HasControlledTemperature = HasControlledTemperature,

            Height = decimal.TryParse(Height, out decimal h) ? h : null,
            Width = decimal.TryParse(Width, out decimal w) ? w : null,
            Depth = decimal.TryParse(Depth, out decimal d) ? d : null,

            IsRack = IsRack,
            IsCompartidoType = IsCompartidoType,

            IsGeneral = IsGeneral,
            IsCuarentena = IsCuarentena,
            IsEmbarque = IsEmbarque,
            IsCompartido = IsCompartido,
            IsReciboYEmbarque = IsReciboYEmbarque,

            IsDoble = IsDoble,
            IsSencillo = IsSencillo,

            HasPaso = HasPaso,
            HasCortina = HasCortina,
            Ocupado = Ocupado,
            Placas = string.IsNullOrWhiteSpace(Placas) ? null : Placas.Trim(),

            Level = level,
            Position = position,
            Rack = rack
        };
    }

    private static string GetLocationLabel(LocationDto location)
    {
        var label = location.Ubicacion?.Trim();
        return string.IsNullOrWhiteSpace(label) ? $"ID {location.LocationId}" : label;
    }

    private async Task<(bool Success, string ErrorMessage)> UpdateLocationAsync(LocationDto location)
    {
        try
        {
            var request = BuildRequest(location);
            var result = await _locationService.UpdateLocation(location.LocationId, request);

            if (result.IsSuccess)
                return (true, string.Empty);

            return (false, result.Message ?? $"No se pudo actualizar la ubicacion {GetLocationLabel(location)}.");
        }
        catch (Exception ex)
        {
            return (false, $"Error inesperado al actualizar {GetLocationLabel(location)}: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (IsBulkEdit)
        {
            await SaveBulkAsync();
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedWarehouseId))
        {
            DialogHelper.ShowWarning("Selecciona un almacen.");
            return;
        }

        if (string.IsNullOrWhiteSpace(LocationName))
        {
            DialogHelper.ShowWarning("Captura el nombre de la ubicacion.");
            return;
        }

        try
        {
            IsSaving = true;
            var request = BuildRequest();

            var result = _editLocationId != null
                ? await _locationService.UpdateLocation(_editLocationId.Value, request)
                : await _locationService.CreateLocation(request);

            if (result.IsSuccess)
            {
                DialogHelper.ShowSuccess(result.Data ?? "Operacion realizada correctamente.");
                ResponseForm = true;
                RequestClose?.Invoke();
            }
            else
            {
                DialogHelper.ShowError(result.Message ?? "Ocurrio un error al guardar la ubicacion.");
            }
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError($"Error inesperado: {ex.Message}");
        }
        finally
        {
            IsSaving = false;
        }
    }

    private async Task SaveBulkAsync()
    {
        if (_bulkLocations.Count == 0)
        {
            DialogHelper.ShowWarning("No hay ubicaciones seleccionadas para editar.");
            return;
        }

        try
        {
            IsSaving = true;

            var failures = new List<string>();
            var successCount = 0;

            foreach (var location in _bulkLocations)
            {
                var (success, errorMessage) = await UpdateLocationAsync(location);
                if (success)
                {
                    successCount++;
                    continue;
                }

                failures.Add($"{GetLocationLabel(location)}: {errorMessage}");
            }

            if (failures.Count == 0)
            {
                DialogHelper.ShowSuccess($"{successCount} ubicaciones actualizadas correctamente.");
                ResponseForm = true;
                RequestClose?.Invoke();
                return;
            }

            var summary = successCount > 0
                ? $"Se actualizaron {successCount} de {_bulkLocations.Count} ubicaciones."
                : "No se pudo actualizar ninguna ubicacion.";

            var detailLines = failures
                .Take(3)
                .Select(x => $"- {x}")
                .ToList();

            if (failures.Count > 3)
                detailLines.Add($"- y {failures.Count - 3} mas.");

            DialogHelper.ShowError($"{summary}{Environment.NewLine}{string.Join(Environment.NewLine, detailLines)}");
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError($"Error inesperado: {ex.Message}");
        }
        finally
        {
            IsSaving = false;
        }
    }
}
