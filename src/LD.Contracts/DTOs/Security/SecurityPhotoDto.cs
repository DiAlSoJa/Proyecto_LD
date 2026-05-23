using LD.Contracts.Enums;

namespace LD.Contracts.DTOs.Security;

public class SecurityPhotoDto
{
    public PhotoCategoria_e Categoria { get; set; }
    public int Orden { get; set; }
    public byte[]? Contenido { get; set; }
    public string? FilePath { get; set; }
}
