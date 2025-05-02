using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application.Contracts.Module;
using Modules.Tenants.Application.Contracts;
using Modules.Tenants.Application;
using Modules.Tenants.Infra;

namespace Modules.Tenants.Module;

public class TenantsModule : BaseModule<TenantsModule>, ITenantsModule
{
    public TenantsModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
        : base(moduleService, currentTenantProvider)
    {
    }

    public class Registrar : IModuleRegistrar
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        {
            services.AddUseCaseServices(configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);
            return services;
        }
    }
}
