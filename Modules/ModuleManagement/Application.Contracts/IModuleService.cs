using Haskap.DddBase.Utilities.Module;
using Modules.ModuleManagement.Application.Dtos;

namespace Modules.ModuleManagement.Application.Contracts;
public interface IModuleService
{
    Task<bool> IsEnabledAsync<TModule>(Guid? tenantId, CancellationToken cancellationToken)
        where TModule : class, IModule;
    Task UpdateEnabledModulesAsync(UpdateEnabledModulesInputDto input, CancellationToken cancellationToken);
    Task<List<ModuleOutputDto>> GetModulesAsync(Guid? tenantId, CancellationToken cancellationToken);
    IReadOnlyList<string> GetModuleNamesRegisteredInSystem();
}
