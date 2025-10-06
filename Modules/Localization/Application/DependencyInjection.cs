using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Localization.Application.Contracts;

namespace Modules.Localization.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDbStringLocalizer, DbStringLocalizer>();
        services.AddTransient<ILocalizationService, LocalizationService>();

        return services;
    }
}