using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.RoleAggregate;
using Haskap.DddBase.Domain.Shared.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;

public class RoleEntityTypeConfiguration : BaseEntityTypeConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(RoleConsts.MaxNameLength);

        builder.OwnsMany(x => x.Permissions, x =>
        {
            x.WithOwner().HasForeignKey("RoleId");
            x.HasKey(x => x.Id);
        });
    }
}
