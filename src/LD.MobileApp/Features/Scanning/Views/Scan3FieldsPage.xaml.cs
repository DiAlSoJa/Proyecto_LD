using ZXing.Net.Maui;

namespace MauiAppLogin;

public partial class Scan3FieldsPage : ContentPage
{
    private static readonly Brush DefaultBorderBrush = new SolidColorBrush(Color.FromArgb("#E2E8F0"));
    private static readonly Brush SuccessBorderBrush = new SolidColorBrush(Color.FromArgb("#22C55E"));
    private readonly string _contextText;

    public string EstandarId { get; private set; } = "";
    public string Rack { get; private set; } = "";
    public string Posicion { get; private set; } = "";

    // 0 = EstandarId, 1 = Rack, 2 = Posicion
    private int _step = 0;
    private readonly bool _requiresThreeFields;

    private DateTime _lastScanAt = DateTime.MinValue;
    private string _lastValue = "";
    private bool _isBusy;
    private bool _isClosing;
    private readonly TaskCompletionSource<bool> _completion = new();

    public Scan3FieldsPage(
        bool requiresThreeFields = true,
        string? contextText = null,
        string? initialEstandarId = null,
        string? initialRack = null,
        string? initialPosicion = null)
    {
        _requiresThreeFields = requiresThreeFields;
        _contextText = contextText?.Trim() ?? string.Empty;
        InitializeComponent();

        CameraView.Options = new BarcodeReaderOptions
        {
            Formats =
                BarcodeFormat.Code128 |
                BarcodeFormat.Code39 |
                BarcodeFormat.Itf |
                BarcodeFormat.Ean13 |
                BarcodeFormat.Ean8 |
                BarcodeFormat.UpcA |
                BarcodeFormat.UpcE,
            AutoRotate = true,
            Multiple = false,
            TryHarder = true
        };

        ApplyScanMode();
        ApplyContext();
        ApplyInitialValues(initialEstandarId, initialRack, initialPosicion);
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

    private void ApplyContext()
    {
        if (string.IsNullOrWhiteSpace(_contextText))
        {
            ContextBorder.IsVisible = false;
            return;
        }

        ContextLabel.Text = _contextText;
        ContextBorder.IsVisible = true;
    }

    private void SetValueForStep(string value)
    {
        if (_step == 0)
        {
            if (!IsStandardLabelCode(value))
            {
                HintLabel.Text = "Escanea primero una etiqueta LD de 12 digitos.";
                CameraView.IsDetecting = true;
                return;
            }

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
        else if (_step == 1)
        {
            var normalizedPosition = NormalizePositionCode(value);
            if (normalizedPosition is not null)
            {
                SetPosition(normalizedPosition);
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
            var normalizedPosition = NormalizePositionCode(value);
            if (normalizedPosition is null)
            {
                HintLabel.Text = "La posicion debe ser una letra o un codigo NIV + letra.";
                CameraView.IsDetecting = true;
                return;
            }

            SetPosition(normalizedPosition);
            _step = 3;
            CameraView.IsDetecting = false;
        }

        UpdateHint();
    }

    private void ApplyInitialValues(string? initialEstandarId, string? initialRack, string? initialPosicion)
    {
        var standardId = initialEstandarId?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(standardId))
        {
            EstandarId = standardId;
            EstandarIdLabel.Text = standardId;
            ApplyAcceptedState(EstandarIdBorder);
            _step = 1;
        }

        if (_requiresThreeFields && !string.IsNullOrWhiteSpace(standardId))
        {
            var rack = initialRack?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(rack))
            {
                Rack = rack;
                RackLabel.Text = rack;
                ApplyAcceptedState(RackBorder);
                _step = 2;
            }

            var posicion = initialPosicion?.Trim() ?? string.Empty;
            var normalizedPosition = NormalizePositionCode(posicion);
            if (normalizedPosition is not null)
            {
                Posicion = normalizedPosition;
                PosicionLabel.Text = normalizedPosition;
                ApplyAcceptedState(PosicionBorder);
                _step = 3;
            }
        }

        var completedStep = _requiresThreeFields ? 3 : 1;
        CameraView.IsDetecting = _step < completedStep;
    }

    private static bool IsStandardLabelCode(string value)
    {
        return value.Length == 12 && value.All(char.IsDigit);
    }

    private static bool IsPositionCode(string value)
    {
        return NormalizePositionCode(value) is not null;
    }

    private static string? NormalizePositionCode(string value)
    {
        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length == 1 && char.IsLetter(normalized[0]))
            return normalized;

        if (normalized.Length == 4 &&
            normalized.StartsWith("NIV", StringComparison.Ordinal) &&
            char.IsLetter(normalized[3]))
        {
            return normalized[3].ToString();
        }

        return null;
    }

    private void SetPosition(string value)
    {
        Posicion = value;
        PosicionLabel.Text = value;
        MarkScanAccepted(PosicionBorder);
    }

    private void MarkScanAccepted(Border border)
    {
        ApplyAcceptedState(border);
        _ = PlayCorrectSoundAsync();
    }

    private static void ApplyAcceptedState(Border border)
    {
        border.Stroke = SuccessBorderBrush;
        border.StrokeThickness = 2;
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

    private void ResetFieldBorder(Border border)
    {
        border.Stroke = DefaultBorderBrush;
        border.StrokeThickness = 1;
    }

    private void ResetLastScan()
    {
        _lastValue = "";
        _lastScanAt = DateTime.MinValue;
    }

    private void OnEstandarIdTapped(object sender, TappedEventArgs e)
    {
        if (_isBusy)
            return;

        EstandarId = "";
        EstandarIdLabel.Text = "-";
        ResetFieldBorder(EstandarIdBorder);
        ResetLastScan();

        _step = 0;
        CameraView.IsDetecting = true;
        UpdateHint();
    }

    private void OnRackTapped(object sender, TappedEventArgs e)
    {
        if (_isBusy || !_requiresThreeFields)
            return;

        Rack = "";
        RackLabel.Text = "-";
        ResetFieldBorder(RackBorder);
        ResetLastScan();

        _step = string.IsNullOrWhiteSpace(EstandarId) ? 0 : 1;
        CameraView.IsDetecting = true;
        UpdateHint();
    }

    private void OnPosicionTapped(object sender, TappedEventArgs e)
    {
        if (_isBusy || !_requiresThreeFields)
            return;

        Posicion = "";
        PosicionLabel.Text = "-";
        ResetFieldBorder(PosicionBorder);
        ResetLastScan();

        if (string.IsNullOrWhiteSpace(EstandarId))
            _step = 0;
        else
            _step = string.IsNullOrWhiteSpace(Rack) ? 1 : 2;

        CameraView.IsDetecting = true;
        UpdateHint();
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
            await DisplayAlertAsync("Posicion invalida", "La posicion debe ser una letra o un codigo NIV + letra.", "OK");
            return;
        }

        try
        {
            SetBusy(true);
            _isClosing = true;
            await Navigation.PopModalAsync();
            _completion.TrySetResult(true);
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

        if (!_completion.Task.IsCompleted && !_isClosing)
            _completion.TrySetResult(false);
    }
}
