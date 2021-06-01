using System;

namespace Haskap.DddBase.Domain.Core
{
    [Serializable]
    public abstract class Entity<TId> : IEntity<TId>
    {
        public TId Id { get; protected set; }

        protected Entity()
        {

        }

        protected Entity(TId id)
        {
            Id = id;
        }
    }
}
