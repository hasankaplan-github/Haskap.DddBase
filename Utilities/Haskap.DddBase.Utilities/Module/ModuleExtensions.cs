using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Utilities.Module;
public static class ModuleExtensions
{
    public static IServiceCollection AddModule<TModuleRegistrar>(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        where TModuleRegistrar : class, IModuleRegistrar, new()
    {
        var moduleImplementationType = typeof(TModuleRegistrar).DeclaringType!;
        var moduleInterfaceType = moduleImplementationType.GetInterfaces().First(x => x.GetInterface(typeof(IModule).Name) != null);
            //.First(x => x.Name == $"I{moduleImplementationType.Name}");

        services.AddScoped(moduleInterfaceType, moduleImplementationType);
        services.AddScoped(typeof(IModule), moduleImplementationType);

        //var moduleDatabaseMigratorType = moduleImplementationType
        //    //.BaseType!
        //    .GetNestedTypes()
        //    .FirstOrDefault(x => x.GetInterface(typeof(IModuleDatabaseMigrator).Name) != null);

        var moduleDatabaseMigratorType = Type.GetType($"Modules.ModuleManagement.Application.Contracts.Module.BaseModule`1+DatabaseMigrator[[{moduleImplementationType.FullName}, {moduleImplementationType.Assembly.GetName().Name}]], Modules.ModuleManagement.Application.Contracts");
        if (moduleDatabaseMigratorType is not null)
        {
            services.AddScoped(typeof(IModuleDatabaseMigrator), moduleDatabaseMigratorType);
        }

        var moduleRegistrar = new TModuleRegistrar();
        return moduleRegistrar.RegisterModule(services, configuration, connectionStringName, migrationAssembly);
    }
}