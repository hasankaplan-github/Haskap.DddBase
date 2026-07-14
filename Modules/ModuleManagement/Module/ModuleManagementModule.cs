using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application;
using Modules.ModuleManagement.Domain.Shared;
using Modules.ModuleManagement.Infra;
using Modules.ModuleManagement.Application.Contracts;

namespace Modules.ModuleManagement.Module;

public class ModuleManagementModule : BaseModule<ModuleManagementModule>, IModuleManagementModule
{
    public ModuleManagementModule(
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
            return services;
        }
    }
}
