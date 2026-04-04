namespace MauiAppLogin;

public partial class MovementPage : ContentPage
{
	public MovementPage()
	{
		InitializeComponent();
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
         await Shell.Current.GoToAsync("MovementDetail");


    }
    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        var page = new ScanStandarLD();
        await Navigation.PushModalAsync(page);

        // Al volver, ya trae los valores
        if (!string.IsNullOrWhiteSpace(page.EstandarId))
            EstandarIdEntry.Text = page.EstandarId;

    }


}