using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Modules.Localization.Application.Contracts;
using System.Globalization;

namespace Modules.Localization.Presentation.Middlewares;

public class CheckLocalizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RequestLocalizationOptions _requestLocalizationOptions;

    public CheckLocalizationMiddleware(
        RequestDelegate next,
        IOptions<RequestLocalizationOptions> requestLocalizationOptionsOptions)
    {
        _next = next;
        _requestLocalizationOptions = requestLocalizationOptionsOptions.Value;
    }

    public async Task Invoke(
        HttpContext httpContext,
        ILocalizationModule localizationModule)
    {
        if (!await localizationModule.IsEnabledAsync(httpContext.FindTenantId()))
        {
            SetCurrentThreadCulture(_requestLocalizationOptions.DefaultRequestCulture.Culture);
        }

        await _next(httpContext);
    }

    private static void SetCurrentThreadCulture(CultureInfo cultureInfo)
    {
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }
}
