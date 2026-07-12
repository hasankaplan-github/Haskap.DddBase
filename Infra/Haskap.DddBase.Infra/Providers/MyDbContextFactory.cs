using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;

namespace Haskap.DddBase.Infra.Providers;

public class MyDbContextFactory<TDbContextInterface, TDbContexImplemantation> : IMyDbContextFactory<TDbContextInterface>
    where TDbContextInterface : IUnitOfWork
    where TDbContexImplemantation : DbContext, TDbContextInterface
{
    private static readonly SemaphoreSlim s_changeTenantSemaphoreSlim = new SemaphoreSlim(1);

    private readonly IDbContextFactory<TDbContexImplemantation> _innerFactory;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public MyDbContextFactory(
        IDbContextFactory<TDbContexImplemantation> factory,
        ICurrentTenantProvider currentTenantProvider)
    {
        _innerFactory = factory;
        _currentTenantProvider = currentTenantProvider;
    }

    public TDbContextInterface CreateDbContext()
        => _innerFactory.CreateDbContext();

    public TDbContextInterface CreateDbContext(Guid? tenantId)
    {
        s_changeTenantSemaphoreSlim.Wait();
        try
        {
            using var _ = _currentTenantProvider.ChangeCurrentTenant(tenantId);

            return _innerFactory.CreateDbContext();
        }
        finally
        {
            s_changeTenantSemaphoreSlim.Release();
        }
    }

    public async Task<TDbContextInterface> CreateDbContextAsync(CancellationToken cancellationToken)
        => await _innerFactory.CreateDbContextAsync(cancellationToken);

    public async Task<TDbContextInterface> CreateDbContextAsync(Guid? tenantId, CancellationToken cancellationToken)
    {
        await s_changeTenantSemaphoreSlim.WaitAsync(cancellationToken);
        try
        {
            using var _ = _currentTenantProvider.ChangeCurrentTenant(tenantId);

            return await _innerFactory.CreateDbContextAsync(cancellationToken);
        }
        finally
        {
            s_changeTenantSemaphoreSlim.Release();
        }
    }
}
