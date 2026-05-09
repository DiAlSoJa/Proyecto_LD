using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class ChangeLocationPage : ContentPage, IQueryAttributable
{
    private readonly ChangeLocationViewModel _viewModel;

    public string? TextInformation { get; set; }

    public ChangeLocationPage(ChangeLocationViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("TextInformation", out var textInformation))
            TextInformation = textInformation as string;
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        PreviewImage.Source = null;
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        await _viewModel.OnSiguienteClicked();
    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        var page = new Scan3FieldsPage(requiresThreeFields: true);
        await Navigation.PushModalAsync(page);

        var accepted = await page.WaitForResultAsync();
        if (!accepted)
            return;

        if (!string.IsNullOrWhiteSpace(page.EstandarId))
            _viewModel.EstandarId = page.EstandarId;

        if (!string.IsNullOrWhiteSpace(page.Rack))
            _viewModel.Rack = page.Rack;

        if (!string.IsNullOrWhiteSpace(page.Posicion))
            _viewModel.Posicion = page.Posicion;
    }
}
