using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Events;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application.Contracts;
using Modules.Tenants.Application;
using Modules.Tenants.Domain.Shared;
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
            services.AddApplication(configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);
            services.RegisterHandlersFromAssembly(typeof(Application.DependencyInjection).Assembly);

            return services;
        }
    }
}
