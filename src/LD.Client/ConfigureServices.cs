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
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddSingleton<ApiEndpoints>();

            services.AddScoped<ApiService>();
            services.AddScoped<AuthService>();
            services.AddScoped<ClientService>();
            services.AddScoped<ItemService>();
            services.AddScoped<LocationService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<WarehouseService>();
            services.AddScoped<UserService>();
            services.AddScoped<LookupService>();

            return services;
        }
    }
}
