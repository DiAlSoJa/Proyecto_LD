namespace MauiAppLogin.Models;

/// <summary>
/// Request DTO assembled at the end of the 3-step security registration flow.
/// </summary>
public class SecurityRegistrationRequest
{
    // Flow
    public string Tipo { get; set; } = "";

    // Driver / License
    public string Nombre { get; set; } = "";
    public string Licencia { get; set; } = "";
    public DateTime Vencimiento { get; set; }
    public string Celular { get; set; } = "";
    public byte[]? LicenciaFoto1 { get; set; }
    public byte[]? LicenciaFoto2 { get; set; }

    // Vehicle
    public string TipoVehiculo { get; set; } = "";
    public string Linea { get; set; } = "";
    public string Origen { get; set; } = "";
    public string Numero { get; set; } = "";
    public string Placa { get; set; } = "";
    public byte[]? VehiculoFoto1 { get; set; }
    public byte[]? VehiculoFoto2 { get; set; }

    // Signature
    public byte[]? Firma { get; set; }

    public static SecurityRegistrationRequest FromContext(SecurityRegistrationContext ctx)
    {
        return new SecurityRegistrationRequest
        {
            Tipo = ctx.Tipo,
            Nombre = ctx.Nombre,
            Licencia = ctx.Licencia,
            Vencimiento = ctx.Vencimiento,
            Celular = ctx.Celular,
            LicenciaFoto1 = ctx.LicenciaFoto1,
            LicenciaFoto2 = ctx.LicenciaFoto2,
            TipoVehiculo = ctx.TipoVehiculo,
            Linea = ctx.Linea,
            Origen = ctx.Origen,
            Numero = ctx.Numero,
            Placa = ctx.Placa,
            VehiculoFoto1 = ctx.VehiculoFoto1,
            VehiculoFoto2 = ctx.VehiculoFoto2,
            Firma = ctx.Firma
        };
    }
}
