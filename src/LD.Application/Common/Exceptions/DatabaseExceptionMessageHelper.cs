using System;
using System.Collections.Generic;
using System.Linq;

namespace LD.Application.Common.Exceptions;

public static class DatabaseExceptionMessageHelper
{
    public static string GetUserMessage(Exception exception, string entityName, string uniquenessHint)
    {
        var detail = GetInnermostMessage(exception);
        var normalized = detail.ToLowerInvariant();

        if (ContainsAny(normalized, "duplicate key", "primary key constraint", "unique index", "unique key constraint"))
        {
            return $"Ya existe un {entityName} con la misma combinacion de {uniquenessHint}.";
        }

        if (ContainsAny(normalized, "foreign key constraint", "reference constraint"))
        {
            return $"Alguno de los valores relacionados del {entityName} no existe o no es valido.";
        }

        if (ContainsAny(normalized, "truncated", "exceeds the maximum length", "string or binary data would be truncated"))
        {
            return $"Algun campo del {entityName} excede la longitud permitida.";
        }

        if (IsDatabaseUpdateException(exception))
        {
            return $"No se pudo guardar el {entityName}. Detalle tecnico: {detail}";
        }

        return detail;
    }

    public static string GetUserMessage(Exception exception)
    {
        var detail = GetInnermostMessage(exception);
        var normalized = detail.ToLowerInvariant();

        if (ContainsAny(normalized, "duplicate key", "primary key constraint", "unique index", "unique key constraint"))
        {
            return "Ya existe un registro con la misma combinacion de datos.";
        }

        if (ContainsAny(normalized, "foreign key constraint", "reference constraint"))
        {
            return "Alguno de los valores relacionados no existe o no es valido.";
        }

        if (ContainsAny(normalized, "truncated", "exceeds the maximum length", "string or binary data would be truncated"))
        {
            return "Algun campo excede la longitud permitida.";
        }

        if (IsDatabaseUpdateException(exception))
        {
            return $"No se pudo guardar el registro. Detalle tecnico: {detail}";
        }

        return detail;
    }

    private static string GetInnermostMessage(Exception exception)
    {
        var message = exception.GetBaseException().Message?.Trim();
        return string.IsNullOrWhiteSpace(message) ? exception.Message : message;
    }

    private static bool IsDatabaseUpdateException(Exception exception) =>
        exception.GetType().Name.Contains("DbUpdateException", StringComparison.OrdinalIgnoreCase) ||
        exception.GetBaseException().GetType().Name.Contains("SqlException", StringComparison.OrdinalIgnoreCase);

    private static bool ContainsAny(string value, params string[] patterns) =>
        patterns.Any(pattern => value.Contains(pattern, StringComparison.OrdinalIgnoreCase));
}
