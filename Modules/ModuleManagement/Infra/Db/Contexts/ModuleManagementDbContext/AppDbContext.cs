using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.AuditLog.Domain.AuditHistoryLogAggregate;
using Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext.EntityTypeConfigurations;
using Modules.ModuleManagement.Domain;
using Modules.ModuleManagement.Domain.ModuleAggregate;

namespace Modules.ModuleManagement.Infra.Db.Contexts.ModuleManagementDbContext;
public class AppDbContext : BaseEfCoreNpgsqlDbContext, IModuleManagementDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options, 
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterGenericProvider? globalQueryFilterGenericProvider)
        : base(
            options,
            currentTenantProvider,
            globalQueryFilterGenericProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("module_management");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("ModuleManagementDbContext"));

        base.OnModelCreating(builder);
    }

    public DbSet<EnabledModule> EnabledModule { get; set; }
}
