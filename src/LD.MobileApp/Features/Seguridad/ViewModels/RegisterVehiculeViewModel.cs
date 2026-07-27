using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Contracts.TruckType;
using MauiAppLogin.Common.Imaging;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MauiAppLogin.Views.Controls;
using MvvmHelpers.Commands;
using Plugin.Maui.OCR;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace MauiAppLogin.ViewModels;

public partial class RegisterVehiculeViewModel : ObservableObject
{
    private static readonly VehiclePhotoSlot[] BasePhotoSlots =
    {
        VehiclePhotoSlot.NumeroVehiculo,
        VehiclePhotoSlot.PlacaVehiculo
    };

    private static readonly VehiclePhotoSlot[] BoxPhotoSlots =
    {
        VehiclePhotoSlot.NumeroCaja,
        VehiclePhotoSlot.PlacaCaja,
        VehiclePhotoSlot.Sello
    };

    private readonly SecurityRegistrationContext _context;
    private readonly ILoaderService _loaderService;
    private readonly IDialogService _dialogService;
    private readonly TruckTypeService _truckTypeService;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private string tipoVehiculo = string.Empty;

    [ObservableProperty]
    private bool tieneCaja;

    private TruckTypeDto? selectedTruckType;

    public TruckTypeDto? SelectedTruckType
    {
        get => selectedTruckType;
        set
        {
            if (!SetProperty(ref selectedTruckType, value))
                return;

            TipoVehiculo = value?.Name ?? string.Empty;
            TieneCaja = value?.TieneCaja ?? false;

            if (!TieneCaja)
            {
                ClearBoxFields();
                RemoveBoxPhotos();
            }

            RefreshPhotoGuidance();
        }
    }

    [ObservableProperty]
    private string linea = string.Empty;

    [ObservableProperty]
    private string origen = string.Empty;

    [ObservableProperty]
    private string numero = string.Empty;

    [ObservableProperty]
    private string placa = string.Empty;

    [ObservableProperty]
    private string numeroCaja = string.Empty;

    [ObservableProperty]
    private string placaCaja = string.Empty;

    [ObservableProperty]
    private string sello = string.Empty;

    [ObservableProperty]
    private string photoInstruction = "Toma la foto del numero del vehiculo.";

    [ObservableProperty]
    private string captureButtonText = "Tomar foto del numero del vehiculo";

    [ObservableProperty]
    private string photoStatusText = "0/2 fotos requeridas";

    [ObservableProperty]
    private ImageSource? previewImage;

    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<TruckTypeDto> TruckTypes { get; } = new();
    public ObservableCollection<VehiclePhotoItem> Photos { get; } = new();

    public ICommand CapturarCommand { get; }
    public ICommand GaleriaCommand { get; }
    public ICommand CancelarCommand { get; }
    public ICommand SiguienteCommand { get; }
    public ICommand AtrasCommand { get; }
    public ICommand RemovePhotoCommand { get; }
    public ICommand ViewPhotoCommand { get; }

    public RegisterVehiculeViewModel(
        SecurityRegistrationContext context,
        ILoaderService loaderService,
        IDialogService dialogService,
        TruckTypeService truckTypeService)
    {
        _context = context;
        _loaderService = loaderService;
        _dialogService = dialogService;
        _truckTypeService = truckTypeService;

        CapturarCommand = new AsyncCommand(CapturarAsync);
        GaleriaCommand = new AsyncCommand(SeleccionarGaleriaAsync);
        CancelarCommand = new Command(Cancelar);
        SiguienteCommand = new AsyncCommand(SiguienteAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);
        RemovePhotoCommand = new MvvmHelpers.Commands.Command<VehiclePhotoItem>(RemovePhoto);
        ViewPhotoCommand = new MvvmHelpers.Commands.Command<VehiclePhotoItem>(ViewPhoto);

        LoadFromContext();
        _ = LoadTruckTypesAsync();
    }

