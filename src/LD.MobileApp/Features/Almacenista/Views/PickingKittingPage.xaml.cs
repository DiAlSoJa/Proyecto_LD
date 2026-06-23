using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Kitting;
using MauiAppLogin.Features.Almacenista.Models;
using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class PickingKittingPage : ContentPage, IQueryAttributable
{
    private readonly KittingDetailService _kittingDetailService;
    private readonly KittingIssueService _kittingIssueService;
    private readonly ObservableCollection<PickingKittingIssueItem> _items = new();
    private readonly ObservableCollection<PickingKittingIssueItem> _filtered = new();

    private int _kittingId;
    private string _kittingCode = string.Empty;
    private int _pendingCount;

    public PickingKittingPage(
        KittingDetailService kittingDetailService,
        KittingIssueService kittingIssueService)
    {
        InitializeComponent();

        _kittingDetailService = kittingDetailService;
        _kittingIssueService = kittingIssueService;
        IssueCollection.ItemsSource = _filtered;
    }

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

        if (query.TryGetValue("IssueCount", out var issueCountValue) &&
            int.TryParse(issueCountValue?.ToString(), out var issueCount))
        {
            _pendingCount = issueCount;
        }

        UpdateHeader();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPendingIssuesAsync();
    }

    private async Task LoadPendingIssuesAsync()
    {
        if (_kittingId <= 0)
            return;

        try
        {
            var detailsResponse = await _kittingDetailService.GetKittingDetailsByKittingId(_kittingId);
            if (!detailsResponse.IsSuccess || detailsResponse.Data is null)
            {
                await DisplayAlertAsync("Kit", detailsResponse.Message ?? "No se pudieron cargar las líneas del kit.", "OK");
                return;
            }

            var issueResponse = await _kittingIssueService.GetKittingIssues();
            if (!issueResponse.IsSuccess || issueResponse.Data is null)
            {
                await DisplayAlertAsync("Kit", issueResponse.Message ?? "No se pudieron cargar las líneas del kit.", "OK");
                return;
            }

            var detailIds = detailsResponse.Data
                .Where(x => x.KittingDetailId > 0)
                .Select(x => x.KittingDetailId)
                .ToHashSet();

            var items = issueResponse.Data
                .Where(x => detailIds.Contains(x.KittingDetailId))
                .Where(x => x.ReceivedQuantity.GetValueOrDefault() > 0)
                .Where(x => !KittingStatusNames.IsValidation(x.SupplyStatus))
                .OrderBy(x => x.StandardIdStr ?? x.StandardId ?? string.Empty)
                .ThenBy(x => x.PartNumber)
                .Select(issue => BuildIssueItem(issue, _kittingCode))
                .ToList();

            _items.Clear();
            foreach (var item in items)
                _items.Add(item);

            _pendingCount = _items.Count;
            UpdateHeader();
            ApplyFilter(FiltroEntry.Text);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private static PickingKittingIssueItem BuildIssueItem(KittingIssueDetailDto issue, string kittingCode)
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
            PartNumber = partText,
            QuantityText = $"Cantidad: {quantity}",
            LocationText = $"Ubicación: {locationText}",
            Title = $"StandardId: {standardText}",
            Subtitle = $"Parte: {partText}",
            InstructionText = $"Kit {kittingCode} - StandardId {standardText} - Parte {partText} - Cantidad {quantity} - Ubicación {locationText}"
        };
    }

    private static string ResolveStandardId(KittingIssueDetailDto issue)
    {
        var standardId = issue.StandardId?.Trim();
        if (!string.IsNullOrWhiteSpace(standardId))
            return standardId;

        standardId = issue.StandardIdStr?.Trim();
        return standardId ?? string.Empty;
    }

    private void UpdateHeader()
    {
        KitHeaderLabel.Text = string.IsNullOrWhiteSpace(_kittingCode)
            ? "Kit: -"
            : $"Kit: {_kittingCode}";

        KitCountLabel.Text = _pendingCount == 1
            ? "1 línea pendiente"
            : $"{_pendingCount} líneas pendientes";
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
                item.LocationText.ToLowerInvariant().Contains(text))
            {
                _filtered.Add(item);
            }
        }
    }

    private async void OnIssueSelected(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection?.FirstOrDefault() as PickingKittingIssueItem;
        if (selected is null)
            return;

        IssueCollection.SelectedItem = null;

        if (string.IsNullOrWhiteSpace(selected.StandardId))
        {
            await DisplayAlertAsync("StandardId requerido", "La línea seleccionada no tiene StandardId disponible.", "OK");
            return;
        }

        var parameters = new Dictionary<string, object>
        {
            { "ExpectedStandardId", selected.StandardId },
            { "TextInformation", selected.InstructionText },
            { "KittingReceiptDetailId", selected.KittingReceiptDetailId },
            { "KittingId", _kittingId }
        };

        await Shell.Current.GoToAsync(nameof(ChangeLocationPage), parameters);
    }
}
