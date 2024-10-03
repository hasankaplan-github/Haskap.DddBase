using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Utilities.Module;
public interface IModule
{
    IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName);
}
