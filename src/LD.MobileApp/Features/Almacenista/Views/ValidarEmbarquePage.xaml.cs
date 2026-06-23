using LD.Client.Services;
using LD.Contracts.Constants;
using MauiAppLogin.Features.Almacenista.Models;
using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class ValidarEmbarquePage : ContentPage
{
    private readonly KittingService _kittingService;
    private readonly KittingDetailService _kittingDetailService;
    private readonly KittingIssueService _kittingIssueService;
    private readonly ObservableCollection<PickingKittingItem> _items = new();
    private readonly ObservableCollection<PickingKittingItem> _filtered = new();

    public ValidarEmbarquePage(
        KittingService kittingService,
        KittingDetailService kittingDetailService,
        KittingIssueService kittingIssueService)
    {
        InitializeComponent();

        _kittingService = kittingService;
        _kittingDetailService = kittingDetailService;
        _kittingIssueService = kittingIssueService;
        KittingCollection.ItemsSource = _filtered;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadValidationKittingsAsync();
    }

    private async Task LoadValidationKittingsAsync()
    {
        try
        {
            var response = await _kittingService.GetKittings();
            if (!response.IsSuccess || response.Data is null)
            {
                await DisplayAlertAsync("Validar embarque", response.Message ?? "No se pudieron cargar los embarques.", "OK");
                return;
            }

            var issueCountByKitting = await BuildPendingIssueCountByKittingAsync();

            _items.Clear();
            foreach (var item in response.Data
                         .Where(IsValidation)
                         .Select(kitting => BuildValidationItem(
                             kitting,
                             issueCountByKitting.TryGetValue(kitting.KittingId, out var issueCount)
                                 ? issueCount
                                 : 0)))
            {
                _items.Add(item);
            }

            ApplyFilter(FiltroEntry.Text);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async Task<Dictionary<int, int>> BuildPendingIssueCountByKittingAsync()
    {
        var detailResponse = await _kittingDetailService.GetKittingDetails();
        var issueResponse = await _kittingIssueService.GetKittingIssues();

        if (!detailResponse.IsSuccess || detailResponse.Data is null ||
            !issueResponse.IsSuccess || issueResponse.Data is null)
        {
            return new Dictionary<int, int>();
        }

        var detailToKitting = detailResponse.Data
            .Where(x => x.KittingDetailId > 0)
            .ToDictionary(x => x.KittingDetailId, x => x.KittingId);

        return issueResponse.Data
            .Where(x => x.KittingDetailId > 0)
            .Where(x => x.ReceivedQuantity.GetValueOrDefault() > 0)
            .Where(x => !KittingStatusNames.IsValidation(x.SupplyStatus))
            .Select(x => detailToKitting.TryGetValue(x.KittingDetailId, out var kittingId) ? kittingId : 0)
            .Where(kittingId => kittingId > 0)
            .GroupBy(kittingId => kittingId)
            .ToDictionary(group => group.Key, group => group.Count());
    }

    private static bool IsValidation(LD.Contracts.Kitting.KittingDto kitting) =>
        KittingStatusNames.IsValidation(kitting.Status);

    private static PickingKittingItem BuildValidationItem(LD.Contracts.Kitting.KittingDto kitting, int issueCount)
    {
        var code = string.IsNullOrWhiteSpace(kitting.KittingCode)
            ? kitting.KittingId.ToString()
            : kitting.KittingCode.Trim();
        var client = kitting.Client?.Trim() ?? string.Empty;
        var project = kitting.Project?.Trim() ?? string.Empty;
        var invoice = kitting.InvoiceNumber?.Trim() ?? string.Empty;
        var status = KittingStatusNames.Normalize(kitting.Status);
        var subtitle = string.IsNullOrWhiteSpace(client) && string.IsNullOrWhiteSpace(project)
            ? "Sin cliente / proyecto"
            : string.IsNullOrWhiteSpace(client)
                ? project
                : string.IsNullOrWhiteSpace(project)
                    ? client
                    : $"{client} - {project}";
        var instruction = string.IsNullOrWhiteSpace(invoice)
            ? $"Validar kitting {code} - {subtitle}".Trim()
            : $"Validar kitting {code} - Factura {invoice} - {subtitle}".Trim();

        return new PickingKittingItem
        {
            KittingId = kitting.KittingId,
            KittingCode = code,
            IssueCount = issueCount,
            Client = client,
            Project = project,
            InvoiceNumber = invoice,
            Status = status,
            Title = $"[{issueCount}] {code}",
            Subtitle = subtitle,
            InstructionText = instruction
        };
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
                item.KittingCode.ToLowerInvariant().Contains(text) ||
                item.Client.ToLowerInvariant().Contains(text) ||
                item.Project.ToLowerInvariant().Contains(text) ||
                item.InvoiceNumber.ToLowerInvariant().Contains(text) ||
                item.Status.ToLowerInvariant().Contains(text))
            {
                _filtered.Add(item);
            }
        }
    }

    private async void OnKittingSelected(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection?.FirstOrDefault() as PickingKittingItem;
        if (selected is null)
            return;

        KittingCollection.SelectedItem = null;

        var parameters = new Dictionary<string, object>
        {
            { "KittingId", selected.KittingId },
            { "KittingCode", selected.KittingCode },
            { "IssueCount", selected.IssueCount }
        };

        await Shell.Current.GoToAsync(nameof(ValidarEmbarqueDetailPage), parameters);
    }
}
