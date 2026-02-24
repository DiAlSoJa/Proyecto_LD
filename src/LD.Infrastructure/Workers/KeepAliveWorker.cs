using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public KeepAliveWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<LdProyectDbContext>();

                    await context.Database.ExecuteSqlRawAsync("SELECT 1");

                    await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
                }

            }catch (Exception ex)
            {
            }
        }
    }

}
