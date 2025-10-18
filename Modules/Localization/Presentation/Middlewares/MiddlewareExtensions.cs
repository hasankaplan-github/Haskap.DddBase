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
    public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder builder, Action<RequestLocalizationOptions> optionsAction)
    {
        var newOptions = new RequestLocalizationOptions();
        optionsAction(newOptions);

        using var scope = builder.ApplicationServices.CreateScope();

        var localizationService = scope.ServiceProvider.GetService(typeof(ILocalizationService)) as ILocalizationService;

        AddActiveSupportedCultures(newOptions, localizationService!).GetAwaiter().GetResult();
        SetDefaultLocale(newOptions, localizationService!).GetAwaiter().GetResult();
        AddDbRequestCultureProvider(newOptions).GetAwaiter().GetResult();

        builder.UseRequestLocalization(newOptions);
        builder.UseMiddleware<CheckLocalizationModuleMiddleware>();
        builder.UseMiddleware<WriteCurrentLocalizationCacheKeyMiddleware>();

        return builder;
    }

    public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();

        var localizationService = scope.ServiceProvider.GetService(typeof(ILocalizationService)) as ILocalizationService;
        var options = (scope.ServiceProvider.GetService(typeof(IOptions<RequestLocalizationOptions>)) as IOptions<RequestLocalizationOptions>)!.Value;

        AddActiveSupportedCultures(options, localizationService!).GetAwaiter().GetResult();
        SetDefaultLocale(options, localizationService!).GetAwaiter().GetResult();
        AddDbRequestCultureProvider(options).GetAwaiter().GetResult();

        builder.UseRequestLocalization();
        builder.UseMiddleware<CheckLocalizationModuleMiddleware>();
        builder.UseMiddleware<WriteCurrentLocalizationCacheKeyMiddleware>();

        return builder;
    }

    private static async Task AddActiveSupportedCultures(RequestLocalizationOptions options, ILocalizationService localizationService)
    {
        var activeSupportedCultures = (await localizationService.GetActiveSupportedLocalesAsync())
            .Select(x => new CultureInfo(x.Locale.Value))
            .ToList();

        options.SupportedCultures = activeSupportedCultures;
        options.SupportedUICultures = activeSupportedCultures;
    }

    private static async Task SetDefaultLocale(RequestLocalizationOptions options, ILocalizationService localizationService)
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

    private static async Task AddDbRequestCultureProvider(RequestLocalizationOptions options)
    {
        options.RequestCultureProviders.Add(new DbRequestCultureProvider() { Options = options });
    }
}
