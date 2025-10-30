using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Domain.Providers;

public interface IDbContextProvider
{
    TDbContext GetDbContext<TDbContext>() where TDbContext : IUnitOfWork;
}
