using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Tenants.Domain.TenantAggregate;

namespace Modules.Tenants.Domain;
public interface ITenantsDbContext : IUnitOfWork
{
    DbSet<Tenant> Tenant { get; set; }
}
