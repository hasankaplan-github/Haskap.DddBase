using Haskap.DddBase.Domain.ViewLevelExceptionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;

public class ViewLevelExceptionEntityTypeConfiguration : BaseEntityTypeConfiguration<ViewLevelException>
{
    public override void Configure(EntityTypeBuilder<ViewLevelException> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
