using Haskap.DddBase.Application;
using Haskap.DddBase.Domain.Events;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Haskap.DddBase.Utilities.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Modules.ModuleManagement.Application.Contracts.Module;
using Modules.ModuleManagement.Application.Dtos.Module;
using Modules.ModuleManagement.Domain;
using Modules.ModuleManagement.Domain.ModuleAggregate;
using Modules.ModuleManagement.IntegrationEvents;

namespace Modules.ModuleManagement.Application.Module;

public class ModuleService : UseCaseService, IModuleService
{
    private IModuleManagementDbContext _moduleManagementDbContext;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly IBaseCacheKeyProvider _baseCacheKeyProvider;
    private readonly IMemoryCache _memoryCache;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IEventPublisher _eventPublisher;
    private readonly IDbContextProvider _dbContextProvider;

    public ModuleService(
        IModuleManagementDbContext moduleManagementDbContext,
        ICurrentTenantProvider currentTenantProvider,
        IBaseCacheKeyProvider baseCacheKeyProvider,
        IMemoryCache memoryCache,
        IServiceScopeFactory serviceScopeFactory,
        IEventPublisher eventPublisher,
        IDbContextProvider dbContextProvider)
    {
        _moduleManagementDbContext = moduleManagementDbContext;
        _currentTenantProvider = currentTenantProvider;
        _baseCacheKeyProvider = baseCacheKeyProvider;
        _memoryCache = memoryCache;
        _serviceScopeFactory = serviceScopeFactory;
        _eventPublisher = eventPublisher;
        _dbContextProvider = dbContextProvider;
    }

    public async Task<bool> IsEnabledAsync<TModule>(Guid? tenantId, CancellationToken cancellationToken)
        where TModule : class, IModule
    {
        var moduleName = typeof(TModule).Name;

        var modules = await GetModulesAsync(tenantId, cancellationToken);

        return modules.Where(x => x.Name == moduleName)
            .First()
            .IsEnabled;
    }

    public IReadOnlyList<string> GetModuleNamesRegisteredInSystem()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var modules = scope.ServiceProvider.GetServices<IModule>();
        var moduleNames = modules
            .Select(x => x.ModuleName)
            .ToList();

        return moduleNames.AsReadOnly();
    }

    public async Task<List<ModuleOutputDto>> GetModulesAsync(Guid? tenantId, CancellationToken cancellationToken)
    {
        using var _ = _currentTenantProvider.ChangeCurrentTenant(tenantId);

        var cacheKey = _baseCacheKeyProvider.GetModuleStatusesCacheKey(tenantId);

        var modules = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            var tenantCts = await _memoryCache.GetOrCreateAsync(_baseCacheKeyProvider.GetTenantCancellationTokenSourceCacheKey(tenantId), async ce =>
            {
                var cts = new CancellationTokenSource();
                ce.AddExpirationToken(new CancellationChangeToken(cts.Token));
                return cts;
            });
            cacheEntry.AddExpirationToken(new CancellationChangeToken(tenantCts!.Token));
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            _moduleManagementDbContext = _dbContextProvider.GetDbContext<IModuleManagementDbContext>();
            var enabledModules = await _moduleManagementDbContext.EnabledModule
                .Where(x => x.TenantId == tenantId)
                .Select(x => x.Name)
                .ToListAsync(cancellationToken);

            var registeredModuleNames = GetModuleNamesRegisteredInSystem();

            return registeredModuleNames
                .Select(x => new ModuleOutputDto
                {
                    Name = x,
                    IsEnabled = enabledModules.Contains(x)
                })
                .ToList();
        });

        return modules;
    }

    public async Task UpdateEnabledModulesAsync(UpdateEnabledModulesInputDto input, CancellationToken cancellationToken)
    {
        DetectInvalidModuleNamesAndThrowIfAny([.. input.CheckedModuleNames ?? Enumerable.Empty<string>(), .. input.UncheckedModuleNames ?? Enumerable.Empty<string>()]);

        using var _ = _currentTenantProvider.ChangeCurrentTenant(input.TenantId);
        _moduleManagementDbContext = _dbContextProvider.GetDbContext<IModuleManagementDbContext>();
        using var transaction = _moduleManagementDbContext.Database.CurrentTransaction ?? await _moduleManagementDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var enabledModules = await _moduleManagementDbContext.EnabledModule
            .ToListAsync(cancellationToken);

            var updated = UpdateEnabledModules(enabledModules, input.UncheckedModuleNames, input.CheckedModuleNames);
            if (updated)
            {
                await _moduleManagementDbContext.EnabledModule
                .ExecuteDeleteAsync(cancellationToken);

                _moduleManagementDbContext.EnabledModule
                    .AddRange(enabledModules);

                await _moduleManagementDbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                var cacheKey = _baseCacheKeyProvider.GetTenantCancellationTokenSourceCacheKey(input.TenantId);
                var existingTenantCts = _memoryCache.Get<CancellationTokenSource>(cacheKey);
                existingTenantCts?.Cancel();

                var tenantCts = new CancellationTokenSource();
                _memoryCache.Set(cacheKey, tenantCts, new CancellationChangeToken(tenantCts.Token));

                await _eventPublisher.PublishAsync(new EnabledModulesUpdatedIntegrationEvent(
                    input.TenantId,
                    input.UncheckedModuleNames,
                    input.CheckedModuleNames), cancellationToken);
            }
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }


        void DetectInvalidModuleNamesAndThrowIfAny(List<string> updatedModuleNames)
        {
            var validModuleNames = GetModuleNamesRegisteredInSystem();
            var hasInvalidModuleName = updatedModuleNames.Any(x => !validModuleNames.Contains(x));

            if (hasInvalidModuleName)
            {
                throw new InvalidOperationException("Invalid module name detected.");
            }
        }
    }

    private bool AddEnabledModules(List<EnabledModule> existingEnabledModules, IEnumerable<string>? checkedList)
    {
        if (checkedList?.Any() != true)
        {
            return false;
        }

        var toBeAdded = checkedList
           .Except(existingEnabledModules.Select(x => x.Name))
           .ToList();

        foreach (var enabledModuleName in toBeAdded)
        {
            existingEnabledModules.Add(new(
                id: GuidGenerator.CreateSimpleGuid(),
                enabledModuleName));
        }

        return toBeAdded.Count > 0;
    }

    private bool RemoveEnabledModules(List<EnabledModule> existingEnabledModules, IEnumerable<string>? uncheckedList)
    {
        if (uncheckedList?.Any() != true)
        {
            return false;
        }

        if (!existingEnabledModules.Any())
        {
            return false;
        }

        var toBeDeleted = existingEnabledModules
            .IntersectBy(uncheckedList, x => x.Name)
            .ToList();

        foreach (var module in toBeDeleted)
        {
            existingEnabledModules.RemoveAll(x => x.Name == module.Name);
        }

        return toBeDeleted.Count > 0;
    }

    private bool UpdateEnabledModules(List<EnabledModule> existingEnabledModules, IEnumerable<string>? uncheckedList, IEnumerable<string>? checkedList)
    {
        var removed = RemoveEnabledModules(existingEnabledModules, uncheckedList);

        var added = AddEnabledModules(existingEnabledModules, checkedList);

        return removed || added;
    }
}