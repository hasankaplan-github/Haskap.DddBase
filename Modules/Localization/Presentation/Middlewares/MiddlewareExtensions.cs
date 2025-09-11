using Microsoft.AspNetCore.Builder;

namespace Modules.Localization.Presentation.Middlewares;
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder builder, Action<RequestLocalizationOptions> optionsAction)
    {
        builder.UseRequestLocalization(optionsAction);
        builder.UseMiddleware<CheckLocalizationMiddleware>();

        return builder;
    }

    public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder builder)
    {
        builder.UseRequestLocalization();
        builder.UseMiddleware<CheckLocalizationMiddleware>();

        return builder;
    }
}
