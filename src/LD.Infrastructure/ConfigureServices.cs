using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LD.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var migrationAssembly = typeof(LdProyectDbContext).Assembly.GetName().Name;
        services.AddDbContext<LdProyectDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly(migrationAssembly)
            )
        );

        return services;
    }
}
