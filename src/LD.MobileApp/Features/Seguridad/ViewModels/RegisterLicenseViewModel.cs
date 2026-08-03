using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Contracts.Enums;
using MauiAppLogin.Common.Imaging;
using MauiAppLogin.Features.Seguridad.Ocr;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MauiAppLogin.Views.Controls;
using MvvmHelpers.Commands;
using Plugin.Maui.OCR;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace MauiAppLogin.ViewModels;

[QueryProperty(nameof(Tipo), "tipo")]
public partial class RegisterLicenseViewModel : ObservableObject
{
    private static readonly string[] NameLabels =
    {
        "NOMBRE",
        "NOMBRES",
        "NAME"
    };

    private static readonly string[] LicenseLabels =
    {
        "LICENCIA",
        "NUMERO DE LICENCIA",
        "NÚMERO DE LICENCIA",
        "NO DE LICENCIA",
        "NO. DE LICENCIA",
        "NRO DE LICENCIA",
        "NUM LICENCIA",
        "LIC."
    };

    private static readonly string[] DateFormats =
    {
        "d/M/yyyy",
        "dd/MM/yyyy",
        "d/M/yy",
        "dd/MM/yy",
        "yyyy/M/d",
        "yyyy/MM/dd",
        "yyyy/M/dd",
        "yyyy/MM/d"
    };

    private readonly SecurityRegistrationContext _context;
    private readonly ILoaderService _loaderService;
    private readonly IDialogService _dialogService;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private string tipo = "";

    [ObservableProperty]
    private string warehouseName = "";

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string licencia = "";

    private DateTime? vencimiento;

    [ObservableProperty]
    private string vencimientoTexto = "";

    private void SetVencimiento(DateTime? value)
    {
        vencimiento = value;
        VencimientoTexto = value?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty;
    }

    [ObservableProperty]
    private string celular = "";

    [ObservableProperty]
    private ImageSource? previewImage;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string photoInstruction = "Toma primero la foto del frente de la licencia.";

    [ObservableProperty]
    private bool canCapturePhoto = true;

    private bool _captureFlowStarted;
    private bool _captureFlowActive;

    public ObservableCollection<LicensePhotoItem> Photos { get; } = new();

    public ICommand CapturarCommand { get; }
    public ICommand GaleriaCommand { get; }
    public ICommand CancelarCommand { get; }
    public ICommand SiguienteCommand { get; }
    public ICommand AtrasCommand { get; }
    public ICommand RemovePhotoCommand { get; }
    public ICommand ViewPhotoCommand { get; }

    public RegisterLicenseViewModel(
        SecurityRegistrationContext context,
        ILoaderService loaderService,
        IDialogService dialogService)
    {
        _context       = context;
        _loaderService = loaderService;
        _dialogService = dialogService;

        CapturarCommand       = new AsyncCommand(CapturarFotosPendientesAsync);
        GaleriaCommand        = new AsyncCommand(SeleccionarGaleriaAsync);
        CancelarCommand       = new Command(Cancelar);
        SiguienteCommand      = new AsyncCommand(SiguienteAsync);
        AtrasCommand          = new AsyncCommand(AtrasAsync);
        RemovePhotoCommand    = new MvvmHelpers.Commands.Command<LicensePhotoItem>(RemovePhoto);
        ViewPhotoCommand      = new MvvmHelpers.Commands.Command<LicensePhotoItem>(ViewPhoto);

        LoadFromContext();
    }

    public async Task StartCaptureFlowAsync()
    {
        if (_captureFlowStarted)
            return;

        _captureFlowStarted = true;

        if (GetNextRequiredSide().HasValue)
            await CapturarFotosPendientesAsync();
    }

