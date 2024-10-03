using Haskap.DddBase.Modules.ViewLevelExceptions.Application.UseCaseServices;
using Haskap.DddBase.Modules.ViewLevelExceptions.Infra;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Modules.ViewLevelExceptions.Module;
public class ViewLevelExceptionModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName)
    {
        services.AddUseCaseServices();
        services.AddInfra(configuration, connectionStringName);

        return services;
    }
}
