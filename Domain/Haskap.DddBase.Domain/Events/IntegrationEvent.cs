using Haskap.DddBase.Utilities.Guids;

namespace Haskap.DddBase.Domain.Events;

public record IntegrationEvent : Event
{
    public IntegrationEvent(Guid id) : base(id)
    {
        
    }

    public IntegrationEvent() : base(GuidGenerator.CreateSimpleGuid())
    {
        
    }
}