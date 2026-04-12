using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers.Commands;
using Plugin.Maui.OCR;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace MauiAppLogin.ViewModels;

[QueryProperty(nameof(Tipo), "tipo")]
public partial class RegisterLicenseViewModel : ObservableObject
{
    [ObservableProperty]
    private string tipo = "";

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string licencia = "";

    [ObservableProperty]
    private string vencimiento = "";

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

    private byte[]? _lastCaptureBytes;
    private bool _foto1Taken;

    public ICommand CapturarCommand { get; }
    public ICommand CancelarCommand { get; }
    public ICommand SiguienteCommand { get; }
    public ICommand AtrasCommand { get; }

    public RegisterLicenseViewModel()
    {
        CapturarCommand = new AsyncCommand(CapturarAsync);
        CancelarCommand = new Command(Cancelar);
        SiguienteCommand = new AsyncCommand(SiguienteAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);
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
            _lastCaptureBytes = mem.ToArray();

            // OCR
            var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(_lastCaptureBytes);
            var text = ocrResult?.AllText ?? "";

            if (!string.IsNullOrWhiteSpace(text))
                ParseText(text);

            // Preview
            var img = ImageSource.FromStream(() => new MemoryStream(_lastCaptureBytes));
            PreviewImage = img;

            if (!_foto1Taken)
            {
                Thumb1 = img;
                _foto1Taken = true;
            }
            else
            {
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
        await Shell.Current.GoToAsync(nameof(RegisterVehicule));
    }

    private async Task AtrasAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
