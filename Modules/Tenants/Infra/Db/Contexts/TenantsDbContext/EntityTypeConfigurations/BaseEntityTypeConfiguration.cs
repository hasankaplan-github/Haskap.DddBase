using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Tenants.Domain;

namespace Modules.Tenants.Infra.Db.Contexts.TenantsDbContext.EntityTypeConfigurations;

public class BaseEntityTypeConfiguration<TEntity> : Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations.BaseEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);
        if (typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
        {
            builder.Property(x => (x as IEntity).Id).ValueGeneratedNever();
        }


        //if (typeof(IHasClusteredIndex).IsAssignableFrom(typeof(TEntity)))
        //{
        //    builder.HasKey(x => x.Id).IsClustered(false);
        //    builder.HasIndex(x => (x as IHasClusteredIndex).ClusteredIndex).IsClustered();
        //}
    }
}
