using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Haskap.DddBase.Domain;
public interface IUnitOfWork
{
    DatabaseFacade Database {  get; }
    ChangeTracker ChangeTracker { get; }

    int SaveChanges();

    int SaveChanges(bool acceptAllChangesOnSuccess);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));

    EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        where TEntity : class;

    ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken = default)
        where TEntity : class;

    void AddRange(params object[] entities);

    Task AddRangeAsync(params object[] entities);

    void AddRange(IEnumerable<object> entities);

    Task AddRangeAsync(
        IEnumerable<object> entities,
        CancellationToken cancellationToken = default);
}
