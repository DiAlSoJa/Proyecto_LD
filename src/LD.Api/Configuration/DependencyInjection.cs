using Application;
using LD.Api.Common.Validation;
using LD.Application.Common.Results;
using LD.Application.Common.Interfaces.Auth;
using LD.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            .AddSwaggerDocumentation();

        services
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(entry => entry.Value?.Errors.Count > 0)
                        .SelectMany(entry => entry.Value!.Errors.Select(error =>
                            ModelValidationMessageHelper.GetUserMessage(
                                entry.Key,
                                string.IsNullOrWhiteSpace(error.ErrorMessage)
                                    ? error.Exception?.Message
                                    : error.ErrorMessage)))
                        .Distinct(StringComparer.Ordinal)
                        .ToList();

                    if (errors.Count == 0)
                    {
                        errors.Add("La información enviada no es válida.");
                    }

                    var result = Result<string>.Failure(
                        "Revisa los datos capturados.",
                        errors,
                        StatusCodes.Status400BadRequest);

                    return new BadRequestObjectResult(result);
                };
            });
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

    /// <summary>Swagger/OpenAPI con esquema de seguridad Bearer.</summary>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Warehouse System Api",
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