    private void LoadFromContext()
    {
        WarehouseName = _context.WarehouseName;

        if (!string.IsNullOrEmpty(_context.Nombre))
            Nombre = _context.Nombre;

        if (!string.IsNullOrEmpty(_context.Licencia))
            Licencia = _context.Licencia;

        if (_context.Vencimiento.HasValue)
            SetVencimiento(_context.Vencimiento);
        else
            VencimientoTexto = string.Empty;

        if (!string.IsNullOrEmpty(_context.Celular))
            Celular = _context.Celular;

        Photos.Clear();

        foreach (var entry in _context.LicenciaFotos.OrderBy(x => x.Orden))
        {
            var side = ResolvePhotoSide(entry);
            Photos.Add(CreatePhotoItem(entry.Bytes, entry.Orden, side));
        }

        OrderPhotos();
        UpdatePhotoInstruction();
    }

    private void SaveToContext()
    {
        OrderPhotos();

        _context.Tipo        = Tipo;
        _context.Nombre      = Nombre;
        _context.Licencia    = Licencia;
        _context.Vencimiento = vencimiento;
        _context.Celular     = Celular;

        _context.LicenciaFotos = Photos.Select(p => new SecurityPhotoEntry
        {
            Categoria = PhotoCategoria_e.Licencia,
            Orden     = p.Orden,
            Side      = p.Side,
            Bytes     = p.Bytes,
        }).ToList();
    }

    private async Task CapturarFotosPendientesAsync()
    {
        if (_captureFlowActive)
            return;

        _captureFlowActive = true;

        try
        {
            while (GetNextRequiredSide() is { } side)
            {
                await _dialogService.ShowInfoAsync(
                    side == LicensePhotoSide.Front
                        ? "Licencia - Foto 1 de 2: FRENTE"
                        : "Licencia - Foto 2 de 2: ATRÁS",
                    side == LicensePhotoSide.Front
                        ? "Coloca el frente completo de la licencia dentro de la cámara."
                        : "Ahora coloca la parte de atrás completa de la licencia dentro de la cámara.");

                if (!await CapturarAsync(side))
                    break;
            }
        }
        finally
        {
            _captureFlowActive = false;
        }
    }

