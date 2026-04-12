using LD.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class SecurityRegistration : AuditableEntity
{
    [Key]
    public int SecurityRegistrationId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Tipo { get; set; } = "";           // Carga / Descarga

    // Driver / License
    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = "";

    [MaxLength(50)]
    public string Licencia { get; set; } = "";

    public DateTime Vencimiento { get; set; }

    [MaxLength(20)]
    public string Celular { get; set; } = "";

    [MaxLength(500)]
    public string? LicenciaFoto1 { get; set; }      // ruta en disco

    [MaxLength(500)]
    public string? LicenciaFoto2 { get; set; }

    // Vehicle
    [MaxLength(30)]
    public string TipoVehiculo { get; set; } = "";

    [MaxLength(100)]
    public string Linea { get; set; } = "";

    [MaxLength(100)]
    public string Origen { get; set; } = "";

    [MaxLength(50)]
    public string Numero { get; set; } = "";

    [MaxLength(20)]
    public string Placa { get; set; } = "";

    [MaxLength(500)]
    public string? VehiculoFoto1 { get; set; }      // ruta en disco

    [MaxLength(500)]
    public string? VehiculoFoto2 { get; set; }

    [MaxLength(500)]
    public string? Firma { get; set; }              // ruta en disco
}
