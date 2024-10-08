using Haskap.DddBase.Modules.Tenants.Application.Contracts.Tenants;
using Haskap.DddBase.Modules.Tenants.Application.UseCaseServices.Tenants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Modules.Tenants.Application.UseCaseServices;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddTransient<ITenantService, TenantService>();

        return services;
    }
}
