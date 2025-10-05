using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Localization.Domain.SupportedLocaleAggregate;

namespace Modules.Localization.Infra.Db.Contexts.LocalizationDbContext.EntityTypeConfigurations;

public class SupportedLocaleEntityTypeConfiguration : BaseEntityTypeConfiguration<SupportedLocale>
{
    public override void Configure(EntityTypeBuilder<SupportedLocale> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.Locale);
    }
}
