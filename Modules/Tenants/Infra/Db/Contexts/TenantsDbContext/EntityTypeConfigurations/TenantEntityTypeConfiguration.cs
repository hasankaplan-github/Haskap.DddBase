using Haskap.DddBase.Domain.Shared.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Tenants.Domain.TenantAggregate;
using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;

namespace Modules.Tenants.Infra.Db.Contexts.TenantsDbContext.EntityTypeConfigurations;

public class TenantEntityTypeConfiguration : BaseEntityTypeConfiguration<Tenant>
{
    public override void Configure(EntityTypeBuilder<Tenant> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .HasMaxLength(TenantConsts.MaxNameLength);

        builder.Property(x => x.TenantOrder).UseIdentityColumn();
    }
}
