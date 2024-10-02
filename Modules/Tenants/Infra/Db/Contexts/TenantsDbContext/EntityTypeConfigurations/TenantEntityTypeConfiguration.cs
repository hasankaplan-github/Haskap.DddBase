using Haskap.DddBase.Domain.Shared.Consts;
using Haskap.DddBase.Modules.Tenants.Domain.TenantAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.Tenants.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;

public class TenantEntityTypeConfiguration : BaseEntityTypeConfiguration<Tenant>
{
    public override void Configure(EntityTypeBuilder<Tenant> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .HasMaxLength(TenantConsts.MaxNameLength);
    }
}
