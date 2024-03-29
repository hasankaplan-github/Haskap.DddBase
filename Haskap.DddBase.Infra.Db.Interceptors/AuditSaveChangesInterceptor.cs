﻿using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Db.Interceptors;


/*
    Buradaki TUserId Guid? şeklinde nullable  olarak verilecek.
    Entity' de de IAuditable<Guid?> olarak verilecek ve user id ler nullable olmuş olacak.
 */
public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserIdProvider? _currentUserIdProvider;

    public AuditSaveChangesInterceptor(ICurrentUserIdProvider? currentUserIdProvider)
    {
        _currentUserIdProvider = currentUserIdProvider;
    }

    private void SetAddedAuditProperties(DbContext dbContext)
    {
        var addedAuditableEntities = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => x.Entity is IAuditable  //x.Entity.GetType().GetInterfaces().Any(y=>y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IAuditable<>))  //typeof(IAuditable<>).IsAssignableFrom(x.Entity.GetType())
                                                && x.State == EntityState.Added)
                                        .Select(x => x.Entity as IAuditable)
                                        .ToList();

        foreach (var addedAuditableEntity in addedAuditableEntities)
        {
            dbContext.Entry(addedAuditableEntity).Property(x => x.ModifiedUserId).IsModified = false;
            dbContext.Entry(addedAuditableEntity).Property(x => x.ModifiedOn).IsModified = false;

            addedAuditableEntity.CreatedOn = DateTime.UtcNow;
            addedAuditableEntity.CreatedUserId = _currentUserIdProvider?.CurrentUserId;
        }
    }

    private void SetModifiedAuditProperties(DbContext dbContext)
    {
        var modifiedAuditableEntities = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => x.Entity is IAuditable  //x.Entity.GetType().GetInterfaces().Any(y=>y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IAuditable<>))  //typeof(IAuditable<>).IsAssignableFrom(x.Entity.GetType())
                                                && x.State == EntityState.Modified)
                                        .Select(x => x.Entity as IAuditable)
                                        .ToList();

        foreach (var modifiedAuditableEntity in modifiedAuditableEntities)
        {
            dbContext.Entry(modifiedAuditableEntity).Property(x => x.CreatedUserId).IsModified = false;
            dbContext.Entry(modifiedAuditableEntity).Property(x => x.CreatedOn).IsModified = false;

            modifiedAuditableEntity.ModifiedOn = DateTime.UtcNow;
            modifiedAuditableEntity.ModifiedUserId = _currentUserIdProvider?.CurrentUserId;
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

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetAuditProperties(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
