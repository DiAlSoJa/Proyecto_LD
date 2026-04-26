namespace LD.Contracts.DTOs.Security;

public class SecurityTaskDto
{
    public int SecurityTaskId { get; set; }
    public int SecurityRegistrationId { get; set; }
    public string Placa { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string TipoVehiculo { get; set; } = "";
    public string Linea { get; set; } = "";
    public string TipoAccion { get; set; } = "";
    public bool Completada { get; set; }
    public DateTime? FechaCompletada { get; set; }
    public string? RealizadaPor { get; set; }
    public string? CortinaNumero { get; set; }
    public DateTime CreatedAt { get; set; }
}
