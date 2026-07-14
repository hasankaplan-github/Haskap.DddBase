using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Email.Application;
using Modules.Email.Domain.Shared;
using Modules.ModuleManagement.Application.Contracts;

namespace Modules.Email.Module;
public class EmailModule : BaseModule<EmailModule>, IEmailModule
{
    public EmailModule(
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
            //services.AddInfra(configuration, connectionStringName, migrationAssembly);
            //services.AddPresentation(configuration);
            return services;
        }

        //IList<Assembly> IModuleRegistrar.GetWolverineHandlerAssemblies()
        //{
        //    return [typeof(Application.DependencyInjection).Assembly];
        //}
    }
}