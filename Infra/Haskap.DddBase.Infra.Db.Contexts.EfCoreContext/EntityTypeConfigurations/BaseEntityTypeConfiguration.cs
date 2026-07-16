using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;

// https://github.com/hikalkan/presentations/blob/master/2018-04-06-Multi-Tenancy/src/MultiTenancyDraft/Infrastructure/MultiTenancyMiddleware.cs

public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
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
