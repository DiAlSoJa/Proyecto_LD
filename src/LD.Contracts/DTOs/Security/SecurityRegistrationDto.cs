using LD.Contracts.Enums;

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
    public string TipoVehiculo { get; set; } = "";
    public string Linea { get; set; } = "";
    public string Origen { get; set; } = "";
    public string Numero { get; set; } = "";
    public string Placa { get; set; } = "";
    public bool IsActive { get; set; }
    public RegistroEstado_e Estado { get; set; }
    public int? CortinaId { get; set; }
    public string? CortinaNumero { get; set; }
    public List<SecurityPhotoDto> Fotos { get; set; } = new();   // Licencia + Vehiculo
    public SecurityPhotoDto? Firma { get; set; }                  // exactamente una
}
