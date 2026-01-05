using Microsoft.AspNetCore.Builder;

namespace Modules.Localization.Presentation.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<CustomRequestLocalizationMiddleware>();
        builder.UseRequestLocalization();
        builder.UseMiddleware<CheckLocalizationModuleMiddleware>();
        builder.UseMiddleware<WriteCurrentLocalizationCacheKeyMiddleware>();

        return builder;
    }
}
