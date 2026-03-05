using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;

namespace MauiAppLogin;

public partial class WarehouseOperations : ContentPage
{
    private ObservableCollection<ListItemOperation> _items;
    private ObservableCollection<ListItemOperation> _filtered;

    public WarehouseOperations()
    {
        InitializeComponent();

        // Datos hardcodeados de ejemplo
        _items = new ObservableCollection<ListItemOperation>
        {
            new() { Titulo = "[AX-25000C] →  ESCARH ", Subtitulo = " JABIL B2  → 04 Mar 10:00 am ", Subtitulo2="En espera de asignar cortina" },
            new() { Titulo = "[K005580] →  PAQUETERIA ", Subtitulo = " JABIL B2  → 04 Mar 11:00 am" , Subtitulo2="En espera de iniciar descarga "},
            new() { Titulo = "[OV9550D] →  ESCARH ", Subtitulo = " JABIL B2  → 04 Mar 11:50 am", Subtitulo2="Cargando en cortina 2" },
            new() { Titulo = "[PCPV4550] →  PAQUETERIA JR ", Subtitulo = " JABIL B2  → 04 Mar 12:00 am", Subtitulo2="Descargando en cortina 10"},
            new() { Titulo = "[VO44710] →  ESCARH ", Subtitulo = " JABIL B2  → 04 Mar 12:12 am" , Subtitulo2="En espera de asignar cortina"},
          

        };

        _filtered = new ObservableCollection<ListItemOperation>(_items);
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
        var selected = e.CurrentSelection?.FirstOrDefault() as ListItemOperation;
        if (selected == null) return;

        // IMPORTANT: limpiar selección para poder volver a seleccionar el mismo elemento
        ItemsList.SelectedItem = null;
        await AbrirOpcionesAsync(selected.Subtitulo2);
        /*
        var parameters = new Dictionary<string, object>
        {
            { "TextInformation", "ASI37913 - RECZX" }
        };
        await Shell.Current.GoToAsync("ChangeLocationPage", parameters);*/
        // Acción al tocar (aquí puedes navegar o llamar API)
        //await DisplayAlert("Seleccionado", selected.Titulo, "OK");

        // Ejemplo si quieres navegar:
        // await Shell.Current.GoToAsync("MovimientosEtiquetaPage");
    }


    private async Task AbrirOpcionesAsync(string modulo)
    {
        if(modulo.Contains("En espera de asignar cortina"))
        {
            modulo = "Asignar Cortina";
        }
        
        if(modulo.Contains("En espera de iniciar"))
        {
            modulo = "Iniciar Descarga";
        }
        
        if(modulo.Contains("Descargando"))
        {
            modulo = "Descargando";
        }
        if(modulo.Contains("Cargando"))
        {
            modulo = "Cargando";
        }


        var opciones = modulo switch
        {
            "Asignar Cortina" => new[] { "Asignar Cortina" },
            "Iniciar Descarga" => new[] { "Iniciar Carga/Descarga" },
            "Descargando" => new[] { "Finalizar Carga/Descarga" },
            "Cargando" => new[] { "Finalizar Carga/Descarga" },
            _ => new[] { "Nuevo", "Consultar", "Historial" }
        };

        var popup = new OptionPopup(modulo, opciones);

        this.ShowPopup(popup);

        var seleccion = await popup.Result;
        if (string.IsNullOrWhiteSpace(seleccion))
            return;

        switch (seleccion)
        {
            
            case "Asignar Cortina":
                await AbrirOpcionesCortinasAsync(seleccion);
                break;
            case "Iniciar Carga/Descarga":
            case "Finalizar Carga/Descarga":
                await Shell.Current.GoToAsync("TaskSecurity");
                break;
        }

        //await DisplayAlert("Selección", $"{modulo} -> {seleccion}", "OK");

        // Aquí haces navegación real:
        // if (modulo == "Caseta" && seleccion == "Entrada")
        //     await Navigation.PushAsync(new CasetaEntradaPage());
    }



    private async Task AbrirOpcionesCortinasAsync(string modulo)
    {
       

        var opciones = modulo switch
        {
            "Asignar Cortina" => new[] { "1","2","6","9" },            
            _ => new[] { "Nuevo", "Consultar", "Historial" }
        };

        var popup = new OptionPopup(modulo, opciones);

        this.ShowPopup(popup);

        var seleccion = await popup.Result;
        if (string.IsNullOrWhiteSpace(seleccion))
            return;

        switch (seleccion)
        {
            case "Iniciar Descarga":
            case "Descargando":
            case "Cargando":
                await Shell.Current.GoToAsync("TaskSecurity");
                break;
            default:
                await DisplayAlert("Selección", $"{modulo} -> {seleccion}", "OK");
                break;

        }

        

        // Aquí haces navegación real:
        // if (modulo == "Caseta" && seleccion == "Entrada")
        //     await Navigation.PushAsync(new CasetaEntradaPage());



    }


}
 


public class ListItemOperation
{
    public string Titulo { get; set; } = "";
    public string Subtitulo { get; set; } = "";
    public string Subtitulo2 { get; set; } = "";
}