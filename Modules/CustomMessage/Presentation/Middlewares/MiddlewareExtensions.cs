using Microsoft.AspNetCore.Builder;

namespace Modules.CustomMessage.Presentation.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMessage(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<CustomMessageMiddleware>();

        return builder;
    }
}
