using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Modules.AuditLog.Domain.Shared;

namespace Modules.AuditLog.Infra.Db.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserIdProvider? _currentUserIdProvider;
    private readonly IAuditLogModule _auditLogModule;

    public AuditSaveChangesInterceptor(
        ICurrentUserIdProvider? currentUserIdProvider,
        IAuditLogModule auditLogModule)
    {
        _currentUserIdProvider = currentUserIdProvider;
        _auditLogModule = auditLogModule;
    }

    private void SetAddedAuditProperties(DbContext dbContext)
    {
        var addedAuditableEntityEntries = dbContext.ChangeTracker
                                        .Entries<IAuditable>()
                                        .Where(x => x.State == EntityState.Added)
                                        //.Select(x => x.Entity)
                                        .ToList();

        foreach (var addedAuditableEntityEntry in addedAuditableEntityEntries)
        {
            addedAuditableEntityEntry.Property(x => x.ModifiedUserId).IsModified = false;
            addedAuditableEntityEntry.Property(x => x.ModifiedOnUtc).IsModified = false;

            //dbContext.Entry(addedAuditableEntity).Property(x => x.ModifiedUserId).IsModified = false;
            //dbContext.Entry(addedAuditableEntity).Property(x => x.ModifiedOn).IsModified = false;

            addedAuditableEntityEntry.Entity.CreatedOnUtc = DateTime.UtcNow;
            addedAuditableEntityEntry.Entity.CreatedUserId = _currentUserIdProvider?.CurrentUserId;
        }
    }

    private void SetModifiedAuditProperties(DbContext dbContext)
    {
        var modifiedAuditableEntityEntries = dbContext.ChangeTracker
                                        .Entries<IAuditable>()
                                        .Where(x => x.State == EntityState.Modified)
                                        //.Select(x => x.Entity)
                                        .ToList();

        foreach (var modifiedAuditableEntityEntry in modifiedAuditableEntityEntries)
        {
            modifiedAuditableEntityEntry.Property(x => x.CreatedUserId).IsModified = false;
            modifiedAuditableEntityEntry.Property(x => x.CreatedOnUtc).IsModified = false;

            //dbContext.Entry(modifiedAuditableEntity).Property(x => x.CreatedUserId).IsModified = false;
            //dbContext.Entry(modifiedAuditableEntity).Property(x => x.CreatedOn).IsModified = false;

            modifiedAuditableEntityEntry.Entity.ModifiedOnUtc = DateTime.UtcNow;
            modifiedAuditableEntityEntry.Entity.ModifiedUserId = _currentUserIdProvider?.CurrentUserId;
        }
    }

    private void SetAuditProperties(DbContext dbContext)
    {
        if(!_auditLogModule.IsEnabledAsync().GetAwaiter().GetResult())
        {
            return;
        }

        SetAddedAuditProperties(dbContext);
        SetModifiedAuditProperties(dbContext);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetAuditProperties(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetAuditProperties(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
