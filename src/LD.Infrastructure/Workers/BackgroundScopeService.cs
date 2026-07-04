using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LD.Infrastructure.Workers;

/// <summary>
/// Base class for background workers that need to consume scoped services.
/// Creates a new DI scope on every iteration so that scoped services
/// (e.g. SyncService, CallWebService) are never captured in a singleton.
/// </summary>
public abstract class ScopedBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    protected ScopedBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// How long to wait between iterations. Override to customise per worker.
    /// </summary>
    protected virtual TimeSpan GetInterval() => TimeSpan.FromMinutes(1);

    /// <summary>
    /// Creates a new DI scope. Available to derived classes for on-demand use.
    /// </summary>
    protected IServiceScope CreateScope() => _scopeFactory.CreateScope();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using var scope = _scopeFactory.CreateScope();
                await DoWorkAsync(scope.ServiceProvider, stoppingToken);
                await Task.Delay(GetInterval(), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Apagado normal del host; no propagar.
        }
    }

    /// <summary>
    /// Called once per interval with a fresh DI scope.
    /// Resolve scoped services from <paramref name="serviceProvider"/>.
    /// </summary>
    protected abstract Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken stoppingToken);
}

