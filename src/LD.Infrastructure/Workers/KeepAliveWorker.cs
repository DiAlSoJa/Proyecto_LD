using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Workers
{
    public class KeepAliveWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<KeepAliveWorker> _logger;

        public KeepAliveWorker(IServiceScopeFactory scopeFactory, ILogger<KeepAliveWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger=logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           // while (!stoppingToken.IsCancellationRequested)
           // {
           //     try
           //     {
           //         using var scope = _scopeFactory.CreateScope();
           //         var context = scope.ServiceProvider.GetRequiredService<LdProyectDbContext>();

           //         await context.Database.ExecuteSqlRawAsync("SELECT 1");
           //     }
           //     catch (Exception ex)
           //     {
           //         _logger.LogWarning(ex, "KeepAlive query falló");
           //     }

           //     await Task.Delay(TimeSpan.FromMinutes(4), stoppingToken);
           //}
        
        }
    }

}
