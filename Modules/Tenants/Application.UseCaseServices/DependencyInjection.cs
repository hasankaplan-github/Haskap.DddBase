using Haskap.DddBase.Application.Contracts.Tenants;
using Haskap.DddBase.Modules.Tenants.Application.UseCaseServices.Mappings;
using Haskap.DddBase.Modules.Tenants.Application.UseCaseServices.Tenants;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.Tenants.Application.UseCaseServices;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddTransient<ITenantService, TenantService>();

        return services;
    }
}
