using LD.Infrastructure.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace LD.Api.Configuration;

/// <summary>
/// Configuración de Serilog para el host: bootstrap logger a archivo JSON +
/// lectura de configuración/servicios en runtime + sink a la tabla sys_logs.
/// </summary>
public static class SerilogConfiguration
{
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        var logPath = Path.Combine(builder.Environment.ContentRootPath, "logs", "log-.json");

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                new CompactJsonFormatter(),
                logPath,
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

            configuration
                .ReadFrom.Configuration(context.Configuration)   // niveles/overrides desde appsettings ("Serilog")
                .ReadFrom.Services(services)
                .AddInfrastructureLogging()                      // Enrich.FromLogContext
                .WriteTo.Console()
                .WriteToSysLogs(connectionString);               // sink MSSqlServer → sys_logs (Warning+)
        });

        return builder;
    }
}
