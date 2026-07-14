namespace LD.Contracts.Requests;

public class CortinaRequest
{
    public int? CortinaId { get; set; }
    public string Numero { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public bool EstaDisponible { get; set; } = true;
    public int WarehouseId { get; set; }
}
