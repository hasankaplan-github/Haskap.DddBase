using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

public static class EventPublisher
{
    [ThreadStatic] // ensure separate func per thread to support parallel invocation
    public static Func<IMediator> Mediator;
    public static async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : INotification
    {
        var mediator = Mediator.Invoke();
        await mediator.Publish<TEvent>(@event, cancellationToken);
    }
}