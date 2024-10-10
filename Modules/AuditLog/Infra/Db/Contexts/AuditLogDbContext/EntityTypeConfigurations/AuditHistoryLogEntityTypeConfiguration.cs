using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.AuditLog.Domain.AuditHistoryLogAggregate;

namespace Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext.EntityTypeConfigurations;

internal class AuditHistoryLogEntityTypeConfiguration : BaseEntityTypeConfiguration<AuditHistoryLog>
{
    public override void Configure(EntityTypeBuilder<AuditHistoryLog> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ModificationType).HasConversion<string>();
        builder.Property(x => x.OwnershipType).HasConversion<string>();
    }
}
