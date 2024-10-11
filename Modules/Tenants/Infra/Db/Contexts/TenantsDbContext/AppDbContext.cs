using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Modules.Tenants.Domain;
using Modules.Tenants.Domain.TenantAggregate;

namespace Modules.Tenants.Infra.Db.Contexts.TenantsDbContext;

public class AppDbContext : BaseEfCoreNpgsqlDbContext, ITenantsDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options, 
        IGlobalQueryFilterGenericProvider? globalQueryFilterGenericProvider)
        : base(
            options, 
            currentTenantProvider: null,
            globalQueryFilterGenericProvider)
    {
    }

    public DbSet<Tenant> Tenant { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("tenants");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("TenantsDbContext"));

        base.OnModelCreating(builder);
    }
}
