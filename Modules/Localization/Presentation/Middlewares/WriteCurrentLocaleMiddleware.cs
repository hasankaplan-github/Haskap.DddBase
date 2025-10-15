using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Http;
using Modules.Localization.Application.Contracts;
using System.Threading.Channels;

namespace Modules.Localization.Presentation.Middlewares;

public class WriteCurrentLocaleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Channel<Locale> _currentLocaleChannel;

    public WriteCurrentLocaleMiddleware(
        RequestDelegate next,
        Channel<Locale> currentLocaleChannel)
    {
        _next = next;
        _currentLocaleChannel = currentLocaleChannel;
    }

    public async Task Invoke(
        HttpContext httpContext,
        ILocalizationModule localizationModule)
    {
        if (!await localizationModule.IsEnabledAsync(httpContext.FindTenantId()))
        {
            if (Locale.Default is not null)
            {
                await _currentLocaleChannel.Writer.WriteAsync(Locale.Default);
            }
        }
        else
        {
            await _currentLocaleChannel.Writer.WriteAsync(Locale.CurrentUiLocale);
        }

        await _next(httpContext);
    }
}
