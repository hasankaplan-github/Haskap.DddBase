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
