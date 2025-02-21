using Modules.ViewLevelExceptions.Application.UseCaseServices;
using Modules.ViewLevelExceptions.Infra;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Haskap.DddBase.Domain.Providers;
using Modules.ViewLevelExceptions.Application.Contracts;
using Modules.ModuleManagement.Application.Contracts.Module;

namespace Modules.ViewLevelExceptions.Module;

public class ViewLevelExceptionModule : BaseModule<ViewLevelExceptionModule>, IViewLevelExceptionModule
{
    public ViewLevelExceptionModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
        : base(moduleService, currentTenantProvider)
    {
    }

    public class Registrar : IModuleRegistrar
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        {
            services.AddScoped<IViewLevelExceptionModule, ViewLevelExceptionModule>();
            services.AddUseCaseServices(configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);
            return services;
        }
    }
}
