using System;
using System.ComponentModel;
using LD.Contracts.Enums;

namespace LD.Contracts.InventoryMovement
{
    public class InventoryMovementDto
    {
        [DisplayName("Id")]
        public int MovementId { get; set; }

        [DisplayName("Producto Id")]
        public int? ProductId { get; set; }

        [DisplayName("Cliente Id")]
        public int ClientId { get; set; }

        [DisplayName("Proyecto Id")]
        public int ProjectId { get; set; }

        [DisplayName("Número de Parte")]
        public string PartNumber { get; set; } = string.Empty;

        [DisplayName("Descripción")]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Fecha")]
        public DateTime Fecha { get; set; }

        [DisplayName("Hora")]
        public TimeSpan Hora { get; set; }

        [DisplayName("Usuario")]
        public string UserId { get; set; } = string.Empty;

        [DisplayName("Nombre de Usuario")]
        public string UserName { get; set; } = string.Empty;

        [DisplayName("Lote")]
        public string LotNumber { get; set; } = string.Empty;

        [DisplayName("Referencia")]
        public string Reference { get; set; } = string.Empty;

        [DisplayName("Orden de Compra")]
        public string PurchaseOrder { get; set; } = string.Empty;

        [DisplayName("Pedimento")]
        public string CustomsDeclarationNumber { get; set; } = string.Empty;

        [DisplayName("Fecha de Caducidad")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Tipo de Documento")]
        public DocumentType_e DocumentType { get; set; }

        [DisplayName("Tipo de Movimiento")]
        public MovementType_e MovementType { get; set; }

        [DisplayName("Documento")]
        public string DocumentId { get; set; } = string.Empty;

        [DisplayName("Status")]
        public string StatusId { get; set; } = string.Empty;

        [DisplayName("Ubicación Id")]
        public int? LocationId { get; set; }

        [DisplayName("Cantidad")]
        public decimal? Qty { get; set; }

        [DisplayName("Estandar Id")]
        public int? StandardId { get; set; }

        [DisplayName("Estandar")]
        public string StandardIdStr { get; set; } = string.Empty;

        public string Cliente { get; set; } = string.Empty;
        public string Proyecto { get; set; } = string.Empty;
        public string Almacen { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
    }
}
