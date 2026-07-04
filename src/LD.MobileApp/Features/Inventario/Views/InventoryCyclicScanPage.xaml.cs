using System;
using System.Collections.ObjectModel;
using System.Linq;
using LD.Client.Services;
using LD.Contracts.InventarioCiclico;
using LD.Contracts.Requests;
using MauiAppLogin.Controls;
using MauiAppLogin.Features.Inventario.Models;

namespace MauiAppLogin;

public partial class InventoryCyclicScanPage : ContentPage
{
    private const string LocationPromptText = "Escanea o escribe la ubicacion. Si empieza con INV, se quitara ese prefijo antes de validar.";
    private readonly CyclicInventoryService _cyclicInventoryService;
    private readonly IDialogService _dialogService;
    private readonly ObservableCollection<InventoryScanItem> _scans = new();
    private InventoryGroup? _inventory;
    private InventoryDetail? _detail;
    private string _locationText = LocationPromptText;
    private string _statusMessage = "Escanea o escribe la ubicacion para continuar.";
    private bool _statusIsError;
    private bool _isSaving;
    private bool _isLoadingScans;
    private bool _scansLoaded;

    public InventoryCyclicScanPage(
        CyclicInventoryService cyclicInventoryService,
        IDialogService dialogService)
    {
        InitializeComponent();
        BindingContext = this;
        _cyclicInventoryService = cyclicInventoryService;
        _dialogService = dialogService;
        ScansCollection.ItemsSource = _scans;
    }

    public string LocationText
    {
        get => _locationText;
        private set
        {
            if (_locationText == value)
                return;

            _locationText = value;
            OnPropertyChanged();
        }
    }

    public bool CanEditLocation => _inventory is not null && !_isSaving && !_isLoadingScans;

