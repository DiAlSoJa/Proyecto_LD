using System;
using System.ComponentModel;

namespace LD.Contracts.AvailableInventory;

public class AvailableInventoryDto
{
    [DisplayName("Id")]
    public int AvailableInventoryId { get; set; }

    [DisplayName("Producto Id")]
    public int? ProductId { get; set; }

    [DisplayName("Cliente Id")]
    public int ClientId { get; set; }

    [DisplayName("Proyecto Id")]
    public int ProjectId { get; set; }

    [DisplayName("Numero de Parte")]
    public string PartNumber { get; set; } = string.Empty;

    [DisplayName("Descripcion")]
    public string Description { get; set; } = string.Empty;

    [DisplayName("Fecha")]
    public DateTime Fecha { get; set; }

    [DisplayName("Hora")]
    public TimeSpan Hora { get; set; }

    [DisplayName("Usuario")]
    public string UserId { get; set; } = string.Empty;

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

    [DisplayName("Documento")]
    public string DocumentId { get; set; } = string.Empty;

    [DisplayName("Status")]
    public string StatusId { get; set; } = string.Empty;

    [DisplayName("Ubicacion Id")]
    public int? LocationId { get; set; }

    [DisplayName("Cantidad")]
    public decimal? Qty { get; set; }

    [DisplayName("Estandar Id")]
    public int? StandardId { get; set; }

    [DisplayName("Estandar")]
    public string StandardIdStr { get; set; } = string.Empty;

    public string Cliente { get; set; } = string.Empty;
    public string Proyecto { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;
}
