using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Events;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Events;
using Haskap.DddBase.Infra.Interceptors;
using Haskap.DddBase.Infra.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        services.AddScoped<IGlobalQueryFilterManagerProvider, GlobalQueryFilterManagerProvider>();
        services.AddSingleton<IBaseCacheKeyProvider, BaseCacheKeyProvider>();

        services.AddScoped<MultiTenancySaveChangesInterceptor>();

        services.AddSingleton<IEventPublisher, EventPublisher>();

        services.AddSingleton<IHashProvider, HashProvider>();
    }

    extension(IServiceCollection services)
    {
        public IServiceCollection AddMyDbContextFactory<TDbContextInterface, TDbContextImplementation>(
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContextImplementation : DbContext, TDbContextInterface
            where TDbContextInterface : class, IUnitOfWork
        {
            services.AddDbContextFactory<TDbContextImplementation>(optionsAction, lifetime);

            services.TryAdd(new ServiceDescriptor(
                typeof(IMyDbContextFactory<TDbContextInterface>),
                typeof(MyDbContextFactory<TDbContextInterface, TDbContextImplementation>),
                lifetime));

            services.TryAdd(new ServiceDescriptor(
                typeof(TDbContextInterface),
                typeof(TDbContextImplementation),
                lifetime));

            //services.AddScoped<IMyDbContextFactory<TDbContextInterface>, MyDbContextFactory<TDbContextInterface, TDbContextImplementation>>();
            //services.AddScoped<TDbContextInterface, TDbContextImplementation>();

            return services;
        }
    }
}