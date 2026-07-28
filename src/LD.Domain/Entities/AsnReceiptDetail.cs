using LD.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities
{
    public class AsnReceiptDetail : AuditableEntity
    {
        [Key]
        [Required]
        public int AsnReceiptDetailId { get; set; }

        [Required]
        public int PalletNumber { get; set; }

        [Required]
        public int AsnDetailId { get; set; }

        public int? ProductId { get; set; }

        public bool DeleteRow { get; set; }

        public int? StandardId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PartNumber { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? StandardQuantity { get; set; }

        public decimal? MaximumQuantity { get; set; }

        [MaxLength(50)]
        public string? SD { get; set; }

        public decimal? ReceivedQuantity { get; set; }

        [MaxLength(30)]
        public string? Status { get; set; }

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

        public AsnDetail? AsnDetail { get; set; }
        public Product? Product { get; set; }
        public Location? Location { get; set; }
        public StandardLabel? StandardLabel { get; set; }
    }
}
