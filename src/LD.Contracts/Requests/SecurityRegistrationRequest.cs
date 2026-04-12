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
    public byte[]? LicenciaFoto1 { get; set; }
    public byte[]? LicenciaFoto2 { get; set; }

    // Vehicle
    public string TipoVehiculo { get; set; } = "";   // Caja / Tractor
    public string Linea { get; set; } = "";
    public string Origen { get; set; } = "";
    public string Numero { get; set; } = "";
    public string Placa { get; set; } = "";
    public byte[]? VehiculoFoto1 { get; set; }
    public byte[]? VehiculoFoto2 { get; set; }

    // Signature
    public byte[]? Firma { get; set; }
}
