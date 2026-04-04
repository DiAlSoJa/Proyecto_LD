namespace MauiAppLogin;

public partial class DamageReportDetailPage : ContentPage
{
    private ImageSource? _foto1;
    private ImageSource? _foto2;

    public DamageReportDetailPage()
    {

        InitializeComponent();
    }

    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlert("Cámara", "Este dispositivo no soporta captura de fotos.", "OK");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null) return;

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            mem.Position = 0;

            var img = ImageSource.FromStream(() => new MemoryStream(mem.ToArray()));

            // Preview grande
            PreviewImage.Source = img;

            // Guardar en slots de miniaturas (2 fotos)
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
            await DisplayAlert("Error", ex.Message, "OK");
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
        await Shell.Current.GoToAsync("DamageReportPrintPage");


    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


}


