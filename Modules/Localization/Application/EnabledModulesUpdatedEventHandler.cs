using Haskap.DddBase.Domain.Events;
using Modules.Localization.Application.Contracts;
using Modules.ModuleManagement.IntegrationEvents;

namespace Modules.Localization.Application;

public class EnabledModulesUpdatedEventHandler : IEventHandler<EnabledModulesUpdatedIntegrationEvent>
{
    private readonly ILocalizationModule _localizationModule;
    private readonly ILocalizationService _localizationService;

    public EnabledModulesUpdatedEventHandler(
        ILocalizationModule localizationModule,
        ILocalizationService localizationService)
    {
        _localizationModule = localizationModule;
        _localizationService = localizationService;
    }

    public async Task HandleAsync(EnabledModulesUpdatedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        if (@event.UncheckedModuleNames?.Contains(_localizationModule.ModuleName) == true ||
            @event.CheckedModuleNames?.Contains(_localizationModule.ModuleName) == true)
        {
            await _localizationService.InvalidateAllLocalesCachesAsync([@event.TenantId], cancellationToken);
        }
    }
}
