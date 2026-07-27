using LD.Contracts.DTOs.Security;

namespace LD.Contracts.Requests;

public class SecurityRegistrationRequest
{
    // Flow
    public string Tipo { get; set; } = "";           // Carga / Descarga

    // Driver / License
    public string Nombre { get; set; } = "";
    public string Licencia { get; set; } = "";
    public DateTime Vencimiento { get; set; }
    public string Celular { get; set; } = "";

    // Vehicle
    public bool TieneCaja { get; set; }
    public string TipoVehiculo { get; set; } = "";   // Caja / Tractor
    public string Linea { get; set; } = "";
    public string Origen { get; set; } = "";
    public string Numero { get; set; } = "";
    public string Placa { get; set; } = "";
    public string NumeroCaja { get; set; } = "";
    public string PlacaCaja { get; set; } = "";
    public string Sello { get; set; } = "";

    public List<SecurityPhotoDto> Fotos { get; set; } = new();
}
