using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.Shared.Consts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Presentation.Middlewares;
public class LocalDateTimeProviderMiddleware
{
    private readonly RequestDelegate _next;

    public LocalDateTimeProviderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        ILocalDateTimeProvider localDateTimeProvider)
    {
        var userSystemTimeZoneId = httpContext.User.FindFirst(LocalDateTimeProviderConsts.UserSystemTimeZoneIdClaimKey)?.Value ?? string.Empty;
        localDateTimeProvider.SystemTimeZoneId = userSystemTimeZoneId;

        await _next(httpContext);
    }
}
