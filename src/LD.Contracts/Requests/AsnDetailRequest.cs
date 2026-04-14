using System;

namespace LD.Contracts.Requests
{
    public class AsnDetailRequest
    {
        public int AsnDetailId { get; set; }
        public int AsnId { get; set; }
        public int ProductId { get; set; }

        public string PartNumber { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal Quantity { get; set; }

        public string? Status { get; set; }
        public string? SD { get; set; }
        public string? LotNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public string? CustomerReference { get; set; }
        public decimal? ExchangeRate { get; set; }

        public string? PurchaseOrder { get; set; }
        public string? CustomsDeclarationNumber { get; set; }
        public decimal? StandardQuantity { get; set; }
        public decimal? MaximumQuantity { get; set; }
    }
}
