using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

public static class MediatorWrapper
{
    //[ThreadStatic] // ensure separate func per thread to support parallel invocation
    public static Func<IMediator> MediatorFunc;
    public static async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : INotification
    {
        var mediator = MediatorFunc.Invoke();
        await mediator.Publish<TEvent>(@event, cancellationToken);
    }

    public static async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) 
    {
        var mediator = MediatorFunc.Invoke();
        return await mediator.Send<TResponse>(request, cancellationToken);
    }
}