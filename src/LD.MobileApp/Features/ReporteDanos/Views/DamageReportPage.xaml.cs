namespace MauiAppLogin;

public partial class DamageReportPage : ContentPage
{
    public DamageReportPage()
    {
        InitializeComponent();
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        PreviewImage.Source = null;
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        var standardId = EstandarIdEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(standardId))
        {
            await DisplayAlert("StandardId requerido", "Ingresa o captura un StandardId para continuar.", "OK");
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(DamageReportDetailPage)}?standardId={Uri.EscapeDataString(standardId)}");
    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        var page = new ScanStandarLD();
        await Navigation.PushModalAsync(page);

        if (!string.IsNullOrWhiteSpace(page.EstandarId))
            EstandarIdEntry.Text = page.EstandarId;
    }
}
