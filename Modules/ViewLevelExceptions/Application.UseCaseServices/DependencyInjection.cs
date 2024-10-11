using Modules.ViewLevelExceptions.Application.Contracts.ViewLevelExceptions;
using Modules.ViewLevelExceptions.Application.UseCaseServices.ViewLevelExceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.ViewLevelExceptions.Application.UseCaseServices;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddTransient<IViewLevelExceptionService, ViewLevelExceptionService>();

        return services;
    }
}
