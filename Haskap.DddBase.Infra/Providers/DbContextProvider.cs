using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Infra.Providers;

public class DbContextProvider : IDbContextProvider
{
    private readonly IServiceProvider _serviceProvider;

    public DbContextProvider(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TDbContext GetDbContext<TDbContext>() where TDbContext : IUnitOfWork
    {
        return _serviceProvider.GetRequiredService<TDbContext>();
    }
}
