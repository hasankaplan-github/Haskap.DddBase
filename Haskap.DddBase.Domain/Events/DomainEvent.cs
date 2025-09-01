using Haskap.DddBase.Utilities.Guids;

namespace Haskap.DddBase.Domain.Events;

public record DomainEvent : Event
{
    public DomainEvent(Guid id) : base(id)
    {
        
    }

    public DomainEvent() : base(GuidGenerator.CreateSimpleGuid())
    {
        
    }
}