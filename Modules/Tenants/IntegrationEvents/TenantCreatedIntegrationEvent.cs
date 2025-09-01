using Haskap.DddBase.Domain.Events;

namespace Modules.Tenants.IntegrationEvents;
public record TenantCreatedIntegrationEvent(
    Guid TenantId,
    string TenantName) : IntegrationEvent;