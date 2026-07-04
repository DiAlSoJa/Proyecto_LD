using Serilog;
using Serilog.Formatting.Compact;

namespace LD.Api.Configuration;

/// <summary>
/// Configuración de Serilog para el host: bootstrap logger a archivo JSON +
/// lectura de configuración/servicios en runtime.
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
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .WriteTo.Console();
        });

        return builder;
    }
}
