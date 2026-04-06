using Plugin.Maui.OCR;
using System.Text.RegularExpressions;

namespace MauiAppLogin;



public partial class RegisterLicense : ContentPage
{
    private ImageSource? _foto1;
    private ImageSource? _foto2;

    public RegisterLicense()
	{

		InitializeComponent();
	}
    private async Task<string> ExtractText(Stream imageStream)
    {
        using var ms = new MemoryStream();
        await imageStream.CopyToAsync(ms);
        var result = await OcrPlugin.Default.RecognizeTextAsync(ms.ToArray());

        return result?.AllText ?? "No se detectó texto";
    }
    private void ParseText(string text)
    {
        var nombre = Regex.Match(text, @"NOMBRE[:\s]+([A-Z\s]+)");
        if (nombre.Success)
            NombreEntry.Text = nombre.Groups[1].Value;

        var licencia = Regex.Match(text, @"\d{6,10}");
        if (licencia.Success)
            LicenciaEntry.Text = licencia.Value;
    }
    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlertAsync("Cámara", "Este dispositivo no soporta captura de fotos.", "OK");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null) return;

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            mem.Position = 0;


            var text = await ExtractText(mem);

            await DisplayAlertAsync("Texto detectado", text, "OK");

  
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

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        // Limpia el preview (o navega atrás, tú decides)
        PreviewImage.Source = null;
        // Si quieres regresar:
        // await Navigation.PopAsync();
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        // Aquí normalmente validarías y regresarías datos al registro de vehículo
        // Por ahora: solo vuelve atrás
        //await Navigation.PopAsync(); 
        await Shell.Current.GoToAsync("RegisterVehicule");


    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


}


