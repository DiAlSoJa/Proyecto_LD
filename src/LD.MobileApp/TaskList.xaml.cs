using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class TaskList : ContentPage
{
    private ObservableCollection<ListItemTask> _items;
    private ObservableCollection<ListItemTask> _filtered;

    public TaskList()
    {
        InitializeComponent();

        // Datos hardcodeados de ejemplo
        _items = new ObservableCollection<ListItemTask>
        {
            new() { Titulo = "[2672] →  R79A", Subtitulo = " 04 Feb 08:40 am  →  5'S  → Baja " },
            new() { Titulo = "[0673] →  R67B", Subtitulo = " 04 Feb 08:40 am  →  PALLET EN RIESGO  → Baja " },
            new() { Titulo = "[2674] →  T69B", Subtitulo = " 04 Feb 08:40 am  →  5'S  → Media " }  
            

        };

        _filtered = new ObservableCollection<ListItemTask>(_items);
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
        var selected = e.CurrentSelection?.FirstOrDefault() as ListItemTask;
        if (selected == null) return;

        // IMPORTANT: limpiar selección para poder volver a seleccionar el mismo elemento
        ItemsList.SelectedItem = null;

       
        await Shell.Current.GoToAsync("TaskResolve");
        // Acción al tocar (aquí puedes navegar o llamar API)
        //await DisplayAlert("Seleccionado", selected.Titulo, "OK");

        // Ejemplo si quieres navegar:
        // await Shell.Current.GoToAsync("MovimientosEtiquetaPage");
        
    }


    private async void OnTaskNewClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("NewTask");
    }

}

public class ListItemTask
{
    public string Titulo { get; set; } = "";
    public string Subtitulo { get; set; } = "";
}