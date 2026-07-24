using System;
using System.ComponentModel;

namespace LD.Contracts.DTOs.KittingFolioCapture;

public class KittingFolioCaptureDto
{
    [DisplayName("Id")]
    public int KittingFolioCaptureId { get; set; }

    public int KittingId { get; set; }

    public int? KittingDetailId { get; set; }

    [DisplayName("Creado")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Cliente")]
    public string Client { get; set; } = string.Empty;

    [DisplayName("Proyecto")]
    public string Project { get; set; } = string.Empty;

    [DisplayName("Kitting")]
    public string KittingCode { get; set; } = string.Empty;

    [DisplayName("Estatus Kitting")]
    public string KittingStatus { get; set; } = string.Empty;

    [DisplayName("Guia")]
    public string GuideNumber { get; set; } = string.Empty;

    [DisplayName("Factura")]
    public string InvoiceNumber { get; set; } = string.Empty;

    [DisplayName("Parte")]
    public string PartNumber { get; set; } = string.Empty;

    [DisplayName("Descripcion")]
    public string Description { get; set; } = string.Empty;

    [DisplayName("Cantidad")]
    public decimal Quantity { get; set; }

    [DisplayName("Lote")]
    public string LotNumber { get; set; } = string.Empty;

    [DisplayName("Archivo")]
    public string SourceFileName { get; set; } = string.Empty;

    [DisplayName("Linea")]
    public int SourceLineNumber { get; set; }
}
