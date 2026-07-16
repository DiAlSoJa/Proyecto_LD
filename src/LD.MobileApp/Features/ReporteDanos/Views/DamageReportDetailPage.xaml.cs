using LD.Client.Services;
using LD.Client.Configuration;
using LD.Contracts.AvailableInventory;
using LD.Contracts.DamageReports;
using LD.Contracts.Requests;
using MauiAppLogin.Controls;
using Microsoft.Maui.Graphics.Platform;
using System.Text;

namespace MauiAppLogin;

[QueryProperty(nameof(StandardId), "standardId")]
public partial class DamageReportDetailPage : ContentPage
{
    private const float DamagePhotoMaxSize = 1920f;
    private const float DamagePhotoQuality = 0.86f;

    private readonly DamageReportService _damageReportService;
    private readonly AvailableInventoryService _availableInventoryService;
    private readonly StandardLabelService _standardLabelService;
    private readonly IDialogService _dialogService;
    private ImageSource? _foto1;
    private ImageSource? _foto2;
    private ImageSource? _foto3;
    private ImageSource? _foto4;
    private string? _foto1Path;
    private string? _foto2Path;
    private string? _foto3Path;
    private string? _foto4Path;
    private AvailableInventoryDto? _selectedInventory;
    private bool _loaded;
    private bool _isLoading;
    private string _loadingMessage = "Cargando datos...";
    private string _standardId = string.Empty;

