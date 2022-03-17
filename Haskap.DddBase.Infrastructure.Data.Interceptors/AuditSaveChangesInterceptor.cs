using Haskap.DddBase.Domain.Core;
using Haskap.DddBase.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesmer.PaymentSystem.Infrastructure.Data.Interceptors;

public class AuditSaveChangesInterceptor<TUserId> : SaveChangesInterceptor
{
    private readonly LoggedInUserProvider<TUserId> loggedInUserProvider;

    public AuditSaveChangesInterceptor(LoggedInUserProvider<TUserId> loggedInUserProvider)
    {
        this.loggedInUserProvider = loggedInUserProvider;
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
            addedAuditableEntity.CreatedUserId = loggedInUserProvider.UserId;
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
            modifiedAuditableEntity.ModifiedUserId = loggedInUserProvider.UserId;
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
