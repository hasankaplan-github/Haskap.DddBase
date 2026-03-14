namespace Haskap.DddBase.Domain.Providers;

public interface IMyDbContextFactory<TDbContextInterface>
    where TDbContextInterface : IUnitOfWork
{
    TDbContextInterface CreateDbContext();
    Task<TDbContextInterface> CreateDbContextAsync(CancellationToken cancellationToken);
}
