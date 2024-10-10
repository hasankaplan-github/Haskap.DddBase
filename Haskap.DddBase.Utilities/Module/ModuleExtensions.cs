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
    public static IServiceCollection AddModule<T>(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly) 
        where T : class, IModule, new()
    {
        var module = new T();
        return module.RegisterModule(services, configuration, connectionStringName, migrationAssembly);
    }
}