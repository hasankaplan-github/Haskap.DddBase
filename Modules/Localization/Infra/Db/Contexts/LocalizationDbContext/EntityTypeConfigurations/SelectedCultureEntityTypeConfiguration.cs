using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Localization.Domain.SelectedCultureAggregate;

namespace Modules.Localization.Infra.Db.Contexts.LocalizationDbContext.EntityTypeConfigurations;

public class SelectedCultureEntityTypeConfiguration : BaseEntityTypeConfiguration<SelectedCulture>
{
    public override void Configure(EntityTypeBuilder<SelectedCulture> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.UserId);
    }
}
