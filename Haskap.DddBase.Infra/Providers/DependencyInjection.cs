using Haskap.DddBase.Domain.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Infra.Providers;
public static class DependencyInjection
{
    public static void AddBaseProviders(this IServiceCollection services)
    {
        //services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        //services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddScoped<ICurrentUserIdProvider, CurrentUserIdProvider>();
        services.AddSingleton<IVisitIdProvider, VisitIdProvider>();
        services.AddScoped<ICurrentTenantProvider, CurrentTenantProvider>();
        services.AddScoped<IMultiTenancyGlobalQueryFilterProvider, MultiTenancyGlobalQueryFilterProvider>();
        services.AddScoped<ISoftDeleteGlobalQueryFilterProvider, SoftDeleteGlobalQueryFilterProvider>();
        services.AddScoped<IIsActiveGlobalQueryFilterProvider, IsActiveGlobalQueryFilterProvider>();
        services.AddScoped<IGlobalQueryFilterGenericProvider, GlobalQueryFilterGenericProvider>();
        services.AddSingleton<IBaseCacheKeyProvider, BaseCacheKeyProvider>();
    }
}