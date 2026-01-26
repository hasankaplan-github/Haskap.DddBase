using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Bpm.Infra.Db.Contexts.BpmDbContext.EntityTypeConfigurations;

public class ProgressEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.ProcessAggregate.Progress>
{
    public override void Configure(EntityTypeBuilder<Domain.ProcessAggregate.Progress> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Path)
            .WithMany()
            .HasForeignKey(x => x.PathId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
