using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Infra.Providers;

public class DbContextProvider : IDbContextProvider, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly IGlobalQueryFilterManagerProvider _globalQueryFilterManagerProvider;
    private readonly Stack<IServiceScope> _scopes = new();
    private bool _disposedValue;

    public DbContextProvider(
        ICurrentTenantProvider currentTenantProvider,
        IGlobalQueryFilterManagerProvider globalQueryFilterManagerProvider,
        IServiceScopeFactory serviceScopeFactory)
    {
        _currentTenantProvider = currentTenantProvider;
        _globalQueryFilterManagerProvider = globalQueryFilterManagerProvider;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public TDbContext GetDbContext<TDbContext>() where TDbContext : IUnitOfWork
    {
        
        var scope = _serviceScopeFactory.CreateScope();
        _scopes.Push(scope);

        var currentTenantProvider = scope.ServiceProvider.GetRequiredService<ICurrentTenantProvider>();
        currentTenantProvider.ChangeCurrentTenant(_currentTenantProvider.CurrentTenantId);
        var globalQueryFilterGenericProvider = scope.ServiceProvider.GetRequiredService<IGlobalQueryFilterManagerProvider>();
        foreach (var filterProvider in _globalQueryFilterManagerProvider.GetAllProviders())
        {
            globalQueryFilterGenericProvider.AddFilterProvider(filterProvider.Key, filterProvider.Value);
        }

        return scope.ServiceProvider.GetRequiredService<TDbContext>();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                while (_scopes.Count > 0)
                {
                    var scope = _scopes.Pop();
                    scope.Dispose();
                }
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~DbContextProvider()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
