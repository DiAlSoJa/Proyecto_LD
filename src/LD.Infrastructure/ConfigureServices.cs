using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Features.Auth.Commands;
using LD.Infrastructure.Identity;
using LD.Infrastructure.Persistence;
using LD.Infrastructure.Repositories;
using LD.Infrastructure.Services.Auth;
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





        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly));

        return services;
    }

    public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


        return services;
    }
}
