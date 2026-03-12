using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class PickingPage : ContentPage
{
    private ObservableCollection<PickingGrupo> _listaOriginal = new();
    private ObservableCollection<PickingGrupo> _listaFiltrada = new();

    public PickingPage()
    {
        InitializeComponent();
        CargarDatos();
        PickingCollection.ItemsSource = _listaFiltrada;
    }

    private void CargarDatos()
    {
        _listaOriginal = new ObservableCollection<PickingGrupo>
        {
            new PickingGrupo
            {
                Cantidad = 1,
                Folio = "1000060",
                Detalles = new List<PickingDetalle>
                {
                    new PickingDetalle
                    {
                        Pedido = "202001012003 - G21A",
                        Fecha = new DateTime(2026, 03, 03, 11, 00, 00),
                        Ubicacion = "JABIL B2"
                    }
                }
            },
            new PickingGrupo
            {
                Cantidad = 3,
                Folio = "1000061",
                Detalles = new List<PickingDetalle>
                {
                    new PickingDetalle
                    {
                        Pedido = "20200101255 - D01L",
                        Fecha = new DateTime(2026, 03, 03, 11, 00, 00),
                        Ubicacion = "JABIL B2"
                    },
                    new PickingDetalle
                    {
                        Pedido = "20200155500 - J60E",
                        Fecha = new DateTime(2026, 03, 03, 11, 00, 00),
                        Ubicacion = "JABIL B2"
                    },
                    new PickingDetalle
                    {
                        Pedido = "20200185888 - ZD38A",
                        Fecha = new DateTime(2026, 03, 03, 11, 00, 00),
                        Ubicacion = "JABIL B2"
                    }
                }
            }
        };

        _listaFiltrada = new ObservableCollection<PickingGrupo>(_listaOriginal);
    }

    private void OnFiltroChanged(object sender, TextChangedEventArgs e)
    {
        var filtro = e.NewTextValue?.Trim().ToLower() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(filtro))
        {
            PickingCollection.ItemsSource = new ObservableCollection<PickingGrupo>(_listaOriginal);
            return;
        }

        var resultado = _listaOriginal
            .Where(g =>
                g.Folio.ToLower().Contains(filtro) ||
                g.Detalles.Any(d =>
                    d.Pedido.ToLower().Contains(filtro) ||
                    d.Ubicacion.ToLower().Contains(filtro)))
            .ToList();

        PickingCollection.ItemsSource = new ObservableCollection<PickingGrupo>(resultado);
    }

    private async void OnPickedClicked(object sender, TappedEventArgs e)
    {
        if (e.Parameter is PickingDetalle detalle)
        {
            await DisplayAlert("Seleccionado",
                $"Pedido: {detalle.Pedido}\nUbicación: {detalle.Ubicacion}",
                "OK");
        }
    }
}

public class PickingGrupo
{
    public int Cantidad { get; set; }
    public string Folio { get; set; } = string.Empty;
    public List<PickingDetalle> Detalles { get; set; } = new();
}

public class PickingDetalle
{
    public string Pedido { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
}