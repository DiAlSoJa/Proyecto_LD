using LD.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class SecurityRegistrationPhoto
{
    [Key]
    public int SecurityRegistrationPhotoId { get; set; }

    public int SecurityRegistrationId { get; set; }
    public virtual SecurityRegistration SecurityRegistration { get; set; } = null!;

    public int? SecurityTaskId { get; set; }
    public virtual SecurityTask? SecurityTask { get; set; }

    public PhotoCategoria Categoria { get; set; }

    public int Orden { get; set; }

    [MaxLength(150)]
    public string? RealizadaPor { get; set; }

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = "";
}
