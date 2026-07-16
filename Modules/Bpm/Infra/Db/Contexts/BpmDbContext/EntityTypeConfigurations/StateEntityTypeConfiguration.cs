using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Bpm.Domain.ProcessAggregate;
using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;

namespace Modules.Bpm.Infra.Db.Contexts.BpmDbContext.EntityTypeConfigurations;

public class StateEntityTypeConfiguration : BaseEntityTypeConfiguration<State>
{
    public override void Configure(EntityTypeBuilder<State> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.StateType)
            .HasConversion<string>();
    }
}
