using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Features.Auth.Commands;
using LD.Infrastructure.Persistence;
using LD.Infrastructure.Repositories;
using LD.Infrastructure.Services.Auth;
using LD.Infrastructure.Workers;
using Microsoft.AspNetCore.Identity;
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

        services
             .AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<LdProyectDbContext>()
             .AddDefaultTokenProviders();


        services.AddHttpContextAccessor();
        services.AddTransient<IApplicationUserManager, ApplicationUserManager>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserContextService, UserContextService>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly));

        services.AddHostedService<KeepAliveWorker>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        //genericos
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IArchiveRepository<>), typeof(ArchiveRepository<>));

        //especificos
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();



        return services;
    }
}
