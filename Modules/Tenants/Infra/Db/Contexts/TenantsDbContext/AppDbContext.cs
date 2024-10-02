using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Haskap.DddBase.Modules.Tenants.Domain;
using Haskap.DddBase.Modules.Tenants.Domain.TenantAggregate;

namespace Haskap.DddBase.Modules.Tenants.Infra.Db.Contexts.TenantsDbContext;

public class AppDbContext : BaseEfCoreNpgsqlDbContext, ITenantsDbContext
{
    protected AppDbContext(
        DbContextOptions options, 
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterGenericProvider? globalQueryFilterGenericProvider)
        : base(
            options, 
            currentTenantProvider,
            globalQueryFilterGenericProvider)
    {
    }

    public DbSet<Tenant> Tenant { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("TenantsDbContext"));

        base.OnModelCreating(builder);
    }
}
