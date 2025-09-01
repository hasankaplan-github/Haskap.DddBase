using Haskap.DddBase.Domain.Events;

namespace Modules.Tenants.IntegrationEvents;
public record TenantUpdatedIntegrationEvent(
    Guid TenantId,
    string TenantName) : IntegrationEvent;