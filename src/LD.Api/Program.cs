using Application;
using LD.Api.Authorization;
using LD.Api.Hubs;
using LD.Api.Middlewares;
using LD.Api.Services;
using LD.Application;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Constants;
using LD.Infrastructure;
using LD.Infrastructure.Authorization;
using LD.Infrastructure.Logging;
using LD.Infrastructure.Persistence;
using LD.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Compact;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var logPath = Path.Combine(builder.Environment.ContentRootPath, "logs", "log-.json");

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        new CompactJsonFormatter(),
        logPath,
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .WriteTo.Console();
});

// Add services to the container.
builder.Services.AddHttpContextAccessor();

//aplication
builder.Services.AddAutoMapper(typeof(AssemblyMarker).Assembly);

builder.Services.AddApplicationServices();

//infrasctruture
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);
builder.Services.AddInfrastructureRepositories(builder.Configuration);

builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddScoped<IAuthorizationHandler, AnyPermissionHandler>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>()!;

builder.Services.AddAuthentication(options =>
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

            var result = Result<string>.Failure("No autorizado",new List<string> (){"Necesitas authenticatrte" },401);

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
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSignalR();

// SignalR support services
builder.Services.AddSingleton<ConnectedUsersTracker>();
builder.Services.AddSingleton<IConnectedUsersTracker>(sp =>
    sp.GetRequiredService<ConnectedUsersTracker>());
builder.Services.AddSingleton<IRealtimeNotifier, SignalRNotifier>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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

var app = builder.Build();

var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(defaultConnectionString))
{
    app.Logger.LogWarning(
        "DefaultConnection is not configured. Skipping database migration and permission seeding so the API can start.");
}
else
{
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
            var startupLogDir = Path.Combine(builder.Environment.ContentRootPath, "logs");
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

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

//middlewares
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