    private async Task<bool> CapturarAsync(LicensePhotoSide side)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await _dialogService.ShowInfoAsync("Cámara", "Este dispositivo no soporta captura de fotos.");
                return false;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null)
                return false;

            await ProcesarFotoAsync(photo, side);
            return Photos.Any(p => p.Side == side);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            return false;
        }
    }

    private async Task SeleccionarGaleriaAsync()
    {
        try
        {
            var side = GetNextRequiredSide();
            if (side is null)
            {
                await _dialogService.ShowInfoAsync(
                    "Licencia",
                    "Ya tienes las fotos del frente y atrás. Para reemplazar una, elimínala de la galería y vuelve a tomarla.");
                return;
            }

            var photos = await MediaPicker.Default.PickPhotosAsync(new MediaPickerOptions
            {
                Title = "Selecciona una imagen de la licencia"
            });
            var photo = photos?.FirstOrDefault();

            if (photo is null)
                return;

            await ProcesarFotoAsync(photo, side.Value);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async Task ProcesarFotoAsync(FileResult photo, LicensePhotoSide side)
    {
        try
        {
            _loaderService.Show(side == LicensePhotoSide.Front
                ? "Procesando foto del frente..."
                : "Procesando foto de atrás...");

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            var bytes = mem.ToArray();

            var ocrData = await LicenseOcrReader.ReadAsync(bytes);

            if (side == LicensePhotoSide.Front && ocrData.HasAny)
            {
                if (!string.IsNullOrWhiteSpace(ocrData.Nombre))
                    Nombre = ocrData.Nombre;

                if (!string.IsNullOrWhiteSpace(ocrData.Licencia))
                    Licencia = NormalizeLicenseValue(ocrData.Licencia);

                if (ocrData.Vencimiento.HasValue)
                    SetVencimiento(ocrData.Vencimiento.Value);
            }

            var bytesFinales = await ComprimirImagenAsync(bytes);
            var img = ImageSource.FromStream(() => new MemoryStream(bytesFinales));

            var updatedItem = new LicensePhotoItem
            {
                Bytes  = bytesFinales,
                Orden  = Photos.Count(p => p.Side == side),
                Side   = side,
                Title  = GetPhotoTitle(side),
                Source = img,
            };

            UpsertPhotoItem(updatedItem);
            UpdatePhotoInstruction();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private static async Task<byte[]> ComprimirImagenAsync(byte[] bytes)
    {
        try
        {
            return await ImageCompressor.ComprimirAsync(bytes);
        }
        catch
        {
            return bytes;
        }
    }

    private void ParseLicense(string text)
    {
        var normalizedText = text.Replace("\r", "\n");
        var lines = normalizedText.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var nombre = TryExtractValueAfterLabel(normalizedText, NameLabels, @"[^\r\n\d]{3,80}");
        if (string.IsNullOrWhiteSpace(nombre))
            nombre = TryExtractNameFromLines(lines);

        if (!string.IsNullOrWhiteSpace(nombre))
            Nombre = nombre;

        var licencia = TryExtractValueAfterLabel(normalizedText, LicenseLabels, @"[A-Z0-9-]{4,20}");
        if (string.IsNullOrWhiteSpace(licencia))
            licencia = TryExtractLicenseFallback(normalizedText);

        if (!string.IsNullOrWhiteSpace(licencia))
            Licencia = NormalizeLicenseValue(licencia);

        if (TryExtractExpiration(normalizedText, lines, out var parsedVencimiento))
            SetVencimiento(parsedVencimiento);
    }

    private void RemovePhoto(LicensePhotoItem? item)
    {
        if (item is null)
            return;

        Photos.Remove(item);
        OrderPhotos();
        UpdatePhotoInstruction();
    }

    private void ViewPhoto(LicensePhotoItem? item)
    {
        if (item?.Source is null)
            return;

        (Shell.Current.CurrentPage ?? Application.Current?.MainPage)
            ?.ShowPopup(new ImagePreviewPopup(item.Source));
    }

    private void Cancelar()
    {
        PreviewImage = null;
    }

    private async Task SiguienteAsync()
    {
        var hasFront = Photos.Any(p => p.Side == LicensePhotoSide.Front);
        var hasBack = Photos.Any(p => p.Side == LicensePhotoSide.Back);

        if (!_context.WarehouseId.HasValue || _context.WarehouseId.Value <= 0)
        {
            await _dialogService.ShowInfoAsync("Atención", "Selecciona un almacén antes de continuar.");
            return;
        }

        if (string.IsNullOrEmpty(Tipo) ||
            string.IsNullOrEmpty(Licencia) ||
            string.IsNullOrEmpty(Nombre) ||
            string.IsNullOrEmpty(Celular))
        {
            await _dialogService.ShowInfoAsync("Atención", "Llene todos los campos, por favor.");
            return;
        }

        if (!TryParseOcrDate(VencimientoTexto, out var parsedVencimiento))
        {
            await _dialogService.ShowInfoAsync("AtenciÃ³n", "Selecciona el vencimiento de la licencia.");
            return;
        }

        SetVencimiento(parsedVencimiento);

        if (!hasFront || !hasBack)
        {
            await _dialogService.ShowInfoAsync("Atención", "Agregue una foto del frente y otra de atrás de la licencia.");
            return;
        }

        SaveToContext();
        await Shell.Current.GoToAsync(nameof(RegisterVehicule));
    }

    private async Task AtrasAsync()
    {
        _context.Clear();
        await Shell.Current.GoToAsync("..");
    }

    private void OrderPhotos()
    {
        var ordered = Photos
            .OrderBy(p => p.Side)
            .ThenBy(p => p.Orden)
            .ToList();

        Photos.Clear();

        for (var i = 0; i < ordered.Count; i++)
        {
            ordered[i].Orden = i;
            Photos.Add(ordered[i]);
        }
    }

    private void UpsertPhotoItem(LicensePhotoItem item)
    {
        var existingIndex = -1;

        for (var i = 0; i < Photos.Count; i++)
        {
            if (Photos[i].Side == item.Side)
            {
                existingIndex = i;
                break;
            }
        }

        if (existingIndex >= 0)
            Photos[existingIndex] = item;
        else
            Photos.Add(item);

        OrderPhotos();
    }

    private void UpdatePhotoInstruction()
    {
        var hasFront = Photos.Any(p => p.Side == LicensePhotoSide.Front);
        var hasBack = Photos.Any(p => p.Side == LicensePhotoSide.Back);

        PhotoInstruction = (hasFront, hasBack) switch
        {
            (false, false) => "Toma primero la foto del frente de la licencia.",
            (true, false) => "Ahora toma la foto de atrás de la licencia.",
            (false, true) => "Falta la foto del frente de la licencia.",
            _ => "Ya tienes las fotos del frente y atrás de la licencia."
        };

        var nextSide = GetNextRequiredSide();
        CanCapturePhoto = nextSide.HasValue;
    }

    private LicensePhotoSide? GetNextRequiredSide()
    {
        var hasFront = Photos.Any(p => p.Side == LicensePhotoSide.Front);
        if (!hasFront)
            return LicensePhotoSide.Front;

        var hasBack = Photos.Any(p => p.Side == LicensePhotoSide.Back);
        if (!hasBack)
            return LicensePhotoSide.Back;

        return null;
    }

    private static LicensePhotoSide ResolvePhotoSide(SecurityPhotoEntry entry)
    {
        if (entry.Side.HasValue)
            return entry.Side.Value;

        return entry.Orden <= 0 ? LicensePhotoSide.Front : LicensePhotoSide.Back;
    }

    private static LicensePhotoItem CreatePhotoItem(byte[] bytes, int orden, LicensePhotoSide side)
    {
        return new LicensePhotoItem
        {
            Bytes = bytes,
            Orden = orden,
            Side = side,
            Title = GetPhotoTitle(side),
            Source = ImageSource.FromStream(() => new MemoryStream(bytes)),
        };
    }

    private static string GetPhotoTitle(LicensePhotoSide side)
    {
        return side == LicensePhotoSide.Front
            ? "Licencia frente"
            : "Licencia atrás";
    }

    private static string? TryExtractValueAfterLabel(
        string text,
        IEnumerable<string> labels,
        string valuePattern)
    {
        var labelList = labels.ToList();
        var lines = text.Replace("\r", "\n").Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            foreach (var label in labelList)
            {
                var labelIndex = line.IndexOf(label, StringComparison.OrdinalIgnoreCase);
                if (labelIndex < 0)
                    continue;

                var trailingText = line[(labelIndex + label.Length)..];
                var sameLineValue = ExtractFirstMatch(trailingText, valuePattern);
                if (!string.IsNullOrWhiteSpace(sameLineValue))
                    return sameLineValue;

                if (i + 1 < lines.Length)
                {
                    var nextLineValue = ExtractFirstMatch(lines[i + 1], valuePattern);
                    if (!string.IsNullOrWhiteSpace(nextLineValue))
                        return nextLineValue;
                }
            }
        }

        return null;
    }

    private static string? TryExtractNameFromLines(IEnumerable<string> lines)
    {
        foreach (var rawLine in lines)
        {
            var line = CleanOcrValue(rawLine);
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.Length is < 8 or > 80)
                continue;

            if (line.Any(char.IsDigit))
                continue;

            if (Regex.IsMatch(
                    line,
                    @"\b(?:NOMBRE|LICENCIA|VENC|VIGENCIA|CADUC|EXPIRA|EXPIRACION|RFC|CURP|DOMICILIO|FECHA|MEXICO|ESTADO|FOLIO)\b",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            {
                continue;
            }

            if (Regex.IsMatch(
                    line,
                    @"^(?:\p{L}{2,}\s+){1,5}\p{L}{2,}$",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            {
                return line;
            }
        }

        return null;
    }

    private static string? TryExtractLicenseFallback(string text)
    {
        var candidates = Regex.Matches(text, @"\b[A-Z0-9-]{6,20}\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)
            .Select(m => CleanOcrValue(m.Value))
            .Where(value => value.Length is >= 6 and <= 20)
            .Where(value => !Regex.IsMatch(value, @"^\d{1,2}[\/\.-]\d{1,2}[\/\.-]\d{2,4}$"))
            .Where(value => !Regex.IsMatch(value, @"^(?:NOMBRE|LICENCIA|VENC|VIGENCIA|CADUC|EXPIRA|RFC|CURP|DOMICILIO|FECHA|FOLIO)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            .OrderByDescending(value => value.Length)
            .ToList();

        return candidates.FirstOrDefault();
    }

    private static bool TryExtractExpiration(
        string text,
        string[] lines,
        out DateTime vencimiento)
    {
        const string datePattern = @"(?:\d{1,2}[\/\.\-]\d{1,2}[\/\.\-]\d{2,4}|\d{4}[\/\.\-]\d{1,2}[\/\.\-]\d{1,2})";
        const string labelPattern = @"(?:VENC(?:E|IMIENTO)?|VIGENCIA|CADUC(?:A|IDAD)?|EXP(?:IRA|IRACION|IRACIÓN)?|VALIDA(?: HASTA)?)";

        var labelledDate = Regex.Match(
            text,
            $@"{labelPattern}\s*[:#\-]?\s*(?<date>{datePattern})",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        if (labelledDate.Success && TryParseOcrDate(labelledDate.Groups["date"].Value, out vencimiento))
            return true;

        for (var i = 0; i < lines.Length; i++)
        {
            if (!Regex.IsMatch(lines[i], labelPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                continue;

            var nearbyText = lines[i];
            if (i + 1 < lines.Length)
                nearbyText += " " + lines[i + 1];

            var nearbyDate = Regex.Match(nearbyText, datePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (nearbyDate.Success && TryParseOcrDate(nearbyDate.Value, out vencimiento))
                return true;
        }

        var allDates = Regex.Matches(text, datePattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (allDates.Count == 1 && TryParseOcrDate(allDates[0].Value, out vencimiento))
            return true;

        vencimiento = default;
        return false;
    }

    private static bool TryParseOcrDate(string value, out DateTime date)
    {
        var normalized = Regex.Replace(value.Trim(), @"[.\-]", "/");

        if (DateTime.TryParseExact(
                normalized,
                DateFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out date))
        {
            date = date.Date;
            return true;
        }

        if (DateTime.TryParse(
                normalized,
                new CultureInfo("es-MX"),
                DateTimeStyles.AllowWhiteSpaces,
                out date))
        {
            date = date.Date;
            return true;
        }

        date = default;
        return false;
    }

    private static string? ExtractFirstMatch(string value, string pattern)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var match = Regex.Match(
            value,
            $@"(?<value>{pattern})",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        return match.Success ? CleanOcrValue(match.Groups["value"].Value) : null;
    }

    private static string CleanOcrValue(string value)
    {
        return Regex.Replace(value, @"\s+", " ")
            .Trim(' ', ':', ';', ',', '.', '-', '|');
    }

    private static string NormalizeLicenseValue(string value)
    {
        return Regex.Replace(value, @"\s+", string.Empty).Trim().ToUpperInvariant();
    }
}

public class LicensePhotoItem
{
    public byte[] Bytes { get; set; } = Array.Empty<byte>();
    public int Orden { get; set; }
    public LicensePhotoSide Side { get; set; }
    public string Title { get; set; } = string.Empty;
    public ImageSource? Source { get; set; }
}
