using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;

namespace Haskap.DddBase.Infra.Providers;

public class MyDbContextFactory<TDbContextInterface, TDbContexImplemantation> : IMyDbContextFactory<TDbContextInterface>
    where TDbContextInterface : IUnitOfWork
    where TDbContexImplemantation : DbContext, TDbContextInterface
{
    private readonly IDbContextFactory<TDbContexImplemantation> _factory;

    public MyDbContextFactory(IDbContextFactory<TDbContexImplemantation> factory)
        => _factory = factory;

    public TDbContextInterface CreateDbContext()
        => _factory.CreateDbContext();

    public async Task<TDbContextInterface> CreateDbContextAsync(CancellationToken cancellationToken)
        => await _factory.CreateDbContextAsync(cancellationToken);
}
