using Haskap.DddBase.Domain.Common;
using Microsoft.AspNetCore.Http;
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

    public async Task Invoke(HttpContext httpContext)
    {
        await _currentLocaleChannel.Writer.WriteAsync(Locale.CurrentUiLocale);

        await _next(httpContext);
    }
}
