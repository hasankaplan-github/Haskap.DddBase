using Haskap.DddBase.Domain.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Localization.Application.Contracts;
using System.Threading.Channels;

namespace Modules.Localization.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDbStringLocalizer, DbStringLocalizer>();
        services.AddTransient<ICommonStringLocalizer, CommonStringLocalizer>();
        services.AddTransient<ILocalizationService, LocalizationService>();

        services.AddSingleton<Channel<Locale>>(sp => Channel.CreateUnbounded<Locale>());
        services.AddHostedService<CacheCurrentLocalizationBackgroundService>();

        return services;
    }
}