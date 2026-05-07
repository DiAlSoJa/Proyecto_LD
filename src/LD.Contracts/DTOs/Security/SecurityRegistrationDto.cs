namespace LD.Contracts.DTOs.Security;

public class SecurityRegistrationDto
{
    public int SecurityRegistrationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Tipo { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Licencia { get; set; } = "";
    public DateTime Vencimiento { get; set; }
    public string Celular { get; set; } = "";
    public string? LicenciaFoto1 { get; set; }
    public string? LicenciaFoto2 { get; set; }
    public string TipoVehiculo { get; set; } = "";
    public string Linea { get; set; } = "";
    public string Origen { get; set; } = "";
    public string Numero { get; set; } = "";
    public string Placa { get; set; } = "";
    public string? VehiculoFoto1 { get; set; }
    public string? VehiculoFoto2 { get; set; }
    public string? Firma { get; set; }
}
