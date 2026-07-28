using System;
using System.Collections.Generic;
using System.Linq;

namespace LD.Application.Common.Exceptions;

public static class DatabaseExceptionMessageHelper
{
    public static string GetUserMessage(Exception exception, string entityName, string uniquenessHint)
    {
        var normalized = GetCombinedMessages(exception).ToLowerInvariant();

        if (IsDuplicateError(normalized))
        {
            return $"Ya existe un {entityName} con la misma combinación de {uniquenessHint}. Revisa los datos e inténtalo nuevamente.";
        }

        if (IsDeleteRelationshipError(normalized))
        {
            return $"No se puede eliminar el {entityName} porque está siendo utilizado en otra información.";
        }

        if (IsRelationshipError(normalized))
        {
            return $"No se pudo guardar el {entityName} porque uno de los datos seleccionados ya no existe o no es válido.";
        }

        if (IsRequiredValueError(normalized))
        {
            return $"Falta completar un dato obligatorio del {entityName}. Revisa los campos e inténtalo nuevamente.";
        }

        if (IsLengthError(normalized))
        {
            return $"Uno de los campos del {entityName} supera la longitud permitida. Reduce su contenido e inténtalo nuevamente.";
        }

        if (IsConstraintError(normalized))
        {
            return $"Uno de los valores del {entityName} no cumple las reglas permitidas. Revisa los datos capturados.";
        }

        if (IsConcurrencyError(exception, normalized))
        {
            return $"El {entityName} fue modificado por otra persona o proceso. Actualiza la información e inténtalo nuevamente.";
        }

        if (IsDatabaseUpdateException(exception))
        {
            return $"No se pudo guardar el {entityName}. Revisa que los datos sean válidos e inténtalo nuevamente.";
        }

        return GetUserMessage(GetInnermostMessage(exception));
    }

    public static string GetUserMessage(Exception exception)
    {
        var normalized = GetCombinedMessages(exception).ToLowerInvariant();

        if (IsDuplicateError(normalized))
        {
            return "Ya existe un registro con los mismos datos. Revisa los campos que no deben repetirse.";
        }

        if (IsDeleteRelationshipError(normalized))
        {
            return "No se puede eliminar este registro porque está siendo utilizado en otra información.";
        }

        if (IsRelationshipError(normalized))
        {
            return "No se pudo guardar porque uno de los datos seleccionados ya no existe o no es válido. Actualiza la información e inténtalo nuevamente.";
        }

        if (IsRequiredValueError(normalized))
        {
            return "Falta completar un dato obligatorio. Revisa los campos e inténtalo nuevamente.";
        }

        if (IsLengthError(normalized))
        {
            return "Uno de los campos supera la longitud permitida. Reduce su contenido e inténtalo nuevamente.";
        }

        if (IsConstraintError(normalized))
        {
            return "Uno de los valores no cumple las reglas permitidas. Revisa los datos capturados.";
        }

        if (IsConcurrencyError(exception, normalized))
        {
            return "El registro fue modificado por otra persona o proceso. Actualiza la información e inténtalo nuevamente.";
        }

        if (IsTimeoutError(normalized))
        {
            return "La operación tardó demasiado tiempo. Inténtalo nuevamente en unos momentos.";
        }

        if (IsConnectionError(normalized))
        {
            return "No fue posible conectarse con la base de datos. Verifica la conexión e inténtalo nuevamente.";
        }

        if (IsDatabaseUpdateException(exception))
        {
            return "No se pudo guardar la información. Revisa que los datos sean válidos e inténtalo nuevamente.";
        }

        return GetUserMessage(GetInnermostMessage(exception));
    }

