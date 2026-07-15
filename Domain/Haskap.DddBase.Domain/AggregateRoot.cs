namespace Haskap.DddBase.Domain;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>
    where TId : notnull
{
    protected AggregateRoot()
    {

    }

    protected AggregateRoot(TId id)
        : base(id)
    {

    }
}

public abstract class AggregateRoot : AggregateRoot<Guid>, IEntity
{
    protected AggregateRoot()
    {

    }

    protected AggregateRoot(Guid id)
        : base(id)
    {

    }
}
