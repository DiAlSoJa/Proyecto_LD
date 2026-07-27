using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;

namespace LD.Domain.Entities;

public class TruckType : AuditableEntity
{
    [Key]
    public int TruckTypeId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool TieneCaja { get; set; }
}
