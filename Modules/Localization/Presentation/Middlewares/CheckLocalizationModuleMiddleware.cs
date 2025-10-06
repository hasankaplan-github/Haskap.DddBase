using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Http;
using Modules.Localization.Application.Contracts;
using System.Globalization;

namespace Modules.Localization.Presentation.Middlewares;

public class CheckLocalizationModuleMiddleware
{
    private readonly RequestDelegate _next;

    public CheckLocalizationModuleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        ILocalizationModule localizationModule)
    {
        if (!await localizationModule.IsEnabledAsync(httpContext.FindTenantId()))
        {
            SetCurrentThreadCulture(Locale.Default?.CultureInfo ?? new CultureInfo("en-US"));
        }

        await _next(httpContext);
    }

    private static void SetCurrentThreadCulture(CultureInfo cultureInfo)
    {
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }
}
