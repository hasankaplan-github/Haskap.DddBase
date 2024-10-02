using Haskap.DddBase.Modules.Tenants.Application.UseCaseServices;
using Haskap.DddBase.Modules.Tenants.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.Tenants.Module;
public static class DependencyInjection
{
    public static IServiceCollection AddTenantsModule(this IServiceCollection services, IConfiguration configuration, string connectionStringName)
    {
        services.AddUseCaseServices();
        services.AddInfra(configuration, connectionStringName);

        return services;
    }
}
