using LD.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities
{
    public class KittingIssueDetail : AuditableEntity
    {
        [Key]
        [Required]
        public int KittingReceiptDetailId { get; set; }

        [Required]
        public int KittingDetailId { get; set; }

        public int? ProductId { get; set; }

        public bool DeleteRow { get; set; }

        public int? StandardId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PartNumber { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        public decimal? StandardQuantity { get; set; }

        public decimal? MaximumQuantity { get; set; }

        [MaxLength(50)]
        public string? SD { get; set; }

        public decimal? ReceivedQuantity { get; set; }

        [MaxLength(30)]
        public string? Status { get; set; }

        [MaxLength(30)]
        public string? SupplyStatus { get; set; }

        public int? LocationId { get; set; }

        [MaxLength(100)]
        public string? LocationCode { get; set; }

        [MaxLength(50)]
        public string? LotNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [MaxLength(100)]
        public string? Reference { get; set; }

        [MaxLength(50)]
        public string? PurchaseOrder { get; set; }

        [MaxLength(50)]
        public string? CustomsDeclarationNumber { get; set; }

        [MaxLength(30)]
        public string? StatusLine { get; set; }

        public KittingDetail? KittingDetail { get; set; }
        public Product? Product { get; set; }
        public Location? Location { get; set; }
        public StandardLabel? StandardLabel { get; set; }
    }
}
