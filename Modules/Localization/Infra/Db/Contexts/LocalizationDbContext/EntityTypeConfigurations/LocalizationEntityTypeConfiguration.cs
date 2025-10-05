using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Localization.Infra.Db.Contexts.LocalizationDbContext.EntityTypeConfigurations;

public class LocalizationEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.LocalizationAggregate.Localization>
{
    public override void Configure(EntityTypeBuilder<Domain.LocalizationAggregate.Localization> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.Locale);

        builder.HasIndex(x => new { x.Locale.Value, x.Key });
    }
}
