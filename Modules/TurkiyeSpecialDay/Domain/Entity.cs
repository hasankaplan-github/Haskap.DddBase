using Haskap.DddBase.Domain;

namespace Modules.TurkiyeSpecialDay.Domain;

public abstract class Entity : Entity<Guid>, IEntity
{
    protected Entity()
    {

    }

    protected Entity(Guid id)
        : base(id)
    {

    }
}
