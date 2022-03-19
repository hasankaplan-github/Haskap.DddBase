using Haskap.DddBase.Utilities.Guids;
using System;

namespace Haskap.DddBase.Domain.Core;

public record IntegrationEvent : Event
{
    public IntegrationEvent(Guid id) : base(id)
    {
        
    }

    public IntegrationEvent() : base(GuidGenerator.CreateSimpleGuid())
    {
        
    }
}