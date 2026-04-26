using LD.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class Cortina : AuditableEntity
{
    [Key]
    public int CortinaId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Numero { get; set; } = "";

    [MaxLength(150)]
    public string Descripcion { get; set; } = "";

    public bool EstaDisponible { get; set; } = true;

    public int WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; } = null!;

    public virtual ICollection<SecurityRegistration> SecurityRegistrations { get; set; } = new List<SecurityRegistration>();
}
