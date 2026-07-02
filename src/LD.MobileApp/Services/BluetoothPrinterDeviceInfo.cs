namespace MauiAppLogin.Services;

public sealed record BluetoothPrinterDeviceInfo(
    string Address,
    string? Name,
    bool IsBonded,
    bool IsLikelyPrinter)
{
    public string DisplayName => string.IsNullOrWhiteSpace(Name) ? "Sin nombre" : Name!.Trim();

    public string DisplayAddress => Address.Trim();

    public string Summary => $"{DisplayName} ({DisplayAddress})";

    public string StatusText => IsBonded ? "Emparejada" : "Detectada";

    public string BadgeText => IsLikelyPrinter ? "Impresora" : "Bluetooth";
}
