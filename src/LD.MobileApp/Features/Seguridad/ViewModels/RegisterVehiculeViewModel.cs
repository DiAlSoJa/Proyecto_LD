using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Contracts.Enums;
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
    private readonly SecurityRegistrationContext _context;
    private readonly ILoaderService _loaderService;
    private readonly IDialogService _dialogService;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private string tipoVehiculo = "";

    [ObservableProperty]
    private bool isCajaSelected;

    [ObservableProperty]
    private bool isTractorSelected;

    [ObservableProperty]
    private string linea = "";

    [ObservableProperty]
    private string origen = "";

    [ObservableProperty]
    private string numero = "";

    [ObservableProperty]
    private string placa = "";

    [ObservableProperty]
    private ImageSource? previewImage;

    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<VehiclePhotoItem> Photos { get; } = new();

    public ICommand SelectCajaCommand { get; }
    public ICommand SelectTractorCommand { get; }
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
        IDialogService dialogService)
    {
        _context       = context;
        _loaderService = loaderService;
        _dialogService = dialogService;

        SelectCajaCommand    = new Command(SelectCaja);
        SelectTractorCommand = new Command(SelectTractor);

        CapturarCommand      = new AsyncCommand(CapturarAsync);
        CancelarCommand      = new Command(Cancelar);
        SiguienteCommand     = new AsyncCommand(SiguienteAsync);
        AtrasCommand         = new AsyncCommand(AtrasAsync);
        RemovePhotoCommand   = new MvvmHelpers.Commands.Command<VehiclePhotoItem>(RemovePhoto);
        ViewPhotoCommand     = new MvvmHelpers.Commands.Command<VehiclePhotoItem>(ViewPhoto);

        CapturarCommand = new AsyncCommand(CapturarAsync);
        GaleriaCommand = new AsyncCommand(SeleccionarGaleriaAsync);
        CancelarCommand = new Command(Cancelar);
        SiguienteCommand = new AsyncCommand(SiguienteAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);


        LoadFromContext();
    }

    private void LoadFromContext()
    {
        if (!string.IsNullOrEmpty(_context.TipoVehiculo))
        {
            TipoVehiculo      = _context.TipoVehiculo;
            IsCajaSelected    = TipoVehiculo == "Caja";
            IsTractorSelected = TipoVehiculo == "Tractor";
        }
        if (!string.IsNullOrEmpty(_context.Linea))  Linea  = _context.Linea;
        if (!string.IsNullOrEmpty(_context.Origen)) Origen = _context.Origen;
        if (!string.IsNullOrEmpty(_context.Numero)) Numero = _context.Numero;
        if (!string.IsNullOrEmpty(_context.Placa))  Placa  = _context.Placa;

        Photos.Clear();
        foreach (var entry in _context.VehiculoFotos)
        {
            Photos.Add(new VehiclePhotoItem
            {
                Bytes  = entry.Bytes,
                Orden  = entry.Orden,
                Source = ImageSource.FromStream(() => new MemoryStream(entry.Bytes)),
            });
        }
    }

    private void SaveToContext()
    {
        _context.TipoVehiculo = TipoVehiculo;
        _context.Linea  = Linea;
        _context.Origen = Origen;
        _context.Numero = Numero;
        _context.Placa  = Placa;

        _context.VehiculoFotos = Photos.Select(p => new SecurityPhotoEntry
        {
            Categoria = PhotoCategoria_e.Vehiculo,
            Orden     = p.Orden,
            Bytes     = p.Bytes,
        }).ToList();
    }

    private void SelectCaja()
    {
        TipoVehiculo      = "Caja";
        IsCajaSelected    = true;
        IsTractorSelected = false;
    }

    private void SelectTractor()
    {
        TipoVehiculo      = "Tractor";
        IsCajaSelected    = false;
        IsTractorSelected = true;
    }

    private async Task CapturarAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await _dialogService.ShowInfoAsync("Cámara", "Este dispositivo no soporta captura de fotos.");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null) return;

            await ProcesarFotoAsync(photo);
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
            var photos = await MediaPicker.Default.PickPhotosAsync(new MediaPickerOptions
            {
                Title = "Selecciona una imagen del vehículo"
            });
            var photo = photos?.FirstOrDefault();

            if (photo is null) return;

            await ProcesarFotoAsync(photo);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async Task ProcesarFotoAsync(FileResult photo)
    {
        try
        {
            _loaderService.Show("Procesando imagen...");

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            var bytes = mem.ToArray();

            var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(bytes);
            var text = ocrResult?.AllText ?? "";
            if (!string.IsNullOrWhiteSpace(text))
                ParsePlaca(text);

            var bytesFinales = await ComprimirImagenAsync(bytes);
            var img = ImageSource.FromStream(() => new MemoryStream(bytesFinales));

            Photos.Add(new VehiclePhotoItem
            {
                Bytes  = bytesFinales,
                Orden  = Photos.Count,
                Source = img,
            });

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

    private void ParsePlaca(string text)
    {
        if (!string.IsNullOrEmpty(Placa)) return;

        // Placas mexicanas: ABC-1234 · ABC1234 · AB-12-C · 123-ABC · transporte federal XXX-XX-XXXX
        var match = Regex.Match(text,
            @"\b([A-Z]{2,3}[-\s]?\d{3,4}[-\s]?[A-Z]{0,2}|\d{3}[-\s]?[A-Z]{2,3})\b");
        if (match.Success)
            Placa = Regex.Replace(match.Value, @"\s", "").ToUpper();
    }

    private void RemovePhoto(VehiclePhotoItem? item)
    {
        if (item is null) return;
        Photos.Remove(item);
        for (int i = 0; i < Photos.Count; i++)
            Photos[i].Orden = i;
    }

    private void ViewPhoto(VehiclePhotoItem? item)
    {
        if (item?.Source is null) return;
        (Shell.Current.CurrentPage ?? Application.Current?.MainPage)
            ?.ShowPopup(new ImagePreviewPopup(item.Source));
    }

    private void Cancelar()
    {
        PreviewImage = null;
    }

    private async Task SiguienteAsync()
    {
        if (string.IsNullOrEmpty(TipoVehiculo) ||
            string.IsNullOrEmpty(Linea)        ||
            string.IsNullOrEmpty(Placa))
        {
            await _dialogService.ShowInfoAsync("Atención", "Seleccione tipo y llene al menos línea y placa.");
            return;
        }

        if (Photos.Count < 2)
        {
            await _dialogService.ShowInfoAsync("Atención", "Agregue al menos 2 fotos del vehículo.");
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
}

public class VehiclePhotoItem
{
    public byte[] Bytes { get; set; } = Array.Empty<byte>();
    public int Orden { get; set; }
    public ImageSource? Source { get; set; }
}
