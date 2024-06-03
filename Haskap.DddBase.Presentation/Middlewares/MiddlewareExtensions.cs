using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Presentation.Middlewares;
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseIsActive(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IsActiveMiddleware>();
    }

    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MultiTenancyMiddleware>();
    }

    public static IApplicationBuilder UseSoftDelete(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SoftDeleteMiddleware>();
    }

    public static IApplicationBuilder UseLocalDateTimeProvider(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LocalDateTimeProviderMiddleware>();
    }

    public static IApplicationBuilder UseCurrentUserIdProvider(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CurrentUserIdProviderMiddleware>();
    }
}
