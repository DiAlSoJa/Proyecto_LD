using System.Linq;

namespace LD.Contracts.Constants;

public static class KittingStatusNames
{
    public const string Confirmado = "Confirmado";
    public const string Cancelado = "Cancelado";
    public const string Creado = "Creado";
    public const string Surtiendo = "Surtiendo";
    public const string Ubicando = "Ubicando";
    public const string Validacion = "Validación";
    public const string Cargando = "Cargando";

    public const string LegacyValidacion = "Surtido";
    public const string LegacyCargando = "Validado";

    public static bool IsValidation(string? status) =>
        EqualsAny(status, Validacion, LegacyValidacion);

    public static bool IsLoading(string? status) =>
        EqualsAny(status, Cargando, LegacyCargando);

    public static bool IsConfirmed(string? status) =>
        EqualsAny(status, Confirmado, Validacion, LegacyValidacion);

    public static bool IsTerminal(string? status) =>
        IsConfirmed(status) || IsLoading(status) || IsCancelled(status);

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
