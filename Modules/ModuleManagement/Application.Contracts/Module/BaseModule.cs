using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;

namespace Modules.ModuleManagement.Application.Contracts.Module;

public abstract class BaseModule<TModule> : IModule
    where TModule : class
{
    protected readonly IModuleService ModuleService;
    protected readonly ICurrentTenantProvider CurrentTenantProvider;

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
}
