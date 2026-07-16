using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;

namespace Modules.Localization.Infra.Db.Contexts.LocalizationDbContext.EntityTypeConfigurations;

public class LocalizationEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.LocalizationAggregate.Localization>
{
    public override void Configure(EntityTypeBuilder<Domain.LocalizationAggregate.Localization> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.Locale, x =>
        {
            x.HasIndex(l => l.Value);
        });

        builder.HasIndex(x => x.Key);
    }
}
