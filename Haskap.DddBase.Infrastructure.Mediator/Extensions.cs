using Haskap.DddBase.Domain.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infrastructure.Mediator
{
    public static class MediatRExtensionMethods
    {
        public static async Task DispatchDomainEventsAsync<TDbContext, TEntityId>(this IMediator mediator, TDbContext dbContext, CancellationToken cancellationToken = default)
            where TDbContext : DbContext
        {
            var domainEntities = dbContext.ChangeTracker
                .Entries<Entity<TEntityId>>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.DomainEvents.Clear());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
