namespace LD.Contracts.DTOs.LoadMapping;

public class LoadMappingDto
{
    public int MapeoCargaId { get; set; }

    public string Cliente { get; set; } = string.Empty;

    public string Proyecto { get; set; } = string.Empty;

    public string OrdenEntrega { get; set; } = string.Empty;
}
