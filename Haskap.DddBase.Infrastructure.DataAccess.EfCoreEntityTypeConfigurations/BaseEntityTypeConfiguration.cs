using Haskap.DddBase.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haskap.DddBase.Infrastructure.DataAccess.EfCoreEntityTypeConfigurations
{
    public class BaseEntityTypeConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TId>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                builder.HasQueryFilter(x => (x as ISoftDeletable).IsDeleted == false);
            }
        }
    }
}
