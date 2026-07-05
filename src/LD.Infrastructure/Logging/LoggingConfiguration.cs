using System.Data;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace LD.Infrastructure.Logging
{
    /// <summary>
    /// Helpers de configuración de Serilog que viven en infraestructura porque
    /// dependen de detalles de persistencia (SQL Server) y del esquema de columnas.
    /// </summary>
    public static class LoggingConfiguration
    {
        /// <summary>Nombre de la tabla donde se persisten los logs. Reutilizado por el worker de purga.</summary>
        public const string SysLogTableName = "sys_logs";

        /// <summary>Enrichers base compartidos por todos los sinks.</summary>
        public static LoggerConfiguration AddInfrastructureLogging(
            this LoggerConfiguration logger)
        {
            return logger
                .Enrich.FromLogContext();
        }

        /// <summary>
        /// Sink a SQL Server (tabla <see cref="SysLogTableName"/>) restringido a Warning+.
        /// Solo se registra si hay cadena de conexión; si no, se omite silenciosamente
        /// para que la API pueda arrancar igual (ej. Swagger sin BD).
        /// </summary>
        public static LoggerConfiguration WriteToSysLogs(
            this LoggerConfiguration logger,
            string? connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return logger;

            var columnOptions = new ColumnOptions();

            // Guardamos el detalle estructurado como JSON (LogEvent) y quitamos el XML (Properties).
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Add(StandardColumn.LogEvent);

            // TimeStamp en UTC y como datetimeoffset para que el worker de purga compare limpio.
            columnOptions.TimeStamp.DataType = SqlDbType.DateTimeOffset;
            columnOptions.TimeStamp.ConvertToUtc = true;

            // Columnas propias del dominio (WMS multi-cliente). Se llenan vía LogContext
            // desde SysLogEnrichmentMiddleware y desde UseSerilogRequestLogging (StatusCode).
            columnOptions.AdditionalColumns = new List<SqlColumn>
            {
                new("UserId",        SqlDbType.NVarChar, dataLength: 450),
                new("UserName",      SqlDbType.NVarChar, dataLength: 256),
                new("SourceApp",     SqlDbType.NVarChar, dataLength: 50),
                new("RequestPath",   SqlDbType.NVarChar, dataLength: 256),
                new("RequestMethod", SqlDbType.NVarChar, dataLength: 10),
                new("StatusCode",    SqlDbType.Int),
                new("WarehouseId",   SqlDbType.Int),
                new("ClientIp",      SqlDbType.NVarChar, dataLength: 45),
                new("MachineName",   SqlDbType.NVarChar, dataLength: 128),
                new("CorrelationId", SqlDbType.NVarChar, dataLength: 64),
            };

            return logger.WriteTo.MSSqlServer(
                connectionString: connectionString,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = SysLogTableName,
                    AutoCreateSqlTable = true,
                    BatchPostingLimit = 50,
                    BatchPeriod = TimeSpan.FromSeconds(5),
                },
                restrictedToMinimumLevel: LogEventLevel.Warning,
                columnOptions: columnOptions);
        }
    }
}
