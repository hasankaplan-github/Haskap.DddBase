using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Infra.Providers;

public class DbContextProvider : IDbContextProvider
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly IGlobalQueryFilterGenericProvider _globalQueryFilterGenericProvider;
    private readonly Stack<IServiceScope> _scopes = new();

    ~DbContextProvider()
    {
        while (_scopes.Count > 0)
        {
            var scope = _scopes.Pop();
            scope.Dispose();
        }
    }

    public DbContextProvider(
        ICurrentTenantProvider currentTenantProvider,
        IGlobalQueryFilterGenericProvider globalQueryFilterGenericProvider,
        IServiceScopeFactory serviceScopeFactory)
    {
        _currentTenantProvider = currentTenantProvider;
        _globalQueryFilterGenericProvider = globalQueryFilterGenericProvider;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public TDbContext GetDbContext<TDbContext>() where TDbContext : IUnitOfWork
    {
        
        var scope = _serviceScopeFactory.CreateScope();
        _scopes.Push(scope);

        var currentTenantProvider = scope.ServiceProvider.GetRequiredService<ICurrentTenantProvider>();
        currentTenantProvider.ChangeCurrentTenant(_currentTenantProvider.CurrentTenantId);
        var globalQueryFilterGenericProvider = scope.ServiceProvider.GetRequiredService<IGlobalQueryFilterGenericProvider>();
        foreach (var filterProvider in _globalQueryFilterGenericProvider.GetAllProviders())
        {
            globalQueryFilterGenericProvider.AddFilterProvider(filterProvider.Key, filterProvider.Value);
        }

        return scope.ServiceProvider.GetRequiredService<TDbContext>();
    }
}
