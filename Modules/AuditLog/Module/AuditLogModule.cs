using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.AuditLog.Domain.Shared;
using Modules.AuditLog.Infra;
using Modules.ModuleManagement.Application.Contracts;

namespace Modules.AuditLog.Module;

public class AuditLogModule : BaseModule<AuditLogModule>, IAuditLogModule
{
    public AuditLogModule(
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