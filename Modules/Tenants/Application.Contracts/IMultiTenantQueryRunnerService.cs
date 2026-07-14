using Haskap.DddBase.Application.Contracts;
using Haskap.DddBase.Domain;

namespace Modules.Tenants.Application.Contracts;

public interface IMultiTenantQueryRunnerService : IUseCaseService
{
    Task<IList<TResult>> QueryAllTenantsAsync<TResult, TDbContext>(
        Func<TDbContext, CancellationToken, Task<IEnumerable<TResult>>> query,
        CancellationToken ct = default)
        where TDbContext : IUnitOfWork;
}

