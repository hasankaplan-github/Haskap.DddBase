using Haskap.DddBase.Domain.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modules.Localization.Application.Contracts;
using System.Globalization;

namespace Modules.Localization.Presentation.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder builder)
    {
        var scope = builder.ApplicationServices.CreateScope();
        var options = scope.ServiceProvider.GetService<IOptions<RequestLocalizationOptions>>();
        var localizationService = scope.ServiceProvider.GetRequiredService<ILocalizationService>();

        RefreshRequestLocalizationOptionsAsync(options!.Value, localizationService).GetAwaiter().GetResult();

        builder.UseRequestLocalization();
        builder.UseMiddleware<CheckLocalizationModuleMiddleware>();
        builder.UseMiddleware<WriteCurrentLocalizationCacheKeyMiddleware>();

        return builder;
    }

    public static async Task RefreshRequestLocalizationOptionsAsync(RequestLocalizationOptions options, ILocalizationService localizationService)
    {
        await AddActiveSupportedCulturesAsync(options, localizationService);
        await SetDefaultLocaleAsync(options, localizationService);

        static async Task AddActiveSupportedCulturesAsync(RequestLocalizationOptions options, ILocalizationService localizationService)
        {
            var activeSupportedCultures = (await localizationService.GetActiveSupportedLocalesAsync())
                .Select(x => new CultureInfo(x.Locale.Value))
                .ToList();

            options.SupportedCultures = activeSupportedCultures;
            options.SupportedUICultures = activeSupportedCultures;
        }

        static async Task SetDefaultLocaleAsync(RequestLocalizationOptions options, ILocalizationService localizationService)
        {
            var defaultLocale = await localizationService.GetDefaultLocaleAsync();

            if (defaultLocale is not null)
            {
                Locale.Default = new Locale(defaultLocale.Value);
                options.DefaultRequestCulture = new RequestCulture(defaultLocale.Value);
            }
            else
            {
                Locale.Default = new Locale(options.DefaultRequestCulture.Culture.Name);
            }
        }
    }
}
