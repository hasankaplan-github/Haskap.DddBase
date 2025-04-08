using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Tenants.Application.Contracts.Tenants;
using Modules.Tenants.Application.UseCaseServices.Tenants;

namespace Modules.Tenants.Application.UseCaseServices;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITenantService, TenantService>();

        return services;
    }
}
