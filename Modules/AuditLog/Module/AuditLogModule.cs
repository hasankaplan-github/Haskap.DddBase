using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.AuditLog.Infra;

namespace Modules.AuditLog.Module;
public class AuditLogModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string migrationAssembly)
    {
        services.AddInfra(configuration, connectionStringName, migrationAssembly);

        return services;
    }
}
