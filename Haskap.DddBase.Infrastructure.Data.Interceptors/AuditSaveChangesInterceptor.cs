using Haskap.DddBase.Domain.Core;
using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infrastructure.Data.Interceptors;

public class AuditSaveChangesInterceptor<TUser, TUserId> : SaveChangesInterceptor
    where TUser : class, IEntity<TUserId>
{
    private readonly CurrentUserProvider<TUser, TUserId> currentUserProvider;

    public AuditSaveChangesInterceptor(CurrentUserProvider<TUser, TUserId> currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    private void SetAddedAuditProperties(DbContext dbContext)
    {
        var addedAuditableEntities = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => x.Entity is IAuditable<TUserId>  //x.Entity.GetType().GetInterfaces().Any(y=>y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IAuditable<>))  //typeof(IAuditable<>).IsAssignableFrom(x.Entity.GetType())
                                                && x.State == EntityState.Added)
                                        .Select(x => x.Entity as IAuditable<TUserId>)
                                        .ToList();

        foreach (var addedAuditableEntity in addedAuditableEntities)
        {
            dbContext.Entry(addedAuditableEntity).Property(x => x.ModifiedUserId).IsModified = false;
            dbContext.Entry(addedAuditableEntity).Property(x => x.ModifiedAt).IsModified = false;

            addedAuditableEntity.CreatedAt = DateTime.UtcNow;
            addedAuditableEntity.CreatedUserId = currentUserProvider.CurrentUser.Id;
        }
    }

    private void SetModifiedAuditProperties(DbContext dbContext)
    {
        var modifiedAuditableEntities = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => x.Entity is IAuditable<TUserId>  //x.Entity.GetType().GetInterfaces().Any(y=>y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IAuditable<>))  //typeof(IAuditable<>).IsAssignableFrom(x.Entity.GetType())
                                                && x.State == EntityState.Modified)
                                        .Select(x => x.Entity as IAuditable<TUserId>)
                                        .ToList();

        foreach (var modifiedAuditableEntity in modifiedAuditableEntities)
        {
            dbContext.Entry(modifiedAuditableEntity).Property(x => x.CreatedUserId).IsModified = false;
            dbContext.Entry(modifiedAuditableEntity).Property(x => x.CreatedAt).IsModified = false;

            modifiedAuditableEntity.ModifiedAt = DateTime.UtcNow;
            modifiedAuditableEntity.ModifiedUserId = currentUserProvider.CurrentUser.Id;
        }
    }

    private void SetAuditProperties(DbContext dbContext)
    {
        SetAddedAuditProperties(dbContext);
        SetModifiedAuditProperties(dbContext);
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
