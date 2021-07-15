using MediatR;
using System;

public class DomainEvent : INotification
{
    public DateTime CreationDate { get; }

    public DomainEvent()
    {
        CreationDate = DateTime.Now;
    }
}