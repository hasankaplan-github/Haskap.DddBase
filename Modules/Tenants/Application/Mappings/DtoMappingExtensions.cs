using Modules.Tenants.Application.Dtos;
using Modules.Tenants.Domain.TenantAggregate;

namespace Modules.Tenants.Application.Mappings;
public static class DtoMappingExtensions
{
    public static TenantOutputDto ToTenantOutputDto(this Tenant tenant)
    {
        return new()
        {
            Id = tenant.Id,
            Name = tenant.Name,
            TenantOrder = tenant.TenantOrder,
            ConnectionString = tenant.ConnectionString
        };
    }
}
