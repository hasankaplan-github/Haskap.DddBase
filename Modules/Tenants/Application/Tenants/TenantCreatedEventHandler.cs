using Haskap.DddBase.Domain.Events;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.Shared;
using Haskap.DddBase.Domain.Shared.Enums;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.DependencyInjection;
using Modules.Tenants.Domain.TenantAggregate.Events;

namespace Modules.Tenants.Application.Tenants;

public class TenantCreatedEventHandler : IEventHandler<TenantCreatedDomainEvent>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public TenantCreatedEventHandler(
        IServiceProvider serviceProvider,
        ICurrentTenantProvider currentTenantProvider)
    {
        _serviceProvider = serviceProvider;
        _currentTenantProvider = currentTenantProvider;
    }

    public async Task HandleAsync(TenantCreatedDomainEvent @event, CancellationToken cancellationToken)
    {
        if (AppConfig.MultiTenancyType == MultiTenancyType.SharedDb)
        {
            return;
        }

        using var _ = _currentTenantProvider.ChangeCurrentTenant(@event.NewTenant.Id);
     
        var moduleDatabaseMigrators = _serviceProvider.GetServices<IModuleDatabaseMigrator>();

        foreach (var moduleDatabaseMigrator in moduleDatabaseMigrators)
        {
            await moduleDatabaseMigrator.MigrateAsync(_serviceProvider, cancellationToken);
        }
    }
}
