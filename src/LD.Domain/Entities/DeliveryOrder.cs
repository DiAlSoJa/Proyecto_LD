using LD.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities;

public class DeliveryOrder : AuditableEntity
{
    [Key]
    public int DeliveryOrderId { get; set; }

    [MaxLength(30)]
    public string? DeliveryOrderCode { get; set; }

    [MaxLength(30)]
    public string? PreDeliveryOrderCode { get; set; }

    [Required]
    public int ClientId { get; set; }

    [Required]
    public int ProjectId { get; set; }

    [MaxLength(30)]
    public string? Status { get; set; }

    public Client? Client { get; set; }
    public Project? Project { get; set; }

    public ICollection<DeliveryOrderKitting> DeliveryOrderKittings { get; set; } = new List<DeliveryOrderKitting>();
    public ICollection<KittingIssueDetail> KittingIssueDetails { get; set; } = new List<KittingIssueDetail>();
}
