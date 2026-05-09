using ZXing.Net.Maui;

namespace MauiAppLogin;

public partial class Scan3FieldsPage : ContentPage
{
    private static readonly Brush DefaultBorderBrush = new SolidColorBrush(Color.FromArgb("#E2E8F0"));
    private static readonly Brush SuccessBorderBrush = new SolidColorBrush(Color.FromArgb("#22C55E"));

    public string EstandarId { get; private set; } = "";
    public string Rack { get; private set; } = "";
    public string Posicion { get; private set; } = "";

    // 0 = EstandarId, 1 = Rack, 2 = Posicion
    private int _step = 0;
    private readonly bool _requiresThreeFields;

    private DateTime _lastScanAt = DateTime.MinValue;
    private string _lastValue = "";
    private bool _isBusy;
    private readonly TaskCompletionSource<bool> _completion = new();

    public Scan3FieldsPage(bool requiresThreeFields = true)
    {
        _requiresThreeFields = requiresThreeFields;
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

        ApplyScanMode();
        UpdateHint();
    }

    public Task<bool> WaitForResultAsync()
    {
        return _completion.Task;
    }

    private void UpdateHint()
    {
        var hint = _step switch
        {
            0 => "Escanea: Estandar ID",
            1 => _requiresThreeFields ? "Escanea: Rack" : "Listo. Presiona Aceptar.",
            2 => _requiresThreeFields ? "Escanea: Posicion" : "Listo. Presiona Aceptar.",
            _ => "Listo. Presiona Aceptar."
        };

        HintLabel.Text = hint;
    }

    private void ApplyScanMode()
    {
        if (_requiresThreeFields)
            return;

        ScreenTitleLabel.Text = "Escaneo de StandardId";
        RackBorder.IsVisible = false;
        PosicionBorder.IsVisible = false;
        TransferMessageLabel.Text = "Consultando movimientos...";
    }

    private void SetValueForStep(string value)
    {
        if (IsStandardLabelCode(value))
        {
            EstandarId = value;
            EstandarIdLabel.Text = value;
            MarkScanAccepted(EstandarIdBorder);

            if (_requiresThreeFields)
            {
                if (_step == 0)
                    _step = 1;
            }
            else
            {
                _step = 1;
                CameraView.IsDetecting = false;
            }
        }
        else if (_step == 0)
        {
            HintLabel.Text = "Escanea primero una etiqueta LD de 12 digitos.";
            CameraView.IsDetecting = true;
            return;
        }
        else if (_step == 1)
        {
            if (IsPositionCode(value))
            {
                SetPosition(value);
                _step = string.IsNullOrWhiteSpace(Rack) ? 1 : 3;
                CameraView.IsDetecting = _step < 3;
                UpdateHint();
                return;
            }

            Rack = value;
            RackLabel.Text = value;
            MarkScanAccepted(RackBorder);
            _step = string.IsNullOrWhiteSpace(Posicion) ? 2 : 3;
            CameraView.IsDetecting = _step < 3;
        }
        else if (_step == 2)
        {
            if (!IsPositionCode(value))
            {
                HintLabel.Text = "La posicion debe ser una sola letra.";
                CameraView.IsDetecting = true;
                return;
            }

            SetPosition(value);
            _step = 3;
            CameraView.IsDetecting = false;
        }

        UpdateHint();
    }

    private static bool IsStandardLabelCode(string value)
    {
        return value.Length == 12 && value.All(char.IsDigit);
    }

    private static bool IsPositionCode(string value)
    {
        return value.Length == 1 && char.IsLetter(value[0]);
    }

    private void SetPosition(string value)
    {
        Posicion = value;
        PosicionLabel.Text = value;
        MarkScanAccepted(PosicionBorder);
    }

    private void MarkScanAccepted(Border border)
    {
        border.Stroke = SuccessBorderBrush;
        border.StrokeThickness = 2;
        _ = PlayCorrectSoundAsync();
    }

    private static Task PlayCorrectSoundAsync()
    {
        try
        {
#if ANDROID
            var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            var asset = activity?.Assets?.OpenFd("correcto.mp3");
            if (asset is null)
                return Task.CompletedTask;

            var player = new Android.Media.MediaPlayer();
            player.SetDataSource(asset.FileDescriptor, asset.StartOffset, asset.Length);
            player.Prepare();
            player.Completion += (_, _) =>
            {
                player.Release();
                asset.Close();
            };
            player.Start();
#endif
        }
        catch
        {
        }

        return Task.CompletedTask;
    }

    private void CameraView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var value = e.Results?.FirstOrDefault()?.Value?.Trim();
        if (string.IsNullOrWhiteSpace(value))
            return;

        var now = DateTime.UtcNow;
        if (value == _lastValue && (now - _lastScanAt).TotalMilliseconds < 1200)
            return;

        _lastValue = value;
        _lastScanAt = now;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            var completedStep = _requiresThreeFields ? 3 : 1;
            if (_step >= completedStep && !IsStandardLabelCode(value))
                return;

            SetValueForStep(value);
        });
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        EstandarId = Rack = Posicion = "";
        EstandarIdLabel.Text = "-";
        RackLabel.Text = "-";
        PosicionLabel.Text = "-";
        ResetFieldBorders();

        _step = 0;
        _lastValue = "";
        _lastScanAt = DateTime.MinValue;

        CameraView.IsDetecting = true;
        UpdateHint();
    }

    private void ResetFieldBorders()
    {
        EstandarIdBorder.Stroke = DefaultBorderBrush;
        RackBorder.Stroke = DefaultBorderBrush;
        PosicionBorder.Stroke = DefaultBorderBrush;
        EstandarIdBorder.StrokeThickness = 1;
        RackBorder.StrokeThickness = 1;
        PosicionBorder.StrokeThickness = 1;
    }

    private async void OnAcceptClicked(object sender, EventArgs e)
    {
        if (_isBusy)
            return;

        if (string.IsNullOrWhiteSpace(EstandarId) ||
            (_requiresThreeFields && (string.IsNullOrWhiteSpace(Rack) || string.IsNullOrWhiteSpace(Posicion))))
        {
            var message = _requiresThreeFields
                ? "Escanea Estandar ID, Rack y Posicion."
                : "Escanea Estandar ID.";

            await DisplayAlertAsync("Faltan datos", message, "OK");
            return;
        }

        if (_requiresThreeFields && !IsPositionCode(Posicion))
        {
            await DisplayAlertAsync("Posicion invalida", "La posicion debe ser una sola letra.", "OK");
            return;
        }

        try
        {
            SetBusy(true);
            _completion.TrySetResult(true);
            await Navigation.PopModalAsync();
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void SetBusy(bool isBusy)
    {
        _isBusy = isBusy;
        TransferOverlay.IsVisible = isBusy;
        TransferActivity.IsRunning = isBusy;
        ClearButton.IsEnabled = !isBusy;
        AcceptButton.IsEnabled = !isBusy;
        CameraView.IsDetecting = !isBusy && _step < (_requiresThreeFields ? 3 : 1);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CameraView.IsDetecting = false;

        if (!_completion.Task.IsCompleted)
            _completion.TrySetResult(false);
    }
}
