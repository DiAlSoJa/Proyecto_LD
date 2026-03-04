namespace MauiAppLogin;

public partial class ChangeLocationPage : ContentPage, IQueryAttributable
{
    public string textInformation { get; set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("TextInformation"))
        {
            textInformation = query["TextInformation"] as string;
        }
        this.TextInformationLabel.Text = textInformation;


    }

    public ChangeLocationPage()
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
       // await Shell.Current.GoToAsync("RegisterVehicule");


    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        var page = new Scan3FieldsPage();
        await Navigation.PushModalAsync(page);

        // Al volver, ya trae los valores
        if (!string.IsNullOrWhiteSpace(page.EstandarId))
            EstandarIdEntry.Text = page.EstandarId;

        if (!string.IsNullOrWhiteSpace(page.Rack))
            RackEntry.Text = page.Rack;

        if (!string.IsNullOrWhiteSpace(page.Posicion))
            PosicionEntry.Text = page.Posicion;

    }



}