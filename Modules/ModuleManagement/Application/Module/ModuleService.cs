using Haskap.DddBase.Application.UseCaseServices;
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

namespace Modules.ModuleManagement.Application.UseCaseServices.Module;

public class ModuleService : UseCaseService, IModuleService
{
    private readonly IModuleManagementDbContext _moduleManagementDbContext;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly IBaseCacheKeyProvider _baseCacheKeyProvider;
    private readonly IMemoryCache _memoryCache;
    private readonly IServiceCollection _services;

    public ModuleService(
        IModuleManagementDbContext moduleManagementDbContext,
        ICurrentTenantProvider currentTenantProvider,
        IBaseCacheKeyProvider baseCacheKeyProvider,
        IMemoryCache memoryCache,
        IServiceCollection services)
    {
        _moduleManagementDbContext = moduleManagementDbContext;
        _currentTenantProvider = currentTenantProvider;
        _baseCacheKeyProvider = baseCacheKeyProvider;
        _memoryCache = memoryCache;
        _services = services;
    }

    public async Task<bool> IsEnabledAsync<TModule>(Guid? tenantId, CancellationToken cancellationToken)
    {
        var moduleName = typeof(TModule).Name;

        var modules = await GetModulesAsync(tenantId, cancellationToken);

        return modules.Where(x => x.Name == moduleName)
            .Select(x => x.IsEnabled)
            .First();
    }

    public IReadOnlyList<string> GetModuleNames()
    {
        var moduleNames = _services.Where(x => x.ServiceType.GetInterfaces().Contains(typeof(IModule))).Select(x => x.ImplementationType!.Name).ToList();

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

            var enabledModules = await _moduleManagementDbContext.EnabledModule
                .Where(x => x.TenantId == tenantId)
                .Select(x => x.Name)
                .ToListAsync(cancellationToken);

            var moduleNames = GetModuleNames();

            return moduleNames
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
        DetectInvalidModuleNamesAndThrowIfAnyAsync([.. input.CheckedModuleNames ?? Enumerable.Empty<string>(), .. input.UncheckedModuleNames ?? Enumerable.Empty<string>()]);

        using var _ = _currentTenantProvider.ChangeCurrentTenant(input.TenantId);

        using var transaction = _moduleManagementDbContext.Database.CurrentTransaction ?? await _moduleManagementDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var enabledModules = await _moduleManagementDbContext.EnabledModule
            .ToListAsync(cancellationToken);

            UpdateEnabledModules(enabledModules, input.UncheckedModuleNames, input.CheckedModuleNames);

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
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }


        void DetectInvalidModuleNamesAndThrowIfAnyAsync(List<string> updatedModuleNames)
        {
            var validModuleNames = GetModuleNames();
            var hasInvalidModuleName = updatedModuleNames.Any(x => !validModuleNames.Contains(x));

            if (hasInvalidModuleName)
            {
                throw new InvalidOperationException("Invalid module name detected.");
            }
        }
    }

    private void AddEnabledModules(List<EnabledModule> existingEnabledModules, IEnumerable<string>? checkedList)
    {
        if (checkedList?.Any() != true)
        {
            return;
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
    }

    private void RemoveEnabledModules(List<EnabledModule> existingEnabledModules, IEnumerable<string>? uncheckedList)
    {
        if (uncheckedList?.Any() != true)
        {
            return;
        }

        if (!existingEnabledModules.Any())
        {
            return;
        }

        var toBeDeleted = existingEnabledModules
            .IntersectBy(uncheckedList, x => x.Name)
            .ToList();

        foreach (var module in toBeDeleted)
        {
            existingEnabledModules.RemoveAll(x => x.Name == module.Name);
        }
    }

    private void UpdateEnabledModules(List<EnabledModule> existingEnabledModules, IEnumerable<string>? uncheckedList, IEnumerable<string>? checkedList)
    {
        RemoveEnabledModules(existingEnabledModules, uncheckedList);

        AddEnabledModules(existingEnabledModules, checkedList);
    }
}