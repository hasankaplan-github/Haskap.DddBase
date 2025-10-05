using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Modules.Localization.Presentation.Middlewares;
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder builder, Action<RequestLocalizationOptions> optionsAction)
    {
        var newOptions = new RequestLocalizationOptions();
        optionsAction(newOptions);

        builder.UseMiddleware<ReconfigureRequestLocalizationOptionsMiddleware>(Options.Create(newOptions));
        builder.UseRequestLocalization(newOptions);
        builder.UseMiddleware<CheckLocalizationModuleMiddleware>();

        return builder;
    }

    public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder builder)
    {
        var newOptions = new RequestLocalizationOptions();

        builder.UseMiddleware<ReconfigureRequestLocalizationOptionsMiddleware>(Options.Create(newOptions));
        builder.UseRequestLocalization();
        builder.UseMiddleware<CheckLocalizationModuleMiddleware>();

        return builder;
    }
}
