using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Localization.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.RequestCultureProviders.Add(new DbRequestCultureProvider() { Options = options });
        });

        return services;
    }
}
