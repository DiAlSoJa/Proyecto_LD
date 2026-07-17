using System;
using LD.Contracts.Enums;

namespace LD.Contracts.Requests
{
    public class InventoryMovementRequest
    {
        public int MovementId { get; set; }
        public int? ProductId { get; set; }
        public int ClientId { get; set; }
        public int ProjectId { get; set; }

        public string PartNumber { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string UserId { get; set; } = string.Empty;

        public string? LotNumber { get; set; }
        public int? PalletNumber { get; set; }
        public string? Reference { get; set; }
        public string? PurchaseOrder { get; set; }
        public string? CustomsDeclarationNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public DocumentType_e DocumentType { get; set; }
        public MovementType_e MovementType { get; set; }

        public string DocumentId { get; set; } = string.Empty;
        public string? StatusId { get; set; }

        public int? LocationId { get; set; }
        public decimal? Qty { get; set; }
        public int? StandardId { get; set; }
    }
}
