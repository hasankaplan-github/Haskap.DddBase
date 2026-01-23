using Haskap.DddBase.Domain;

namespace Modules.Bpm.Domain;

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