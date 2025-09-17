using Haskap.DddBase.Domain;

namespace Modules.Localization.Domain;

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
