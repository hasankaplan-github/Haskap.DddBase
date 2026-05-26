using Haskap.DddBase.Application;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.Tenants.Application.Contracts.Tenants;
using System.Collections.Concurrent;

namespace Modules.Tenants.Application.Tenants;

public class MultiTenantQueryRunnerService : UseCaseService, IMultiTenantQueryRunnerService
{
    private readonly ITenantService _tenantService;
    private readonly ILogger<MultiTenantQueryRunnerService> _logger;
    //private readonly IDbContextProvider _dbContextProvider;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public MultiTenantQueryRunnerService(
        ITenantService tenantService,
        ILogger<MultiTenantQueryRunnerService> logger,
        //IDbContextProvider dbContextProvider,
        ICurrentTenantProvider currentTenantProvider,
        IServiceProvider serviceProvider)
    {
        _tenantService = tenantService;
        _logger = logger;
        //_dbContextProvider = dbContextProvider;
        _currentTenantProvider = currentTenantProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task<IList<TResult>> QueryAllTenantsAsync<TResult, TDbContext>(
        Func<TDbContext, CancellationToken, Task<IEnumerable<TResult>>> query,
        CancellationToken ct = default)
        where TDbContext : IUnitOfWork
    {
        var tenants = await _tenantService.GetAllAsync(ct);
        var results = new ConcurrentBag<TResult>();
        using var semaphore = new SemaphoreSlim(6);

        var tasks = tenants.Select(async tenant =>
        {
            await semaphore.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                //_currentTenantProvider.ChangeCurrentTenant(tenant.Id);

                //var db = _dbContextProvider.GetDbContext<TDbContext>();
                var dbContextFactory = _serviceProvider.GetRequiredService<IMyDbContextFactory<TDbContext>>();
                await using var db = await dbContextFactory.CreateDbContextAsync(tenant.Id, ct);

                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                // optional: set a per-tenant command timeout
                //db.Database.SetCommandTimeout(TimeSpan.FromSeconds(30));
                IList<TResult> tenantResult;
                try
                {
                    tenantResult = (await query(db, ct).ConfigureAwait(false)).ToList();
                }
                //catch (OperationCanceledException) { throw; }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Query failed for tenant {TenantId}", tenant.Id);
                    throw;
                }

                _logger.LogInformation("Query succeeded for tenant {TenantId} with {Count} results", tenant.Id, tenantResult.Count);

                foreach (var item in tenantResult) results.Add(item);
            }
            finally
            {
                semaphore.Release();
            }
        }).ToArray();

        await Task.WhenAll(tasks).ConfigureAwait(false);
        return results.ToList();
    }
}
