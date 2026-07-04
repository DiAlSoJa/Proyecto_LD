using System;
using System.Collections.ObjectModel;
using System.Linq;
using MauiAppLogin.Features.Inventario.Models;
using Microsoft.Extensions.DependencyInjection;

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

        RefreshDetailsView();
    }

    private void RefreshInventorySummary()
    {
        if (_inventory is null)
            return;

        _inventory.Completados = _inventory.Detalles.Count(x => x.Tomada);
        _inventory.Escaneados = _inventory.Detalles.Count(x => x.Escaneado);
        _inventory.Cantidad = $"{_inventory.Escaneados}/{_inventory.Detalles.Count}";
    }

    private void RefreshDetailsView()
    {
        _filteredDetails.Clear();
        foreach (var detail in _allDetails)
        {
            _filteredDetails.Add(detail);
        }

        UpdateEmptyState();
        OnPropertyChanged(nameof(FilteredCountText));
    }

    private void UpdateEmptyState()
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

        EmptyTitleText = "No hay detalles";
        EmptySubtitleText = "Este inventario no tiene ubicaciones cargadas.";
    }

    private async void OnScanLocationClicked(object sender, EventArgs e)
    {
        if (_inventory is null)
        {
            await DisplayAlert("Inventario", "Selecciona un inventario primero.", "OK");
            return;
        }

        if (_inventory.Detalles.Count == 0)
        {
            await DisplayAlert("Inventario", "Este inventario no tiene ubicaciones cargadas.", "OK");
            return;
        }

        var scanPage = _serviceProvider.GetRequiredService<InventoryCyclicScanPage>();
        scanPage.SetContext(_inventory);
        await Navigation.PushAsync(scanPage);
    }
}
