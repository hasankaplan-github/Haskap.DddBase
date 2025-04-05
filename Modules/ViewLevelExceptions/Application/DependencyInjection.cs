using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ViewLevelExceptions.Application.Contracts.ViewLevelExceptions;
using Modules.ViewLevelExceptions.Application.UseCaseServices.ViewLevelExceptions;

namespace Modules.ViewLevelExceptions.Application.UseCaseServices;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddTransient<IViewLevelExceptionService, ViewLevelExceptionService>();

        return services;
    }
}
