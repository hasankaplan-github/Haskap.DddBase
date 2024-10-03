using Haskap.DddBase.Modules.Tenants.Application.UseCaseServices;
using Haskap.DddBase.Modules.Tenants.Infra;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Modules.Tenants.Module;
public class TenantModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName)
    {
        services.AddUseCaseServices();
        services.AddInfra(configuration, connectionStringName);

        return services;
    }
}
