using MediatR;
using System;

namespace Haskap.DddBase.Domain;

public record Event : INotification
{
    public DateTime UtcCreatedOn { get; protected set; }
    public Guid Id { get; protected set; }

    public Event(Guid id)
    {
        Id = id;
        UtcCreatedOn = DateTime.UtcNow;
    }
}