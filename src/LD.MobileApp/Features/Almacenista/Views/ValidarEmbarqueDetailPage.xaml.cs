using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using MauiAppLogin.Features.Almacenista.Models;
using Microsoft.Maui.Graphics.Platform;
using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class ValidarEmbarqueDetailPage : ContentPage, IQueryAttributable
{
    private const float ValidationPhotoMaxSize = 1920f;
    private const float ValidationPhotoQuality = 0.86f;

#if ANDROID
    private static readonly object FeedbackPlayersLock = new();
    private static readonly List<Android.Media.MediaPlayer> FeedbackPlayers = new();
#endif

    private readonly KittingService _kittingService;
    private readonly KittingDetailService _kittingDetailService;
    private readonly KittingIssueService _kittingIssueService;
    private readonly ObservableCollection<PickingKittingIssueItem> _pendingItems = new();
    private readonly ObservableCollection<PickingKittingIssueItem> _validatedItems = new();

    private int _kittingId;
    private string _kittingCode = string.Empty;
    private bool _loaded;
    private bool _showPendingTab = true;
    private string _statusMessage = "Escanea una etiqueta para validar.";
    private bool _statusIsError;
    private string _kittingSummary = string.Empty;
    private readonly ObservableCollection<ValidarEmbarquePhotoItem> _validationPhotos = new();

    public ValidarEmbarqueDetailPage(
        KittingService kittingService,
        KittingDetailService kittingDetailService,
        KittingIssueService kittingIssueService)
    {
        InitializeComponent();
        BindingContext = this;

        _kittingService = kittingService;
        _kittingDetailService = kittingDetailService;
        _kittingIssueService = kittingIssueService;

        PendingCollection.ItemsSource = _pendingItems;
        ValidatedCollection.ItemsSource = _validatedItems;
        ValidationPhotosCollection.ItemsSource = _validationPhotos;
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set
        {
            if (_statusMessage == value)
                return;

            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    public bool StatusIsError
    {
        get => _statusIsError;
        private set
        {
            if (_statusIsError == value)
                return;

            _statusIsError = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(StatusBoxBackground));
            OnPropertyChanged(nameof(StatusBoxStroke));
            OnPropertyChanged(nameof(StatusTextColor));
        }
    }

    public Color StatusBoxBackground => StatusIsError ? Color.FromArgb("#FEF2F2") : Color.FromArgb("#EFF6FF");
    public Color StatusBoxStroke => StatusIsError ? Color.FromArgb("#FCA5A5") : Color.FromArgb("#BFDBFE");
    public Color StatusTextColor => StatusIsError ? Color.FromArgb("#B91C1C") : Color.FromArgb("#1D4ED8");
    public string HeaderTitle => string.IsNullOrWhiteSpace(_kittingCode)
        ? "Validar"
        : $"Validar {_kittingCode}";

    public string KittingSummary
    {
        get => _kittingSummary;
        private set
        {
            if (_kittingSummary == value)
                return;

            _kittingSummary = value;
            OnPropertyChanged();
        }
    }

    public string PendingCountText => $"{_pendingItems.Count} registro(s)";
    public string ValidatedCountText => $"{_validatedItems.Count} registro(s)";
    public string TotalCountText => $"{TotalCount} registro(s)";
    public string ScannedCountText => $"{ScannedCount} registro(s)";
    public string ProgressPercentText => $"{Math.Round(ProgressPercent * 100)}%";
    public string ProgressCountText => $"{ScannedCount} de {TotalCount}";
    public string RemainingText => TotalCount == 0
        ? "No hay líneas para validar."
        : $"Faltan {Math.Max(TotalCount - ScannedCount, 0)} etiqueta(s) por validar.";
    public string PendingTabTitle => $"Pendientes ({_pendingItems.Count})";
    public string ValidatedTabTitle => $"Validadas ({_validatedItems.Count})";
    public double ProgressPercent => TotalCount <= 0 ? 0 : (double)ScannedCount / TotalCount;
    public int TotalCount => _pendingItems.Count + _validatedItems.Count;
    public int ScannedCount => _validatedItems.Count;
    public bool IsPendingVisible => _showPendingTab;
    public bool IsValidatedVisible => !_showPendingTab;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("KittingId", out var kittingIdValue) &&
            int.TryParse(kittingIdValue?.ToString(), out var kittingId))
        {
            _kittingId = kittingId;
        }

        if (query.TryGetValue("KittingCode", out var kittingCodeValue))
        {
            _kittingCode = kittingCodeValue?.ToString()?.Trim() ?? string.Empty;
        }

        UpdateHeader();
        _loaded = false;
        SetTabSelection(true);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_loaded)
        {
            EscaneoEntry.Focus();
            return;
        }

        _loaded = true;
        await LoadValidationAsync();
        EscaneoEntry.Focus();
    }

    private async Task LoadValidationAsync()
    {
        if (_kittingId <= 0)
        {
            await DisplayAlertAsync("Validar embarque", "No se recibió un embarque válido.", "OK");
            await Shell.Current.GoToAsync("..");
            return;
        }

        try
        {
            var kittingResponse = await _kittingService.GetKittingById(_kittingId);
            if (!kittingResponse.IsSuccess || kittingResponse.Data is null)
            {
                await DisplayAlertAsync("Validar embarque", kittingResponse.Message ?? "No se pudo cargar el embarque.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            ApplyKitting(kittingResponse.Data);
            await RefreshValidationPhotosAsync();
            await LoadIssueListsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private void ApplyKitting(KittingRequest kitting)
    {
        _kittingCode = string.IsNullOrWhiteSpace(kitting.KittingCode)
            ? _kittingCode
            : kitting.KittingCode.Trim();

        OnPropertyChanged(nameof(HeaderTitle));

        KittingSummary = $"{_kittingCode} • KittingId: {kitting.KittingId} • ClienteId: {kitting.ClientId} • ProyectoId: {kitting.ProjectId}";
    }

    private async Task RefreshValidationPhotosAsync()
    {
        try
        {
            var response = await _kittingService.GetValidationPhotos(_kittingId);
            if (!response.IsSuccess || response.Data is null)
            {
                await SetErrorStatusAsync(response.Message ?? "No se pudieron cargar las fotos de validación.", playSound: false);
                return;
            }

            var nextPhotos = new List<ValidarEmbarquePhotoItem>();
            foreach (var photo in response.Data.OrderBy(x => x.SortOrder))
            {
                nextPhotos.Add(new ValidarEmbarquePhotoItem
                {
                    PhotoKey = photo.PhotoKey,
                    SortOrder = photo.SortOrder,
                    RelativePath = photo.RelativePath,
                    ImageUrl = string.IsNullOrWhiteSpace(photo.ImageUrl)
                        ? _kittingService.GetImageUrl(photo.RelativePath)
                        : photo.ImageUrl,
                    IsLegacy = photo.IsLegacy
                });
            }

            _validationPhotos.Clear();
            foreach (var photo in nextPhotos)
            {
                _validationPhotos.Add(photo);
            }
        }
        catch (Exception ex)
        {
            await SetErrorStatusAsync($"Ocurrió un error al cargar las fotos: {ex.Message}", playSound: false);
        }
    }

    private void SetSuccessStatus(string message)
    {
        StatusIsError = false;
        StatusMessage = message;
    }

    private async Task SetErrorStatusAsync(string message, bool playSound = true, bool showDialog = false, string dialogTitle = "Error")
    {
        StatusIsError = true;
        StatusMessage = message;

        if (playSound)
            await PlayFeedbackSoundAsync("error.mp3");

        if (showDialog)
            await DisplayAlertAsync(dialogTitle, message, "OK");
    }

    private Task SetScanNotFoundStatusAsync(string scanValue)
    {
        return SetErrorStatusAsync(
            $"No se encontró una línea de Validación con ese StandardIdStr: {scanValue}",
            playSound: true);
    }

    private async Task LoadIssueListsAsync()
    {
        try
        {
            _pendingItems.Clear();
            _validatedItems.Clear();
            NotifyCountsChanged();

            var detailsResponse = await _kittingDetailService.GetKittingDetailsByKittingId(_kittingId);
            if (!detailsResponse.IsSuccess || detailsResponse.Data is null)
            {
                StatusMessage = detailsResponse.Message ?? "No se pudieron cargar los detalles del embarque.";
                return;
            }

            var detailIds = detailsResponse.Data
                .Where(x => x.KittingDetailId > 0)
                .Select(x => x.KittingDetailId)
                .ToList();

            if (detailIds.Count == 0)
            {
                StatusMessage = "El embarque no tiene lineas para validar.";
                return;
            }

            var issueTasks = detailIds.Select(async detailId =>
            {
                var issuesResponse = await _kittingIssueService.GetKittingIssuesByKittingDetailId(detailId);
                return issuesResponse.IsSuccess && issuesResponse.Data != null
                    ? issuesResponse.Data
                    : new List<LD.Contracts.Kitting.KittingIssueDetailDto>();
            });

            var issues = (await Task.WhenAll(issueTasks))
                .SelectMany(x => x)
                .ToList();

            foreach (var issue in issues
                .Where(x => IsSupplyStatus(x.SupplyStatus, KittingStatusNames.Validacion))
                .OrderBy(x => x.StandardIdStr ?? string.Empty)
                .ThenBy(x => x.PartNumber ?? string.Empty)
                .ThenBy(x => x.ReceivedQuantity ?? 0m))
            {
                _pendingItems.Add(BuildIssueItem(issue, _kittingCode));
            }

            foreach (var issue in issues
                .Where(x => IsSupplyStatus(x.SupplyStatus, KittingStatusNames.Cargando))
                .OrderBy(x => x.StandardIdStr ?? string.Empty)
                .ThenBy(x => x.PartNumber ?? string.Empty)
                .ThenBy(x => x.ReceivedQuantity ?? 0m))
            {
                _validatedItems.Add(BuildIssueItem(issue, _kittingCode));
            }

            NotifyCountsChanged();
            StatusMessage = $"Listo. Validación: {_pendingItems.Count} | Cargando: {_validatedItems.Count}.";
            UpdateTabVisuals();
        }
        catch (Exception ex)
        {
            StatusMessage = "Ocurrió un error al cargar los issues del embarque.";
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async void OnEscaneoCompleted(object sender, EventArgs e)
    {
        var scanValue = EscaneoEntry.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(scanValue))
            return;

        await ProcessScanAsync(scanValue);
        EscaneoEntry.Text = string.Empty;
        EscaneoEntry.Focus();
    }

    private async void OnScanIconTapped(object sender, EventArgs e)
    {
        await OpenScannerAsync();
    }

    private async Task OpenScannerAsync()
    {
        var page = new Scan3FieldsPage(requiresThreeFields: false);
        await Navigation.PushModalAsync(page);

        var accepted = await page.WaitForResultAsync();
        if (!accepted || string.IsNullOrWhiteSpace(page.EstandarId))
            return;

        EscaneoEntry.Text = page.EstandarId;
        await ProcessScanAsync(page.EstandarId);
        EscaneoEntry.Text = string.Empty;
        EscaneoEntry.Focus();
    }

    private async Task ProcessScanAsync(string scanValue)
    {
        var match = FindMatchingIssue(scanValue);
        if (match is null)
        {
            var alreadyValidated = _validatedItems.FirstOrDefault(issue => MatchesScan(issue.StandardId, scanValue));
            if (alreadyValidated is not null)
            {
                await SetErrorStatusAsync($"La línea {alreadyValidated.PartNumber} ya está en Cargando.", playSound: true);
                return;
            }

            await SetScanNotFoundStatusAsync(scanValue);
            return;
        }

        if (IsSupplyStatus(match.SupplyStatus, KittingStatusNames.Cargando))
        {
            await SetErrorStatusAsync($"La línea {match.PartNumber} ya está en Cargando.", playSound: true);
            return;
        }

        if (!IsSupplyStatus(match.SupplyStatus, KittingStatusNames.Validacion))
        {
            await SetErrorStatusAsync($"La línea {match.StandardIdStr} no está en estatus Validación.", playSound: true);
            return;
        }

        if (!MediaPicker.Default.IsCaptureSupported)
        {
            await SetErrorStatusAsync("Este dispositivo no soporta captura de fotos.", playSound: true);
            return;
        }

        var photoPath = await CaptureValidationPhotoPathAsync();
        if (string.IsNullOrWhiteSpace(photoPath))
            return;

        var uploadedPhoto = await UploadValidationPhotoAsync(photoPath);
        if (uploadedPhoto is null)
            return;

        var request = new KittingIssueValidateRequest
        {
            StandardIdStr = scanValue.Trim()
        };

        var response = await _kittingIssueService.ValidateKittingIssue(match.KittingReceiptDetailId, request);
        if (!response.IsSuccess)
        {
            await DeleteValidationPhotoSilentlyAsync(uploadedPhoto.PhotoKey);
            await SetErrorStatusAsync(response.ErrorMessage ?? response.Message ?? "No se pudo validar la línea.", playSound: true);
            return;
        }

        await RefreshIssueRowFromServerAsync(match);
        await RefreshValidationPhotosAsync();
        MoveToValidated(match);

        SetSuccessStatus(response.Message ?? $"Cargando: {match.StandardIdStr} | Cantidad: {match.ReceivedQuantity}");
        NotifyCountsChanged();

        if (_pendingItems.Count == 0)
        {
            StatusMessage = "Todas las líneas quedaron en Cargando.";
        }
    }

    private PickingKittingIssueItem? FindMatchingIssue(string scanValue)
    {
        return _pendingItems.FirstOrDefault(issue =>
            MatchesScan(issue.StandardId, scanValue));
    }

    private static bool MatchesScan(string? source, string scanValue) =>
        !string.IsNullOrWhiteSpace(source)
        && string.Equals(source.Trim(), scanValue.Trim(), StringComparison.OrdinalIgnoreCase);

    private async Task RefreshIssueRowFromServerAsync(PickingKittingIssueItem issueRow)
    {
        if (issueRow.KittingReceiptDetailId <= 0)
            return;

        var response = await _kittingIssueService.GetKittingIssueById(issueRow.KittingReceiptDetailId);
        if (!response.IsSuccess || response.Data == null)
            return;

        ApplyIssueRequest(issueRow, response.Data);
    }

    private static void ApplyIssueRequest(PickingKittingIssueItem issueRow, KittingIssueRequest request)
    {
        issueRow.KittingReceiptDetailId = request.KittingReceiptDetailId;
        issueRow.KittingDetailId = request.KittingDetailId;
        issueRow.StandardId = request.StandardId ?? string.Empty;
        issueRow.StandardIdStr = request.StandardId ?? string.Empty;
        issueRow.SupplyStatus = request.SupplyStatus ?? string.Empty;
        issueRow.PartNumber = request.PartNumber ?? string.Empty;
        issueRow.ReceivedQuantity = request.ReceivedQuantity.GetValueOrDefault();
        issueRow.Title = $"StandardId: {issueRow.StandardId}";
        issueRow.Subtitle = $"Parte: {issueRow.PartNumber}";
    }

    private void MoveToValidated(PickingKittingIssueItem issueRow)
    {
        var itemToMove = _pendingItems.FirstOrDefault(x => x.KittingReceiptDetailId == issueRow.KittingReceiptDetailId);
        if (itemToMove is null)
            return;

        _pendingItems.Remove(itemToMove);
        if (_validatedItems.All(x => x.KittingReceiptDetailId != itemToMove.KittingReceiptDetailId))
        {
            _validatedItems.Add(itemToMove);
        }

        NotifyCountsChanged();
    }

    private static PickingKittingIssueItem BuildIssueItem(LD.Contracts.Kitting.KittingIssueDetailDto issue, string kittingCode)
    {
        var standardId = ResolveStandardId(issue);
        var partNumber = issue.PartNumber?.Trim();
        var location = issue.LocationCode?.Trim();
        var quantity = issue.ReceivedQuantity.GetValueOrDefault();
        var standardText = string.IsNullOrWhiteSpace(standardId) ? "Sin StandardId" : standardId;
        var partText = string.IsNullOrWhiteSpace(partNumber) ? "Sin número de parte" : partNumber;
        var locationText = string.IsNullOrWhiteSpace(location) ? "Sin ubicación" : location;

        return new PickingKittingIssueItem
        {
            KittingReceiptDetailId = issue.KittingReceiptDetailId,
            KittingDetailId = issue.KittingDetailId,
            StandardId = standardId,
            StandardIdStr = standardId,
            SupplyStatus = issue.SupplyStatus ?? string.Empty,
            PartNumber = partText,
            ReceivedQuantity = quantity,
            QuantityText = $"Cantidad: {quantity}",
            LocationText = $"Ubicación: {locationText}",
            Title = $"StandardId: {standardText}",
            Subtitle = $"Parte: {partText}",
            InstructionText = $"Kit {kittingCode} - StandardId {standardText} - Parte {partText} - Cantidad {quantity} - Ubicación {locationText}"
        };
    }

    private static string ResolveStandardId(LD.Contracts.Kitting.KittingIssueDetailDto issue)
    {
        var standardId = issue.StandardId?.Trim();
        if (!string.IsNullOrWhiteSpace(standardId))
            return standardId;

        standardId = issue.StandardIdStr?.Trim();
        return standardId ?? string.Empty;
    }

    private static bool IsSupplyStatus(string? value, string expected) =>
        string.Equals(value?.Trim(), expected, StringComparison.OrdinalIgnoreCase) ||
        (string.Equals(expected, KittingStatusNames.Validacion, StringComparison.OrdinalIgnoreCase) &&
         string.Equals(value?.Trim(), KittingStatusNames.LegacyValidacion, StringComparison.OrdinalIgnoreCase)) ||
        (string.Equals(expected, KittingStatusNames.Cargando, StringComparison.OrdinalIgnoreCase) &&
         string.Equals(value?.Trim(), KittingStatusNames.LegacyCargando, StringComparison.OrdinalIgnoreCase));

    private void UpdateHeader()
    {
        KittingSummary = string.IsNullOrWhiteSpace(_kittingCode)
            ? "Kit: -"
            : $"Kit: {_kittingCode}";
        OnPropertyChanged(nameof(HeaderTitle));
    }

    private async void OnAddPhotoClicked(object sender, EventArgs e)
    {
        if (!MediaPicker.Default.IsCaptureSupported)
        {
            await DisplayAlertAsync("Cámara", "Este dispositivo no soporta captura de fotos.", "OK");
            return;
        }

        await CaptureAndUploadPhotoAsync();
    }

    private async void OnReplacePhotoClicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is null)
            return;

        var photoKey = button.CommandParameter.ToString() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(photoKey))
            return;

        if (!MediaPicker.Default.IsCaptureSupported)
        {
            await DisplayAlertAsync("Camara", "Este dispositivo no soporta captura de fotos.", "OK");
            return;
        }

        try
        {
            var photoPath = await CaptureValidationPhotoPathAsync();
            if (string.IsNullOrWhiteSpace(photoPath))
                return;

            var response = await _kittingService.ReplaceValidationImage(_kittingId, photoKey, photoPath);
            if (!response.IsSuccess || response.Data is null)
            {
                await SetErrorStatusAsync(response.Message ?? "No se pudo reemplazar la foto.", playSound: true);
                return;
            }

            await RefreshValidationPhotosAsync();
            SetSuccessStatus("Foto reemplazada correctamente.");
        }
        catch (Exception ex)
        {
            await SetErrorStatusAsync(ex.Message, playSound: true);
        }
    }

    private async void OnDeletePhotoClicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is null)
            return;

        var photoKey = button.CommandParameter.ToString() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(photoKey))
            return;

        var confirm = await DisplayAlertAsync("Eliminar foto", "¿Deseas eliminar esta foto de validación?", "Sí", "No");
        if (!confirm)
            return;

        try
        {
            var response = await _kittingService.DeleteValidationImage(_kittingId, photoKey);
            if (!response.IsSuccess)
            {
                await SetErrorStatusAsync(response.Message ?? "No se pudo eliminar la foto.", playSound: true);
                return;
            }

            await RefreshValidationPhotosAsync();
            SetSuccessStatus("Foto eliminada correctamente.");
        }
        catch (Exception ex)
        {
            await SetErrorStatusAsync(ex.Message, playSound: true);
        }
    }

    private async Task CaptureAndUploadPhotoAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlertAsync("Cámara", "Este dispositivo no soporta captura de fotos.", "OK");
                return;
            }

            var photoPath = await CaptureValidationPhotoPathAsync();
            if (string.IsNullOrWhiteSpace(photoPath))
                return;

            var uploadedPhoto = await UploadValidationPhotoAsync(photoPath);
            if (uploadedPhoto is null)
                return;

            await RefreshValidationPhotosAsync();
            SetSuccessStatus("Foto cargada correctamente.");
        }
        catch (Exception ex)
        {
            await SetErrorStatusAsync(ex.Message, playSound: true);
        }
    }

    private async Task<ValidarEmbarquePhotoItem?> UploadValidationPhotoAsync(string photoPath)
    {
        try
        {
            var response = await _kittingService.UploadValidationImage(_kittingId, photoPath);
            if (!response.IsSuccess || response.Data is null || string.IsNullOrWhiteSpace(response.Data.PhotoKey))
            {
                await SetErrorStatusAsync(response.Message ?? "No se pudo cargar la foto.", playSound: true);
                return null;
            }

            return new ValidarEmbarquePhotoItem
            {
                PhotoKey = response.Data.PhotoKey,
                SortOrder = response.Data.SortOrder,
                RelativePath = response.Data.RelativePath ?? string.Empty,
                ImageUrl = string.IsNullOrWhiteSpace(response.Data.ImageUrl)
                    ? _kittingService.GetImageUrl(response.Data.RelativePath)
                    : response.Data.ImageUrl,
                IsLegacy = response.Data.IsLegacy
            };
        }
        catch (Exception ex)
        {
            await SetErrorStatusAsync(ex.Message, playSound: true);
            return null;
        }
    }

    private async Task DeleteValidationPhotoSilentlyAsync(string photoKey)
    {
        if (string.IsNullOrWhiteSpace(photoKey))
            return;

        try
        {
            await _kittingService.DeleteValidationImage(_kittingId, photoKey);
        }
        catch
        {
        }
    }

    private static async Task<string?> CaptureValidationPhotoPathAsync()
    {
        try
        {
            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null)
                return null;

            return await CompressPhotoAsync(photo);
        }
        catch
        {
            return null;
        }
    }

    private static async Task<string> CompressPhotoAsync(FileResult photo)
    {
        try
        {
            await using var input = await photo.OpenReadAsync();
            using var image = PlatformImage.FromStream(input);
            if (image is null)
                return photo.FullPath;

            using var resized = image.Downsize(ValidationPhotoMaxSize);
            var outputPath = Path.Combine(
                FileSystem.CacheDirectory,
                $"validation-photo-{Guid.NewGuid():N}.jpg");

            await using var output = File.Create(outputPath);
            resized.Save(output, ImageFormat.Jpeg, ValidationPhotoQuality);

            return outputPath;
        }
        catch
        {
            return photo.FullPath;
        }
    }

    private void OnPendingTabClicked(object sender, EventArgs e)
    {
        SetTabSelection(true);
    }

    private void OnValidatedTabClicked(object sender, EventArgs e)
    {
        SetTabSelection(false);
    }

    private void SetTabSelection(bool showPending)
    {
        _showPendingTab = showPending;
        OnPropertyChanged(nameof(IsPendingVisible));
        OnPropertyChanged(nameof(IsValidatedVisible));
        UpdateTabVisuals();
    }

    private void UpdateTabVisuals()
    {
        if (PendingTabButton is null || ValidatedTabButton is null)
            return;

        PendingTabButton.BackgroundColor = _showPendingTab ? Color.FromArgb("#E8F0FF") : Color.FromArgb("#F8FAFC");
        PendingTabButton.TextColor = _showPendingTab ? Color.FromArgb("#1D4ED8") : Color.FromArgb("#475569");

        ValidatedTabButton.BackgroundColor = _showPendingTab ? Color.FromArgb("#F8FAFC") : Color.FromArgb("#ECFDF5");
        ValidatedTabButton.TextColor = _showPendingTab ? Color.FromArgb("#475569") : Color.FromArgb("#047857");

        OnPropertyChanged(nameof(PendingTabTitle));
        OnPropertyChanged(nameof(ValidatedTabTitle));
        OnPropertyChanged(nameof(TotalCountText));
        OnPropertyChanged(nameof(ScannedCountText));
        OnPropertyChanged(nameof(ProgressPercentText));
        OnPropertyChanged(nameof(ProgressCountText));
        OnPropertyChanged(nameof(RemainingText));
        OnPropertyChanged(nameof(ProgressPercent));
    }

    private void NotifyCountsChanged()
    {
        OnPropertyChanged(nameof(PendingCountText));
        OnPropertyChanged(nameof(ValidatedCountText));
        OnPropertyChanged(nameof(PendingTabTitle));
        OnPropertyChanged(nameof(ValidatedTabTitle));
        OnPropertyChanged(nameof(TotalCountText));
        OnPropertyChanged(nameof(ScannedCountText));
        OnPropertyChanged(nameof(ProgressPercentText));
        OnPropertyChanged(nameof(ProgressCountText));
        OnPropertyChanged(nameof(RemainingText));
        OnPropertyChanged(nameof(ProgressPercent));
    }

    private static async Task PlayFeedbackSoundAsync(string assetName)
    {
#if ANDROID
        try
        {
            if (await TryPlayAndroidPackagedSoundAsync(assetName))
                return;
        }
        catch
        {
        }

        PlayAndroidFallbackTone();
#else
        await Task.CompletedTask;
#endif
    }

