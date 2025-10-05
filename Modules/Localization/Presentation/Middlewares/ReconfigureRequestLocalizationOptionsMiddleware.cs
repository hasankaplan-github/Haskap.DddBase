using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Modules.Localization.Application.Contracts;

namespace Modules.Localization.Presentation.Middlewares;

public class ReconfigureRequestLocalizationOptionsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RequestLocalizationOptions _requestLocalizationOptions;

    public ReconfigureRequestLocalizationOptionsMiddleware(
        RequestDelegate next,
        IOptions<RequestLocalizationOptions> requestLocalizationOptionsOptions)
    {
        _next = next;
        _requestLocalizationOptions = requestLocalizationOptionsOptions.Value;
    }

    public async Task Invoke(
        HttpContext httpContext,
        ILocalizationService localizationService,
        ILocalizationModule localizationModule)
    {
        if (await localizationModule.IsEnabledAsync(httpContext.FindTenantId()))
        {
            await AddSupportedCultures(localizationService);

            await SetDefaultLocale(localizationService);

            await AddDbRequestCultureProvider();
        }

        await _next(httpContext);
    }

    private async Task AddSupportedCultures(ILocalizationService localizationService)
    {
        string[] activeSupportedLocaleValues = [
                ..(_requestLocalizationOptions.SupportedCultures?.Select(x => x.Name) ?? Enumerable.Empty<string>()),
                ..(await localizationService.GetActiveSupportedLocalesAsync()).Select(x => x.LocaleValue)
            ];

        _requestLocalizationOptions
            .AddSupportedCultures(activeSupportedLocaleValues)
            .AddSupportedUICultures(activeSupportedLocaleValues);
    }

    private async Task SetDefaultLocale(ILocalizationService localizationService)
    {
        var defaultLocale = await localizationService.GetDefaultLocaleAsync();

        if (defaultLocale is not null)
        {
            Locale.Default = new Locale(defaultLocale.Value);
            _requestLocalizationOptions.DefaultRequestCulture = new RequestCulture(defaultLocale.Value);
        }
        else
        {
            Locale.Default = new Locale(_requestLocalizationOptions.DefaultRequestCulture.Culture.Name);
        }
    }

    private async Task AddDbRequestCultureProvider()
    {
        _requestLocalizationOptions.AddInitialRequestCultureProvider(new DbRequestCultureProvider());
    }
}
