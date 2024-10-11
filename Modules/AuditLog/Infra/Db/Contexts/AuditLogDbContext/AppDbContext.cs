using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.AuditLog.Domain;
using Modules.AuditLog.Domain.AuditHistoryLogAggregate;
using Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext.EntityTypeConfigurations;

namespace Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext;

public class AppDbContext : BaseEfCoreNpgsqlDbContext, IAuditLogDbContext
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

    public DbSet<AuditHistoryLog> AuditHistoryLog { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("audit_log");

        //builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("AuditLogDbContext"));
        builder.ApplyConfiguration(new AuditHistoryLogEntityTypeConfiguration());

        base.OnModelCreating(builder);
    }
}
