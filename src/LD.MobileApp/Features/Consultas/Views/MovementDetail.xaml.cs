using LD.Client.Services;
using LD.Contracts.AvailableInventory;
using LD.Contracts.InventoryMovement;
using System.Collections.ObjectModel;

namespace MauiAppLogin;

[QueryProperty(nameof(StandardId), "standardId")]
public partial class MovementDetail : ContentPage
{
    private readonly InventoryMovementService _inventoryMovementService;
    private readonly AvailableInventoryService _availableInventoryService;
    private readonly StandardLabelService _standardLabelService;
    private bool _loaded;
    private string _standardId = string.Empty;
    private string _inventoryStatus = "Cargando inventario...";
    private string _movementStatus = "Cargando movimientos...";
    private bool _isLoading;

    public MovementDetail(
        InventoryMovementService inventoryMovementService,
        AvailableInventoryService availableInventoryService,
        StandardLabelService standardLabelService)
    {
        InitializeComponent();
        _inventoryMovementService = inventoryMovementService;
        _availableInventoryService = availableInventoryService;
        _standardLabelService = standardLabelService;
        BindingContext = this;
    }

    public ObservableCollection<AvailableInventoryDto> InventoryItems { get; } = new();
    public ObservableCollection<InventoryMovementDto> Movements { get; } = new();

    public string StandardId
    {
        get => _standardId;
        set
        {
            _standardId = Uri.UnescapeDataString(value ?? string.Empty);
            OnPropertyChanged(nameof(StandardId));
            OnPropertyChanged(nameof(StandardIdText));
            _loaded = false;
        }
    }

    public string StandardIdText => string.IsNullOrWhiteSpace(StandardId) ? "Sin StandardId" : StandardId;

    public string InventoryStatus
    {
        get => _inventoryStatus;
        set
        {
            _inventoryStatus = value;
            OnPropertyChanged();
        }
    }

    public string MovementStatus
    {
        get => _movementStatus;
        set
        {
            _movementStatus = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_loaded)
            return;

        _loaded = true;
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        InventoryItems.Clear();
        Movements.Clear();

        var standardId = await ResolveStandardIdAsync(StandardId);
        if (!standardId.HasValue)
        {
            InventoryStatus = "StandardId inválido.";
            MovementStatus = "No se cargaron movimientos.";
            return;
        }

        try
        {
            IsLoading = true;
            InventoryStatus = "Cargando inventario...";
            MovementStatus = "Cargando movimientos...";

            var inventoryResponse = await _availableInventoryService.GetAvailableInventories(standardId.Value);
            if (inventoryResponse.IsSuccess && inventoryResponse.Data != null)
            {
                foreach (var item in inventoryResponse.Data.OrderBy(x => x.Ubicacion).ThenBy(x => x.PartNumber))
                    InventoryItems.Add(item);

                InventoryStatus = InventoryItems.Count > 0
                    ? $"{InventoryItems.Count} registro(s) de inventario encontrados."
                    : "No se encontró inventario para este StandardId.";
            }
            else
            {
                InventoryStatus = inventoryResponse.Message ?? "No se pudo cargar el inventario.";
            }

            var movementResponse = await _inventoryMovementService.GetInventoryMovements(standardId.Value);
            if (movementResponse.IsSuccess && movementResponse.Data != null)
            {
                foreach (var movement in movementResponse.Data.OrderByDescending(x => x.MovementId))
                    Movements.Add(movement);

                MovementStatus = Movements.Count > 0
                    ? $"{Movements.Count} movimiento(s) encontrados."
                    : "No se encontraron movimientos para este StandardId.";
            }
            else
            {
                MovementStatus = movementResponse.Message ?? "No se pudieron cargar los movimientos.";
            }
        }
        catch (Exception ex)
        {
            InventoryStatus = "Ocurrió un error al cargar la consulta.";
            MovementStatus = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task<int?> ResolveStandardIdAsync(string? standardIdValue)
    {
        var value = standardIdValue?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (int.TryParse(value, out var parsedStandardId) && parsedStandardId > 0)
            return parsedStandardId;

        var labelResponse = await _standardLabelService.GetByCode(value);
        if (!labelResponse.IsSuccess || labelResponse.Data is null || labelResponse.Data.StandarId <= 0)
        {
            InventoryStatus = labelResponse.Message ?? "No se encontró la etiqueta StandardId capturada.";
            return null;
        }

        return labelResponse.Data.StandarId;
    }
}
