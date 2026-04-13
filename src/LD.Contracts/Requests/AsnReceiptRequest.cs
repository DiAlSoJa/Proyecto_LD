using System;

namespace LD.Contracts.Requests
{
    public class AsnReceiptRequest
    {
        public int AsnReceiptDetailId { get; set; }
        public int AsnDetailId { get; set; }
        public int? ProductId { get; set; }

        public bool DeleteRow { get; set; }
        public int? StandardId { get; set; }

        public string PartNumber { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal? StandardQuantity { get; set; }
        public decimal? MaximumQuantity { get; set; }

        public string? SD { get; set; }
        public decimal? ReceivedQuantity { get; set; }
        public string? Status { get; set; }

        public int? LocationId { get; set; }
        public string? LocationCode { get; set; }

        public string? LotNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? Reference { get; set; }
        public string? PurchaseOrder { get; set; }
        public string? CustomsDeclarationNumber { get; set; }
    }
}
