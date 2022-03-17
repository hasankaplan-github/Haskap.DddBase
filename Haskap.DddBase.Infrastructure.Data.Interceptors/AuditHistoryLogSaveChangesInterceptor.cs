using Haskap.DddBase.Domain.Core;
using Haskap.DddBase.Domain.Core.Attributes.AuditHistoryLogAttributes;
using Haskap.DddBase.Domain.Core.AuditHistoryLogAggregate;
using Haskap.DddBase.Infrastructure.Providers;
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

namespace Tesmer.PaymentSystem.Infrastructure.Data.Interceptors;

public class AuditHistoryLogSaveChangesInterceptor<TObjectId> : SaveChangesInterceptor
{
    private readonly LoggedInUserProvider<TObjectId> loggedInUserProvider;
    private readonly VisitIdProvider visitIdProvider;

    public AuditHistoryLogSaveChangesInterceptor(LoggedInUserProvider<TObjectId> loggedInUserProvider, VisitIdProvider visitIdProvider)
    {
        this.loggedInUserProvider = loggedInUserProvider;
        this.visitIdProvider = visitIdProvider;
    }

    private void SetAuditHistoryLogForDeletedObjects(DbContext dbContext)
    {
        var deletedAuditHistoryLogEntityEntries = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => !(x.Entity is AuditHistoryLog)  //x.Entity.GetType().GetInterfaces().Any(y=>y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IAuditable<>))  //typeof(IAuditable<>).IsAssignableFrom(x.Entity.GetType())
                                                && x.State == EntityState.Deleted
                                                && (Attribute.IsDefined(x.Entity.GetType(), typeof(AddAuditHistoryLogAttribute))
                                                    || x.Entity.GetType().GetProperties().Any(y => Attribute.IsDefined(y.PropertyType, typeof(AddAuditHistoryLogAttribute)))
                                                    ))
                                        //.Select(x => x.Entity)
                                        .ToList();



        foreach (var deletedAuditHistoryLogEntityEntry in deletedAuditHistoryLogEntityEntries)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            Dictionary<string, object> originalValues = new Dictionary<string, object>();
            Dictionary<string, object> newValues = new Dictionary<string, object>();

            var propertyEntries = deletedAuditHistoryLogEntityEntry.Properties;

