namespace Haskap.DddBase.Domain.Events;

public record Event : IEvent
{
    public DateTime UtcCreatedOn { get; protected set; }
    public Guid Id { get; protected set; }

    public Event(Guid id)
    {
        Id = id;
        UtcCreatedOn = DateTime.UtcNow;
    }
}