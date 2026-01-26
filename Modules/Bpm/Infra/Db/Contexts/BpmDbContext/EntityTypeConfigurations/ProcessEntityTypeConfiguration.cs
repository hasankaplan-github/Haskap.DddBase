using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Bpm.Domain.ProcessAggregate;

namespace Modules.Bpm.Infra.Db.Contexts.BpmDbContext.EntityTypeConfigurations;

public class ProcessEntityTypeConfiguration : BaseEntityTypeConfiguration<Process>
{
    public override void Configure(EntityTypeBuilder<Process> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.Paths)
            .WithOne()
            .HasForeignKey(x => x.ProcessId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Requests)
            .WithOne()
            .HasForeignKey(x => x.ProcessId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.States)
            .WithOne()
            .HasForeignKey(x => x.ProcessId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
