using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.CustomMessage.Application;
using Modules.CustomMessage.Application.Contracts;
using Modules.ModuleManagement.Application.Contracts.Module;

namespace Modules.CustomMessage.Module;

public class CustomMessageModule : BaseModule<CustomMessageModule>, ICustomMessageModule
{
    public CustomMessageModule(
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

            return services;
        }
    }
}
