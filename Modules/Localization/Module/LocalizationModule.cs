using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Localization.Application.Contracts;
using Modules.Localization.Infra;
using Modules.ModuleManagement.Application.Contracts.Module;

namespace Modules.Localization.Module;

public class LocalizationModule : BaseModule<LocalizationModule>, ILocalizationModule
{
    public LocalizationModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
        : base(moduleService, currentTenantProvider)
    {
    }

    public class Registrar : IModuleRegistrar
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        {
            //services.AddUseCaseServices(configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);
            //services.AddPresentation(configuration);
            return services;
        }
    }
}
