using LD.Client.Services;
using LD.Contracts.ASN;
using MauiAppLogin.Features.Almacenista.Models;
using System.Globalization;
using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class ReceptionDetailPage : ContentPage, IQueryAttributable
{
    private readonly AsnDetailService _asnDetailService;
    private readonly AsnReceiptService _asnReceiptService;
    private readonly ObservableCollection<ReceptionDetailItem> _items = new();
    private readonly ObservableCollection<ReceptionDetailItem> _filtered = new();

    private int _asnId;
    private string _asnCode = string.Empty;
    private string _locationCode = string.Empty;
    private int _palletsPorMover;
    private string _instructionText = string.Empty;
    private int _pendingCount;

    public ReceptionDetailPage(AsnDetailService asnDetailService, AsnReceiptService asnReceiptService)
    {
        InitializeComponent();

        _asnDetailService = asnDetailService;
        _asnReceiptService = asnReceiptService;
        DetailCollection.ItemsSource = _filtered;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("AsnId", out var asnIdValue) &&
            int.TryParse(asnIdValue?.ToString(), out var asnId))
        {
            _asnId = asnId;
        }

        if (query.TryGetValue("AsnCode", out var asnCodeValue))
        {
            _asnCode = asnCodeValue?.ToString()?.Trim() ?? string.Empty;
        }

        if (query.TryGetValue("LocationCode", out var locationCodeValue))
        {
            _locationCode = locationCodeValue?.ToString()?.Trim() ?? string.Empty;
        }

        if (query.TryGetValue("PalletsPorMover", out var palletsValue) &&
            int.TryParse(palletsValue?.ToString(), out var palletsPorMover))
        {
            _palletsPorMover = palletsPorMover;
        }

        if (query.TryGetValue("TextInformation", out var textInformation))
        {
            _instructionText = textInformation?.ToString() ?? string.Empty;
        }

        UpdateHeader();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsnDetailsAsync();
    }

    private async Task LoadAsnDetailsAsync()
    {
        if (_asnId <= 0)
            return;

        try
        {
            var detailsResponse = await _asnDetailService.GetAsnDetailsByAsn(_asnId);
            if (!detailsResponse.IsSuccess || detailsResponse.Data is null)
            {
                await DisplayAlertAsync("ASN", detailsResponse.Message ?? "No se pudieron cargar las lineas del ASN.", "OK");
                return;
            }

            var receiptsResponse = await _asnReceiptService.GetAsnReceipts();
            if (!receiptsResponse.IsSuccess || receiptsResponse.Data is null)
            {
                await DisplayAlertAsync("ASN", receiptsResponse.Message ?? "No se pudieron cargar los recibos del ASN.", "OK");
                return;
            }

            var detailIds = detailsResponse.Data
                .Where(x => x.AsnDetailId > 0)
                .Select(x => x.AsnDetailId)
                .ToHashSet();

            _items.Clear();
            foreach (var item in receiptsResponse.Data
                         .Where(x => detailIds.Contains(x.AsnDetailId))
                         .OrderBy(x => x.PalletNumber > 0 ? x.PalletNumber : int.MaxValue)
                         .ThenBy(x => x.StandardId ?? string.Empty)
                         .ThenBy(x => x.PartNumber)
                         .Select(BuildReceptionDetailItem))
            {
                _items.Add(item);
            }

            _pendingCount = _items.Count;
            UpdateHeader();
            ApplyFilter(FiltroEntry.Text);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private static ReceptionDetailItem BuildReceptionDetailItem(AsnReceiptDetailDto asnReceipt)
    {
        var standardId = asnReceipt.StandardId?.Trim();
        var partNumber = asnReceipt.PartNumber?.Trim();
        var quantity = asnReceipt.ReceivedQuantity.GetValueOrDefault();
        var location = asnReceipt.LocationCode?.Trim();
        var palletNumber = asnReceipt.PalletNumber > 0
            ? asnReceipt.PalletNumber.ToString(CultureInfo.InvariantCulture)
            : "-";
        var standardText = string.IsNullOrWhiteSpace(standardId) ? "Sin StandardId" : standardId;
        var partText = string.IsNullOrWhiteSpace(partNumber) ? "Sin numero de parte" : partNumber;
        var locationText = string.IsNullOrWhiteSpace(location) ? "Sin ubicacion" : location;

        return new ReceptionDetailItem
        {
            AsnDetailId = asnReceipt.AsnDetailId,
            PalletNumber = asnReceipt.PalletNumber,
            Title = $"Pallet {palletNumber} - StandardId: {standardText}",
            StandardId = standardText,
            PartNumber = partText,
            QuantityText = $"Cantidad: {quantity:0.##}",
            Ubicacion = locationText,
            Subtitle = $"Pallet {palletNumber} - Número de Parte: {partText}",
            InstructionText = $"Pallet {palletNumber} - ASN {standardText} - Número de Parte {partText} - Cantidad {quantity:0.##} - Ubicación {locationText}"
        };
    }

    private void UpdateHeader()
    {
        AsnHeaderLabel.Text = string.IsNullOrWhiteSpace(_asnCode)
            ? "ASN: -"
            : $"ASN: {_asnCode}";

        AsnCountLabel.Text = _pendingCount == 1
            ? "1 linea pendiente"
            : $"{_pendingCount} lineas pendientes";
    }

    private void OnFiltroChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter(e.NewTextValue);
    }

    private void ApplyFilter(string? value)
    {
        var text = (value ?? string.Empty).Trim().ToLowerInvariant();

        _filtered.Clear();
        foreach (var item in _items)
        {
            if (string.IsNullOrWhiteSpace(text) ||
                item.StandardId.ToLowerInvariant().Contains(text) ||
                item.PartNumber.ToLowerInvariant().Contains(text) ||
                item.QuantityText.ToLowerInvariant().Contains(text) ||
                item.Ubicacion.ToLowerInvariant().Contains(text))
            {
                _filtered.Add(item);
            }
        }
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection?.FirstOrDefault() as ReceptionDetailItem;
        if (selected is null)
            return;

        DetailCollection.SelectedItem = null;

        var parameters = new Dictionary<string, object>
        {
            { "AsnId", _asnId },
            { "TextInformation", selected.InstructionText }
        };

        await Shell.Current.GoToAsync(nameof(ChangeLocationPage), parameters);
    }
}
