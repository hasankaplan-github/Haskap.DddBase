using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Domain;
public static class DependencyInjection
{
    public static IServiceCollection AddBaseDomainServices(this IServiceCollection services)
    {
        //services.AddTransient<UserDomainService>();

        return services;
    }
}