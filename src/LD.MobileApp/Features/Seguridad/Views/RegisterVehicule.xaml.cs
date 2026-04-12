namespace MauiAppLogin;

public partial class RegisterVehicule : ContentPage
{
    private ImageSource? _foto1;
    private ImageSource? _foto2;
    private string tipoSeleccionado = "";
    

    public RegisterVehicule()
    {

        InitializeComponent();
    }
  

    private void OnCajaTapped(object sender, EventArgs e)
    {
        tipoSeleccionado = "Caja";

        CajaOption.BackgroundColor = Color.FromArgb("#E76F51"); // activo
        TractorOption.BackgroundColor = Color.FromArgb("#E5E7EB"); // inactivo
    }

    private void OnTractorTapped(object sender, EventArgs e)
    {
        tipoSeleccionado = "Tractor";

        TractorOption.BackgroundColor = Color.FromArgb("#E76F51"); // activo
        CajaOption.BackgroundColor = Color.FromArgb("#E5E7EB"); // inactivo
    }


    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        // Aquí puedes validar antes de avanzar
        // Ejemplo: si tipoSeleccionado == "" => alert
        await Shell.Current.GoToAsync("SignatureDriver");
        // await Navigation.PushAsync(new OtraPagina());
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

    private async void OnTerminarClicked(object sender, EventArgs e)
    {
        // Aquí normalmente validarías y regresarías datos al registro de vehículo
        // Por ahora: solo vuelve atrás
        await Navigation.PopAsync();
    }
}


