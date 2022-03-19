using Haskap.DddBase.Utilities.Guids;
using System;

namespace Haskap.DddBase.Domain.Core;

public record DomainEvent : Event
{
    public DomainEvent(Guid id) : base(id)
    {
        
    }

    public DomainEvent() : base(GuidGenerator.CreateSimpleGuid())
    {
        
    }
}