    public DamageReportDetailPage(
        DamageReportService damageReportService,
        AvailableInventoryService availableInventoryService,
        StandardLabelService standardLabelService,
        IDialogService dialogService)
    {
        InitializeComponent();
        _damageReportService = damageReportService;
        _availableInventoryService = availableInventoryService;
        _standardLabelService = standardLabelService;
        _dialogService = dialogService;
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

    public string LoadingMessage
    {
        get => _loadingMessage;
        set
        {
            _loadingMessage = value;
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
            await _dialogService.ShowErrorAsync("StandardId invalido", "No se encontro la etiqueta StandardId capturada.");
            await Shell.Current.GoToAsync("..");
            return;
        }

        try
        {
            LoadingMessage = "Cargando datos...";
            IsLoading = true;
            var response = await _availableInventoryService.GetAvailableInventories(resolvedStandardId.Value);
            if (!response.IsSuccess || response.Data == null || response.Data.Count == 0)
            {
                await _dialogService.ShowErrorAsync("Inventario", response.Message ?? "No se encontro inventario para este StandardId.");
                await Shell.Current.GoToAsync("..");
                return;
            }

            _selectedInventory = response.Data
                .OrderBy(x => x.Ubicacion)
                .ThenBy(x => x.PartNumber)
                .First();

            ApplyInventory(_selectedInventory);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
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
                await _dialogService.ShowInfoAsync("Camara", "Este dispositivo no soporta captura de fotos.");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null) return;

            var photoPath = await CompressPhotoAsync(photo);
            await using var stream = File.OpenRead(photoPath);
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            mem.Position = 0;

            var img = ImageSource.FromStream(() => new MemoryStream(mem.ToArray()));
            PreviewImage.Source = img;

            if (_foto1 == null)
            {
                _foto1 = img;
                _foto1Path = photoPath;
                Thumb1.Source = _foto1;
            }
            else if (_foto2 == null)
            {
                _foto2 = img;
                _foto2Path = photoPath;
                Thumb2.Source = _foto2;
            }
            else if (_foto3 == null)
            {
                _foto3 = img;
                _foto3Path = photoPath;
                Thumb3.Source = _foto3;
            }
            else
            {
                _foto4 = img;
                _foto4Path = photoPath;
                Thumb4.Source = _foto4;
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
    }

    private static async Task<string> CompressPhotoAsync(FileResult photo)
    {
        try
        {
            await using var input = await photo.OpenReadAsync();
            using var image = PlatformImage.FromStream(input);
            if (image is null)
                return photo.FullPath;

            using var resized = image.Downsize(DamagePhotoMaxSize);
            var outputPath = Path.Combine(
                FileSystem.CacheDirectory,
                $"damage-report-{Guid.NewGuid():N}.jpg");

            await using var output = File.Create(outputPath);
            resized.Save(output, ImageFormat.Jpeg, DamagePhotoQuality);

            return outputPath;
        }
        catch
        {
            return photo.FullPath;
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        PreviewImage.Source = null;
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        if (_selectedInventory is null)
        {
            await _dialogService.ShowInfoAsync("Inventario", "Primero carga un StandardId valido.");
            return;
        }

        if (EstadoPicker.SelectedItem is null)
        {
            await _dialogService.ShowInfoAsync("Tipo de dano", "Selecciona el tipo de dano.");
            return;
        }

        if (EstadoPickewr.SelectedItem is null)
        {
            await _dialogService.ShowInfoAsync("Categoria", "Selecciona la categoria.");
            return;
        }

        if (EstadoPickewsr.SelectedItem is null)
        {
            await _dialogService.ShowInfoAsync("Nuevo estatus", "Selecciona el nuevo estatus.");
            return;
        }

        try
        {
            LoadingMessage = "Guardando reporte...";
            IsLoading = true;
            var request = BuildDamageReportRequest(_selectedInventory);
            await UploadPhotosAsync(request);

            var response = await _damageReportService.CreateDamageReport(request);
            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Reporte de danos", response.Message ?? "No se pudo guardar el reporte.");
                return;
            }

            await Shell.Current.GoToAsync($"../{nameof(DamageReportPrintPage)}", new Dictionary<string, object>
            {
                ["report"] = request,
                ["photo1Path"] = _foto1Path ?? string.Empty,
                ["photo2Path"] = _foto2Path ?? string.Empty,
                ["photo3Path"] = _foto3Path ?? string.Empty,
                ["photo4Path"] = _foto4Path ?? string.Empty
            });
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async Task UploadPhotosAsync(DamageReportRequest request)
    {
        request.Photo1Path = await UploadPhotoAsync(_foto1Path, 1);
        request.Photo2Path = await UploadPhotoAsync(_foto2Path, 2);
        request.Photo3Path = await UploadPhotoAsync(_foto3Path, 3);
        request.Photo4Path = await UploadPhotoAsync(_foto4Path, 4);
    }

    private async Task<string?> UploadPhotoAsync(string? photoPath, int photoNumber)
    {
        if (string.IsNullOrWhiteSpace(photoPath) || !File.Exists(photoPath))
            return null;

        var response = await _damageReportService.UploadImage(photoPath, photoNumber);
        if (!response.IsSuccess || response.Data is null || string.IsNullOrWhiteSpace(response.Data.RelativePath))
            throw new InvalidOperationException(response.Message ?? $"No se pudo cargar la foto {photoNumber}.");

        return response.Data.RelativePath;
    }

    private DamageReportRequest BuildDamageReportRequest(AvailableInventoryDto inventory)
    {
        var reportDate = DateTime.Now;

        return new DamageReportRequest
        {
            AvailableInventoryId = inventory.AvailableInventoryId,
            StandardId = inventory.StandardId,
            StandardIdCode = FirstNotEmpty(inventory.StandardIdStr, StandardId),
            ProductId = inventory.ProductId,
            ClientId = inventory.ClientId,
            ProjectId = inventory.ProjectId,
            WarehouseId = inventory.WarehouseId,
            LocationId = inventory.LocationId,
            PartNumber = inventory.PartNumber,
            Description = inventory.Description,
            Location = inventory.Ubicacion,
            CurrentStatus = inventory.StatusId,
            ReceivedQuantity = inventory.Qty,
            AvailableQuantity = inventory.Qty,
            Warehouse = inventory.Almacen,
            Project = inventory.Proyecto,
            Client = inventory.Cliente,
            Asn = FirstNotEmpty(inventory.DocumentId, inventory.Reference),
            ReceptionDate = inventory.Fecha == default ? null : inventory.Fecha,
            InventoryState = EstadoText.Replace("Estado:", string.Empty).Trim(),
            DamageType = EstadoPicker.SelectedItem?.ToString() ?? string.Empty,
            Category = EstadoPickewr.SelectedItem?.ToString() ?? string.Empty,
            NewStatus = EstadoPickewsr.SelectedItem?.ToString() ?? string.Empty,
            DamageReportCode = DamageReportCodeGenerator.Generate(reportDate),
            ReportedByUserId = UserData.Id,
            ReportedByName = FirstNotEmpty(UserData.Name, UserData.UserName),
            Comments = LicenciaEntry.Text,
            Photo1Path = _foto1Path,
            Photo2Path = _foto2Path,
            Photo3Path = _foto3Path,
            Photo4Path = _foto4Path,
            ReportDate = reportDate
        };
    }
}
