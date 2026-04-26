namespace LD.Contracts.DTOs.Security;

public class CortinaDto
{
    public int CortinaId { get; set; }
    public string Numero { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public bool EstaDisponible { get; set; }
    public int WarehouseId { get; set; }
}
