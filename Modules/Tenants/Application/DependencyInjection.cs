using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Tenants.Application.Contracts;

namespace Modules.Tenants.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITenantService, TenantService>();
        services.AddTransient<IMultiTenantQueryRunnerService, MultiTenantQueryRunnerService>();

        return services;
    }
}
