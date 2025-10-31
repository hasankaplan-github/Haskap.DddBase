using Haskap.DddBase.Domain.Events;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Events;
using Haskap.DddBase.Infra.Interceptors;
using Haskap.DddBase.Infra.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Infra;
public static class DependencyInjection
{
    public static void AddBaseInfra(this IServiceCollection services)
    {
        //services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        //services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddScoped<ICurrentUserIdProvider, CurrentUserIdProvider>();
        services.AddScoped<IVisitIdProvider, VisitIdProvider>();
        services.AddScoped<ICurrentTenantProvider, CurrentTenantProvider>();
        services.AddScoped<IMultiTenancyGlobalQueryFilterProvider, MultiTenancyGlobalQueryFilterProvider>();
        services.AddScoped<ISoftDeleteGlobalQueryFilterProvider, SoftDeleteGlobalQueryFilterProvider>();
        services.AddScoped<IIsActiveGlobalQueryFilterProvider, IsActiveGlobalQueryFilterProvider>();
        services.AddScoped<IGlobalQueryFilterGenericProvider, GlobalQueryFilterGenericProvider>();
        services.AddSingleton<IBaseCacheKeyProvider, BaseCacheKeyProvider>();
        services.AddScoped<IDbContextProvider, DbContextProvider>();

        services.AddScoped<MultiTenancySaveChangesInterceptor>();

        services.AddSingleton<IEventPublisher, EventPublisher>();
    }
}