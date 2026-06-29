using LD.Client.Services;
using MauiAppLogin.Features.Almacenista.Models;
using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class ReceptionPage : ContentPage
{
    private readonly AsnService _asnService;
    private readonly ObservableCollection<ListItemRecep> _items;
    private readonly ObservableCollection<ListItemRecep> _filtered;

    public ReceptionPage(AsnService asnService)
    {
        InitializeComponent();

        _asnService = asnService;

        _items = new ObservableCollection<ListItemRecep>();
        _filtered = new ObservableCollection<ListItemRecep>();
        ItemsList.ItemsSource = _filtered;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadLocatingAsnsAsync();
    }

    private async Task LoadLocatingAsnsAsync()
    {
        try
        {
            var response = await _asnService.GetLocatingAsnPallets();
            if (!response.IsSuccess || response.Data == null)
            {
                await DisplayAlertAsync("Recepcion", response.Message ?? "No se pudieron cargar los ASN.", "OK");
                return;
            }

            _items.Clear();
            foreach (var item in response.Data.Select(BuildReceptionItem))
                _items.Add(item);

            ApplyFilter(FiltroEntry.Text);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private static ListItemRecep BuildReceptionItem(LD.Contracts.ASN.LocatingAsnPalletDto asn)
    {
        var location = asn.LocationCode?.Trim() ?? "";
        var locationText = string.IsNullOrWhiteSpace(location) ? "Sin ubicacion" : location;
        var asnCode = string.IsNullOrWhiteSpace(asn.AsnCode) ? asn.AsnId.ToString() : asn.AsnCode;

        return new ListItemRecep
        {
            AsnId = asn.AsnId,
            AsnCode = asnCode,
            LocationCode = location,
            PalletsPorMover = asn.PalletsPorMover,
            Titulo = $"[{asn.PalletsPorMover}] {asnCode}",
            Subtitulo = $"Ubicacion: {locationText}",
            InstructionText = $"ASN {asnCode} - Ubicacion {locationText}"
        };
    }

    private void OnFiltroChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter(e.NewTextValue);
    }

    private void ApplyFilter(string? value)
    {
        var text = (value ?? "").Trim().ToLowerInvariant();

        _filtered.Clear();

        foreach (var it in _items)
        {
            if (string.IsNullOrEmpty(text) ||
                it.Titulo.ToLowerInvariant().Contains(text) ||
                it.Subtitulo.ToLowerInvariant().Contains(text) ||
                it.AsnCode.ToLowerInvariant().Contains(text) ||
                it.LocationCode.ToLowerInvariant().Contains(text))
            {
                _filtered.Add(it);
            }
        }
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection?.FirstOrDefault() as ListItemRecep;
        if (selected == null)
            return;

        ItemsList.SelectedItem = null;

        var parameters = new Dictionary<string, object>
        {
            { "AsnId", selected.AsnId },
            { "AsnCode", selected.AsnCode },
            { "LocationCode", selected.LocationCode },
            { "PalletsPorMover", selected.PalletsPorMover },
            { "TextInformation", selected.InstructionText }
        };

        await Shell.Current.GoToAsync(nameof(ReceptionDetailPage), parameters);
    }
}
