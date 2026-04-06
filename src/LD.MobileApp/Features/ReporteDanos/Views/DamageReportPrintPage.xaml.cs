namespace MauiAppLogin;

public partial class DamageReportPrintPage : ContentPage
{
	public DamageReportPrintPage()
	{
		InitializeComponent();
	}
    private async void OnPrintClicked(object sender, EventArgs e)
    {
        // Aquí normalmente validarías y regresarías datos al registro de vehículo
        // Por ahora: solo vuelve atrás
        //await Navigation.PopAsync(); 
       // await Shell.Current.GoToAsync("DamageReportDetailPage");


    }
    private async void OnPDFClicked(object sender, EventArgs e)
    {
        // Aquí normalmente validarías y regresarías datos al registro de vehículo
        // Por ahora: solo vuelve atrás
        //await Navigation.PopAsync(); 
       // await Shell.Current.GoToAsync("DamageReportDetailPage");


    }
    private async void OnExcelClicked(object sender, EventArgs e)
    {
        // Aquí normalmente validarías y regresarías datos al registro de vehículo
        // Por ahora: solo vuelve atrás
        //await Navigation.PopAsync(); 
       // await Shell.Current.GoToAsync("DamageReportDetailPage");


    }
}