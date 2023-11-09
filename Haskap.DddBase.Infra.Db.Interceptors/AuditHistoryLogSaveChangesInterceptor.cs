using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Attributes.AuditHistoryLogAttributes;
using Haskap.DddBase.Domain.AuditHistoryLogAggregate;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Db.Interceptors;

// burada da TUserId nullable olan Guid? olarak verilecek.
public class AuditHistoryLogSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserIdProvider? _currentUserIdProvider;
    private readonly IVisitIdProvider? _visitIdProvider;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public AuditHistoryLogSaveChangesInterceptor(
        ICurrentUserIdProvider? currentUserIdProvider,
        IVisitIdProvider? visitIdProvider,
        ICurrentTenantProvider currentTenantProvider)
    {
        _currentUserIdProvider = currentUserIdProvider;
        _visitIdProvider = visitIdProvider;
        _currentTenantProvider = currentTenantProvider;
    }


    private AuditHistoryLog GetAuditHistoryLogForEntry(EntityEntry entityEntry)
    {
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

                var propertyHasRemoveAuditHistoryLogAttribute = Attribute.IsDefined(propertyEntry.Metadata.PropertyInfo, typeof(RemoveAuditHistoryLogAttribute));
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

                var propertyHasAddAuditHistoryLogAttribute = Attribute.IsDefined(propertyEntry.Metadata.PropertyInfo, typeof(AddAuditHistoryLogAttribute));
                if (propertyHasAddAuditHistoryLogAttribute)
                {
                    originalValues[propertyName] = propertyEntry.OriginalValue;
                    newValues[propertyName] = propertyEntry.CurrentValue;
                }
            }
        }

        var auditHistoryLog = new AuditHistoryLog(GuidGenerator.CreateSequentialGuid(SequentialGuidType.SequentialAsString));
        var modificationType = DetectModificationType(entityEntry);
        auditHistoryLog.ModificationType = modificationType;
        auditHistoryLog.ModificationDate = DateTime.UtcNow;
        auditHistoryLog.ModifiedUserId = _currentUserIdProvider?.CurrentUserId;
        auditHistoryLog.VisitId = _visitIdProvider?.VisitId;
        auditHistoryLog.TenantId = _currentTenantProvider?.CurrentTenantId;
        auditHistoryLog.ObjectFullType = entityEntry.Entity.GetType().ToString();
        auditHistoryLog.ObjectIds = keyValues.Count == 0 ? null : JsonSerializer.Serialize(keyValues);
        auditHistoryLog.ObjectOriginalValues = modificationType == AuditHistoryLogModificationType.Add || originalValues.Count == 0 ? null : JsonSerializer.Serialize(originalValues);
        auditHistoryLog.ObjectNewValues = modificationType == AuditHistoryLogModificationType.Delete || newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues);
        auditHistoryLog.OwnerIds = null;

        return auditHistoryLog;
    }

    private void SetAuditHistoryLogForObjects(DbContext dbContext)
    {
        var entityEntries = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => !(x.Entity is AuditHistoryLog)  //x.Entity.GetType().GetInterfaces().Any(y=>y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IAuditable<>))  //typeof(IAuditable<>).IsAssignableFrom(x.Entity.GetType())
                                                && x.Metadata.IsOwned() == false
                                                && (x.State == EntityState.Deleted || x.State == EntityState.Modified || x.State == EntityState.Added)
                                                && (Attribute.IsDefined(x.Entity.GetType(), typeof(AddAuditHistoryLogAttribute))
                                                    //|| x.Entity.GetType().GetProperties().Any(y => Attribute.IsDefined(y.PropertyType, typeof(AddAuditHistoryLogAttribute)))
                                                    || x.Entity.GetType().GetProperties().Any(y => y.GetCustomAttributes(true).Any(z => z is AddAuditHistoryLogAttribute))))
                                        //.Select(x => x.Entity)
                                        .ToList();

        foreach (var entityEntry in entityEntries)
        {
            var auditHistoryLog = GetAuditHistoryLogForEntry(entityEntry);
            dbContext.Add(auditHistoryLog);

            var referenceValueObjects = entityEntry.References
                .Where(r =>
                        r.TargetEntry != null
                        //&& r.TargetEntry.State == EntityState.Added
                        && r.TargetEntry.Metadata.IsOwned())
                .Select(x => x.TargetEntry);

            foreach (var vo in referenceValueObjects)
            {
                var auditHistoryLogForOwnedEntry = GetAuditHistoryLogForEntry(vo);
                auditHistoryLogForOwnedEntry.OwnerIds = auditHistoryLog.ObjectIds;
                auditHistoryLogForOwnedEntry.OwnedObjectType = OwnedObjectType.One;
                dbContext.Add(auditHistoryLogForOwnedEntry);
            }

            var collectionValueObjects = entityEntry.Collections;
            foreach (var vos in collectionValueObjects)
            {
                if (vos.CurrentValue is null)
                {
                    continue;
                }

                foreach (var objectInstance in vos.CurrentValue)
                {
                    var vo = vos.FindEntry(objectInstance);
                    var auditHistoryLogForOwnedEntry = GetAuditHistoryLogForEntry(vo);
                    auditHistoryLogForOwnedEntry.OwnerIds = auditHistoryLog.ObjectIds;
                    auditHistoryLogForOwnedEntry.OwnedObjectType = OwnedObjectType.Many;
                    dbContext.Add(auditHistoryLogForOwnedEntry);
                }
            }
        }
    }

    private AuditHistoryLogModificationType DetectModificationType(EntityEntry entry)
    {
        AuditHistoryLogModificationType modificationType = AuditHistoryLogModificationType.Update;

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
                }
                break;
            default:
                break;
        }

        return modificationType;
    }

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
