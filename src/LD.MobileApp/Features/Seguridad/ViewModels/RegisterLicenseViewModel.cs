using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppLogin.Models;
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
    public ICommand CancelarCommand { get; }
    public ICommand SiguienteCommand { get; }
    public ICommand AtrasCommand { get; }

    public RegisterLicenseViewModel(SecurityRegistrationContext context)
    {
        _context = context;

        CapturarCommand = new AsyncCommand(CapturarAsync);
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

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            var bytes = mem.ToArray();

            // OCR
            var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(bytes);
            var text = ocrResult?.AllText ?? "";

            if (!string.IsNullOrWhiteSpace(text))
                ParseText(text);

            // Preview
            var img = ImageSource.FromStream(() => new MemoryStream(bytes));
            PreviewImage = img;

            if (_foto1Bytes is null)
            {
                _foto1Bytes = bytes;
                Thumb1 = img;
            }
            else
            {
                _foto2Bytes = bytes;
                Thumb2 = img;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private void ParseText(string text)
    {
        var nombreMatch = Regex.Match(text, @"NOMBRE[:\s]+([A-Z\s]+)");
        if (nombreMatch.Success)
            Nombre = nombreMatch.Groups[1].Value.Trim();

        var licenciaMatch = Regex.Match(text, @"\d{6,10}");
        if (licenciaMatch.Success)
            Licencia = licenciaMatch.Value;
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
