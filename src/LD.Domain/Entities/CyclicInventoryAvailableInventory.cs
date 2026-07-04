using System.ComponentModel.DataAnnotations;
using LD.Domain.Common;

namespace LD.Domain.Entities;

public class CyclicInventoryAvailableInventory : AuditableEntity
{
    [Key]
    public int CyclicInventoryAvailableInventoryId { get; set; }

    [Required]
    public int CyclicInventoryId { get; set; }

    [Required]
    public int CyclicInventoryDetailId { get; set; }

    [Required]
    public int LocationId { get; set; }

    public int TakeNumber { get; set; } = 1;
    public int? AvailableInventoryId { get; set; }
    public int? StandardId { get; set; }

    [MaxLength(100)]
    public string? StandardIdCode { get; set; }

    public int? ProductId { get; set; }
    public int ClientId { get; set; }
    public int ProjectId { get; set; }

    [MaxLength(100)]
    public string PartNumber { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? LotNumber { get; set; }

    [MaxLength(100)]
    public string? Reference { get; set; }

    [MaxLength(30)]
    public string? AvailableReference { get; set; }

    [MaxLength(50)]
    public string? PurchaseOrder { get; set; }

    [MaxLength(50)]
    public string? CustomsDeclarationNumber { get; set; }

    public DateTime? ExpirationDate { get; set; }

    [MaxLength(50)]
    public string? DocumentId { get; set; }

    [MaxLength(30)]
    public string? StatusId { get; set; }

    [MaxLength(50)]
    public string? SD { get; set; }

    [MaxLength(150)]
    public string? AvailableStatus { get; set; }

    public decimal? Qty { get; set; }
    public decimal Supply { get; set; }
    public decimal FinalAvailable { get; set; }

    public CyclicInventory? CyclicInventory { get; set; }
    public CyclicInventoryDetail? CyclicInventoryDetail { get; set; }
    public AvailableInventory? AvailableInventory { get; set; }
    public Location? Location { get; set; }
    public StandardLabel? StandardLabel { get; set; }
}
