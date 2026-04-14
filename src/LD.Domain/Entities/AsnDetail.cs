using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities
{
    public class AsnDetail : AuditableEntity
    {
        [Key]
        [Required]
        public int AsnDetailId { get; set; }

        [Required]
        public int AsnId { get; set; }

        public int? ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PartNumber { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [MaxLength(30)]
        public string? Status { get; set; }

        [MaxLength(50)]
        public string? SD { get; set; }

        [MaxLength(50)]
        public string? LotNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [MaxLength(100)]
        public string? CustomerReference { get; set; }

        public decimal? ExchangeRate { get; set; }

        [MaxLength(50)]
        public string? PurchaseOrder { get; set; }

        [MaxLength(50)]
        public string? CustomsDeclarationNumber { get; set; }
        public decimal? StandardQuantity { get; set; }

        public decimal? MaximumQuantity { get; set; }
        [MaxLength(30)]
        public string? StatusLine { get; set; }
        public Asn? Asn { get; set; }
        public Product? Product { get; set; }

        public ICollection<AsnReceiptDetail> AsnReceiptDetails { get; set; } = new List<AsnReceiptDetail>();
    }
}
