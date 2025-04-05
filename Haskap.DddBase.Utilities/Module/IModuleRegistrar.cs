using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Haskap.DddBase.Utilities.Module;
public interface IModuleRegistrar
{
    IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly);
    public IList<Assembly> GetWolverineHandlerAssemblies()
    {
        return [];
    }
}
