using CommunityToolkit.Maui.Views;

namespace MauiAppLogin;

public partial class DashboardPage : ContentPage
{
	public DashboardPage()
	{
		InitializeComponent();
	}

    private async void OnAsnPorUbicarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ReceptionPage");
    }
   

    private async void OnUbicacionPasoSurtidoClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("PickingPage");
        
    }

    private async void OnCambioUbicacionClicked(object sender, EventArgs e)
    {
        var parameters = new Dictionary<string, object>
        {
            { "TextInformation", "" }
        };
        await Shell.Current.GoToAsync("ChangeLocationPage", parameters);
    }

    private async void OnTaskManagerClicked(object sender, EventArgs e)
    {
        var parameters = new Dictionary<string, object>
        {
            { "TextInformation", "27 feb - Ir a la ubicación A012 y tomar el pallet 2027020100001 y dejarlo en AX025" }
        };
        await Shell.Current.GoToAsync("ChangeLocationPage", parameters);
    }
    private async void OnTaskManagerSecurityClicked(object sender, EventArgs e)
    {
        var parameters = new Dictionary<string, object>
        {
            { "TextInformation", "Abrir Cortina 5" }
        };
        await Shell.Current.GoToAsync("TaskSecurity", parameters);
    }


    private async void OnMovementClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("MovementPage");
    }
    private async void OnRDClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("DamageReportPage");
    }
    private async void OnCasetaClicked(object sender, EventArgs e)
    {
        await AbrirOpcionesAsync("Caseta");
    }
    private async void OnCortinasClicked(object sender, EventArgs e)
    {
        await AbrirOpcionesAsync("Cortinas");
    }
    private async void OnValidarAcopleClicked(object sender, EventArgs e)
    {
        await AbrirOpcionesAsync("Validar Acople");
    }
    private async void OnValidarDesacopleClicked(object sender, EventArgs e)
    {
        await AbrirOpcionesAsync("Validar Desacople");
    }

    private async Task AbrirOpcionesAsync(string modulo)
    {
        Console.WriteLine($"Abriendo opciones para: {modulo}"); 
        var opciones = modulo switch
        {
            "Caseta" => new[] { "Carga", "Descarga" },
            "Cortinas" => new[] { "Abrir Descarga", "Cerrar Descarga","", "Abrir Carga", "Cerrar Carga" },
            "Validar Acople" => new[] { "Cortina", "Patio" },
            "Validar Desacople" => new[] { "Cortina", "Patio" },
            _ => new[] { "Nuevo", "Consultar", "Historial" }
        };

        var popup = new OptionPopup(modulo, opciones);

        this.ShowPopup(popup);

        var seleccion = await popup.Result;
        if (string.IsNullOrWhiteSpace(seleccion))
            return;

        switch(seleccion)
        {
            case "Carga":
            case "Descarga":                
                await Shell.Current.GoToAsync("RegisterLicense");
                break;
        }

        //await DisplayAlert("Selección", $"{modulo} -> {seleccion}", "OK");

        // Aquí haces navegación real:
        // if (modulo == "Caseta" && seleccion == "Entrada")
        //     await Navigation.PushAsync(new CasetaEntradaPage());
    }




    private void OnAlmacenChanged(object sender, EventArgs e)
    {
        if (sender is Picker p && p.SelectedIndex >= 0)
        {
            var almacen = p.Items[p.SelectedIndex];
            // Aquí puedes filtrar las opciones / cargar datos según almacén
            // Ejemplo:
            // CargarMenuPorAlmacen(almacen);
        }
    }

    private void OnNotificacionesTapped(object sender, TappedEventArgs e)
    {
        var items = new List<NotificationItem>
    {
        new() { Title = "Nueva entrada en Caseta", IsRead = false, Target = "CasetaEntrada" },
        new() { Title = "Inventario bajo en GDL", IsRead = false, Target = "InventarioGDL" },
        new() { Title = "Reporte generado correctamente", IsRead = true, Target = "ReporteDetalle" },
        new() { Title = "Cambio de ubicación aprobado", IsRead = false, Target = "CambioUbicacion" },
    };

        this.ShowPopup(new NotificationsPopup(items, async (notif) =>
        {
            // Aquí decides qué abrir según Target
            switch (notif.Target)
            {
                case "CasetaEntrada":
                    //await Navigation.PushAsync(new CasetaEntradaPage());
                    break;

                case "InventarioGDL":
                    //await Navigation.PushAsync(new ConsultaInventarioPage());
                    break;

                case "ReporteDetalle":
                    //await Navigation.PushAsync(new ReportesPage());
                    break;

                case "CambioUbicacion":
                    //await Navigation.PushAsync(new CambioUbicacionPage());
                    break;

                default:
                    await DisplayAlert("Notificación", notif.Title, "OK");
                    break;
            }
        }));
    }




}