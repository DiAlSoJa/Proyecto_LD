using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class ReceptionPage : ContentPage
{
    private ObservableCollection<ListItemRecep> _items;
    private ObservableCollection<ListItemRecep> _filtered;

    public ReceptionPage()
    {
        InitializeComponent();

        // Datos hardcodeados de ejemplo
        _items = new ObservableCollection<ListItemRecep>
        {
            new() { Titulo = "[1] ASN37913 - REC-ZX", Subtitulo = " JABIL B2 →  552651 " },
            new() { Titulo = "[5] ASN37913 - REC-ZT", Subtitulo =  " JABIL B2 →  552650 " },
            new() { Titulo = "[5] ASN37913 - REC-ZU", Subtitulo =  " JABIL B2 →  552647 " },
             
        };

        _filtered = new ObservableCollection<ListItemRecep>(_items);
        ItemsList.ItemsSource = _filtered;
    }

    private void OnFiltroChanged(object sender, TextChangedEventArgs e)
    {
        var text = (e.NewTextValue ?? "").Trim().ToLowerInvariant();

        _filtered.Clear();

        foreach (var it in _items)
        {
            if (string.IsNullOrEmpty(text) ||
                (it.Titulo?.ToLowerInvariant().Contains(text) ?? false) ||
                (it.Subtitulo?.ToLowerInvariant().Contains(text) ?? false))
            {
                _filtered.Add(it);
            }
        }
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection?.FirstOrDefault() as ListItemRecep;
        if (selected == null) return;

        // IMPORTANT: limpiar selección para poder volver a seleccionar el mismo elemento
        ItemsList.SelectedItem = null;

        var parameters = new Dictionary<string, object>
        {
            { "TextInformation", "ASI37913 - RECZX" }
        };
        await Shell.Current.GoToAsync("ChangeLocationPage", parameters);
        // Acción al tocar (aquí puedes navegar o llamar API)
        //await DisplayAlert("Seleccionado", selected.Titulo, "OK");

        // Ejemplo si quieres navegar:
        // await Shell.Current.GoToAsync("MovimientosEtiquetaPage");
    }
}

public class ListItemRecep
{
    public string Titulo { get; set; } = "";
    public string Subtitulo { get; set; } = "";
}