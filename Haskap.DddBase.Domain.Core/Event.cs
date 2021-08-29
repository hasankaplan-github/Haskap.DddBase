using MediatR;
using System;

public record Event : INotification
{
    public DateTime CreationDate { get; protected set; }

    public Event()
    {
        CreationDate = DateTime.Now;
    }
}