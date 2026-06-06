using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LD.Domain.Entities
{
    public class KittingDetail : AuditableEntity
    {
        [Key]
        [Required]
        public int KittingDetailId { get; set; }

        [Required]
        public int KittingId { get; set; }

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

        public Kitting? Kitting { get; set; }
        public Product? Product { get; set; }

        public ICollection<KittingIssueDetail> KittingIssueDetails { get; set; } = new List<KittingIssueDetail>();
    }
}
