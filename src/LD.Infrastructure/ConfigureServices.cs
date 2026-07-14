using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Persistence;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Interfaces.StandarLabel;
using LD.Application.Common.Interfaces.Storage;
using LD.Application.Common.Models;
using LD.Application.Features.Auth.Commands;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using LD.Infrastructure.Persistence.Interceptors;
using LD.Infrastructure.Realtime;
using LD.Infrastructure.Repositories;
using LD.Infrastructure.Services.Auth;
using LD.Infrastructure.Services.StandardLabel;
using LD.Infrastructure.Services.Storage;
using LD.Infrastructure.Workers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LD.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var blobStorageConnectionString =
            configuration.GetConnectionString("BlobStorage")
            ?? configuration["BlobStorage:ConnectionString"];

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var migrationAssembly = typeof(LdProyectDbContext).Assembly.GetName().Name;
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddDbContext<LdProyectDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>();
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly(migrationAssembly)
                );
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
                options.AddInterceptors(interceptor);
            }
        );
        services
             .AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<LdProyectDbContext>()
             .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddTransient<IApplicationUserManager, ApplicationUserManager>();

        if (environment.IsDevelopment() || string.IsNullOrWhiteSpace(blobStorageConnectionString))
        {
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
        }
        else
        {
            services.AddScoped<IFileStorageService, AzureBlobFileStorageService>();
        }
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IStandarIdService, StandardLabelService>();
        services.AddScoped<ITransactionManager, TransactionManager>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly));

        services.AddHostedService<TaskDispatcherWorker>();
        services.AddHostedService<SysLogRetentionWorker>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddRealtime();

        return services;
    }

    /// <summary>
    /// SignalR y sus servicios de soporte (tracker de conexiones + notifier).
    /// El realtime es un detalle de transporte, por eso vive en la capa de infraestructura.
    /// El mapeo del endpoint del hub (MapHub) se hace en el pipeline de LD.Api.
    /// </summary>
    private static IServiceCollection AddRealtime(this IServiceCollection services)
    {
        services.AddSignalR();

        // El tracker es un singleton con estado en memoria. Se registra una sola vez
        // como concreto y la interfaz resuelve a esa MISMA instancia, para que el Hub
        // (concreto) y los consumidores (interfaz) compartan el mismo estado.
        services.AddSingleton<ConnectedUsersTracker>();
        services.AddSingleton<IConnectedUsersTracker>(sp =>
            sp.GetRequiredService<ConnectedUsersTracker>());
        services.AddSingleton<IRealtimeNotifier, SignalRNotifier>();

        return services;
    }

    public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IArchiveRepository<>), typeof(ArchiveRepository<>));

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IOperationalTaskRepository, OperationalTaskRepository>();
        services.AddScoped<IWarehouseTaskRepository, WarehouseTaskRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IInventoryMovementRepository, InventoryMovementRepository>();
        services.AddScoped<IAvailableInventoryRepository, AvailableInventoryRepository>();
        services.AddScoped<IInventarioCiclicoRepository, InventarioCiclicoRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IFamilyRepository, FamilyRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<IDimensionerRepository, DimensionerRepository>();
        services.AddScoped<IAsnDetailRepository, AsnDetailRepository>();
        services.AddScoped<IAsnReceiptDetailRepository, AsnReceiptRepository>();
        services.AddScoped<IAsnRepository, AsnRepository>();
        services.AddScoped<IKittingRepository, KittingRepository>();
        services.AddScoped<ISecurityRegistrationRepository, SecurityRegistrationRepository>();
        services.AddScoped<IReportQueryRepository, ReportQueryRepository>();
        services.AddScoped<ISystemFieldRepository, SystemFieldRepository>();
        services.AddScoped<ILookupRepository<ScanType>, ScanTypeLookupRepository>();
        services.AddScoped<ILookupRepository<ScanSaveType>, ScanSaveTypeLookupRepository>();

        services.AddScoped<ISecurityRegistrationRepository, SecurityRegistrationRepository>();
        services.AddScoped<ISecurityTaskRepository,         SecurityTaskRepository>();
        services.AddScoped<IChecklistRepository,            ChecklistRepository>();

        return services;
    }
}
