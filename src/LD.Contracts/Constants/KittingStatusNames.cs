using System.Linq;

namespace LD.Contracts.Constants;

public static class KittingStatusNames
{
    public const string Confirmado = "Confirmado";
    public const string Cancelado = "Cancelado";
    public const string Creado = "Creado";
    public const string Cargado = "Cargado";
    public const string CargadoParcial = "Cargado Parcial";
    public const string Salida = "Salida";
    public const string Surtiendo = "Surtiendo";
    public const string Ubicando = "Ubicando";
    public const string Validacion = "Validación";
    public const string Cargando = "Cargando";
    public const string LegacyEmbarcado = "Embarcado";

    public const string LegacyValidacion = "Surtido";
    public const string LegacyCargando = "Validado";

    public static bool IsValidation(string? status) =>
        EqualsAny(status, Validacion, LegacyValidacion);

    public static bool IsLoading(string? status) =>
        EqualsAny(status, Cargando, LegacyCargando);

    public static bool IsLoaded(string? status) =>
        EqualsAny(status, Cargado, Confirmado);

    public static bool IsPartiallyLoaded(string? status) =>
        EqualsAny(status, CargadoParcial);

    public static bool IsSalida(string? status) =>
        EqualsAny(status, Salida, LegacyEmbarcado);

    public static bool IsConfirmed(string? status) =>
        EqualsAny(status, Confirmado, Validacion, LegacyValidacion);

    public static bool IsTerminal(string? status) =>
        IsConfirmed(status) || IsLoading(status) || IsLoaded(status) || IsPartiallyLoaded(status) || IsCancelled(status) || IsSalida(status);

    public static bool IsCancelled(string? status) =>
        EqualsAny(status, Cancelado);

    public static bool IsSending(string? status) =>
        EqualsAny(status, Surtiendo, Ubicando);

    public static string? Normalize(string? status)
    {
        var trimmed = Trim(status);
        if (string.IsNullOrWhiteSpace(trimmed))
            return null;

        return trimmed switch
        {
            LegacyEmbarcado => Salida,
            LegacyValidacion => Validacion,
            LegacyCargando => Cargando,
            _ => trimmed
        };
    }

    public static string Display(string? status)
    {
        var normalized = Normalize(status) ?? string.Empty;
        return normalized switch
        {
            LegacyValidacion => Validacion,
            LegacyCargando => Cargando,
            _ => normalized
        };
    }

    public static string ToLowerDisplay(string? status) =>
        Display(status).ToLowerInvariant();

    private static bool EqualsAny(string? value, params string[] candidates) =>
        candidates.Any(candidate => string.Equals(Trim(value), candidate, StringComparison.OrdinalIgnoreCase));

    private static string Trim(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
}
