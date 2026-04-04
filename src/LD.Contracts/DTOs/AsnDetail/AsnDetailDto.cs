using System;
using System.ComponentModel;

namespace LD.Contracts.ASN
{
    public class AsnDetailDto
    {
        [DisplayName("Id")]
        public int AsnDetailId { get; set; }

        public int AsnId { get; set; }

        public int? ProductId { get; set; }

        [DisplayName("Número de Parte")]
        public string PartNumber { get; set; } = string.Empty;

        [DisplayName("Descripción")]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Cantidad")]
        public decimal Quantity { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; } = string.Empty;

        [DisplayName("SD")]
        public string SD { get; set; } = string.Empty;

        [DisplayName("Número de Lote")]
        public string LotNumber { get; set; } = string.Empty;

        [DisplayName("Fecha de Caducidad")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Referencia Cliente")]
        public string CustomerReference { get; set; } = string.Empty;

        [DisplayName("Tipo de Cambio")]
        public decimal? ExchangeRate { get; set; }

        [DisplayName("Orden de Compra")]
        public string PurchaseOrder { get; set; } = string.Empty;

        [DisplayName("Pedimento")]
        public string CustomsDeclarationNumber { get; set; } = string.Empty;

        [DisplayName("Split")]
        public decimal Split { get; set; }
    }
}