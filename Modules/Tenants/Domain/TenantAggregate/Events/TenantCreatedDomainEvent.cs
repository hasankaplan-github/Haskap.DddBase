using Haskap.DddBase.Domain.Events;
using Modules.Tenants.Application.Dtos;

namespace Modules.Tenants.Domain.TenantAggregate.Events;

public record TenantCreatedDomainEvent(TenantOutputDto NewTenant) : DomainEvent;