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
    public static IServiceCollection AddModule<TModule>(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly) 
        where TModule : class, IModule, new()
    {
        if (!IsModuleEnabled<TModule>(configuration))
        {
            return services;
        }

        var module = new TModule();
        return module.RegisterModule(services, configuration, connectionStringName, migrationAssembly);
    }

    public static bool IsModuleEnabled<TModule>(IConfiguration configuration)
        where TModule : class, IModule
    {
        var exists = bool.TryParse(configuration[$"Modules:IsEnabled:{typeof(TModule).Name}"], out var isEnabled);

        if (exists && isEnabled)
        {
            return true;
        }

        return false;
    }

    public static IServiceCollection AddCoreModule<TModule>(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        where TModule : class, IModule, new()
    {
        var module = new TModule();
        return module.RegisterModule(services, configuration, connectionStringName, migrationAssembly);
    }
}