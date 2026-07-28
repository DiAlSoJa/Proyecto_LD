using System.Text.RegularExpressions;

namespace LD.Api.Common.Validation;

public static partial class ModelValidationMessageHelper
{
    private static readonly Dictionary<string, string> FieldNames = new(StringComparer.OrdinalIgnoreCase)
    {
        ["WarehouseId"] = "el almacén",
        ["Priority"] = "la prioridad",
        ["Activity"] = "la actividad",
        ["Name"] = "el nombre",
        ["Description"] = "la descripción",
        ["PartNumber"] = "el número de parte",
        ["DamageType"] = "el tipo de daño",
        ["Category"] = "la categoría",
        ["NewStatus"] = "el nuevo estatus",
        ["Comments"] = "los comentarios",
        ["ResolutionObservations"] = "las observaciones de resolución",
        ["CompletedBy"] = "el nombre de quien completó la tarea",
        ["CompletedByName"] = "el nombre de quien completó la tarea",
        ["Photo1Path"] = "la primera fotografía",
        ["Photo2Path"] = "la segunda fotografía",
        ["Photo3Path"] = "la tercera fotografía",
        ["Photo4Path"] = "la cuarta fotografía",
        ["ResolvedPhoto1Path"] = "la primera fotografía de resolución",
        ["ResolvedPhoto2Path"] = "la segunda fotografía de resolución",
        ["ResolvedPhoto3Path"] = "la tercera fotografía de resolución",
        ["ResolvedPhoto4Path"] = "la cuarta fotografía de resolución"
    };

    public static string GetUserMessage(string field, string? errorMessage)
    {
        var subject = GetFieldSubject(field);

        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            return $"{Capitalize(subject)} contiene un valor no válido.";
        }

        var message = errorMessage.Trim();
        var normalized = message.ToLowerInvariant();

        if (normalized.Contains("request body is required", StringComparison.Ordinal) ||
            normalized.Contains("non-empty request body", StringComparison.Ordinal))
        {
            return "Debes proporcionar la información que se desea guardar.";
        }

        if (normalized.Contains("field is required", StringComparison.Ordinal) ||
            (normalized.StartsWith("the ", StringComparison.Ordinal) &&
             normalized.EndsWith(" field is required.", StringComparison.Ordinal)))
        {
            return $"{Capitalize(subject)} es obligatorio.";
        }

        var maximumLength = MaximumLengthRegex().Match(message);
        if (maximumLength.Success)
        {
            return $"{Capitalize(subject)} no puede superar {maximumLength.Groups[1].Value} caracteres.";
        }

        var minimumLength = MinimumLengthRegex().Match(message);
        if (minimumLength.Success)
        {
            return $"{Capitalize(subject)} debe contener al menos {minimumLength.Groups[1].Value} caracteres.";
        }

        if (ContainsAny(
                normalized,
                "json value could not be converted",
                "is not valid for",
                "input was not valid",
                "failed to read parameter",
                "could not be converted"))
        {
            return $"{Capitalize(subject)} tiene un formato incorrecto.";
        }

        return LooksLikeEnglishFrameworkMessage(normalized)
            ? $"{Capitalize(subject)} contiene un valor no válido."
            : message;
    }

    private static string GetFieldSubject(string field)
    {
        if (string.IsNullOrWhiteSpace(field) || field == "$")
        {
            return "la información enviada";
        }

        var cleanField = field
            .Replace("$.", string.Empty, StringComparison.Ordinal)
            .Split('.', StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault() ?? string.Empty;

        cleanField = ArrayIndexRegex().Replace(cleanField, string.Empty);

        return FieldNames.TryGetValue(cleanField, out var displayName)
            ? displayName
            : "este campo";
    }

    private static bool LooksLikeEnglishFrameworkMessage(string message) =>
        ContainsAny(
            message,
            "the field",
            "the value",
            "must be",
            "must not",
            "is invalid",
            "not valid",
            "could not",
            "failed",
            "required");

    private static string Capitalize(string value) =>
        string.IsNullOrEmpty(value)
            ? value
            : char.ToUpperInvariant(value[0]) + value[1..];

    private static bool ContainsAny(string value, params string[] patterns) =>
        patterns.Any(pattern => value.Contains(pattern, StringComparison.Ordinal));

    [GeneratedRegex(@"maximum length of ['""]?(\d+)", RegexOptions.IgnoreCase)]
    private static partial Regex MaximumLengthRegex();

    [GeneratedRegex(@"minimum length of ['""]?(\d+)", RegexOptions.IgnoreCase)]
    private static partial Regex MinimumLengthRegex();

    [GeneratedRegex(@"\[\d+\]")]
    private static partial Regex ArrayIndexRegex();
}
