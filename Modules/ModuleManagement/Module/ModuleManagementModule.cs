using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application.Contracts.Module;
using Modules.ModuleManagement.Application.UseCaseServices;
using Modules.ModuleManagement.Domain;
using Modules.ModuleManagement.Infra;

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
            services.AddScoped<IModuleManagementModule, ModuleManagementModule>();
            services.AddUseCaseServices(configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);
            return services;
        }
    }
}
