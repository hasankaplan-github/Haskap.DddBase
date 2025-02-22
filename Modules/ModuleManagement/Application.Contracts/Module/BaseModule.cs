using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Modules.ModuleManagement.Domain.ModuleAggregate.Exceptions;

namespace Modules.ModuleManagement.Application.Contracts.Module;

public abstract class BaseModule<TModule> : IModule
    where TModule : class
{
    protected readonly IModuleService ModuleService;
    protected readonly ICurrentTenantProvider CurrentTenantProvider;

    public string ModuleName => typeof(TModule).Name;

    public BaseModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
    {
        ModuleService = moduleService;
        CurrentTenantProvider = currentTenantProvider;
    }

    public virtual async Task<bool> IsEnabledAsync(CancellationToken cancellationToken = default)
    {
        return await ModuleService.IsEnabledAsync<TModule>(CurrentTenantProvider.CurrentTenantId, cancellationToken);
    }

    public virtual async Task ThrowIfDisabledAsync(string requestPath, CancellationToken cancellationToken = default)
    {
        if (!await IsEnabledAsync(cancellationToken))
        {
            throw new ModuleIsDisabledException(ModuleName, requestPath);
        }
    }
}
