using ZXing.Net.Maui;

namespace MauiAppLogin;

public partial class Scan3FieldsPage : ContentPage
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

    public Scan3FieldsPage()
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
                BarcodeFormat.UpcE ,
                

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
            1 => "Escanea: Posición",
            2 => "Escanea: Rack",
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
        }
        else if (_step == 1)
        {

            Posicion = value;
            PosicionLabel.Text = value;
            
            _step = 2;
        }
        else if (_step == 2)
        {
            Rack = value;
            RackLabel.Text = value;
            _step = 3;

            // Ya tenemos todo: detén detección, pero no cierres automático (tú decides)
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
        RackLabel.Text = "—";
        PosicionLabel.Text = "—";

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
        if (string.IsNullOrWhiteSpace(EstandarId) ||
            string.IsNullOrWhiteSpace(Rack) ||
            string.IsNullOrWhiteSpace(Posicion))
        {
            await DisplayAlert("Faltan datos", "Escanea Estandar ID, Rack y Posición.", "OK");
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