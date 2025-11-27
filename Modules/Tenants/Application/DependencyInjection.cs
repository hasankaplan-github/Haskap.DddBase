using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Tenants.Application.Contracts.Tenants;
using Modules.Tenants.Application.Tenants;

namespace Modules.Tenants.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITenantService, TenantService>();
        services.AddTransient<IMultiTenantQueryRunnerService, MultiTenantQueryRunnerService>();

        return services;
    }
}
