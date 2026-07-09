using System;
using System.ComponentModel;

namespace LD.Contracts.ASN
{
    public class AsnReceiptDetailDto
    {
        [DisplayName("Id")]
        public int AsnReceiptDetailId { get; set; }

        [DisplayName("Pallet Number")]
        public int PalletNumber { get; set; }

        public int AsnDetailId { get; set; }

        public int? ProductId { get; set; }

       

        [DisplayName("EstandarID")]
        public string? StandardId { get; set; }

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
