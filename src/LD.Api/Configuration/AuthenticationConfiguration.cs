using LD.Application.Common.Models;
using LD.Application.Common.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace LD.Api.Configuration;

/// <summary>
/// Autenticación JWT del API: validación del token, extracción del token por
/// query string para SignalR, y respuestas 401/403 en formato <see cref="Result{T}"/>.
/// </summary>
public static class AuthenticationConfiguration
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        var jwtSettings = configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>()!;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };

            options.Events = new JwtBearerEvents
            {
                // SignalR WebSocket/SSE: el navegador no puede enviar el header Authorization,
                // así que el token llega en la query string "?access_token=..."
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(accessToken) &&
                        context.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = async context =>
                {
                    context.HandleResponse(); // evita la respuesta default

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var result = Result<string>.Failure("No autorizado", new List<string>() { "Necesitas authenticatrte" }, 401);

                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                },
                OnForbidden = async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    var result = Result<string>.Failure("No tienes acceso", new List<string>() { "Necesitas auhtorizacion" }, 403);

                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                }
            };
        });

        return services;
    }
}
