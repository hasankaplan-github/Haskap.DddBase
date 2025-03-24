using Modules.ModuleManagement.Application.Dtos.Module;

namespace Modules.ModuleManagement.Application.Contracts.Module;
public interface IModuleService
{
    Task<bool> IsEnabledAsync<TModule>(Guid? tenantId, CancellationToken cancellationToken);
    Task UpdateEnabledModulesAsync(UpdateEnabledModulesInputDto input, CancellationToken cancellationToken);
    Task<List<ModuleOutputDto>> GetModulesAsync(Guid? tenantId, CancellationToken cancellationToken);
    IReadOnlyList<string> GetModuleNames();
}
