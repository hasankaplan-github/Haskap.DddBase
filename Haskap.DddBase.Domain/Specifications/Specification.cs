using System.Linq.Expressions;

namespace Haskap.DddBase.Domain.Specifications
{
    public abstract class Specification<TEntity>
    {
        private Func<TEntity, bool>? predicateCache = null;

        public virtual bool IsSatisfiedBy(TEntity entity)
        {
            predicateCache ??= ToExpression().Compile();
            return predicateCache(entity);
        }

        public abstract Expression<Func<TEntity, bool>> ToExpression();

        public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity> specification)
        {
            return specification.ToExpression();
        }
    }
}
