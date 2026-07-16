using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.ModuleManagement.Domain.ModuleAggregate;
using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;

namespace Modules.ModuleManagement.Infra.Db.Contexts.ModuleManagementDbContext.EntityTypeConfigurations;

public class EnabledModuleEntityTypeConfiguration : BaseEntityTypeConfiguration<EnabledModule>
{
    public override void Configure(EntityTypeBuilder<EnabledModule> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.TenantId);
    }
}