#if ANDROID
    private static async Task<bool> TryPlayAndroidPackagedSoundAsync(string assetName)
    {
        var audioPath = Path.Combine(FileSystem.CacheDirectory, assetName);
        if (!File.Exists(audioPath))
        {
            await using var input = await FileSystem.OpenAppPackageFileAsync(assetName);
            await using var output = File.Create(audioPath);
            await input.CopyToAsync(output);
        }

        var player = new Android.Media.MediaPlayer();
        player.SetAudioStreamType(Android.Media.Stream.Music);
        player.SetDataSource(audioPath);
        player.Prepare();

        lock (FeedbackPlayersLock)
        {
            FeedbackPlayers.Add(player);
        }

        player.Completion += (_, _) => ReleaseFeedbackPlayer(player);
        player.Error += (_, _) =>
        {
            ReleaseFeedbackPlayer(player);
            PlayAndroidFallbackTone();
        };

        player.Start();
        return true;
    }

    private static void ReleaseFeedbackPlayer(Android.Media.MediaPlayer player)
    {
        lock (FeedbackPlayersLock)
        {
            FeedbackPlayers.Remove(player);
        }

        player.Release();
    }

    private static void PlayAndroidFallbackTone()
    {
        try
        {
            var tone = new Android.Media.ToneGenerator(Android.Media.Stream.Notification, 100);
            tone.StartTone(Android.Media.Tone.PropNack, 250);

            _ = Task.Delay(350).ContinueWith(_ => tone.Release());
        }
        catch
        {
        }
    }
#endif

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
