using LD.Domain.Common;
using LD.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class SecurityRegistration : AuditableEntity
{
    [Key]
    public int SecurityRegistrationId { get; set; }

    public RegistroEstado Estado { get; set; } = RegistroEstado.Registrado;

    public int? CortinaId { get; set; }
    public virtual Cortina? Cortina { get; set; }

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

    // Vehicle
    public bool TieneCaja { get; set; }

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

    [MaxLength(50)]
    public string NumeroCaja { get; set; } = "";

    [MaxLength(20)]
    public string PlacaCaja { get; set; } = "";

    [MaxLength(50)]
    public string Sello { get; set; } = "";

    public ICollection<SecurityRegistrationPhoto> Photos { get; set; } = new List<SecurityRegistrationPhoto>();
}
