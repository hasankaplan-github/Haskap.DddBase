using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Events;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Localization.Application;
using Modules.Localization.Domain.Shared;
using Modules.Localization.Infra;
using Modules.Localization.Presentation;
using Modules.ModuleManagement.Application.Contracts;

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
            services.AddApplication(configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);
            services.AddPresentation(configuration);
            services.RegisterHandlersFromAssembly(typeof(Application.DependencyInjection).Assembly);
            return services;
        }
    }
}
