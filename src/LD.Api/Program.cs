using LD.Api.Configuration;
using LD.Application;
using LD.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

try
{
    builder.Services
        .AddApplicationServices()
        .AddInfrastructureServices(builder.Configuration, builder.Environment)
        .AddInfrastructureRepositories(builder.Configuration)
        .AddApiServices(builder.Configuration);

    var app = builder.Build();

    app.InitializeDatabase();

    app.UseApiPipeline();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La API terminó inesperadamente durante el arranque");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
z|