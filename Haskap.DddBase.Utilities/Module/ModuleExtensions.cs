using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Utilities.Module;
public static class ModuleExtensions
{
    public static IServiceCollection AddModule<TModuleRegistrar>(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly) 
        where TModuleRegistrar : class, IModuleRegistrar, new()
    {
        if (!IsModuleEnabled<TModuleRegistrar>(configuration))
        {
            return services;
        }

        var moduleRegistrar = new TModuleRegistrar();
        return moduleRegistrar.RegisterModule(services, configuration, connectionStringName, migrationAssembly);
    }

    public static bool IsModuleEnabled<TModuleRegistrar>(IConfiguration configuration)
        where TModuleRegistrar : class, IModuleRegistrar
    {
        var exists = bool.TryParse(configuration[$"Modules:IsEnabled:{typeof(TModuleRegistrar).DeclaringType!.Name}"], out var isEnabled);

        if (exists && isEnabled)
        {
            return true;
        }

        return false;
    }

    public static IServiceCollection AddCoreModule<TModuleRegistrar>(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        where TModuleRegistrar : class, IModuleRegistrar, new()
    {
        var moduleRegistrar = new TModuleRegistrar();
        return moduleRegistrar.RegisterModule(services, configuration, connectionStringName, migrationAssembly);
    }
}