using MediatR;
using System;

namespace Haskap.DddBase.Domain.Core;

public record Event : INotification
{
    public DateTime CreationDate { get; protected set; }
    public Guid Id { get; protected set; }

    public Event(Guid id)
    {
        Id = id;
        CreationDate = DateTime.Now;
    }
}