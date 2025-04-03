using Modules.Tenants.Application.Contracts.Tenants;
using Modules.Tenants.Application.UseCaseServices.Tenants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Haskap.DddBase.Utilities.Mediator;

namespace Modules.Tenants.Application.UseCaseServices;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        services.AddMediatorConsumersFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddTransient<ITenantService, TenantService>();

        return services;
    }
}
