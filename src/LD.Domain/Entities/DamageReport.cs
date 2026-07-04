using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class DamageReport : AuditableEntity
    {
        [Key]
        [Required]
        public int DamageReportId { get; set; }

        public int? AvailableInventoryId { get; set; }

        public int? StandardId { get; set; }

        [MaxLength(50)]
        public string? StandardIdCode { get; set; }

        public int? ProductId { get; set; }

        public int? ClientId { get; set; }

        public int? ProjectId { get; set; }

        public int? WarehouseId { get; set; }

        public int? LocationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PartNumber { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [MaxLength(30)]
        public string? CurrentStatus { get; set; }

        public decimal? ReceivedQuantity { get; set; }

        public decimal? AvailableQuantity { get; set; }

        [MaxLength(150)]
        public string? Warehouse { get; set; }

        [MaxLength(150)]
        public string? Project { get; set; }

        [MaxLength(150)]
        public string? Client { get; set; }

        [MaxLength(50)]
        public string? Asn { get; set; }

        public DateTime? ReceptionDate { get; set; }

        [MaxLength(50)]
        public string? InventoryState { get; set; }

        [Required]
        [MaxLength(100)]
        public string DamageType { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NewStatus { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? DamageReportCode { get; set; }

        [MaxLength(1000)]
        public string? Comments { get; set; }

        [MaxLength(500)]
        public string? Photo1Path { get; set; }

        [MaxLength(500)]
        public string? Photo2Path { get; set; }

        [MaxLength(500)]
        public string? Photo3Path { get; set; }

        [MaxLength(500)]
        public string? Photo4Path { get; set; }

        public DateTime ReportDate { get; set; }

        [MaxLength(450)]
        public string? ReportedByUserId { get; set; }

        [MaxLength(250)]
        public string? ReportedByName { get; set; }

        public AvailableInventory? AvailableInventory { get; set; }

        [ForeignKey(nameof(StandardId))]
        public StandardLabel? StandardLabel { get; set; }
        public Product? Product { get; set; }
        public Location? InventoryLocation { get; set; }
        public Client? InventoryClient { get; set; }
        public Project? InventoryProject { get; set; }
        public Warehouse? InventoryWarehouse { get; set; }
    }
}
