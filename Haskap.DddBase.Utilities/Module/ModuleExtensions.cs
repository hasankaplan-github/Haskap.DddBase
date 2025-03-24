using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Utilities.Module;
public static class ModuleExtensions
{
    public static IServiceCollection AddModule<TModuleRegistrar>(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        where TModuleRegistrar : class, IModuleRegistrar, new()
    {
        var moduleImplementationType = typeof(TModuleRegistrar).DeclaringType!;
        var moduleInterfaceType = moduleImplementationType.GetInterfaces().First(x => x.GetInterfaces().Contains(typeof(IModule)));
            //.First(x => x.Name == $"I{moduleImplementationType.Name}");

        services.AddScoped(moduleInterfaceType, moduleImplementationType);

        var moduleRegistrar = new TModuleRegistrar();
        return moduleRegistrar.RegisterModule(services, configuration, connectionStringName, migrationAssembly);
    }
}