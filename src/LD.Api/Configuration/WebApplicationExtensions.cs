using LD.Api.Authorization;
using LD.Api.Middlewares;
using LD.Infrastructure.Realtime;
using LD.Contracts.Constants;
using LD.Infrastructure.Persistence;
using LD.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace LD.Api.Configuration;

/// <summary>
/// Inicialización en el arranque (migración + siembra de permisos + carga de policies
/// dinámicas) y configuración del pipeline HTTP.
/// </summary>
public static class WebApplicationExtensions
{
    public static void InitializeDatabase(this WebApplication app)
    {
        var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            app.Logger.LogWarning(
                "DefaultConnection is not configured. Skipping database migration and permission seeding so the API can start.");
            return;
        }

        try
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LdProyectDbContext>();
            db.Database.Migrate();

            // Permisos que no se gestionan vía migración (HasData) se siembran aquí.
            PermissionSeeder.Seed(db);

            // Carga de policies dinámicas: cada permiso en BD se registra como policy.
            var authOptions = scope.ServiceProvider.GetRequiredService<IOptions<AuthorizationOptions>>();

            var permissions = db.Permissions
                .Select(p => p.Key)
                .ToList();

            foreach (var permission in permissions)
            {
                authOptions.Value.AddPolicy(permission, policy =>
                    policy.Requirements.Add(new PermissionRequirement(permission)));
            }

            var anyPermissionPolicies = new[]
            {
                AnyPermissionRequirement.BuildPolicyName(new[]
                {
                    PermissionKeys.Asn_View,
                    PermissionKeys.WarehouseStaff_Asn_View
                }),
                AnyPermissionRequirement.BuildPolicyName(new[]
                {
                    PermissionKeys.Shipment_View,
                    PermissionKeys.KittingFolioCapture_Access
                })
            };

            foreach (var policyName in anyPermissionPolicies)
            {
                var policyPermissions = AnyPermissionRequirement.ParsePolicyName(policyName);
                authOptions.Value.AddPolicy(policyName, policy =>
                    policy.Requirements.Add(new AnyPermissionRequirement(policyPermissions)));
            }
        }
        catch (Exception ex)
        {
            app.Logger.LogError(
                ex,
                "Database initialization failed during startup. Swagger will remain available, but database-backed features may not work until the connection string or database are fixed.");

            try
            {
                var startupLogDir = Path.Combine(app.Environment.ContentRootPath, "logs");
                Directory.CreateDirectory(startupLogDir);

                var startupLogPath = Path.Combine(startupLogDir, "startup-exception.txt");
                var startupLogContent = $"""
                    {DateTime.UtcNow:O}
                    {ex}

                    """;

                File.AppendAllText(startupLogPath, startupLogContent);
            }
            catch
            {
                // Best-effort fallback only. If writing the file fails, we still rethrow the original exception.
            }

            throw;
        }
    }

    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        // Swagger siempre habilitado (no solo en Development).
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("AllowAll");
        app.UseHttpsRedirection();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        // Enriquece el LogContext con usuario/origen/request. Va después de auth
        // (para tener context.User) y antes de UseSerilogRequestLogging.
        app.UseMiddleware<SysLogEnrichmentMiddleware>();

        app.UseSerilogRequestLogging();

        app.MapControllers();
        app.MapHub<NotificationHub>("/hubs/notifications");

        return app;
    }
}