    public bool CanScanStandardId => _detail is not null && _scansLoaded && !_detail.Escaneado && !_isSaving && !_isLoadingScans;

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
    public string ScansCountText => _scans.Count.ToString();
    public string FinishLocationButtonText => _detail?.Escaneado == true ? "Ubicacion cerrada" : "Terminar ubicacion";

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_inventory is null)
        {
            RefreshInteractionState(focusLocation: true);
            return;
        }

        if (_detail is null)
        {
            RefreshInteractionState(focusLocation: true);
            return;
        }

        if (!_scansLoaded && !_isLoadingScans)
        {
            _ = LoadScansAsync();
            return;
        }

        RefreshInteractionState(
            focusStandardId: CanScanStandardId,
            focusLocation: _detail.Escaneado);
    }

    public void SetContext(InventoryGroup inventory)
    {
        _inventory = inventory;
        _detail = null;
        _scansLoaded = false;

        LocationText = LocationPromptText;

        _scans.Clear();
        StatusIsError = false;
        StatusMessage = "Escanea o escribe la ubicacion para continuar.";
        OnPropertyChanged(nameof(ScansCountText));

        if (LocationEntry is not null)
        {
            LocationEntry.Text = string.Empty;
        }

        if (StandardIdEntry is not null)
        {
            StandardIdEntry.Text = string.Empty;
        }

        RefreshInteractionState(focusLocation: true);
    }

    private async Task LoadScansAsync(bool updateStatus = true)
    {
        if (_inventory is null || _detail is null || _isLoadingScans)
            return;

        try
        {
            SetLoadingScans(true);

            var response = await _cyclicInventoryService.GetCyclicInventoryScans(
                _inventory.InventarioCiclicoId,
                _detail.InventarioCiclicoDetalleId);

            if (!response.IsSuccess)
            {
                _scansLoaded = false;
                SetErrorStatus(response.Message ?? "No se pudieron cargar los escaneos guardados.");
                return;
            }

            _scans.Clear();
            var scans = (response.Data ?? new List<CyclicInventoryScanDto>())
                .OrderBy(x => x.ScannedAt)
                .ThenBy(x => x.CyclicInventoryScanId)
                .ToList();

            foreach (var scan in scans)
            {
                _scans.Add(MapScan(scan, _scans.Count + 1));
            }

            _scansLoaded = true;
            OnPropertyChanged(nameof(ScansCountText));

            if (updateStatus)
            {
                if (_detail.Escaneado)
                {
                    SetSuccessStatus("La ubicacion ya esta cerrada.");
                }
                else if (_scans.Count == 0)
                {
                    SetSuccessStatus("Ubicacion validada. Escanea un StandardId.");
                }
                else
                {
                    SetSuccessStatus($"{_scans.Count} escaneo(s) guardado(s).");
                }
            }
        }
        catch (Exception ex)
        {
            _scansLoaded = false;
            SetErrorStatus($"Error al cargar los escaneos: {ex.Message}");
        }
        finally
        {
            SetLoadingScans(false);
            RefreshInteractionState(
                focusStandardId: _detail is not null && _scansLoaded && !_detail.Escaneado,
                focusLocation: _detail is not null && (_detail.Escaneado || !_scansLoaded));
        }
    }

    private async void OnScanTapped(object sender, TappedEventArgs e)
    {
        if (_isSaving || _isLoadingScans)
            return;

        InventoryScanItem? item = null;

        if (sender is Border border && border.BindingContext is InventoryScanItem borderItem)
        {
            item = borderItem;
        }
        else if (sender is TapGestureRecognizer tap && tap.CommandParameter is InventoryScanItem tapItem)
        {
            item = tapItem;
        }

        if (item is null || _inventory is null || _detail is null)
            return;

        if (_detail.Escaneado)
        {
            SetSuccessStatus("La ubicacion ya esta cerrada. No se pueden eliminar escaneos.");
            RefreshInteractionState();
            return;
        }

        var confirm = await DisplayAlertAsync(
            "Eliminar escaneo",
            $"Deseas eliminar el escaneo {item.StandardId}?",
            "Si",
            "No");

        if (!confirm)
            return;

        await DeleteScanAsync(item);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (_isSaving)
            return;

        await GoBackAsync();
    }

    private async void OnLocationCompleted(object sender, EventArgs e)
    {
        if (_isSaving || _isLoadingScans)
            return;

        if (_inventory is null)
        {
            await HandleLocationErrorAsync("No se pudo identificar el inventario.");
            return;
        }

        var locationCode = NormalizeLocationCode(LocationEntry?.Text);
        if (string.IsNullOrWhiteSpace(locationCode))
        {
            await HandleLocationErrorAsync("Captura o escanea una ubicacion.");
            return;
        }

        var detail = FindDetailByLocation(locationCode);
        if (detail is null)
        {
            await HandleLocationErrorAsync($"La ubicacion {locationCode} no existe en este inventario.");
            return;
        }

        _detail = detail;
        _scans.Clear();
        _scansLoaded = false;
        OnPropertyChanged(nameof(ScansCountText));

        if (LocationEntry is not null)
        {
            LocationEntry.Text = detail.Ubicacion.Trim();
        }

        if (StandardIdEntry is not null)
        {
            StandardIdEntry.Text = string.Empty;
        }

        LocationText = $"Ubicacion validada: {detail.Ubicacion.Trim()}";
        StatusIsError = false;
        StatusMessage = "Cargando escaneos guardados...";

        await LoadScansAsync();
    }

    private async Task HandleLocationErrorAsync(string message)
    {
        SetErrorStatus(message);
        LocationText = LocationPromptText;

        if (LocationEntry is not null)
        {
            LocationEntry.Text = string.Empty;
        }

        if (StandardIdEntry is not null)
        {
            StandardIdEntry.Text = string.Empty;
        }

        await PlayFeedbackSoundAsync("error.mp3");
        RefreshInteractionState(focusLocation: true);
    }

    private async void OnStandardIdCompleted(object sender, EventArgs e)
    {
        if (_isSaving || _isLoadingScans)
            return;

        if (!CanScanStandardId)
        {
            SetErrorStatus("Valida la ubicacion antes de escanear StandardId.");
            RefreshInteractionState(focusLocation: true);
            return;
        }

        if (_detail?.Escaneado == true)
        {
            SetSuccessStatus("La ubicacion ya esta cerrada. No se pueden agregar mas escaneos.");
            RefreshInteractionState();
            return;
        }

        var standardId = StandardIdEntry?.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(standardId))
        {
            SetErrorStatus("Captura o escanea un StandardId.");
            StandardIdEntry?.Focus();
            return;
        }

        if (!IsValidStandardId(standardId))
        {
            SetErrorStatus("La etiqueta debe tener al menos 12 digitos numericos.");
            StandardIdEntry?.Focus();
            return;
        }

        if (IsDuplicateStandardId(standardId))
        {
            SetErrorStatus($"La etiqueta {standardId} ya fue escaneada.");
            StandardIdEntry?.Focus();
            return;
        }

        await SaveScanAsync(standardId);
    }

    private async Task SaveScanAsync(string standardId)
    {
        if (_inventory is null || _detail is null)
            return;

        if (_detail.Escaneado)
        {
            SetSuccessStatus("La ubicacion ya esta cerrada. No se pueden agregar mas escaneos.");
            RefreshInteractionState();
            return;
        }

        if (IsDuplicateStandardId(standardId))
        {
            SetErrorStatus($"La etiqueta {standardId} ya fue escaneada.");
            RefreshInteractionState(focusStandardId: true);
            return;
        }

        try
        {
            SetSaving(true);

            var response = await _cyclicInventoryService.CreateCyclicInventoryScan(
                _inventory.InventarioCiclicoId,
                _detail.InventarioCiclicoDetalleId,
                new CreateCyclicInventoryScanRequest
                {
                    StandardId = standardId
                });

            if (!response.IsSuccess)
            {
                SetErrorStatus(response.Message ?? "No se pudo guardar el escaneo.");
                StandardIdEntry?.Focus();
                return;
            }

            var scan = response.Data is null
                ? new CyclicInventoryScanDto
                {
                    StandardId = standardId,
                    ScannedAt = DateTime.Now
                }
                : response.Data;

            _scans.Add(MapScan(scan, _scans.Count + 1));
            OnPropertyChanged(nameof(ScansCountText));
            SetSuccessStatus($"{standardId} agregado a la tabla.");

            if (ScansCollection is not null && _scans.Count > 0)
            {
                ScansCollection.ScrollTo(_scans.Last(), position: ScrollToPosition.End, animate: false);
            }

            if (StandardIdEntry is not null)
            {
                StandardIdEntry.Text = string.Empty;
                StandardIdEntry.Focus();
            }
        }
        catch (Exception ex)
        {
            SetErrorStatus($"Error al guardar el escaneo: {ex.Message}");
            StandardIdEntry?.Focus();
        }
        finally
        {
            SetSaving(false);
            RefreshInteractionState();
        }
    }

    private static InventoryScanItem MapScan(CyclicInventoryScanDto scan, int numero)
    {
        return new InventoryScanItem
        {
            CyclicInventoryScanId = scan.CyclicInventoryScanId,
            Numero = numero,
            StandardId = scan.StandardId,
            Hora = scan.ScannedAt.ToString("dd-MM-yyyy HH:mm")
        };
    }

    private async Task DeleteScanAsync(InventoryScanItem item)
    {
        if (_inventory is null || _detail is null)
            return;

        if (_detail.Escaneado)
        {
            SetSuccessStatus("La ubicacion ya esta cerrada. No se pueden eliminar escaneos.");
            RefreshInteractionState();
            return;
        }

        try
        {
            SetSaving(true);

            var response = await _cyclicInventoryService.DeleteCyclicInventoryScan(
                _inventory.InventarioCiclicoId,
                _detail.InventarioCiclicoDetalleId,
                item.CyclicInventoryScanId);

            if (!response.IsSuccess)
            {
                SetErrorStatus(response.Message ?? "No se pudo eliminar el escaneo.");
                StandardIdEntry?.Focus();
                return;
            }

            await LoadScansAsync(updateStatus: false);
            SetSuccessStatus($"Escaneo {item.StandardId} eliminado.");
        }
        catch (Exception ex)
        {
            SetErrorStatus($"Error al eliminar el escaneo: {ex.Message}");
        }
        finally
        {
            SetSaving(false);
            RefreshInteractionState(focusStandardId: true);
        }
    }

    private async void OnFinishLocationClicked(object sender, EventArgs e)
    {
        if (_isSaving)
            return;

        if (_inventory is null || _detail is null)
        {
            await _dialogService.ShowErrorAsync("Inventario", "No se pudo identificar la ubicacion actual.");
            return;
        }

        if (_detail.Escaneado)
        {
            SetSuccessStatus("La ubicacion ya esta cerrada.");
            RefreshInteractionState();
            return;
        }

        var confirm = await DisplayAlertAsync(
            "Cerrar ubicacion",
            "Deseas cerrar la ubicacion?",
            "Si",
            "No");

        if (!confirm)
            return;

        try
        {
            SetSaving(true);

            var response = await _cyclicInventoryService.FinishCyclicInventoryLocation(
                _inventory.InventarioCiclicoId,
                _detail.InventarioCiclicoDetalleId);

            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Inventario", response.Message ?? "No se pudo terminar la ubicacion.");
                return;
            }

            _detail.Escaneado = true;
            _detail.Tomada = true;
            _inventory.Escaneados = _inventory.Detalles.Count(x => x.Escaneado);
            _inventory.Completados = _inventory.Detalles.Count(x => x.Tomada);
            _inventory.Cantidad = $"{_inventory.Escaneados}/{_inventory.Detalles.Count}";
            RefreshInteractionState();

            _scans.Clear();
            foreach (var scan in (response.Data ?? new List<CyclicInventoryScanDto>())
                         .OrderBy(x => x.ScannedAt)
                         .ThenBy(x => x.CyclicInventoryScanId))
            {
                _scans.Add(MapScan(scan, _scans.Count + 1));
            }

            OnPropertyChanged(nameof(ScansCountText));
            await _dialogService.ShowSuccessAsync("Ubicacion terminada", "La ubicacion se marco como escaneada.");
            await GoBackAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Inventario", $"Error al terminar la ubicacion: {ex.Message}");
        }
        finally
        {
            SetSaving(false);
            RefreshInteractionState(focusStandardId: true);
        }
    }

    private async Task GoBackAsync()
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
            return;
        }

        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
    }

    private InventarioCiclicoRequest BuildUpdateRequest()
    {
        var detalles = _inventory!.Detalles
            .Select(x => new CyclicInventoryDetailDto
            {
                InventarioCiclicoDetalleId = x.InventarioCiclicoDetalleId,
                InventarioCiclicoId = _inventory.InventarioCiclicoId,
                LocationId = x.LocationId,
                TakeNumber = x.TakeNumber <= 0 ? 1 : x.TakeNumber,
                Ubicacion = x.Ubicacion,
                Tomada = x.Tomada,
                Teorico = x.Teorico,
                Fisico = x.Fisico,
                MismaUbicacion = x.MismaUbicacion,
                EnOtraUbicacion = x.EnOtraUbicacion,
                ResultadoPrimeraToma = x.ResultadoPrimeraToma,
                ResultadoSegundaToma = x.ResultadoSegundaToma,
                ResultadoTerceraToma = x.ResultadoTerceraToma,
                ResultadoCuartaToma = x.ResultadoCuartaToma,
                ResultadoFinal = x.ResultadoFinal,
                PartNumber = x.PartNumber,
                Escaneado = x.Escaneado
            })
            .ToList();

        return new InventarioCiclicoRequest
        {
            InventarioCiclicoId = _inventory.InventarioCiclicoId,
            Fecha = _inventory.Fecha,
            AuditorUserId = _inventory.AuditorUserId,
            AuditorNombre = _inventory.Auditor,
            WarehouseId = _inventory.WarehouseId,
            Estatus = _inventory.Estatus,
            FechaTerminado = _inventory.FechaTerminado,
            LocationIds = _inventory.Detalles
                .Select(x => x.LocationId)
                .Distinct()
                .ToList(),
            Detalles = detalles
        };
    }

    private void SetSuccessStatus(string message)
    {
        StatusIsError = false;
        StatusMessage = message;
    }

    private void SetErrorStatus(string message)
    {
        StatusIsError = true;
        StatusMessage = message;
    }

    private void SetSaving(bool isSaving)
    {
        if (_isSaving == isSaving)
            return;

        _isSaving = isSaving;
        RefreshInteractionState();
    }

    private void SetLoadingScans(bool isLoadingScans)
    {
        if (_isLoadingScans == isLoadingScans)
            return;

        _isLoadingScans = isLoadingScans;
        RefreshInteractionState();
    }

    private void RefreshInteractionState(bool focusStandardId = false, bool focusLocation = false)
    {
        OnPropertyChanged(nameof(CanEditLocation));
        OnPropertyChanged(nameof(CanScanStandardId));
        OnPropertyChanged(nameof(FinishLocationButtonText));

        if (focusLocation && LocationEntry is not null && CanEditLocation)
        {
            LocationEntry.Focus();
            return;
        }

        if (focusStandardId && StandardIdEntry is not null && CanScanStandardId)
        {
            StandardIdEntry.Focus();
        }
    }

    private static bool IsValidStandardId(string standardId)
    {
        return standardId.Length >= 12 && standardId.All(char.IsDigit);
    }

    private static string NormalizeLocationCode(string? value)
    {
        var normalized = (value ?? string.Empty).Trim();

        if (normalized.StartsWith("INV", StringComparison.OrdinalIgnoreCase))
        {
            normalized = normalized[3..].TrimStart('-', ' ', ':');
        }

        return normalized.Trim();
    }

    private InventoryDetail? FindDetailByLocation(string locationCode)
    {
        if (_inventory is null)
            return null;

        return _inventory.Detalles
            .Where(detail => string.Equals(detail.Ubicacion?.Trim(), locationCode, StringComparison.OrdinalIgnoreCase))
            .OrderBy(detail => detail.Escaneado ? 1 : 0)
            .ThenBy(detail => detail.TakeNumber)
            .ThenBy(detail => detail.InventarioCiclicoDetalleId)
            .FirstOrDefault();
    }

    private bool IsDuplicateStandardId(string standardId)
    {
        return _scans.Any(x =>
            string.Equals(x.StandardId?.Trim(), standardId.Trim(), StringComparison.OrdinalIgnoreCase));
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
#elif WINDOWS
        try
        {
            Console.Beep();
        }
        catch
        {
        }
#else
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(120));
        }
        catch
        {
        }

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
        player.Completion += (_, _) =>
        {
            player.Release();
        };
        player.Error += (_, _) =>
        {
            player.Release();
            PlayAndroidFallbackTone();
        };
        player.Start();
        return true;
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
}
