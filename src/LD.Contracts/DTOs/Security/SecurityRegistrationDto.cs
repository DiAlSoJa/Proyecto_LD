namespace LD.Contracts.DTOs.Security;

public class SecurityRegistrationDto
{
    public int SecurityRegistrationId { get; set; }
    public string Tipo { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Placa { get; set; } = "";
    public string TipoVehiculo { get; set; } = "";
    public string Linea { get; set; } = "";
    public string Origen { get; set; } = "";
    public string Celular { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
