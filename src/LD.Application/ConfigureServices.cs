using Application;
using FluentValidation;
using LD.Application.Common.Behaviors;
using LD.Application.Features.Clients.Profiles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LD.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddValidatorsFromAssembly(
            typeof(AssemblyMarker).Assembly
        );


        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>)
        );

        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));

        return services;
    }
}
