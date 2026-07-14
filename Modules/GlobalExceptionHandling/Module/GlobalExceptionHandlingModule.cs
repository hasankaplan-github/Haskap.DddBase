using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.GlobalExceptionHandling.Domain.Shared;
using Modules.ModuleManagement.Application.Contracts;

namespace Modules.GlobalExceptionHandling.Module;

public class GlobalExceptionHandlingModule : BaseModule<GlobalExceptionHandlingModule>, IGlobalExceptionHandlingModule
{
    public GlobalExceptionHandlingModule(
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
            //services.AddInfra(configuration, connectionStringName, migrationAssembly);
            return services;
        }
    }
}
