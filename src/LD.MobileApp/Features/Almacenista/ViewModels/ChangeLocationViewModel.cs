using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;

namespace MauiAppLogin.ViewModels;

public partial class ChangeLocationViewModel : ObservableObject
{
    private readonly AvailableInventoryService _availableInventoryService;
    private readonly StandardLabelService _standardLabelService;
    private readonly AsnService _asnService;

    [ObservableProperty]
    private string estandarId = string.Empty;

    [ObservableProperty]
    private string rack = string.Empty;

    [ObservableProperty]
    private string posicion = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    private string instructionText = string.Empty;

    public int AsnId { get; set; }

    public bool HasInstructionText => !string.IsNullOrWhiteSpace(InstructionText);

    public string InstructionText
    {
        get => instructionText;
        set
        {
            if (SetProperty(ref instructionText, value))
                OnPropertyChanged(nameof(HasInstructionText));
        }
    }

    public ChangeLocationViewModel(
        AvailableInventoryService availableInventoryService,
        StandardLabelService standardLabelService,
        AsnService asnService)
    {
        _availableInventoryService = availableInventoryService;
        _standardLabelService = standardLabelService;
        _asnService = asnService;
    }

    public async Task OnSiguienteClicked()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(EstandarId))
        {
            await Shell.Current.DisplayAlertAsync("Standard ID requerido", "Captura un Standard ID valido.", "OK");
            return;
        }

        var rackValue = Rack?.Trim() ?? string.Empty;
        var posicionValue = Posicion?.Trim() ?? string.Empty;
        var ubicacionDestino = $"{rackValue}{posicionValue}";

        if (string.IsNullOrWhiteSpace(rackValue) || string.IsNullOrWhiteSpace(posicionValue))
        {
            await Shell.Current.DisplayAlertAsync("Ubicacion requerida", "Captura rack y posicion.", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            var standardId = await ResolveStandardIdAsync(EstandarId);
            if (!standardId.HasValue)
                return;

            var response = AsnId > 0
                ? await _asnService.LocateAsnPallet(AsnId, standardId.Value, ubicacionDestino)
                : await _availableInventoryService.ChangeLocation(standardId.Value, ubicacionDestino);

            if (!response.IsSuccess)
            {
                await Shell.Current.DisplayAlertAsync("Error", response.Message ?? "No se pudo cambiar la ubicacion.", "OK");
                return;
            }

            await Shell.Current.DisplayAlertAsync("Exito", response.Message ?? "Ubicacion actualizada correctamente.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task<int?> ResolveStandardIdAsync(string? standardIdValue)
    {
        var value = standardIdValue?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (int.TryParse(value, out var internalStandardId) && internalStandardId > 0)
            return internalStandardId;

        var labelResponse = await _standardLabelService.GetByCode(value);
        if (!labelResponse.IsSuccess || labelResponse.Data is null || labelResponse.Data.StandarId <= 0)
        {
            await Shell.Current.DisplayAlertAsync(
                "Etiqueta LD no encontrada",
                labelResponse.Message ?? "No se encontro la etiqueta LD capturada.",
                "OK");
            return null;
        }

        return labelResponse.Data.StandarId;
    }
}
