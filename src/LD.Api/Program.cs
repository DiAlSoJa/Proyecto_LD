using Application;
using LD.Api.Authorization;
using LD.Api.Middlewares;
using LD.Application;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Constants;
using LD.Infrastructure;
using LD.Infrastructure.Authorization;
using LD.Infrastructure.Logging;
using LD.Infrastructure.Persistence;
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
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddInfrastructureRepositories(builder.Configuration);

builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LdProyectDbContext>();
    db.Database.Migrate();

    var authOptions = scope.ServiceProvider.GetRequiredService<IOptions<AuthorizationOptions>>();

    var permissions = db.Permissions
        .Select(p => p.Key)
        .ToList();

    foreach (var permission in permissions)
    {
        authOptions.Value.AddPolicy(permission, policy =>
            policy.Requirements.Add(new PermissionRequirement(permission)));
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSerilogRequestLogging();

//middlewares
app.UseMiddleware<GlobalExceptionMiddleware>();

app.Run();
