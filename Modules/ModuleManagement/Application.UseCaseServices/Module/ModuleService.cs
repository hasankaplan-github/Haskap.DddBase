using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Modules.ModuleManagement.Application.Contracts.Module;
using Modules.ModuleManagement.Application.Dtos.Module;
using Modules.ModuleManagement.Domain;
using Modules.ModuleManagement.Domain.ModuleAggregate;

namespace Modules.ModuleManagement.Application.UseCaseServices.Module;

public class ModuleService : UseCaseService, IModuleService
{
    private readonly Haskap.DddBase.Domain.Shared.Consts.Modules _modules;
    private readonly IModuleManagementDbContext _moduleManagementDbContext;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly IBaseCacheKeyProvider _baseCacheKeyProvider;
    private readonly IMemoryCache _memoryCache;

    public ModuleService(
        IOptions<Haskap.DddBase.Domain.Shared.Consts.Modules> modulesOptions,
        IModuleManagementDbContext moduleManagementDbContext,
        ICurrentTenantProvider currentTenantProvider,
        IBaseCacheKeyProvider baseCacheKeyProvider,
        IMemoryCache memoryCache)
    {
        _modules = modulesOptions.Value;
        _moduleManagementDbContext = moduleManagementDbContext;
        _currentTenantProvider = currentTenantProvider;
        _baseCacheKeyProvider = baseCacheKeyProvider;
        _memoryCache = memoryCache;
    }

    public async Task<bool> IsEnabledAsync<TModule>(Guid? tenantId, CancellationToken cancellationToken)
    {
        var moduleName = typeof(TModule).Name;

        if (!_modules.IsEnabled[moduleName])
        {
            return false;
        }

        var modules = await GetModulesAsync(tenantId, cancellationToken);

        return modules.Where(x => x.Name == moduleName)
            .Select(x => x.IsEnabled)
            .First();
    }

    public async Task<List<ModuleOutputDto>> GetModulesAsync(Guid? tenantId, CancellationToken cancellationToken)
    {
        using var _ = _currentTenantProvider.ChangeCurrentTenant(tenantId);

        var cacheKey = _baseCacheKeyProvider.GetModuleStatusesCacheKey(tenantId);

        var modules = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

            var enabledModules = await _moduleManagementDbContext.EnabledModule
                .Where(x => x.TenantId == tenantId)
                .Select(x => x.Name)
                .ToListAsync(cancellationToken);

            return _modules.IsEnabled
                .Select(x => new ModuleOutputDto
                {
                    Name = x.Key,
                    IsEnabled = enabledModules.Contains(x.Key)
                })
                .ToList();
        });

        return modules;
    }

    public async Task UpdateEnabledModulesAsync(UpdateEnabledModulesInputDto input, CancellationToken cancellationToken)
    {
        DetectInvalidModuleNamesAndThrowIfAnyAsync([.. input.CheckedModuleNames ?? Enumerable.Empty<string>(), .. input.UncheckedModuleNames ?? Enumerable.Empty<string>()]);

        using var _ = _currentTenantProvider.ChangeCurrentTenant(input.TenantId);

        using var transaction = await _moduleManagementDbContext.Database.BeginTransactionAsync(cancellationToken);
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

            _memoryCache.Remove(_baseCacheKeyProvider.GetModuleStatusesCacheKey(input.TenantId));
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }


        void DetectInvalidModuleNamesAndThrowIfAnyAsync(List<string> updatedModuleNames)
        {
            var hasInvalidModuleName = updatedModuleNames.Any(x => !_modules.IsEnabled.ContainsKey(x));

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