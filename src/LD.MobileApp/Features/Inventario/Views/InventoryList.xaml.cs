using System;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.InventarioCiclico;
using MauiAppLogin.Controls;
using MauiAppLogin.Features.Inventario.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiAppLogin;

public partial class InventoryList : ContentPage
{
    private readonly CyclicInventoryService _cyclicInventoryService;
    private readonly IDialogService _dialogService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ObservableCollection<InventoryGroup> _primeraToma = new();
    private readonly ObservableCollection<InventoryGroup> _segundaToma = new();
    private readonly ObservableCollection<InventoryGroup> _terceraToma = new();
    private bool _isLoading;
    private string _activeTab = "P";

    public InventoryList(
        CyclicInventoryService cyclicInventoryService,
        IDialogService dialogService,
        IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _cyclicInventoryService = cyclicInventoryService;
        _dialogService = dialogService;
        _serviceProvider = serviceProvider;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isLoading)
            return;

        await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        if (_isLoading)
            return;

        try
        {
            _isLoading = true;

            LimpiarDatos();
            MostrarTab("P");
            ActualizarContadores();

            if (string.IsNullOrWhiteSpace(UserData.Id))
            {
                await _dialogService.ShowErrorAsync(
                    "Inventario",
                    "No se pudo identificar al usuario actual.");
                return;
            }

            var response = await _cyclicInventoryService.GetCyclicInventories(
                auditorUserId: UserData.Id);

            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync(
                    "Inventario",
                    response.Message ?? "No se pudieron cargar los inventarios ciclicos.");
                return;
            }

            foreach (var inventario in (response.Data ?? [])
                         .OrderByDescending(x => x.Fecha)
                         .ThenByDescending(x => x.InventarioCiclicoId))
            {
                _primeraToma.Add(MapToInventoryGroup(inventario));
            }

