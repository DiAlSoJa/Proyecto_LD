using LD.Application.Common.Interfaces.Repository;
using LD.Contracts.DTOs.ReportQueries;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using System.Globalization;

namespace LD.Infrastructure.Repositories;

public class ReportQueryRepository : Repository<ReportQuery>, IReportQueryRepository
{
    public ReportQueryRepository(LdProyectDbContext ldProyectDbContext)
        : base(ldProyectDbContext)
    {
    }

    public async Task<ReportQueryExecutionResultDto> ExecuteAsync(
        string sqlQuery,
        IReadOnlyDictionary<string, string?> parameters,
        CancellationToken cancellationToken = default)
    {
        var connection = _context.Database.GetDbConnection();
        var shouldCloseConnection = connection.State != ConnectionState.Open;

        if (shouldCloseConnection)
            await _context.Database.OpenConnectionAsync(cancellationToken);

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 120;

            foreach (var parameter in parameters)
            {
                var dbParameter = command.CreateParameter();
                dbParameter.ParameterName = parameter.Key.StartsWith('@')
                    ? parameter.Key
                    : $"@{parameter.Key}";
                dbParameter.Value = ParseParameterValue(parameter.Value);
                command.Parameters.Add(dbParameter);
            }

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var columnNames = BuildColumnNames(reader);
            var rows = new List<Dictionary<string, string?>>();

            while (await reader.ReadAsync(cancellationToken))
            {
                var row = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

                for (var index = 0; index < reader.FieldCount; index++)
                {
                    var columnName = columnNames[index];
                    row[columnName] = reader.IsDBNull(index)
                        ? null
                        : FormatValue(reader.GetValue(index));
                }

                rows.Add(row);
            }

            return new ReportQueryExecutionResultDto
            {
                Columns = columnNames,
                Rows = rows
            };
        }
        finally
        {
            if (shouldCloseConnection)
                await _context.Database.CloseConnectionAsync();
        }
    }

    private static List<string> BuildColumnNames(DbDataReader reader)
    {
        var columnNames = new List<string>(reader.FieldCount);
        var usedNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (var index = 0; index < reader.FieldCount; index++)
        {
            var baseName = reader.GetName(index);
            if (string.IsNullOrWhiteSpace(baseName))
                baseName = $"Column{index + 1}";

            var candidate = baseName.Trim();
            if (usedNames.TryGetValue(candidate, out var currentCount))
            {
                currentCount++;
                usedNames[candidate] = currentCount;
                candidate = $"{candidate}_{currentCount}";
            }
            else
            {
                usedNames[candidate] = 1;
            }

            columnNames.Add(candidate);
        }

        return columnNames;
    }

    private static object ParseParameterValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DBNull.Value;

        var trimmed = value.Trim();
        if (string.Equals(trimmed, "null", StringComparison.OrdinalIgnoreCase))
            return DBNull.Value;

        if (bool.TryParse(trimmed, out var boolValue))
            return boolValue;

        if (int.TryParse(trimmed, NumberStyles.Integer, CultureInfo.CurrentCulture, out var intValue))
            return intValue;

        if (long.TryParse(trimmed, NumberStyles.Integer, CultureInfo.CurrentCulture, out var longValue))
            return longValue;

        if (decimal.TryParse(trimmed, NumberStyles.Number, CultureInfo.CurrentCulture, out var decimalValue))
            return decimalValue;

        if (DateTime.TryParse(trimmed, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out var dateValue))
            return dateValue;

        if (DateTime.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out dateValue))
            return dateValue;

        if (Guid.TryParse(trimmed, out var guidValue))
            return guidValue;

        return trimmed;
    }

    private static string FormatValue(object value)
    {
        return value switch
        {
            DateTime dateTime => dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture),
            DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture),
            TimeSpan timeSpan => timeSpan.ToString(),
            bool booleanValue => booleanValue ? "True" : "False",
            IFormattable formattable => formattable.ToString(null, CultureInfo.CurrentCulture) ?? string.Empty,
            _ => Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty
        };
    }
}
