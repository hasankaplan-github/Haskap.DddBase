using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Haskap.DddBase.Modules.ViewLevelExceptions.Domain;
using Haskap.DddBase.Modules.ViewLevelExceptions.Domain.ViewLevelExceptionAggregate;

namespace Haskap.DddBase.Modules.ViewLevelExceptions.Infra.Db.Contexts.ViewLevelExceptionsDbContext;

public class AppDbContext : BaseEfCoreNpgsqlDbContext, IViewLevelExceptionsDbContext
{
    protected AppDbContext(
        DbContextOptions options, 
        IGlobalQueryFilterGenericProvider? globalQueryFilterGenericProvider)
        : base(
            options, 
            currentTenantProvider: null,
            globalQueryFilterGenericProvider)
    {
    }

    public DbSet<ViewLevelException> ViewLevelException { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("ViewLevelExceptionsDbContext"));

        base.OnModelCreating(builder);
    }
}
