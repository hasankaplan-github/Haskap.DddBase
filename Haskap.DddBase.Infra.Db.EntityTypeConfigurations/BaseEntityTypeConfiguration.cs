using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haskap.DddBase.Infrastructure.Data.EfCoreEntityTypeConfigurations;

//public class BaseEntityTypeConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
//where TEntity : Entity<TId>
public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        {
            builder.Property(x => (x as ISoftDeletable).IsDeleted).IsRequired();
            builder.HasQueryFilter(x => (x as ISoftDeletable).IsDeleted == false);
        }
        
        /*
        var isSoftDeletable = false;
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        {
            isSoftDeletable = true;
            builder.Property(x => (x as ISoftDeletable).IsDeleted).IsRequired();
            //builder.HasQueryFilter(x => (x as ISoftDeletable).IsDeleted == false);
        }

        var hasMultiTenancy = false;
        if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
        {
            hasMultiTenancy = true;
        }

        if (isSoftDeletable || hasMultiTenancy)
        {
            builder.HasQueryFilter(x =>
                (!isSoftDeletable || (x as ISoftDeletable).IsDeleted == false) &&
                (!hasMultiTenancy || !CurrentTenantProvider.MultiTenancyIsEnabled || (x as IMultiTenant).TenantId == CurrentTenantProvider.CurrentTenant.Id));
            // CurrentTenant middleware içinde set edilmesi gerekiyor.
            // https://github.com/hikalkan/presentations/blob/master/2018-04-06-Multi-Tenancy/src/MultiTenancyDraft/Infrastructure/MultiTenancyMiddleware.cs
        }
        */
    }

    /*
     private static readonly MethodInfo ConfigureBasePropertiesMethodInfo
        = typeof(AbpDbContext<TDbContext>)
            .GetMethod(
                nameof(ConfigureBaseProperties),
                BindingFlags.Instance | BindingFlags.NonPublic
            );

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        TrySetDatabaseProvider(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureBasePropertiesMethodInfo
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });
        }
    } 

    protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        if (mutableEntityType.IsOwned())
        {
            return;
        }

        if (!typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
        {
            return;
        }

        modelBuilder.Entity<TEntity>().ConfigureByConvention();

        ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
    }

    protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        if (mutableEntityType.BaseType == null && ShouldFilterEntity<TEntity>(mutableEntityType))
        {
            var filterExpression = CreateFilterExpression<TEntity>();
            if (filterExpression != null)
            {
                modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
            }
        }
    }
     */
}
