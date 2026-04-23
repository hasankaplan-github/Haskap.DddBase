using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application.Contracts.Module;
using Modules.SpecialDayBase.Application.Contracts;
using Modules.SpecialDayBase.Application;

namespace Modules.SpecialDayBase.Module;

public class SpecialDayBaseModule : BaseModule<SpecialDayBaseModule>, ISpecialDayBaseModule
{
    public SpecialDayBaseModule(
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
