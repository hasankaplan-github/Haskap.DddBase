using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Infra.Interceptors;
public static class DependencyInjection
{
    public static void AddMultiTenancyIterceptor(this IServiceCollection services)
    {
        services.AddScoped<MultiTenancySaveChangesInterceptor>();
    }
}
