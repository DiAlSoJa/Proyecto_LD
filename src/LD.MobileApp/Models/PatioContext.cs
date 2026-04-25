namespace MauiAppLogin.Models;

// Singleton: comparte el vehículo/cortina seleccionados entre páginas de Patio
public class PatioContext
{
    public VehiculoEnPatio? VehiculoSeleccionado { get; set; }
    public Cortina? CortinaSeleccionada { get; set; }

    public void Clear()
    {
        VehiculoSeleccionado = null;
        CortinaSeleccionada = null;
    }
}