            ActualizarContadores();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                "Inventario",
                $"Error al cargar los inventarios ciclicos: {ex.Message}");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void LimpiarDatos()
    {
        _primeraToma.Clear();
        _segundaToma.Clear();
        _terceraToma.Clear();
    }

    private void ActualizarContadores()
    {
        if (LblCountPrimera is null || LblCountSegunda is null || LblCountTercera is null)
            return;

        LblCountPrimera.Text = $"({_primeraToma.Count})";
        LblCountSegunda.Text = $"({_segundaToma.Count})";
        LblCountTercera.Text = $"({_terceraToma.Count})";
    }

    private void MostrarTab(string tab)
    {
        if (InventoryCollection is null)
            return;

        _activeTab = tab;

        switch (tab)
        {
            case "P":
                InventoryCollection.ItemsSource = _primeraToma;
                ActivarTab("P");
                break;

            case "S":
                InventoryCollection.ItemsSource = _segundaToma;
                ActivarTab("S");
                break;

            case "T":
                InventoryCollection.ItemsSource = _terceraToma;
                ActivarTab("T");
                break;
        }
    }

    private void ActivarTab(string tab)
    {
        if (TabPrimera is null || TabSegunda is null || TabTercera is null
            || LblPrimeraText is null || LblCountPrimera is null
            || LblSegundaText is null || LblCountSegunda is null
            || LblTerceraText is null || LblCountTercera is null)
        {
            return;
        }

        bool esPrimera = tab == "P";
        bool esSegunda = tab == "S";
        bool esTercera = tab == "T";

        TabPrimera.BackgroundColor = esPrimera ? Color.FromArgb("#1F3A5F") : Color.FromArgb("#F8FAFC");
        TabPrimera.Stroke = esPrimera ? Colors.Transparent : Color.FromArgb("#CBD5E1");
        TabPrimera.StrokeThickness = esPrimera ? 0 : 1;

        TabSegunda.BackgroundColor = esSegunda ? Color.FromArgb("#1F3A5F") : Color.FromArgb("#F8FAFC");
        TabSegunda.Stroke = esSegunda ? Colors.Transparent : Color.FromArgb("#CBD5E1");
        TabSegunda.StrokeThickness = esSegunda ? 0 : 1;

        TabTercera.BackgroundColor = esTercera ? Color.FromArgb("#1F3A5F") : Color.FromArgb("#F8FAFC");
        TabTercera.Stroke = esTercera ? Colors.Transparent : Color.FromArgb("#CBD5E1");
        TabTercera.StrokeThickness = esTercera ? 0 : 1;

        LblPrimeraText.TextColor = esPrimera ? Colors.White : Color.FromArgb("#334155");
        LblCountPrimera.TextColor = esPrimera ? Colors.White : Color.FromArgb("#334155");

        LblSegundaText.TextColor = esSegunda ? Colors.White : Color.FromArgb("#334155");
        LblCountSegunda.TextColor = esSegunda ? Colors.White : Color.FromArgb("#334155");

        LblTerceraText.TextColor = esTercera ? Colors.White : Color.FromArgb("#334155");
        LblCountTercera.TextColor = esTercera ? Colors.White : Color.FromArgb("#334155");
    }

    private void OnPrimeraTapped(object sender, TappedEventArgs e)
    {
        MostrarTab("P");
    }

    private void OnSegundaTapped(object sender, TappedEventArgs e)
    {
        MostrarTab("S");
    }

    private void OnTerceraTapped(object sender, TappedEventArgs e)
    {
        MostrarTab("T");
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
            return;
        }

        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
    }

    private async void OnInventorySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection?.FirstOrDefault() is not InventoryGroup selected)
            return;

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }

        var detailPage = _serviceProvider.GetRequiredService<InventoryCyclicDetailPage>();
        detailPage.SetInventory(selected);
        await Navigation.PushAsync(detailPage);
    }

    private static InventoryGroup MapToInventoryGroup(CyclicInventoryDto inventario)
    {
        var detalles = inventario.Detalles
            .OrderBy(x => x.Ubicacion)
            .ThenBy(x => x.PartNumber ?? string.Empty)
            .Select(x => new InventoryDetail
            {
                InventarioCiclicoDetalleId = x.InventarioCiclicoDetalleId,
                LocationId = x.LocationId,
                TakeNumber = x.TakeNumber <= 0 ? 1 : x.TakeNumber,
                Pedido = string.IsNullOrWhiteSpace(x.PartNumber) ? x.Ubicacion : x.PartNumber!.Trim(),
                PartNumber = string.IsNullOrWhiteSpace(x.PartNumber) ? null : x.PartNumber.Trim(),
                Fecha = inventario.Fecha,
                Ubicacion = x.Ubicacion,
                Color = GetDetailColor(x),
                Teorico = x.Teorico,
                Fisico = x.Fisico,
                MismaUbicacion = x.MismaUbicacion,
                EnOtraUbicacion = x.EnOtraUbicacion,
                ResultadoPrimeraToma = x.ResultadoPrimeraToma,
                ResultadoSegundaToma = x.ResultadoSegundaToma,
                ResultadoTerceraToma = x.ResultadoTerceraToma,
                ResultadoCuartaToma = x.ResultadoCuartaToma,
                ResultadoFinal = x.ResultadoFinal,
                Tomada = x.Tomada,
                Escaneado = x.Escaneado
            })
            .ToList();

        var completados = inventario.Detalles.Count(x => x.Tomada);
        var escaneados = inventario.Detalles.Count(x => x.Escaneado);
        var totalUbicaciones = inventario.Detalles.Count;

        return new InventoryGroup
        {
            InventarioCiclicoId = inventario.InventarioCiclicoId,
            WarehouseId = inventario.WarehouseId,
            AuditorUserId = inventario.AuditorUserId?.Trim() ?? string.Empty,
            Fecha = inventario.Fecha,
            FechaTerminado = inventario.FechaTerminado,
            Almacen = inventario.Almacen?.Trim() ?? string.Empty,
            Auditor = inventario.Auditor?.Trim() ?? string.Empty,
            Estatus = inventario.Estatus?.Trim() ?? string.Empty,
            Completados = completados,
            Escaneados = escaneados,
            Cantidad = $"{escaneados}/{totalUbicaciones}",
            Folio = inventario.InventarioCiclicoId.ToString(),
            Detalles = detalles
        };
    }

    private static string GetDetailColor(CyclicInventoryDetailDto detail)
    {
        if (detail.Tomada)
            return "#D2F2B6";

        if (detail.Escaneado)
            return "#FDE68A";

        return "#FFFFFF";
    }
}
