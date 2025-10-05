using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Localization.Domain.SelectedLocaleAggregate;

namespace Modules.Localization.Infra.Db.Contexts.LocalizationDbContext.EntityTypeConfigurations;

public class SelectedLocaleEntityTypeConfiguration : BaseEntityTypeConfiguration<SelectedLocale>
{
    public override void Configure(EntityTypeBuilder<SelectedLocale> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.Locale);

        builder.HasIndex(x => x.UserId);
    }
}
