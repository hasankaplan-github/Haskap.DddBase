using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application.Contracts.Module;
using Modules.ViewLevelExceptions.Application.UseCaseServices;
using Modules.ViewLevelExceptions.Domain;
using Modules.ViewLevelExceptions.Infra;

namespace Modules.ViewLevelExceptions.Module;

public class ViewLevelExceptionsModule : BaseModule<ViewLevelExceptionsModule>, IViewLevelExceptionsModule
{
    public ViewLevelExceptionsModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
        : base(moduleService, currentTenantProvider)
    {
    }

    public class Registrar : IModuleRegistrar
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        {
            services.AddScoped<IViewLevelExceptionsModule, ViewLevelExceptionsModule>();
            services.AddUseCaseServices(configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);
            return services;
        }
    }
}
