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

[QueryProperty(nameof(Tipo), "tipo")]
public partial class RegisterLicenseViewModel : ObservableObject
{
    private readonly SecurityRegistrationContext _context;
    private readonly ILoaderService _loaderService;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private string tipo = "";

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string licencia = "";

    [ObservableProperty]
    private DateTime vencimiento = DateTime.Today;

    [ObservableProperty]
    private string celular = "";

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

    public ICommand CapturarCommand { get; }
    public ICommand GaleriaCommand { get; }
    public ICommand CancelarCommand { get; }
    public ICommand SiguienteCommand { get; }
    public ICommand AtrasCommand { get; }

    public RegisterLicenseViewModel(SecurityRegistrationContext context, ILoaderService loaderService)
    {
        _context = context;
        _loaderService = loaderService;

        CapturarCommand = new AsyncCommand(CapturarAsync);
        GaleriaCommand = new AsyncCommand(SeleccionarGaleriaAsync);
        CancelarCommand = new Command(Cancelar);
        SiguienteCommand = new AsyncCommand(SiguienteAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);

        LoadFromContext();
    }

    private void LoadFromContext()
    {
        if (!string.IsNullOrEmpty(_context.Nombre)) Nombre = _context.Nombre;
        if (!string.IsNullOrEmpty(_context.Licencia)) Licencia = _context.Licencia;
        if (_context.Vencimiento != default) Vencimiento = _context.Vencimiento;
        if (!string.IsNullOrEmpty(_context.Celular)) Celular = _context.Celular;

        _foto1Bytes = _context.LicenciaFoto1;
        _foto2Bytes = _context.LicenciaFoto2;

        if (_foto1Bytes is not null)
            Thumb1 = ImageSource.FromStream(() => new MemoryStream(_foto1Bytes));
        if (_foto2Bytes is not null)
            Thumb2 = ImageSource.FromStream(() => new MemoryStream(_foto2Bytes));
    }

    private void SaveToContext()
    {
        _context.Tipo = Tipo;
        _context.Nombre = Nombre;
        _context.Licencia = Licencia;
        _context.Vencimiento = Vencimiento;
        _context.Celular = Celular;
        _context.LicenciaFoto1 = _foto1Bytes;
        _context.LicenciaFoto2 = _foto2Bytes;
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
                Title = "Selecciona una imagen de la licencia"
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
                ParseLicense(text);

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

    private void ParseLicense(string text)
    {
        // Nombre: etiqueta explícita primero, luego línea con dos o más palabras en mayúsculas
        var nombreLabel = Regex.Match(text,
            @"(?:NOMBRE|NAME)[:\s]+([A-ZÁÉÍÓÚÜÑ][A-ZÁÉÍÓÚÜÑ\s]{3,60})",
            RegexOptions.IgnoreCase);

        if (nombreLabel.Success)
        {
            Nombre = nombreLabel.Groups[1].Value.Trim();
        }
        else
        {
            var nombreBloque = Regex.Match(text,
                @"^([A-ZÁÉÍÓÚÜÑ]{2,}\s+[A-ZÁÉÍÓÚÜÑ]{2,}(?:\s+[A-ZÁÉÍÓÚÜÑ]{2,})*)\s*$",
                RegexOptions.Multiline);
            if (nombreBloque.Success)
                Nombre = nombreBloque.Groups[1].Value.Trim();
        }

        // Licencia: número de 6-12 dígitos (evita años y teléfonos cortos)
        var licMatch = Regex.Match(text, @"\b(\d{6,12})\b");
        if (licMatch.Success)
            Licencia = licMatch.Groups[1].Value;
    }

    private void Cancelar()
    {
        PreviewImage = null;
    }

    private async Task SiguienteAsync()
    {
        if (string.IsNullOrEmpty(Tipo) ||
            string.IsNullOrEmpty(Licencia) ||
            string.IsNullOrEmpty(Nombre) ||
            string.IsNullOrEmpty(Celular))
        {
            await Shell.Current.DisplayAlertAsync("Atención", "Llene todos los campos, por favor.", "OK");
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
}
