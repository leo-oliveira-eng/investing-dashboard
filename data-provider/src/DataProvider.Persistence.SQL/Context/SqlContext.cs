using BaseEntity.Domain.Entities;
using BaseEntity.Domain.Mediator.Contracts;
using DataProvider.Persistence.SQL.Context.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataProvider.Persistence.SQL.Context
{
    public class SqlContext : DbContext
    {
        IMediatorHandler MediatorHandler { get; }

        public SqlContext(DbContextOptions<SqlContext> dbContextOptions, IMediatorHandler mediatorHandler) 
            : base(dbContextOptions) 
        {
            MediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(Entity).IsAssignableFrom(type.ClrType) && (type.BaseType is null || !typeof(Entity).IsAssignableFrom(type.BaseType.ClrType)))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }
        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();

            var result = base.SaveChanges();

            await MediatorHandler.PublishDomainEvents(this).ConfigureAwait(false);

            return Task.FromResult(result).Result;

        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is not Entity)
                    continue;

                switch (entry.State)
                {
                    case EntityState.Deleted:
                        ((Entity)entry.Entity).Delete();
                        entry.State = EntityState.Modified;
                        break;
                }
            }
        }
    }
}
