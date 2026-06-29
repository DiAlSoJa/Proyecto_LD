using System;
using System.ComponentModel;

namespace LD.Contracts.Kitting
{
    public class KittingIssueDetailDto
    {
        [DisplayName("Id")]
        public int KittingReceiptDetailId { get; set; }

        public int KittingDetailId { get; set; }

        [DisplayName("DeliveryOrderId")]
        public int? DeliveryOrderId { get; set; }

        [DisplayName("Orden de entrega")]
        public string? DeliveryOrderCode { get; set; }

        public int? ProductId { get; set; }

        [DisplayName("EstandarID")]
        public string? StandardId { get; set; }

        [DisplayName("EstandarID")]
        public string? StandardIdStr { get; set; }

        [DisplayName("Número de Parte")]
        public string PartNumber { get; set; } = string.Empty;

        [DisplayName("Descripción")]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Estandar")]
        public decimal? StandardQuantity { get; set; }

        [DisplayName("Máxima")]
        public decimal? MaximumQuantity { get; set; }

        [DisplayName("SD")]
        public string SD { get; set; } = string.Empty;

        [DisplayName("Recibida")]
        public decimal? ReceivedQuantity { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; } = string.Empty;

        [DisplayName("Ubicación")]
        public string LocationCode { get; set; } = string.Empty;

        [DisplayName("Supply Status")]
        public string SupplyStatus { get; set; } = string.Empty;

        public int? LocationId { get; set; }

        [DisplayName("Número de Lote")]
        public string LotNumber { get; set; } = string.Empty;

        [DisplayName("Fecha de Caducidad")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Referencia")]
        public string Reference { get; set; } = string.Empty;

        [DisplayName("Orden de Compra")]
        public string PurchaseOrder { get; set; } = string.Empty;

        [DisplayName("Pedimento")]
        public string CustomsDeclarationNumber { get; set; } = string.Empty;
    }
}
