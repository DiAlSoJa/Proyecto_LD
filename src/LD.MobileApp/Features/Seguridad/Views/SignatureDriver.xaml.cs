namespace MauiAppLogin;

using CommunityToolkit.Maui.Views;


public partial class SignatureDriver : ContentPage
{
	public SignatureDriver()
	{
		InitializeComponent();
	}
    private void OnClearClicked(object sender, EventArgs e)
    {
        SignaturePad.Clear();
    }

    private void OnUndoClicked(object sender, EventArgs e)
    {
        // DrawingView soporta Undo
        SignaturePad.Clear();
    }

    private async void OnFinalizarClicked(object sender, EventArgs e)
    {
        // Aquí puedes validar si hay trazos
        // Si quieres exportar imagen:
        // var imageStream = await SignaturePad.GetImageStream(600, 300);

        await DisplayAlertAsync("Listo", "Firma registrada.", "OK");
        await Shell.Current.GoToAsync("//dashboard"); // o navegar a donde quieras
    }
}