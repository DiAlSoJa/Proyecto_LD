using LD.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class LoadMapping : AuditableEntity
{
    [Key]
    public int LoadMappingId { get; set; }

    [Required]
    public int ClientId { get; set; }

    [Required]
    public int ProjectId { get; set; }

    [MaxLength(50)]
    public string? DeliveryOrderCode { get; set; }

    public Client? Client { get; set; }

    public Project? Project { get; set; }

    public ICollection<LoadMappingScan> Scans { get; set; } = new List<LoadMappingScan>();
}
