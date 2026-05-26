namespace Haskap.DddBase.Domain.Providers;

public interface IMyDbContextFactory<TDbContextInterface>
    where TDbContextInterface : IUnitOfWork
{
    TDbContextInterface CreateDbContext();
    TDbContextInterface CreateDbContext(Guid? tenantId);
    Task<TDbContextInterface> CreateDbContextAsync(CancellationToken cancellationToken);
    Task<TDbContextInterface> CreateDbContextAsync(Guid? tenantId, CancellationToken cancellationToken);
}
