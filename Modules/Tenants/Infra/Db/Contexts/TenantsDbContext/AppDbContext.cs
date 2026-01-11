using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.Tenants.Domain;
using Modules.Tenants.Domain.TenantAggregate;

namespace Modules.Tenants.Infra.Db.Contexts.TenantsDbContext;

public class AppDbContext : BaseEfCoreNpgsqlDbContext, ITenantsDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options, 
        IGlobalQueryFilterManagerProvider? globalQueryFilterGenericProvider)
        : base(
            options, 
            currentTenantProvider: null,
            globalQueryFilterGenericProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("tenants");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("TenantsDbContext"));

        base.OnModelCreating(builder);
    }

    public DbSet<Tenant> Tenant { get; set; }
}
