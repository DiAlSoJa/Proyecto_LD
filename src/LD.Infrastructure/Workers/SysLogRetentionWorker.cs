using LD.Infrastructure.Logging;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LD.Infrastructure.Workers;

/// <summary>
/// Purga registros de <see cref="LoggingConfiguration.SysLogTableName"/> más antiguos
/// que <see cref="RetentionPeriod"/>. Corre una vez al día (y una al arrancar).
/// SQL Express no tiene SQL Agent, por eso la retención vive como worker en proceso.
/// </summary>
public sealed class SysLogRetentionWorker : ScopedBackgroundService
{
    private static readonly TimeSpan RetentionPeriod = TimeSpan.FromDays(7);
    private static readonly TimeSpan CheckInterval   = TimeSpan.FromHours(24);

    private readonly ILogger<SysLogRetentionWorker> _logger;

    public SysLogRetentionWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<SysLogRetentionWorker> logger)
        : base(scopeFactory)
    {
        _logger = logger;
    }

    protected override TimeSpan GetInterval() => CheckInterval;

    protected override async Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken)
    {
        try
        {
            var db = serviceProvider.GetRequiredService<LdProyectDbContext>();
            var cutoff = DateTimeOffset.UtcNow - RetentionPeriod;

            var deleted = await db.Database.ExecuteSqlRawAsync(
                $"DELETE FROM [{LoggingConfiguration.SysLogTableName}] WHERE [TimeStamp] < {{0}}",
                new object[] { cutoff },
                stoppingToken);

            if (deleted > 0)
                _logger.LogInformation(
                    "SysLogRetentionWorker: {Count} registros de {Table} purgados (> {Days} días)",
                    deleted, LoggingConfiguration.SysLogTableName, RetentionPeriod.TotalDays);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // Un ciclo con error no debe tumbar el worker; se reintenta al próximo intervalo.
            _logger.LogError(ex, "Error purgando {Table} en SysLogRetentionWorker",
                LoggingConfiguration.SysLogTableName);
        }
    }
}
