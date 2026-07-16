using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Bpm.Infra.Db.Contexts.BpmDbContext.EntityTypeConfigurations;

public class PathEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.ProcessAggregate.Path>
{
    public override void Configure(EntityTypeBuilder<Domain.ProcessAggregate.Path> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.FromState)
            .WithMany()
            .HasForeignKey(x => x.FromStateId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ToState)
           .WithMany()
           .HasForeignKey(x => x.ToStateId)
           .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Command)
           .WithMany()
           .HasForeignKey(x => x.CommandId)
           .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Roles)
            .WithOne()
            .HasForeignKey(x => x.PathId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