            // class seviyesinde tanımlanmışsa
            var classHasAuditHistoryLogAttribute = Attribute.IsDefined(deletedAuditHistoryLogEntityEntry.Entity.GetType(), typeof(AddAuditHistoryLogAttribute));
            if (classHasAuditHistoryLogAttribute)
            {
                foreach (var propertyEntry in propertyEntries)
                {
                    var propertyName = propertyEntry.Metadata.Name;

                    if (propertyEntry.Metadata.IsPrimaryKey())
                    {
                        keyValues[propertyName] = propertyEntry.CurrentValue;
                        continue;
                    }

                    var propertyHasRemoveAuditHistoryLogAttribute = Attribute.IsDefined(propertyEntry.Metadata.PropertyInfo, typeof(RemoveAuditHistoryLogAttribute));
                    if (propertyHasRemoveAuditHistoryLogAttribute)
                    {
                        continue;
                    }

                    originalValues[propertyName] = propertyEntry.OriginalValue;
                    //newValues[propertyName] = propertyEntry.CurrentValue;
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
                        keyValues[propertyName] = propertyEntry.CurrentValue;
                        continue;
                    }

                    var propertyHasAddAuditHistoryLogAttribute = Attribute.IsDefined(propertyEntry.Metadata.PropertyInfo, typeof(AddAuditHistoryLogAttribute));
                    if (propertyHasAddAuditHistoryLogAttribute)
                    {
                        originalValues[propertyName] = propertyEntry.OriginalValue;
                        //newValues[propertyName] = propertyEntry.CurrentValue;
                    }
                }
            }

            var auditHistoryLog = new AuditHistoryLog(GuidGenerator.CreateSequentialGuid(SequentialGuidType.SequentialAsString));
            auditHistoryLog.ModificationType = AuditHistoryLogModificationType.Delete;
            auditHistoryLog.ModificationDate = DateTime.UtcNow;
            auditHistoryLog.ModifiedUserId = loggedInUserProvider.UserId?.ToString();
            auditHistoryLog.VisitId = visitIdProvider.VisitId;
            auditHistoryLog.ObjectFullType = deletedAuditHistoryLogEntityEntry.Entity.GetType().ToString();
            auditHistoryLog.ObjectIds = keyValues.Count == 0 ? null : JsonSerializer.Serialize(keyValues);
            auditHistoryLog.ObjectOriginalValues = originalValues.Count == 0 ? null : JsonSerializer.Serialize(originalValues);
            auditHistoryLog.ObjectNewValues = newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues);

            dbContext.Add(auditHistoryLog);
        }
    }

    private void SetAuditHistoryLogForAddedObjects(DbContext dbContext)
    {
        var addedAuditHistoryLogEntityEntries = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => !(x.Entity is AuditHistoryLog)  //x.Entity.GetType().GetInterfaces().Any(y=>y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IAuditable<>))  //typeof(IAuditable<>).IsAssignableFrom(x.Entity.GetType())
                                                && x.State == EntityState.Added
                                                && (Attribute.IsDefined(x.Entity.GetType(), typeof(AddAuditHistoryLogAttribute))
                                                    || x.Entity.GetType().GetProperties().Any(y => Attribute.IsDefined(y.PropertyType, typeof(AddAuditHistoryLogAttribute)))
                                                    ))
                                        //.Select(x => x.Entity)
                                        .ToList();



        foreach (var addedAuditHistoryLogEntityEntry in addedAuditHistoryLogEntityEntries)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            Dictionary<string, object> originalValues = new Dictionary<string, object>();
            Dictionary<string, object> newValues = new Dictionary<string, object>();

            var propertyEntries = addedAuditHistoryLogEntityEntry.Properties;

            // class seviyesinde tanımlanmışsa
            var classHasAuditHistoryLogAttribute = Attribute.IsDefined(addedAuditHistoryLogEntityEntry.Entity.GetType(), typeof(AddAuditHistoryLogAttribute));
            if (classHasAuditHistoryLogAttribute)
            {
                foreach (var propertyEntry in propertyEntries)
                {
                    var propertyName = propertyEntry.Metadata.Name;

                    if (propertyEntry.Metadata.IsPrimaryKey())
                    {
                        keyValues[propertyName] = propertyEntry.CurrentValue;
                        continue;
                    }

                    var propertyHasRemoveAuditHistoryLogAttribute = Attribute.IsDefined(propertyEntry.Metadata.PropertyInfo, typeof(RemoveAuditHistoryLogAttribute));
                    if (propertyHasRemoveAuditHistoryLogAttribute)
                    {
                        continue;
                    }

                    //originalValues[propertyName] = propertyEntry.OriginalValue;
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
                        keyValues[propertyName] = propertyEntry.CurrentValue;
                        continue;
                    }

                    var propertyHasAddAuditHistoryLogAttribute = Attribute.IsDefined(propertyEntry.Metadata.PropertyInfo, typeof(AddAuditHistoryLogAttribute));
                    if (propertyHasAddAuditHistoryLogAttribute)
                    {
                        //originalValues[propertyName] = propertyEntry.OriginalValue;
                        newValues[propertyName] = propertyEntry.CurrentValue;
                    }
                }
            }

            var auditHistoryLog = new AuditHistoryLog(GuidGenerator.CreateSequentialGuid(SequentialGuidType.SequentialAsString));
            auditHistoryLog.ModificationType = AuditHistoryLogModificationType.Add;
            auditHistoryLog.ModificationDate = DateTime.UtcNow;
            auditHistoryLog.ModifiedUserId = loggedInUserProvider.UserId?.ToString();
            auditHistoryLog.VisitId = visitIdProvider.VisitId;
            auditHistoryLog.ObjectFullType = addedAuditHistoryLogEntityEntry.Entity.GetType().ToString();
            auditHistoryLog.ObjectIds = keyValues.Count == 0 ? null : JsonSerializer.Serialize(keyValues);
            auditHistoryLog.ObjectOriginalValues = originalValues.Count == 0 ? null : JsonSerializer.Serialize(originalValues);
            auditHistoryLog.ObjectNewValues = newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues);

            dbContext.Add(auditHistoryLog);
        }
    }

    private void SetAuditHistoryLogForModifiedObjects(DbContext dbContext)
    {
        var modifiedAuditHistoryLogEntityEntries = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => !(x.Entity is AuditHistoryLog)  //x.Entity.GetType().GetInterfaces().Any(y=>y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IAuditable<>))  //typeof(IAuditable<>).IsAssignableFrom(x.Entity.GetType())
                                                && x.State == EntityState.Modified
                                                && (Attribute.IsDefined(x.Entity.GetType(), typeof(AddAuditHistoryLogAttribute))
                                                    || x.Entity.GetType().GetProperties().Any(y => Attribute.IsDefined(y.PropertyType, typeof(AddAuditHistoryLogAttribute)))
                                                    ))
                                        //.Select(x => x.Entity)
                                        .ToList();



        foreach (var modifiedAuditHistoryLogEntityEntry in modifiedAuditHistoryLogEntityEntries)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            Dictionary<string, object> originalValues = new Dictionary<string, object>();
            Dictionary<string, object> newValues = new Dictionary<string, object>();

            var propertyEntries = modifiedAuditHistoryLogEntityEntry.Properties;

            // class seviyesinde tanımlanmışsa
            var classHasAuditHistoryLogAttribute = Attribute.IsDefined(modifiedAuditHistoryLogEntityEntry.Entity.GetType(), typeof(AddAuditHistoryLogAttribute));
            if (classHasAuditHistoryLogAttribute)
            {
                foreach (var propertyEntry in propertyEntries)
                {
                    var propertyName = propertyEntry.Metadata.Name;

                    if (propertyEntry.Metadata.IsPrimaryKey())
                    {
                        keyValues[propertyName] = propertyEntry.CurrentValue;
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
                        keyValues[propertyName] = propertyEntry.CurrentValue;
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
            auditHistoryLog.ModificationType = DetectModificationType(modifiedAuditHistoryLogEntityEntry);
            auditHistoryLog.ModificationDate = DateTime.UtcNow;
            auditHistoryLog.ModifiedUserId = loggedInUserProvider.UserId?.ToString();
            auditHistoryLog.VisitId = visitIdProvider.VisitId;
            auditHistoryLog.ObjectFullType = modifiedAuditHistoryLogEntityEntry.Entity.GetType().ToString();
            auditHistoryLog.ObjectIds = keyValues.Count == 0 ? null : JsonSerializer.Serialize(keyValues);
            auditHistoryLog.ObjectOriginalValues = originalValues.Count == 0 ? null : JsonSerializer.Serialize(originalValues);
            auditHistoryLog.ObjectNewValues = newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues);

            dbContext.Add(auditHistoryLog);
        }
    }

    private AuditHistoryLogModificationType DetectModificationType(EntityEntry entry)
    {
        AuditHistoryLogModificationType modificationType = AuditHistoryLogModificationType.Update;
        if (entry.Entity is ISoftDeletable)
        {
            PropertyEntry isDeletedPropertyEntry = entry.Property("IsDeleted");
            if (isDeletedPropertyEntry.IsModified)
            {
                if (bool.Parse(isDeletedPropertyEntry.CurrentValue.ToString()) == true)
                {
                    modificationType = AuditHistoryLogModificationType.SoftDelete;
                }
                else
                {
                    modificationType = AuditHistoryLogModificationType.Undelete;
                }
            }
        }

        return modificationType;
    }

    private void SetAuditHistoryLogForObjects(DbContext dbContext)
    {
        SetAuditHistoryLogForAddedObjects(dbContext);
        SetAuditHistoryLogForModifiedObjects(dbContext);
        SetAuditHistoryLogForDeletedObjects(dbContext);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetAuditHistoryLogForObjects(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetAuditHistoryLogForObjects(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
