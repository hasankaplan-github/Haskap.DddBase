using Haskap.DddBase.Modules.ViewLevelExceptions.Application.UseCaseServices;
using Haskap.DddBase.Modules.ViewLevelExceptions.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.ViewLevelExceptions.Module;
public static class DependencyInjection
{
    public static IServiceCollection AddViewLevelExceptionsModule(this IServiceCollection services, IConfiguration configuration, string connectionStringName)
    {
        services.AddUseCaseServices();
        services.AddInfra(configuration, connectionStringName);

        services.AddTransient<IModuleApi, ModuleApi>();

        return services;
    }
}
