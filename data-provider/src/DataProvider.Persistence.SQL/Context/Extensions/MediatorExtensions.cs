using BaseEntity.Domain.Entities;
using BaseEntity.Domain.Mediator.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.Persistence.SQL.Context.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task PublishDomainEvents<TContext>(this IMediatorHandler mediator, TContext context) where TContext : DbContext
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(entry => entry.Entity?.DomainEvents.Count > 0);

            var domainEvents = domainEntities
                .SelectMany(entry => entry.Entity.DomainEvents)
                .ToList();            

            var tasks = domainEvents
                .Select(async (domainEvent) => await mediator.PublishEvent(domainEvent));

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
