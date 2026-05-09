using LD.Client.Services;
using LD.Contracts.AvailableInventory;

namespace MauiAppLogin;

[QueryProperty(nameof(StandardId), "standardId")]
public partial class DamageReportDetailPage : ContentPage
{
    private readonly AvailableInventoryService _availableInventoryService;
    private readonly StandardLabelService _standardLabelService;
    private ImageSource? _foto1;
    private ImageSource? _foto2;
    private bool _loaded;
    private bool _isLoading;
    private string _standardId = string.Empty;

    public DamageReportDetailPage(
        AvailableInventoryService availableInventoryService,
        StandardLabelService standardLabelService)
    {
        InitializeComponent();
        _availableInventoryService = availableInventoryService;
        _standardLabelService = standardLabelService;
        BindingContext = this;
    }

    public string StandardId
    {
        get => _standardId;
        set
        {
            _standardId = Uri.UnescapeDataString(value ?? string.Empty);
            OnPropertyChanged();
            OnPropertyChanged(nameof(StandardIdText));
            _loaded = false;
        }
    }

    public string StandardIdText { get; private set; } = "Estandar ID: 20260201000001";
    public string PartNumberText { get; private set; } = "Numero de Parte: LX522250 -S2DS22DS2";
    public string UbicacionText { get; private set; } = "Ubicacion: A00253";
    public string StatusText { get; private set; } = "Status: A";
    public string CantidadRecibidaText { get; private set; } = "Cantidad Recibida: 52";
    public string AlmacenText { get; private set; } = "Almacen: B1";
    public string ProyectoText { get; private set; } = "Proyecto: JABIL";
    public string ClienteText { get; private set; } = "Cliente: JABIL";
    public string DescripcionText { get; private set; } = "Descripcion: SP-1-0225-2201";
    public string AsnText { get; private set; } = "ASN: ASN5522255";
    public string FechaRecepcionText { get; private set; } = "Fecha de Recepcion: 01-01-2026";
    public string DisponibleText { get; private set; } = "Disponible: 20";
    public string EstadoText { get; private set; } = "Estado: OK";

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
        await LoadInventoryAsync();
    }

    private async Task LoadInventoryAsync()
    {
        var resolvedStandardId = await ResolveStandardIdAsync(StandardId);
        if (!resolvedStandardId.HasValue)
        {
            await DisplayAlertAsync("StandardId invalido", "No se encontro la etiqueta StandardId capturada.", "OK");
            return;
        }

        try
        {
            IsLoading = true;
            var response = await _availableInventoryService.GetAvailableInventories(resolvedStandardId.Value);
            if (!response.IsSuccess || response.Data == null || response.Data.Count == 0)
            {
                await DisplayAlertAsync("Inventario", response.Message ?? "No se encontro inventario para este StandardId.", "OK");
                return;
            }

            ApplyInventory(response.Data
                .OrderBy(x => x.Ubicacion)
                .ThenBy(x => x.PartNumber)
                .First());
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

    private async Task<int?> ResolveStandardIdAsync(string? standardIdValue)
    {
        var value = standardIdValue?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (int.TryParse(value, out var parsedStandardId) && parsedStandardId > 0)
            return parsedStandardId;

        var labelResponse = await _standardLabelService.GetByCode(value);
        if (!labelResponse.IsSuccess || labelResponse.Data is null || labelResponse.Data.StandarId <= 0)
            return null;

        return labelResponse.Data.StandarId;
    }

    private void ApplyInventory(AvailableInventoryDto inventory)
    {
        StandardIdText = BuildText("Estandar ID", FirstNotEmpty(inventory.StandardIdStr, inventory.StandardId?.ToString()), "20260201000001");
        PartNumberText = BuildText("Numero de Parte", inventory.PartNumber, "LX522250 -S2DS22DS2");
        UbicacionText = BuildText("Ubicacion", inventory.Ubicacion, "A00253");
        StatusText = BuildText("Status", inventory.StatusId, "A");
        CantidadRecibidaText = BuildText("Cantidad Recibida", FormatDecimal(inventory.Qty), "52");
        AlmacenText = BuildText("Almacen", inventory.Almacen, "B1");
        ProyectoText = BuildText("Proyecto", inventory.Proyecto, "JABIL");
        ClienteText = BuildText("Cliente", inventory.Cliente, "JABIL");
        DescripcionText = BuildText("Descripcion", inventory.Description, "SP-1-0225-2201");
        AsnText = BuildText("ASN", FirstNotEmpty(inventory.DocumentId, inventory.Reference), "ASN5522255");
        FechaRecepcionText = BuildText("Fecha de Recepcion", FormatDate(inventory.Fecha), "01-01-2026");
        DisponibleText = BuildText("Disponible", FormatDecimal(inventory.Qty), "20");
        EstadoText = BuildText("Estado", "OK", "OK");

        OnPropertyChanged(nameof(StandardIdText));
        OnPropertyChanged(nameof(PartNumberText));
        OnPropertyChanged(nameof(UbicacionText));
        OnPropertyChanged(nameof(StatusText));
        OnPropertyChanged(nameof(CantidadRecibidaText));
        OnPropertyChanged(nameof(AlmacenText));
        OnPropertyChanged(nameof(ProyectoText));
        OnPropertyChanged(nameof(ClienteText));
        OnPropertyChanged(nameof(DescripcionText));
        OnPropertyChanged(nameof(AsnText));
        OnPropertyChanged(nameof(FechaRecepcionText));
        OnPropertyChanged(nameof(DisponibleText));
        OnPropertyChanged(nameof(EstadoText));
    }

    private static string BuildText(string label, string? value, string fallback)
    {
        return $"{label}: {FirstNotEmpty(value, fallback)}";
    }

    private static string FirstNotEmpty(params string?[] values)
    {
        return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim() ?? string.Empty;
    }

    private static string FormatDecimal(decimal? value)
    {
        return value.HasValue ? value.Value.ToString("0.##") : string.Empty;
    }

    private static string FormatDate(DateTime value)
    {
        return value == default ? string.Empty : value.ToString("dd-MM-yyyy");
    }

    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlertAsync("Camara", "Este dispositivo no soporta captura de fotos.", "OK");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null) return;

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            mem.Position = 0;

            var img = ImageSource.FromStream(() => new MemoryStream(mem.ToArray()));
            PreviewImage.Source = img;

            if (_foto1 == null)
            {
                _foto1 = img;
                Thumb1.Source = _foto1;
            }
            else
            {
                _foto2 = img;
                Thumb2.Source = _foto2;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        PreviewImage.Source = null;
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("DamageReportPrintPage");
    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

}
