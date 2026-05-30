using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppLogin.Common.Imaging;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using Plugin.Maui.OCR;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace MauiAppLogin.ViewModels;

public partial class RegisterVehiculeViewModel : ObservableObject
{
    private readonly SecurityRegistrationContext _context;
    private readonly ILoaderService _loaderService;

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
    private ImageSource? thumb1;

    [ObservableProperty]
    private ImageSource? thumb2;

    [ObservableProperty]
    private bool isBusy;

    private byte[]? _foto1Bytes;
    private byte[]? _foto2Bytes;

    public ICommand SelectCajaCommand { get; }
    public ICommand SelectTractorCommand { get; }
    public ICommand CapturarCommand { get; }
    public ICommand GaleriaCommand { get; }
    public ICommand CancelarCommand { get; }
    public ICommand SiguienteCommand { get; }
    public ICommand AtrasCommand { get; }

    public RegisterVehiculeViewModel(SecurityRegistrationContext context, ILoaderService loaderService)
    {
        _context = context;
        _loaderService = loaderService;

        SelectCajaCommand = new Command(SelectCaja);
        SelectTractorCommand = new Command(SelectTractor);
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
            TipoVehiculo = _context.TipoVehiculo;
            IsCajaSelected = TipoVehiculo == "Caja";
            IsTractorSelected = TipoVehiculo == "Tractor";
        }
        if (!string.IsNullOrEmpty(_context.Linea)) Linea = _context.Linea;
        if (!string.IsNullOrEmpty(_context.Origen)) Origen = _context.Origen;
        if (!string.IsNullOrEmpty(_context.Numero)) Numero = _context.Numero;
        if (!string.IsNullOrEmpty(_context.Placa)) Placa = _context.Placa;

        _foto1Bytes = _context.VehiculoFoto1;
        _foto2Bytes = _context.VehiculoFoto2;

        if (_foto1Bytes is not null)
            Thumb1 = ImageSource.FromStream(() => new MemoryStream(_foto1Bytes));
        if (_foto2Bytes is not null)
            Thumb2 = ImageSource.FromStream(() => new MemoryStream(_foto2Bytes));
    }

    private void SaveToContext()
    {
        _context.TipoVehiculo = TipoVehiculo;
        _context.Linea = Linea;
        _context.Origen = Origen;
        _context.Numero = Numero;
        _context.Placa = Placa;
        _context.VehiculoFoto1 = _foto1Bytes;
        _context.VehiculoFoto2 = _foto2Bytes;
    }

    private void SelectCaja()
    {
        TipoVehiculo = "Caja";
        IsCajaSelected = true;
        IsTractorSelected = false;
    }

    private void SelectTractor()
    {
        TipoVehiculo = "Tractor";
        IsCajaSelected = false;
        IsTractorSelected = true;
    }

    private async Task CapturarAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await Shell.Current.DisplayAlertAsync("Cámara", "Este dispositivo no soporta captura de fotos.", "OK");
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

            if (_foto1Bytes is null)
            {
                _foto1Bytes = bytesFinales;
                Thumb1 = img;
            }
            else
            {
                _foto2Bytes = bytesFinales;
                Thumb2 = img;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
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

    private void Cancelar()
    {
        PreviewImage = null;
    }

    private async Task SiguienteAsync()
    {
        if (string.IsNullOrEmpty(TipoVehiculo) ||
            string.IsNullOrEmpty(Linea) ||
            string.IsNullOrEmpty(Placa))
        {
            await Shell.Current.DisplayAlertAsync("Atención", "Seleccione tipo y llene al menos línea y placa.", "OK");
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
