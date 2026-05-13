using LD.Client.Services;

namespace MauiAppLogin;

public partial class DamageReportPage : ContentPage
{
    private readonly AvailableInventoryService _availableInventoryService;
    private readonly StandardLabelService _standardLabelService;
    private bool _isLoading;
    private string _loadingMessage = "Buscando etiqueta...";

    public DamageReportPage(
        AvailableInventoryService availableInventoryService,
        StandardLabelService standardLabelService)
    {
        InitializeComponent();
        _availableInventoryService = availableInventoryService;
        _standardLabelService = standardLabelService;
        BindingContext = this;
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

    public string LoadingMessage
    {
        get => _loadingMessage;
        set
        {
            _loadingMessage = value;
            OnPropertyChanged();
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        PreviewImage.Source = null;
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        var standardId = EstandarIdEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(standardId))
        {
            await DisplayAlert("StandardId requerido", "Ingresa o captura un StandardId para continuar.", "OK");
            return;
        }

        try
        {
            LoadingMessage = "Buscando etiqueta...";
            IsLoading = true;

            var resolvedStandardId = await ResolveStandardIdAsync(standardId);
            if (!resolvedStandardId.HasValue)
            {
                await DisplayAlertAsync("Etiqueta no encontrada", "No se encontro la etiqueta capturada. Ingresa o escanea una etiqueta valida.", "OK");
                return;
            }

            var inventoryResponse = await _availableInventoryService.GetAvailableInventories(resolvedStandardId.Value);
            if (!inventoryResponse.IsSuccess || inventoryResponse.Data == null || inventoryResponse.Data.Count == 0)
            {
                await DisplayAlertAsync("Inventario no encontrado", inventoryResponse.Message ?? "No se encontro inventario para esta etiqueta.", "OK");
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(DamageReportDetailPage)}?standardId={Uri.EscapeDataString(standardId)}");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task<int?> ResolveStandardIdAsync(string standardIdValue)
    {
        var value = standardIdValue.Trim();

        if (int.TryParse(value, out var parsedStandardId) && parsedStandardId > 0)
            return parsedStandardId;

        var labelResponse = await _standardLabelService.GetByCode(value);
        if (!labelResponse.IsSuccess || labelResponse.Data is null || labelResponse.Data.StandarId <= 0)
            return null;

        return labelResponse.Data.StandarId;
    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        var page = new Scan3FieldsPage(requiresThreeFields: false);
        await Navigation.PushModalAsync(page);

        var accepted = await page.WaitForResultAsync();
        if (!accepted)
            return;

        if (!string.IsNullOrWhiteSpace(page.EstandarId))
            EstandarIdEntry.Text = page.EstandarId;
    }
}
