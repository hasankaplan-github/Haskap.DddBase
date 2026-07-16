using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Bpm.Domain.ProcessAggregate;

namespace Modules.Bpm.Infra.Db.Contexts.BpmDbContext.EntityTypeConfigurations;

public class RequestEntityTypeConfiguration : BaseEntityTypeConfiguration<Request>
{
    public override void Configure(EntityTypeBuilder<Request> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.CurrentState)
            .WithMany()
            .HasForeignKey(x => x.CurrentStateId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Progresses)
            .WithOne()
            .HasForeignKey(x => x.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
