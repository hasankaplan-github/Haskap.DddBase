using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haskap.DddBase.Modules.Tenants.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;

public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        //if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        //{
        //    builder.Property(x => (x as ISoftDeletable).IsDeleted).IsRequired();
        //    builder.HasQueryFilter(x => (x as ISoftDeletable).IsDeleted == false);
        //}
        
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
}
