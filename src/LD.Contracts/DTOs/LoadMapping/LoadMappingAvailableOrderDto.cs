namespace LD.Contracts.DTOs.LoadMapping;

public class LoadMappingAvailableOrderDto
{
    public int ClientId { get; set; }
    public int ProjectId { get; set; }
    public int WarehouseId { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string Proyecto { get; set; } = string.Empty;
    public string Almacen { get; set; } = string.Empty;
    public string OrdenEntrega { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int KittingsCount { get; set; }
}
