using Haskap.DddBase.Utilities.Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ViewLevelExceptions.Application.Contracts.ViewLevelExceptions;
using Modules.ViewLevelExceptions.Application.UseCaseServices.ViewLevelExceptions;

namespace Modules.ViewLevelExceptions.Application.UseCaseServices;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatorConsumersFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddTransient<IViewLevelExceptionService, ViewLevelExceptionService>();

        return services;
    }
}
