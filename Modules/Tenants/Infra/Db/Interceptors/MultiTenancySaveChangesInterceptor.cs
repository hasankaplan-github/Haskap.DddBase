using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Modules.Tenants.Infra.Db.Interceptors;

// burada da TUserId nullable olan Guid? olarak verilecek.
public class MultiTenancySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentTenantProvider? _currentTenantProvider;

    public MultiTenancySaveChangesInterceptor(
        ICurrentTenantProvider? currentTenantProvider)
    {
        _currentTenantProvider = currentTenantProvider;
    }


    private void SetTenantId(DbContext dbContext)
    {
        var entityEntries = dbContext.ChangeTracker
                                        .Entries<IHasMultiTenant>()
                                        .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added)
                                        .ToList();

        foreach (var entityEntry in entityEntries)
        {
            var multiTenantEntity = entityEntry.Entity;
            multiTenantEntity.TenantId = _currentTenantProvider?.CurrentTenantId;
        }
    }


    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetTenantId(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetTenantId(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

}
