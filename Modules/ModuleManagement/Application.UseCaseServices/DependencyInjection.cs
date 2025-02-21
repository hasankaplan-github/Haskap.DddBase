using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application.Contracts.Module;
using Modules.ModuleManagement.Application.UseCaseServices.Module;

namespace Modules.ModuleManagement.Application.UseCaseServices;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<Haskap.DddBase.Domain.Shared.Consts.Modules>().BindConfiguration(Haskap.DddBase.Domain.Shared.Consts.Modules.SectionName);

        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddTransient<IModuleService, ModuleService>();

        return services;
    }
}
