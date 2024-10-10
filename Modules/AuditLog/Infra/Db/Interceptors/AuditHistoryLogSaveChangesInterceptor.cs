using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Attributes.AuditHistoryLogAttributes;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.Shared.Enums;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Modules.AuditLog.Domain.AuditHistoryLogAggregate;
using System.Text.Json;

namespace Modules.AuditLog.Infra.Db.Interceptors;

// burada da TUserId nullable olan Guid? olarak verilecek.
public class AuditHistoryLogSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserIdProvider? _currentUserIdProvider;
    private readonly IVisitIdProvider? _visitIdProvider;
    private readonly ICurrentTenantProvider? _currentTenantProvider;

    public AuditHistoryLogSaveChangesInterceptor(
        ICurrentUserIdProvider? currentUserIdProvider,
        IVisitIdProvider? visitIdProvider,
        ICurrentTenantProvider? currentTenantProvider)
    {
        _currentUserIdProvider = currentUserIdProvider;
        _visitIdProvider = visitIdProvider;
        _currentTenantProvider = currentTenantProvider;
    }


    private AuditHistoryLog? GetAuditHistoryLogForEntry(EntityEntry entityEntry)
    {
        var modificationType = DetectModificationType(entityEntry);

        if (modificationType == AuditHistoryLogModificationType.None)
        {
            return null;
        }

        var keyValues = new Dictionary<string, object>();
        var originalValues = new Dictionary<string, object?>();
        var newValues = new Dictionary<string, object?>();

        var propertyEntries = entityEntry.Properties;

        // class seviyesinde tanımlanmışsa
        var classHasAuditHistoryLogAttribute = Attribute.IsDefined(entityEntry.Entity.GetType(), typeof(AddAuditHistoryLogAttribute));
        if (classHasAuditHistoryLogAttribute)
        {
            foreach (var propertyEntry in propertyEntries)
            {
                var propertyName = propertyEntry.Metadata.Name;

                if (propertyEntry.Metadata.IsPrimaryKey())
                {
                    keyValues[propertyName] = propertyEntry.CurrentValue!;
                    continue;
                }

                var propertyInfo = propertyEntry.Metadata.PropertyInfo;
                if (propertyInfo is null)
                {
                    continue;
                }

                var propertyHasRemoveAuditHistoryLogAttribute = Attribute.IsDefined(propertyInfo, typeof(RemoveAuditHistoryLogAttribute));
                if (propertyHasRemoveAuditHistoryLogAttribute)
                {
                    continue;
                }

                originalValues[propertyName] = propertyEntry.OriginalValue;
                newValues[propertyName] = propertyEntry.CurrentValue;
            }
        }
        // property seviyesinde tanımlanmışsa
        else
        {
            // only properties
            foreach (var propertyEntry in propertyEntries)
            {
                var propertyName = propertyEntry.Metadata.Name;

                if (propertyEntry.Metadata.IsPrimaryKey())
                {
                    keyValues[propertyName] = propertyEntry.CurrentValue!;
                    continue;
                }

                var propertyInfo = propertyEntry.Metadata.PropertyInfo;
                if (propertyInfo is null)
                {
                    continue;
                }

                var propertyHasAddAuditHistoryLogAttribute = Attribute.IsDefined(propertyInfo, typeof(AddAuditHistoryLogAttribute));
                if (propertyHasAddAuditHistoryLogAttribute)
                {
                    originalValues[propertyName] = propertyEntry.OriginalValue;
                    newValues[propertyName] = propertyEntry.CurrentValue;
                }
            }
        }

        var auditHistoryLog = new AuditHistoryLog(GuidGenerator.CreateSequentialGuid(SequentialGuidType.SequentialAsString));
        auditHistoryLog.ModificationType = modificationType;
        auditHistoryLog.ModificationDateUtc = DateTime.UtcNow;
        auditHistoryLog.ModifiedUserId = _currentUserIdProvider?.CurrentUserId;
        auditHistoryLog.VisitId = _visitIdProvider?.VisitId;
        auditHistoryLog.TenantId = _currentTenantProvider?.CurrentTenantId;
        auditHistoryLog.ObjectFullType = entityEntry.Entity.GetType().ToString();
        auditHistoryLog.ObjectIds = keyValues.Count == 0 ? null : JsonSerializer.Serialize(keyValues);
        auditHistoryLog.ObjectOriginalValues = modificationType == AuditHistoryLogModificationType.Add || originalValues.Count == 0 ? null : JsonSerializer.Serialize(originalValues);
        auditHistoryLog.ObjectNewValues = modificationType == AuditHistoryLogModificationType.Delete || newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues);
        auditHistoryLog.OwnershipType = entityEntry.Metadata.IsOwned() ? AuditHistoryLogOwnershipType.OwnedType : AuditHistoryLogOwnershipType.EntityType;

        return auditHistoryLog;
    }

    private AuditHistoryLogModificationType DetectModificationType(EntityEntry entry)
    {
        AuditHistoryLogModificationType modificationType = AuditHistoryLogModificationType.None;

        if (Attribute.IsDefined(entry.Entity.GetType(), typeof(AddAuditHistoryLogAttribute))
            || entry.Entity.GetType().GetProperties().Any(y => y.GetCustomAttributes(true).Any(z => z is AddAuditHistoryLogAttribute)))
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    modificationType = AuditHistoryLogModificationType.Delete;
                    break;
                case EntityState.Added:
                    modificationType = AuditHistoryLogModificationType.Add;
                    break;
                case EntityState.Modified:
                    if (entry.Entity is ISoftDeletable)
                    {
                        PropertyEntry isDeletedPropertyEntry = entry.Property("IsDeleted");
                        if (isDeletedPropertyEntry.IsModified)
                        {
                            //if (bool.Parse(isDeletedPropertyEntry.CurrentValue.ToString()) == true)
                            if ((bool)isDeletedPropertyEntry.CurrentValue! == true)
                            {
                                modificationType = AuditHistoryLogModificationType.SoftDelete;
                            }
                            else
                            {
                                modificationType = AuditHistoryLogModificationType.Undelete;
                            }
                        }
                        else
                        {
                            modificationType = AuditHistoryLogModificationType.Update;
                        }
                    }
                    else
                    {
                        modificationType = AuditHistoryLogModificationType.Update;
                    }
                    break;
                default:
                    break;
            }
        }                               

        return modificationType;
    }

    private void SetAuditHistoryLogForObjects(DbContext dbContext)
    {
        var entityEntries = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => !(x.Entity is AuditHistoryLog)
                                            && x.State != EntityState.Detached && x.State != EntityState.Unchanged
                                        )
                                        .ToList();

        foreach (var entityEntry in entityEntries)
        {
            var auditHistoryLog = GetAuditHistoryLogForEntry(entityEntry);
            if (auditHistoryLog is not null)
            {
                dbContext.Add(auditHistoryLog);
            }
        }
        //InnerSetAuditHistoryLogForObjects(entityEntries, dbContext, null);
    }


    //private void InnerSetAuditHistoryLogForObjects(List<EntityEntry>? entityEntries, DbContext dbContext, AuditHistoryLog? ownerLog) 
    //{
    //    foreach (var entityEntry in entityEntries)
    //    {
    //        var auditHistoryLog = GetAuditHistoryLogForEntry(entityEntry, ownerLog?.ObjectIds);

    //        if (auditHistoryLog.ModificationType != AuditHistoryLogModificationType.None)
    //        {
    //            dbContext.Add(auditHistoryLog);
    //        }

    //        var referenceEntityEntries = entityEntry.References
    //            .Where(r => r.TargetEntry != null)
    //            .Select(x => x.TargetEntry)
    //            .ToList();

    //        InnerSetAuditHistoryLogForObjects(referenceEntityEntries, dbContext, auditHistoryLog);

    //        var collectionEntries = entityEntry.Collections;
    //        foreach (var collectionEntry in collectionEntries)
    //        {
    //            if (collectionEntry.CurrentValue is null)
    //            {
    //                continue;
    //            }

    //            var collectionEntityEntries = collectionEntry.CurrentValue
    //                .Cast<object>()
    //                .Select(x => collectionEntry.FindEntry(x))
    //                .ToList();

    //            InnerSetAuditHistoryLogForObjects(collectionEntityEntries, dbContext, auditHistoryLog);
    //        }
    //    }
    //}


    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetAuditHistoryLogForObjects(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetAuditHistoryLogForObjects(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

}
