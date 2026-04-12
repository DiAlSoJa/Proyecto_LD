namespace MauiAppLogin.Models;


public class SecurityRegistrationContext
{
    // Flow type
    public string Tipo { get; set; } = "";           // Carga / Descarga

    // Step 1 — License
    public string Nombre { get; set; } = "";
    public string Licencia { get; set; } = "";
    public DateTime Vencimiento { get; set; } = DateTime.Today;
    public string Celular { get; set; } = "";
    public byte[]? LicenciaFoto1 { get; set; }
    public byte[]? LicenciaFoto2 { get; set; }

    // Step 2 — Vehicle
    public string TipoVehiculo { get; set; } = "";   // Caja / Tractor
    public string Linea { get; set; } = "";
    public string Origen { get; set; } = "";
    public string Numero { get; set; } = "";
    public string Placa { get; set; } = "";
    public byte[]? VehiculoFoto1 { get; set; }
    public byte[]? VehiculoFoto2 { get; set; }

    // Step 3 — Signature
    public byte[]? Firma { get; set; }

    public void Clear()
    {
        Tipo = "";
        Nombre = "";
        Licencia = "";
        Vencimiento = DateTime.Today;
        Celular = "";
        LicenciaFoto1 = null;
        LicenciaFoto2 = null;

        TipoVehiculo = "";
        Linea = "";
        Origen = "";
        Numero = "";
        Placa = "";
        VehiculoFoto1 = null;
        VehiculoFoto2 = null;

        Firma = null;
    }
}
