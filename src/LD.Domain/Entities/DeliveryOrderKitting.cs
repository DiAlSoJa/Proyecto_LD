using LD.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class DeliveryOrderKitting : AuditableEntity
{
    [Key]
    public int DeliveryOrderKittingId { get; set; }

    [Required]
    public int DeliveryOrderId { get; set; }

    [Required]
    public int KittingId { get; set; }

    [Required]
    public int SortOrder { get; set; }

    public DeliveryOrder? DeliveryOrder { get; set; }
    public Kitting? Kitting { get; set; }
}