    public static string GetUserMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return "Ocurrió un error inesperado. Inténtalo nuevamente.";
        }

        var trimmedMessage = message.Trim();
        var normalized = trimmedMessage.ToLowerInvariant();

        if (IsDuplicateError(normalized))
        {
            return "Ya existe un registro con los mismos datos. Revisa los campos que no deben repetirse.";
        }

        if (IsDeleteRelationshipError(normalized))
        {
            return "No se puede eliminar este registro porque está siendo utilizado en otra información.";
        }

        if (IsRelationshipError(normalized))
        {
            return "No se pudo guardar porque uno de los datos seleccionados ya no existe o no es válido. Actualiza la información e inténtalo nuevamente.";
        }

        if (IsRequiredValueError(normalized))
        {
            return "Falta completar un dato obligatorio. Revisa los campos e inténtalo nuevamente.";
        }

        if (IsLengthError(normalized))
        {
            return "Uno de los campos supera la longitud permitida. Reduce su contenido e inténtalo nuevamente.";
        }

        if (IsConstraintError(normalized))
        {
            return "Uno de los valores no cumple las reglas permitidas. Revisa los datos capturados.";
        }

        if (IsConcurrencyError(null, normalized))
        {
            return "El registro fue modificado por otra persona o proceso. Actualiza la información e inténtalo nuevamente.";
        }

        if (IsTimeoutError(normalized))
        {
            return "La operación tardó demasiado tiempo. Inténtalo nuevamente en unos momentos.";
        }

        if (IsConnectionError(normalized))
        {
            return "No fue posible conectarse con la base de datos. Verifica la conexión e inténtalo nuevamente.";
        }

        if (ContainsAny(
                normalized,
                "an error occurred while saving the entity changes",
                "see the inner exception for details"))
        {
            return "No se pudo guardar la información. Revisa que los datos sean válidos e inténtalo nuevamente.";
        }

        if (ContainsAny(
                normalized,
                "object reference not set",
                "sequence contains no elements",
                "index was out of range",
                "specified cast is not valid",
                "unhandled exception",
                "microsoft.entityframeworkcore",
                "system.invalidoperationexception",
                "system.nullreferenceexception"))
        {
            return "Ocurrió un error inesperado al procesar la información. Inténtalo nuevamente; si el problema continúa, contacta al administrador.";
        }

        return trimmedMessage;
    }

    private static string GetInnermostMessage(Exception exception)
    {
        var message = exception.GetBaseException().Message?.Trim();
        return string.IsNullOrWhiteSpace(message) ? exception.Message : message;
    }

    private static string GetCombinedMessages(Exception exception)
    {
        var messages = new List<string>();

        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (!string.IsNullOrWhiteSpace(current.Message))
            {
                messages.Add(current.Message);
            }
        }

        return string.Join(" | ", messages);
    }

    private static bool IsDatabaseUpdateException(Exception exception) =>
        exception.GetType().Name.Contains("DbUpdateException", StringComparison.OrdinalIgnoreCase) ||
        exception.GetBaseException().GetType().Name.Contains("SqlException", StringComparison.OrdinalIgnoreCase);

    private static bool IsDuplicateError(string message) =>
        ContainsAny(
            message,
            "duplicate key",
            "primary key constraint",
            "unique index",
            "unique key constraint",
            "cannot insert duplicate",
            "violación de la restricción unique");

    private static bool IsDeleteRelationshipError(string message) =>
        ContainsAny(message, "delete statement conflicted", "conflicted with the reference constraint");

    private static bool IsRelationshipError(string message) =>
        ContainsAny(
            message,
            "foreign key constraint",
            "reference constraint",
            "insert statement conflicted",
            "update statement conflicted");

    private static bool IsRequiredValueError(string message) =>
        ContainsAny(
            message,
            "cannot insert the value null",
            "does not allow nulls",
            "value cannot be null",
            "required property",
            "a required relationship");

    private static bool IsLengthError(string message) =>
        ContainsAny(
            message,
            "truncated",
            "exceeds the maximum length",
            "string or binary data would be truncated",
            "data too long");

    private static bool IsConstraintError(string message) =>
        ContainsAny(
            message,
            "check constraint",
            "arithmetic overflow",
            "conversion failed",
            "error converting data type",
            "out-of-range value");

    private static bool IsConcurrencyError(Exception? exception, string message) =>
        exception?.GetType().Name.Contains("DbUpdateConcurrencyException", StringComparison.OrdinalIgnoreCase) == true ||
        ContainsAny(
            message,
            "database operation was expected to affect",
            "concurrency conflict",
            "optimistic concurrency");

    private static bool IsTimeoutError(string message) =>
        ContainsAny(
            message,
            "execution timeout expired",
            "command timeout",
            "operation has timed out",
            "timeout period elapsed",
            "task was canceled");

    private static bool IsConnectionError(string message) =>
        ContainsAny(
            message,
            "network-related or instance-specific error",
            "could not open a connection",
            "no connection could be made",
            "server was not found or was not accessible",
            "the connection is broken",
            "transport-level error");

    private static bool ContainsAny(string value, params string[] patterns) =>
        patterns.Any(pattern => value.Contains(pattern, StringComparison.OrdinalIgnoreCase));
}
