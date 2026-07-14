using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.TurkiyeSpecialDay.Infra;
using Modules.TurkiyeSpecialDay.Domain.Shared;
using Modules.ModuleManagement.Application.Contracts;

namespace Modules.TurkiyeSpecialDay.Module;
public class TurkiyeSpecialDayModule : BaseModule<TurkiyeSpecialDayModule>, ITurkiyeSpecialDayModule
{
    public TurkiyeSpecialDayModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
        : base(moduleService, currentTenantProvider)
    {
    }

    public class Registrar : IModuleRegistrar
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        {
            services.AddInfra(configuration, connectionStringName, migrationAssembly);
            return services;
        }
    }
}
