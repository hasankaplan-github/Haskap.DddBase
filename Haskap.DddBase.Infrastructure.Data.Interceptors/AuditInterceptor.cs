using Haskap.DddBase.Domain.Core;
using Haskap.DddBase.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesmer.PaymentSystem.Infrastructure.Data.NpgsqlDbContext.Interceptors;

public class AuditInterceptor<TUserId> : SaveChangesInterceptor
{
    private readonly LoggedInUserProvider<TUserId> loggedInUserProvider;

    public AuditInterceptor(LoggedInUserProvider<TUserId> loggedInUserProvider)
    {
        this.loggedInUserProvider = loggedInUserProvider;
    }

    private void SetAuditProperties(DbContext dbContext)
    {
        var addedOrModifiedEntities = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => x.Entity is IAuditable<TUserId>
                                                && (x.State == EntityState.Added || x.State == EntityState.Modified))
                                        .ToList();

        foreach (var entityEntry in addedOrModifiedEntities)
        {
            var auditableEntity = entityEntry.Entity as IAuditable<TUserId>;

            if (entityEntry.State == EntityState.Added)
            {
                auditableEntity.CreatedAt = DateTime.UtcNow;
                auditableEntity.CreatedUserId = loggedInUserProvider.UserId;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                dbContext.Entry(auditableEntity).Property(x => x.CreatedUserId).IsModified = false;
                dbContext.Entry(auditableEntity).Property(x => x.CreatedAt).IsModified = false;

                auditableEntity.ModifiedAt = DateTime.UtcNow;
                auditableEntity.ModifiedUserId = loggedInUserProvider.UserId;
            }
        }
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetAuditProperties(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetAuditProperties(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
