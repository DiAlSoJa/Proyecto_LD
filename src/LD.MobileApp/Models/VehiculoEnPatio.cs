namespace MauiAppLogin.Models;

public class VehiculoEnPatio
{
    public int Id { get; set; }
    public string Placa { get; set; } = "";
    public DateTime HoraEntrada { get; set; }
    public string Operador { get; set; } = "";
    public string TipoVehiculo { get; set; } = "";
    public string Linea { get; set; } = "";
    public string? CortinaAsignada { get; set; }
    public string Status { get; set; } = "Dentro";
}
