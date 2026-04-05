using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Ubicaciones.ViewModels;

public partial class NuevaUbicacionViewModel : ObservableObject
{
    private readonly LocationService _locationService;
    private readonly LookupService _lookupService;

    private int? _editLocationId;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nueva ubicación";

    [ObservableProperty]
    private bool isSaving;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEditFields))]
    private bool isEditing;

    public bool CanEditFields => !IsEditing;

    // ── Combos ──
    [ObservableProperty]
    private List<DropDownDto> warehousesSource = [];

    [ObservableProperty]
    private string? selectedWarehouseId;

    // ── Datos principales ──
    [ObservableProperty]
    private string locationName = "";

    [ObservableProperty]
    private bool isActive = true;

    [ObservableProperty]
    private bool isFiscal;

    [ObservableProperty]
    private bool hasControlledTemperature;

    // ── Dimensiones ──
    [ObservableProperty]
    private string height = "";

    [ObservableProperty]
    private string width = "";

    [ObservableProperty]
    private string depth = "";

    // ── Tipo (RadioButton grupo 1) ──
    [ObservableProperty]
    private bool isRack = true;

    [ObservableProperty]
    private bool isCompartidoType;

    // ── Sub-tipo (RadioButton grupo 2) ──
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

    // ── Tamaño (RadioButton grupo 3) ──
    [ObservableProperty]
    private bool isDoble;

    [ObservableProperty]
    private bool isSencillo = true;

    // ── Extras ──
    [ObservableProperty]
    private bool hasPaso;

    [ObservableProperty]
    private bool hasCortina;

    public NuevaUbicacionViewModel(LocationService locationService, LookupService lookupService)
    {
        _locationService = locationService;
        _lookupService = lookupService;
    }

    public void SetLocation(LocationDto location)
    {
        _editLocationId = location.LocationId;
        HeaderTitle = "Editar ubicación";
        IsEditing = true;
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
            DialogHelper.ShowError($"Ocurrió un error al cargar los almacenes: {ex.Message}");
        }
    }

    private async Task CargarDatosAsync()
    {
        try
        {
            var response = await _locationService.GetLocationById(_editLocationId ?? 0);

            if (!response.IsSuccess || response.Data == null)
            {
                DialogHelper.ShowError(response.Message ?? "No se pudo cargar la ubicación.");
                return;
            }

            var l = response.Data;

            SelectedWarehouseId = l.WarehouseId.ToString();
            LocationName = l.LocationName ?? "";

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
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError($"Ocurrió un error al cargar la información: {ex.Message}");
        }
    }

    private LocationRequest BuildRequest()
    {
        var name = LocationName.Trim();

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
            LocationId = _editLocationId ?? 0,
            WarehouseId = int.TryParse(SelectedWarehouseId, out int wId) ? wId : 0,
            LocationName = name,

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

            Level = level,
            Position = position,
            Rack = rack
        };
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedWarehouseId))
        {
            DialogHelper.ShowWarning("Selecciona un almacén.");
            return;
        }

        if (string.IsNullOrWhiteSpace(LocationName))
        {
            DialogHelper.ShowWarning("Captura el nombre de la ubicación.");
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
                DialogHelper.ShowSuccess(result.Data ?? "Operación realizada correctamente.");
                ResponseForm = true;
                RequestClose?.Invoke();
            }
            else
            {
                DialogHelper.ShowError(result.Message ?? "Ocurrió un error al guardar la ubicación.");
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
}
