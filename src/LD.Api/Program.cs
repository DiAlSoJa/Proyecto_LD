using LD.Api.Configuration;
using LD.Application;
using LD.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration, builder.Environment)
    .AddInfrastructureRepositories(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

app.InitializeDatabase();

app.UseApiPipeline();

app.Run();
