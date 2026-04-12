using ZXing.Net.Maui;

namespace MauiAppLogin;

public partial class ScanStandarLD : ContentPage
{
    // Resultado final
    public string EstandarId { get; private set; } = "";
    public string Rack { get; private set; } = "";
    public string Posicion { get; private set; } = "";

    // 0 = EstandarId, 1 = Rack, 2 = Posición
    private int _step = 0;

    // anti-duplicados
    private DateTime _lastScanAt = DateTime.MinValue;
    private string _lastValue = "";

    public ScanStandarLD()
    {
        InitializeComponent();

        CameraView.Options = new BarcodeReaderOptions
        {
            Formats =
                BarcodeFormat.Code128 |
                BarcodeFormat.Code39 |
                BarcodeFormat.Ean13 |
                BarcodeFormat.Ean8 |
                BarcodeFormat.UpcA |
                BarcodeFormat.UpcE,


            AutoRotate = true,
            Multiple = false,
            TryHarder = true
        };


        UpdateHint();
    }

    private void UpdateHint()
    {
        var hint = _step switch
        {
            0 => "Escanea: Estandar ID",           
            _ => "Listo"
        };

        HintLabel.Text = hint;
    }

    private void SetValueForStep(string value)
    {
        if (_step == 0)
        {
            EstandarId = value;
            EstandarIdLabel.Text = value;
            _step = 1;   
            CameraView.IsDetecting = false;
            HintLabel.Text = "Listo. Presiona Aceptar.";
        }

        UpdateHint();
    }

    private void CameraView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var value = e.Results?.FirstOrDefault()?.Value?.Trim();
        if (string.IsNullOrWhiteSpace(value))
            return;

        // Debounce: evita leer 10 veces el mismo código
        var now = DateTime.UtcNow;
        if (value == _lastValue && (now - _lastScanAt).TotalMilliseconds < 1200)
            return;

        _lastValue = value;
        _lastScanAt = now;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Si ya completó los 3, ignora
            if (_step >= 3) return;

            SetValueForStep(value);
        });
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        EstandarId = Rack = Posicion = "";
        EstandarIdLabel.Text = "—";
        _step = 0;
        _lastValue = "";
        _lastScanAt = DateTime.MinValue;

        CameraView.IsDetecting = true;
        UpdateHint();
    }

    private void OnRescanClicked(object sender, EventArgs e)
    {
        // “Releer” vuelve a escanear el campo actual
        if (_step >= 3) _step = 2; // si ya estaba completo, permite releer Posición
        CameraView.IsDetecting = true;
        UpdateHint();
    }

    private async void OnAcceptClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EstandarId) )
        {
            await DisplayAlertAsync("Faltan datos", "Escanea Estandar ID.", "OK");
            return;
        }

        await Navigation.PopModalAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CameraView.IsDetecting = false;
    }
}