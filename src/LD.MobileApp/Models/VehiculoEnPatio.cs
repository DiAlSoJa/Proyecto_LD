namespace MauiAppLogin.Models;

using LD.Contracts.Enums;

public class VehiculoEnPatio
{
    public int Id { get; set; }
    public string Placa { get; set; } = "";
    public bool TieneCaja { get; set; }
    public DateTime HoraEntrada { get; set; }
    public string Operador { get; set; } = "";
    public string TipoOperacion { get; set; } = ""; // Carga / Descarga
    public string TipoVehiculo { get; set; } = "";
    public string Linea { get; set; } = "";
    public string Numero { get; set; } = "";
    public string NumeroCaja { get; set; } = "";
    public string PlacaCaja { get; set; } = "";
    public string Sello { get; set; } = "";
    public string? CortinaAsignada { get; set; }
    public string Status { get; set; } = "Dentro";
    public RegistroEstado_e Estado { get; set; } = RegistroEstado_e.Registrado;
}
