using Haskap.DddBase.Domain.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Modules.Localization.Application.Contracts;
using System.Globalization;

namespace Modules.Localization.Presentation.Middlewares;

public class CustomRequestLocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RequestLocalizationOptions _options;

    public CustomRequestLocalizationMiddleware(
        RequestDelegate next,
        IOptions<RequestLocalizationOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public async Task Invoke(
        HttpContext httpContext,
        ILocalizationService localizationService)
    {
        await AddActiveSupportedCulturesAsync(_options, localizationService);
        await SetDefaultLocaleAsync(_options, localizationService);

        await _next(httpContext);
    }

    private async Task AddActiveSupportedCulturesAsync(RequestLocalizationOptions options, ILocalizationService localizationService)
    {
        var activeSupportedCultures = (await localizationService.GetActiveSupportedLocalesAsync())
            .Select(x => new CultureInfo(x.Locale.Value))
            .ToList();

        options.SupportedCultures = activeSupportedCultures;
        options.SupportedUICultures = activeSupportedCultures;
    }

    private async Task SetDefaultLocaleAsync(RequestLocalizationOptions options, ILocalizationService localizationService)
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
