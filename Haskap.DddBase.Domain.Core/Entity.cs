using MediatR;
using System;
using System.Collections.Generic;

namespace Haskap.DddBase.Domain.Core
{
    [Serializable]
    public abstract class Entity<TId> : IEntity<TId>
    {
        public readonly IList<INotification> DomainEvents = new List<INotification>();
        
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