    private void LoadFromContext()
    {
        TipoVehiculo = _context.TipoVehiculo;
        TieneCaja = _context.TieneCaja;
        Linea = _context.Linea;
        Origen = _context.Origen;
        Numero = _context.Numero;
        Placa = _context.Placa;
        NumeroCaja = _context.NumeroCaja;
        PlacaCaja = _context.PlacaCaja;
        Sello = _context.Sello;

        Photos.Clear();
        foreach (var entry in _context.VehiculoFotos.OrderBy(x => x.Orden))
        {
            var slot = ResolvePhotoSlot(entry.Orden);
            if (slot is null)
                continue;

            Photos.Add(CreatePhotoItem(entry.Bytes, slot.Value));
        }

        OrderPhotos();
        RefreshPhotoGuidance();
    }

    private async Task LoadTruckTypesAsync()
    {
        try
        {
            var response = await _truckTypeService.GetTruckTypes();
            if (!response.IsSuccess || response.Data is null)
            {
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudieron cargar los tipos de camion.");
                return;
            }

            TruckTypes.Clear();
            foreach (var truckType in response.Data)
                TruckTypes.Add(truckType);

            ApplySelectedTruckType();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
    }

    private void ApplySelectedTruckType()
    {
        if (string.IsNullOrWhiteSpace(TipoVehiculo) || TruckTypes.Count == 0)
        {
            SelectedTruckType = null;
            return;
        }

        SelectedTruckType = TruckTypes.FirstOrDefault(x =>
            string.Equals(x.Name, TipoVehiculo, StringComparison.OrdinalIgnoreCase));
    }

    private void SaveToContext()
    {
        OrderPhotos();

        _context.TipoVehiculo = TipoVehiculo;
        _context.TieneCaja = TieneCaja;
        _context.Linea = Linea;
        _context.Origen = Origen;
        _context.Numero = Numero;
        _context.Placa = Placa;
        _context.NumeroCaja = TieneCaja ? NumeroCaja : string.Empty;
        _context.PlacaCaja = TieneCaja ? PlacaCaja : string.Empty;
        _context.Sello = TieneCaja ? Sello : string.Empty;

        var allowedSlots = GetRequiredSlots();
        _context.VehiculoFotos = Photos
            .Where(p => allowedSlots.Contains(p.Slot))
            .Select(p => new SecurityPhotoEntry
            {
                Categoria = PhotoCategoria_e.Vehiculo,
                Orden = (int)p.Slot,
                Bytes = p.Bytes,
            })
            .ToList();
    }

    private async Task CapturarAsync()
    {
        try
        {
            var slot = GetNextRequiredSlot();
            if (slot is null)
            {
                await _dialogService.ShowInfoAsync("Vehiculo", "Ya tienes las fotos requeridas. Si necesitas reemplazar una, elimina la foto anterior.");
                return;
            }

            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await _dialogService.ShowInfoAsync("Camara", "Este dispositivo no soporta captura de fotos.");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null)
                return;

            await ProcesarFotoAsync(photo, slot.Value);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async Task SeleccionarGaleriaAsync()
    {
        try
        {
            var slot = GetNextRequiredSlot();
            if (slot is null)
            {
                await _dialogService.ShowInfoAsync("Vehiculo", "Ya tienes las fotos requeridas. Si necesitas reemplazar una, elimina la foto anterior.");
                return;
            }

            var photos = await MediaPicker.Default.PickPhotosAsync(new MediaPickerOptions
            {
                Title = $"Selecciona una imagen para {GetPhotoTitle(slot.Value)}"
            });
            var photo = photos?.FirstOrDefault();

            if (photo is null)
                return;

            await ProcesarFotoAsync(photo, slot.Value);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async Task ProcesarFotoAsync(FileResult photo, VehiclePhotoSlot slot)
    {
        try
        {
            _loaderService.Show($"Procesando {GetPhotoTitle(slot).ToLowerInvariant()}...");

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            var bytes = mem.ToArray();

            var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(bytes);
            var text = ocrResult?.AllText ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(text))
                TryApplyOcrText(slot, text);

            var bytesFinales = await ComprimirImagenAsync(bytes);
            var img = ImageSource.FromStream(() => new MemoryStream(bytesFinales));

            UpsertPhotoItem(new VehiclePhotoItem
            {
                Slot = slot,
                Bytes = bytesFinales,
                Title = GetPhotoTitle(slot),
                Source = img,
            });

            RefreshPhotoGuidance();
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

    private void TryApplyOcrText(VehiclePhotoSlot slot, string text)
    {
        if (slot == VehiclePhotoSlot.PlacaVehiculo)
        {
            ParsePlaca(text, isBox: false);
        }
        else if (slot == VehiclePhotoSlot.PlacaCaja)
        {
            ParsePlaca(text, isBox: true);
        }
    }

    private void ParsePlaca(string text, bool isBox)
    {
        var currentValue = isBox ? PlacaCaja : Placa;
        if (!string.IsNullOrEmpty(currentValue))
            return;

        var match = Regex.Match(
            text,
            @"\b([A-Z]{2,3}[-\s]?\d{3,4}[-\s]?[A-Z]{0,2}|\d{3}[-\s]?[A-Z]{2,3})\b");

        if (!match.Success)
            return;

        var normalized = Regex.Replace(match.Value, @"\s", string.Empty).ToUpperInvariant();

        if (isBox)
            PlacaCaja = normalized;
        else
            Placa = normalized;
    }

    private void RemovePhoto(VehiclePhotoItem? item)
    {
        if (item is null)
            return;

        Photos.Remove(item);
        OrderPhotos();
        RefreshPhotoGuidance();
    }

    private void ViewPhoto(VehiclePhotoItem? item)
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
        var requiredSlots = GetRequiredSlots();
        var capturedSlots = Photos.Where(p => requiredSlots.Contains(p.Slot)).Select(p => p.Slot).ToHashSet();

        if (string.IsNullOrWhiteSpace(TipoVehiculo) ||
            string.IsNullOrWhiteSpace(Linea) ||
            string.IsNullOrWhiteSpace(Numero) ||
            string.IsNullOrWhiteSpace(Placa))
        {
            await _dialogService.ShowInfoAsync("Atencion", "Llene tipo, linea, numero y placa del vehiculo.");
            return;
        }

        if (TieneCaja &&
            (string.IsNullOrWhiteSpace(NumeroCaja) ||
             string.IsNullOrWhiteSpace(PlacaCaja) ||
             string.IsNullOrWhiteSpace(Sello)))
        {
            await _dialogService.ShowInfoAsync("Atencion", "Llene numero, placa y sello de la caja.");
            return;
        }

        if (capturedSlots.Count < requiredSlots.Count)
        {
            await _dialogService.ShowInfoAsync("Atencion", "Agregue todas las fotos requeridas del vehiculo.");
            return;
        }

        SaveToContext();
        await Shell.Current.GoToAsync(nameof(SignatureDriver));
    }

    private async Task AtrasAsync()
    {
        SaveToContext();
        await Shell.Current.GoToAsync("..");
    }

    private void OrderPhotos()
    {
        var ordered = Photos
            .OrderBy(p => p.Slot)
            .ToList();

        Photos.Clear();

        foreach (var item in ordered)
        {
            item.Title = GetPhotoTitle(item.Slot);
            Photos.Add(item);
        }
    }

    private void UpsertPhotoItem(VehiclePhotoItem item)
    {
        var existingIndex = -1;

        for (var i = 0; i < Photos.Count; i++)
        {
            if (Photos[i].Slot == item.Slot)
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

    private void RefreshPhotoGuidance()
    {
        var requiredSlots = GetRequiredSlots();
        var capturedCount = Photos.Count(p => requiredSlots.Contains(p.Slot));
        var nextSlot = GetNextRequiredSlot();

        PhotoStatusText = $"{capturedCount}/{requiredSlots.Count} fotos requeridas";

        if (nextSlot is null)
        {
            PhotoInstruction = TieneCaja
                ? "Ya capturaste las fotos requeridas del vehiculo y de la caja."
                : "Ya capturaste las fotos requeridas del vehiculo.";
            CaptureButtonText = "Reemplazar foto";
            return;
        }

        PhotoInstruction = GetInstructionForSlot(nextSlot.Value);
        CaptureButtonText = GetCaptureTextForSlot(nextSlot.Value);
    }

    private IReadOnlyList<VehiclePhotoSlot> GetRequiredSlots()
    {
        return TieneCaja
            ? BasePhotoSlots.Concat(BoxPhotoSlots).ToArray()
            : BasePhotoSlots;
    }

    private VehiclePhotoSlot? GetNextRequiredSlot()
    {
        foreach (var slot in GetRequiredSlots())
        {
            if (!Photos.Any(p => p.Slot == slot))
                return slot;
        }

        return null;
    }

    private VehiclePhotoSlot? ResolvePhotoSlot(int orden)
    {
        var requiredSlots = GetRequiredSlots();
        if (orden < 0 || orden >= requiredSlots.Count)
            return null;

        return requiredSlots[orden];
    }

    private static VehiclePhotoItem CreatePhotoItem(byte[] bytes, VehiclePhotoSlot slot)
    {
        return new VehiclePhotoItem
        {
            Slot = slot,
            Bytes = bytes,
            Title = GetPhotoTitle(slot),
            Source = ImageSource.FromStream(() => new MemoryStream(bytes)),
        };
    }

    private void RemoveBoxPhotos()
    {
        var boxPhotos = Photos
            .Where(p => BoxPhotoSlots.Contains(p.Slot))
            .ToList();

        if (boxPhotos.Count == 0)
            return;

        foreach (var photo in boxPhotos)
            Photos.Remove(photo);

        OrderPhotos();
        RefreshPhotoGuidance();
    }

    private void ClearBoxFields()
    {
        NumeroCaja = string.Empty;
        PlacaCaja = string.Empty;
        Sello = string.Empty;
    }

    private static string GetPhotoTitle(VehiclePhotoSlot slot)
    {
        return slot switch
        {
            VehiclePhotoSlot.NumeroVehiculo => "Numero del vehiculo",
            VehiclePhotoSlot.PlacaVehiculo => "Placas del vehiculo",
            VehiclePhotoSlot.NumeroCaja => "Numero de la caja",
            VehiclePhotoSlot.PlacaCaja => "Placas de la caja",
            VehiclePhotoSlot.Sello => "Sello de la caja",
            _ => $"Foto {((int)slot) + 1}"
        };
    }

    private static string GetInstructionForSlot(VehiclePhotoSlot slot)
    {
        return slot switch
        {
            VehiclePhotoSlot.NumeroVehiculo => "Toma la foto del numero del vehiculo.",
            VehiclePhotoSlot.PlacaVehiculo => "Ahora toma la foto de las placas del vehiculo.",
            VehiclePhotoSlot.NumeroCaja => "Toma la foto del numero de la caja.",
            VehiclePhotoSlot.PlacaCaja => "Ahora toma la foto de las placas de la caja.",
            VehiclePhotoSlot.Sello => "Por ultimo toma la foto del sello de la caja.",
            _ => "Toma la siguiente foto requerida."
        };
    }

    private static string GetCaptureTextForSlot(VehiclePhotoSlot slot)
    {
        return $"Tomar foto de {GetPhotoTitle(slot).ToLowerInvariant()}";
    }
}

public enum VehiclePhotoSlot
{
    NumeroVehiculo = 0,
    PlacaVehiculo = 1,
    NumeroCaja = 2,
    PlacaCaja = 3,
    Sello = 4
}

public class VehiclePhotoItem
{
    public VehiclePhotoSlot Slot { get; set; }
    public int Orden => (int)Slot;
    public byte[] Bytes { get; set; } = Array.Empty<byte>();
    public string Title { get; set; } = string.Empty;
    public ImageSource? Source { get; set; }
}
