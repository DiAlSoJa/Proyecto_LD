using MauiAppLogin.Features.Inventario.Models;
using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class InventoryList : ContentPage
{
    private readonly ObservableCollection<InventoryGroup> _primeraToma = new();
    private readonly ObservableCollection<InventoryGroup> _segundaToma = new();
    private readonly ObservableCollection<InventoryGroup> _terceraToma = new();

    public InventoryList()
    {
        InitializeComponent();
        CargarDatos();
        ActualizarContadores();
        MostrarTab("P");
    }

    private void CargarDatos()
    {
        _primeraToma.Clear();
        _segundaToma.Clear();
        _terceraToma.Clear();

        _primeraToma.Add(new InventoryGroup
        {
            Cantidad = "1/1",
            Folio = "1000060",
            Detalles = new List<InventoryDetail>
            {
                new InventoryDetail
                {
                    Pedido = "202001012003 - G21A",
                    Fecha = new DateTime(2026, 03, 03, 11, 00, 00),
                    Ubicacion = "JABIL B2",
                    Color ="#D2F2B6"
                }
            }
        });

        _primeraToma.Add(new InventoryGroup
        {
            Cantidad = "2/3",
            Folio = "1000061",
            Detalles = new List<InventoryDetail>
            {
                new InventoryDetail
                {
                    Pedido = "20200101255 - D01L",
                    Fecha = new DateTime(2026, 03, 03, 11, 00, 00),
                    Ubicacion = "JABIL B2",
                    Color ="#D2F2B6"
                },
                new InventoryDetail
                {
                    Pedido = "20200155500 - J60E",
                    Fecha = new DateTime(2026, 03, 03, 11, 00, 00),
                    Ubicacion = "JABIL B2",
                    Color ="#D2F2B6"
                },
                new InventoryDetail
                {
                    Pedido = "20200185888 - ZD38A",
                    Fecha = new DateTime(2026, 03, 03, 11, 00, 00),
                    Ubicacion = "JABIL B2",
                    Color ="#FFFFFF"
                }
            }
        });

        _segundaToma.Add(new InventoryGroup
        {
            Cantidad = "0/2",
            Folio = "2000010",
            Detalles = new List<InventoryDetail>
            {
                new InventoryDetail
                {
                    Pedido = "HOLD0001 - QA01",
                    Fecha = new DateTime(2026, 03, 03, 09, 30, 00),
                    Ubicacion = "HOLD A1",
                    Color ="#FFFFFF"
                },
                new InventoryDetail
                {
                    Pedido = "HOLD0002 - QA02",
                    Fecha = new DateTime(2026, 03, 03, 09, 45, 00),
                    Ubicacion = "HOLD A2",
                    Color ="#FFFFFF"
                }
            }
        });

        _terceraToma.Add(new InventoryGroup
        {
            Cantidad = "1/1",
            Folio = "3000001",
            Detalles = new List<InventoryDetail>
            {
                new InventoryDetail
                {
                    Pedido = "DMG0001 - BOX01",
                    Fecha = new DateTime(2026, 03, 03, 08, 15, 00),
                    Ubicacion = "SCRAP B1",
                    Color ="#D2F2B6"
                }
            }
        });
    }

    private void ActualizarContadores()
    {
        LblCountPrimera.Text = "("+_primeraToma.Count.ToString()+")";
        LblCountSegunda.Text = "(" + _segundaToma.Count.ToString() + ")";
        LblCountTercera.Text = "(" + _terceraToma.Count.ToString() + ")";
    }

    private void MostrarTab(string tab)
    {
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
        bool esPrimera = tab == "P";
        bool esSegunda = tab == "S";
        bool esTercera = tab == "T";

        TabPrimera.BackgroundColor = esPrimera ? Color.FromArgb("#1F3A5F") : Color.FromArgb("#F8FAFC");
        TabPrimera.Stroke = esPrimera ? null : Color.FromArgb("#CBD5E1");
        TabPrimera.StrokeThickness = esPrimera ? 0 : 1;

        TabSegunda.BackgroundColor = esSegunda ? Color.FromArgb("#1F3A5F") : Color.FromArgb("#F8FAFC");
        TabSegunda.Stroke = esSegunda ? null : Color.FromArgb("#CBD5E1");
        TabSegunda.StrokeThickness = esSegunda ? 0 : 1;

        TabTercera.BackgroundColor = esTercera ? Color.FromArgb("#1F3A5F") : Color.FromArgb("#F8FAFC");
        TabTercera.Stroke = esTercera ? null : Color.FromArgb("#CBD5E1");
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

    private async void OnDetailTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is InventoryDetail detail)
        {
            await DisplayAlertAsync(
                "Detalle",
                $"Pedido: {detail.Pedido}\nFecha: {detail.Fecha:dd-MM-yyyy hh:mm tt}\nUbicación: {detail.Ubicacion}",
                "OK");
        }
    }
}


