using System;

public record IntegrationEvent : Event
{
    public IntegrationEvent(Guid id) : base(id)
    {
        
    }

    public IntegrationEvent() : base(Guid.NewGuid())
    {
        
    }
}