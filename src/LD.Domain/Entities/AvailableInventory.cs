using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using LD.Domain.Common;
using LD.Domain.Enums;

namespace LD.Domain.Entities
{
    public class AvailableInventory : AuditableEntity
    {

        [Key]
        [Required]
        public int AvailableInventoryId { get; set; }

        [Required]
        public int? ProductId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PartNumber { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public String UserId { get; set; }

        [MaxLength(50)]
        public string? LotNumber { get; set; }

        public int? PalletNumber { get; set; }

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
        public string DocumentId { get; set; }

        [MaxLength(30)]
        public string? StatusId { get; set; }

        [MaxLength(50)]
        public string? SD { get; set; }

        [MaxLength(150)]
        public string? AvailableStatus { get; set; }


        public int? LocationId { get; set; }

        public decimal? Qty { get; set; }

        public decimal Supply { get; set; }

        public decimal FinalAvailable { get; set; }

        public int? StandardId { get; set; }


        public Product? Product { get; set; }
        public Location? Location { get; set; }
        public Client? Client { get; set; }
        public Project? Project { get; set; }
        public StandardLabel? StandardLabel { get; set; }
    }
}


