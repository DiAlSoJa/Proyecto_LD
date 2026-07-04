using Application;
using LD.Api.Services;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace LD.Api.Configuration;

/// <summary>
/// Registro de servicios propios de la capa API (composition root): autenticación,
/// autorización por permisos, CORS, tiempo real (SignalR), controllers y Swagger.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(AssemblyMarker).Assembly);

        services
            .AddJwtAuthentication(configuration)
            .AddPermissionAuthorization()
            .AddDefaultCors()
            .AddRealtime()
            .AddSwaggerDocumentation();

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        return services;
    }

    /// <summary>Autorización basada en permisos dinámicos cargados desde BD.</summary>
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services)
    {
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthorizationHandler, PermissionHandler>();
        services.AddScoped<IAuthorizationHandler, AnyPermissionHandler>();
        services.AddAuthorization();

        return services;
    }

    /// <summary>Política CORS permisiva "AllowAll" (intencional).</summary>
    public static IServiceCollection AddDefaultCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    /// <summary>SignalR y sus servicios de soporte (tracker de conexiones + notifier).</summary>
    public static IServiceCollection AddRealtime(this IServiceCollection services)
    {
        services.AddSignalR();

        services.AddSingleton<ConnectedUsersTracker>();
        services.AddSingleton<IConnectedUsersTracker>(sp =>
            sp.GetRequiredService<ConnectedUsersTracker>());
        services.AddSingleton<IRealtimeNotifier, SignalRNotifier>();

        return services;
    }

    /// <summary>Swagger/OpenAPI con esquema de seguridad Bearer.</summary>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Mi API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando Bearer. Ejemplo: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        return services;
    }
}
