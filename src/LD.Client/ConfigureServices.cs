using LD.Client.Services;
using LD.Forms.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Client
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddLDClient(this IServiceCollection services,Action<ApiSettings> configure)
        {
            services.Configure(configure);

            services.AddSingleton<ApiEndpoints>();

            services.AddSingleton<ApiService>();

            services.AddScoped<ApiService>();
            services.AddScoped<AuthService>();
            services.AddScoped<ClientService>();
            services.AddScoped<ModuleService>();
            services.AddScoped<ProductService>();
            services.AddScoped<LocationService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<WarehouseService>();
            services.AddScoped<UserService>();
            services.AddScoped<RoleService>();
            services.AddScoped<UnitService>();
            services.AddScoped<InventaryStatusService>();
            services.AddScoped<CurrencyService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<FamilyService>();
            services.AddScoped<DimensionerService>();
            services.AddScoped<VehicleService>();
            services.AddScoped<SecurityService>();
            services.AddScoped<InventoryMovementService>();
            services.AddScoped<AvailableInventoryService>();

            services.AddScoped<LookupService>();
            services.AddScoped<AsnService>();
            services.AddScoped<AsnDetailService>();
            services.AddScoped<AsnReceiptService>();

            return services;
        }
    }
}
