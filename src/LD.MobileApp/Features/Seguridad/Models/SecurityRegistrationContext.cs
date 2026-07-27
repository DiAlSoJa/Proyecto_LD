using LD.Contracts.Enums;

namespace MauiAppLogin.Models;

public enum LicensePhotoSide
{
    Front,
    Back
}

public class SecurityPhotoEntry
{
    public PhotoCategoria_e Categoria { get; set; }
    public int Orden { get; set; }
    public LicensePhotoSide? Side { get; set; }
    public byte[] Bytes { get; set; } = Array.Empty<byte>();
}

public class SecurityRegistrationContext
{
    // Flow type
    public string Tipo { get; set; } = "";           // Carga / Descarga

    // Step 1 — License
    public string Nombre { get; set; } = "";
    public string Licencia { get; set; } = "";
    public DateTime? Vencimiento { get; set; }
    public string Celular { get; set; } = "";
    public List<SecurityPhotoEntry> LicenciaFotos { get; set; } = new();

    // Step 2 — Vehicle
    public bool TieneCaja { get; set; }
    public string TipoVehiculo { get; set; } = "";   // Caja / Tractor
    public string Linea { get; set; } = "";
    public string Origen { get; set; } = "";
    public string Numero { get; set; } = "";
    public string Placa { get; set; } = "";
    public string NumeroCaja { get; set; } = "";
    public string PlacaCaja { get; set; } = "";
    public string Sello { get; set; } = "";
    public List<SecurityPhotoEntry> VehiculoFotos { get; set; } = new();

    // Step 3 — Signature
    public byte[]? Firma { get; set; }

    public void Clear()
    {
        Tipo = "";
        Nombre = "";
        Licencia = "";
        Vencimiento = null;
        Celular = "";
        LicenciaFotos.Clear();

        TipoVehiculo = "";
        TieneCaja = false;
        Linea = "";
        Origen = "";
        Numero = "";
        Placa = "";
        NumeroCaja = "";
        PlacaCaja = "";
        Sello = "";
        VehiculoFotos.Clear();

        Firma = null;
    }
}
