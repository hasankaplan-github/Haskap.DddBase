using Modules.Tenants.Application.UseCaseServices;
using Modules.Tenants.Infra;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Tenants.Module;
public class TenantModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string migrationAssembly)
    {
        services.AddUseCaseServices(configuration);
        services.AddInfra(configuration, connectionStringName, migrationAssembly);

        return services;
    }
}
