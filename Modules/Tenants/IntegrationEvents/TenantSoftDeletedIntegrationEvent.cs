using Haskap.DddBase.Domain.Events;

namespace Modules.Tenants.IntegrationEvents;
public record TenantSoftDeletedIntegrationEvent(Guid TenantId) : IntegrationEvent;