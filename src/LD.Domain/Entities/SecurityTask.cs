using LD.Domain.Common;
using LD.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class SecurityTask : AuditableEntity
{
    [Key]
    public int SecurityTaskId { get; set; }

    public int SecurityRegistrationId { get; set; }
    public virtual SecurityRegistration SecurityRegistration { get; set; } = null!;

    public DateTime FechaIniciada { get; set; }

    [Required]
    [MaxLength(50)]
    public string TipoAccion { get; set; } = "";     // AbrirCortina | CerrarRegistro

    public bool Completada { get; set; } = false;

    public DateTime? FechaCompletada { get; set; }

    [MaxLength(150)]
    public string? RealizadaPor { get; set; }
}
