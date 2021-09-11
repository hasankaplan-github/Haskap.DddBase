using System;

public record DomainEvent : Event
{
    public DomainEvent(Guid id) : base(id)
    {
        
    }

    public DomainEvent() : base(Guid.NewGuid())
    {
        
    }
}