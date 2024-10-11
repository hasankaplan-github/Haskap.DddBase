using Modules.ViewLevelExceptions.Application.UseCaseServices;
using Modules.ViewLevelExceptions.Infra;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.ViewLevelExceptions.Module;
public class ViewLevelExceptionModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        services.AddUseCaseServices(configuration);
        services.AddInfra(configuration, connectionStringName, migrationAssembly);

        return services;
    }
}
