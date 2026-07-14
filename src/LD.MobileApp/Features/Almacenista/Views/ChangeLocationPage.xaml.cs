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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshEntryTexts();
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _viewModel.ExpectedStandardId = string.Empty;
        _viewModel.EstandarId = string.Empty;
        _viewModel.AsnId = 0;
        _viewModel.KittingReceiptDetailId = 0;
        _viewModel.KittingId = 0;
        _viewModel.InstructionText = string.Empty;
        TextInformation = null;

        if (query.TryGetValue("ExpectedStandardId", out var expectedStandardId))
        {
            _viewModel.ExpectedStandardId = expectedStandardId?.ToString() ?? string.Empty;
        }

        if (query.TryGetValue("StandardId", out var standardId))
        {
            _viewModel.EstandarId = standardId?.ToString() ?? string.Empty;
        }

        if (query.TryGetValue("AsnId", out var asnIdValue) &&
            int.TryParse(asnIdValue?.ToString(), out var asnId))
        {
            _viewModel.AsnId = asnId;
        }

        if (query.TryGetValue("KittingReceiptDetailId", out var kittingReceiptDetailIdValue) &&
            int.TryParse(kittingReceiptDetailIdValue?.ToString(), out var kittingReceiptDetailId))
        {
            _viewModel.KittingReceiptDetailId = kittingReceiptDetailId;
        }

        if (query.TryGetValue("KittingId", out var kittingIdValue) &&
            int.TryParse(kittingIdValue?.ToString(), out var kittingId))
        {
            _viewModel.KittingId = kittingId;
        }

        if (query.TryGetValue("TextInformation", out var textInformation))
        {
            TextInformation = textInformation as string;
            _viewModel.InstructionText = TextInformation ?? string.Empty;
        }

        RefreshEntryTexts();
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        SyncEntriesToViewModel();
        await _viewModel.OnSiguienteClicked();
    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        var page = new Scan3FieldsPage(
            requiresThreeFields: true,
            contextText: _viewModel.InstructionText,
            initialEstandarId: _viewModel.EstandarId,
            initialRack: _viewModel.Rack,
            initialPosicion: _viewModel.Posicion);
        await Navigation.PushModalAsync(page);

        var accepted = await page.WaitForResultAsync();
        if (!accepted)
            return;

        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            if (!string.IsNullOrWhiteSpace(page.EstandarId))
                _viewModel.EstandarId = page.EstandarId;

            if (!string.IsNullOrWhiteSpace(page.Rack))
                _viewModel.Rack = page.Rack;

            if (!string.IsNullOrWhiteSpace(page.Posicion))
                _viewModel.Posicion = page.Posicion;

            RefreshEntryTexts();
        });
    }

    private void SyncEntriesToViewModel()
    {
        _viewModel.EstandarId = EstandarIdEntry.Text?.Trim() ?? string.Empty;
        _viewModel.Rack = RackEntry.Text?.Trim() ?? string.Empty;
        _viewModel.Posicion = PosicionEntry.Text?.Trim() ?? string.Empty;
    }

    private void RefreshEntryTexts()
    {
        EstandarIdEntry.Text = _viewModel.EstandarId;
        RackEntry.Text = _viewModel.Rack;
        PosicionEntry.Text = _viewModel.Posicion;
    }
}
