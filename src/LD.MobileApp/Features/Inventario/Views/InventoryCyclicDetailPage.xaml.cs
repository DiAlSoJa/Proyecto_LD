using System;
using MauiAppLogin.Features.Inventario.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiAppLogin;

public partial class InventoryCyclicDetailPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ObservableCollection<InventoryDetail> _allDetails = new();
    private readonly ObservableCollection<InventoryDetail> _filteredDetails = new();
    private InventoryGroup? _inventory;
    private string _emptyTitleText = "No hay detalles";
    private string _emptySubtitleText = "Este inventario no tiene ubicaciones cargadas.";

    public InventoryCyclicDetailPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        BindingContext = this;
        DetailsCollection.ItemsSource = _filteredDetails;
        _serviceProvider = serviceProvider;
    }

    public string FilteredCountText => _filteredDetails.Count.ToString();

    public string EmptyTitleText
    {
        get => _emptyTitleText;
        private set
        {
            if (_emptyTitleText == value)
                return;

            _emptyTitleText = value;
            OnPropertyChanged();
        }
    }

    public string EmptySubtitleText
    {
        get => _emptySubtitleText;
        private set
        {
            if (_emptySubtitleText == value)
                return;

            _emptySubtitleText = value;
            OnPropertyChanged();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        RefreshInventorySummary();

        if (FiltroEntry is not null)
        {
            FiltroEntry.Focus();
        }
    }

    public void SetInventory(InventoryGroup inventory)
    {
        _inventory = inventory;

        RefreshInventorySummary();

        _allDetails.Clear();
        foreach (var detail in inventory.Detalles
                     .OrderBy(x => x.Ubicacion)
                     .ThenBy(x => x.PartNumber ?? x.Pedido))
        {
            _allDetails.Add(detail);
        }

        ApplyFilter(FiltroEntry?.Text);
    }

    private void RefreshInventorySummary()
    {
        if (_inventory is null)
            return;

        _inventory.Completados = _inventory.Detalles.Count(x => x.Tomada);
        _inventory.Escaneados = _inventory.Detalles.Count(x => x.Escaneado);
        _inventory.Cantidad = $"{_inventory.Escaneados}/{_inventory.Detalles.Count}";
    }

    private void OnFiltroChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter(e.NewTextValue);
    }

    private void ApplyFilter(string? value)
    {
        var text = (value ?? string.Empty).Trim().ToLowerInvariant();

        _filteredDetails.Clear();
        foreach (var detail in _allDetails)
        {
            if (MatchesFilter(detail, text))
            {
                _filteredDetails.Add(detail);
            }
        }

        UpdateEmptyState(text);
        OnPropertyChanged(nameof(FilteredCountText));
    }

    private static bool MatchesFilter(InventoryDetail detail, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return true;

        return detail.Pedido.ToLowerInvariant().Contains(text)
            || detail.Ubicacion.ToLowerInvariant().Contains(text)
            || (detail.PartNumber ?? string.Empty).ToLowerInvariant().Contains(text)
            || (detail.ResultadoFinal ?? string.Empty).ToLowerInvariant().Contains(text)
            || (detail.ResultadoPrimeraToma ?? string.Empty).ToLowerInvariant().Contains(text)
            || (detail.ResultadoSegundaToma ?? string.Empty).ToLowerInvariant().Contains(text)
            || detail.DetailSummary.ToLowerInvariant().Contains(text);
    }

    private void UpdateEmptyState(string filterText)
    {
        if (_inventory is null)
        {
            EmptyTitleText = "Sin inventario";
            EmptySubtitleText = "Selecciona un inventario para ver sus detalles.";
            return;
        }

        if (_allDetails.Count == 0)
        {
            EmptyTitleText = "No hay detalles";
            EmptySubtitleText = "Este inventario no tiene ubicaciones cargadas.";
            return;
        }

        if (_filteredDetails.Count == 0 && !string.IsNullOrWhiteSpace(filterText))
        {
            EmptyTitleText = "Sin coincidencias";
            EmptySubtitleText = "Prueba con otra ubicacion o pedido.";
            return;
        }

        EmptyTitleText = "No hay detalles";
        EmptySubtitleText = "Este inventario no tiene ubicaciones cargadas.";
    }

    private async void OnDetailSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection?.FirstOrDefault() is not InventoryDetail selected)
            return;

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }

        if (_inventory is null)
            return;

        var scanPage = _serviceProvider.GetRequiredService<InventoryCyclicScanPage>();
        scanPage.SetContext(_inventory, selected);
        await Navigation.PushAsync(scanPage);
    }
}
