using Haskap.DddBase.Application.Contracts.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Localization.Application.Contracts;
using Modules.Localization.Application.Dtos;
using System.Threading.Channels;

namespace Modules.Localization.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDbStringLocalizer, DbStringLocalizer>();
        services.AddTransient<ICommonStringLocalizer, CommonStringLocalizer>();
        services.AddTransient<ILocalizationService, LocalizationService>();

        services.AddSingleton<Channel<LocalizationCacheInfoDto>>(sp => Channel.CreateUnbounded<LocalizationCacheInfoDto>());
        services.AddHostedService<CacheCurrentLocalizationBackgroundService>();

        return services;
    }
}