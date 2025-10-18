using Haskap.DddBase.Domain.Events;

namespace Modules.ModuleManagement.IntegrationEvents;
public record EnabledModulesUpdatedIntegrationEvent(
    Guid? TenantId,
    IList<string>? UncheckedModuleNames,
    IList<string>? CheckedModuleNames) : IntegrationEvent;