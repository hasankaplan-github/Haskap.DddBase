using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.Localization.Domain;
using Modules.Localization.Domain.SelectedLocaleAggregate;
using Modules.Localization.Domain.SupportedLocaleAggregate;

namespace Modules.Localization.Infra.Db.Contexts.LocalizationDbContext;
public class AppDbContext : BaseEfCoreNpgsqlDbContext, ILocalizationDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options, 
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterGenericProvider? globalQueryFilterGenericProvider)
        : base(
            options,
            currentTenantProvider,
            globalQueryFilterGenericProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("localization");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("LocalizationDbContext"));

        base.OnModelCreating(builder);
    }

    public DbSet<SelectedLocale> SelectedLocale { get; set; }
    public DbSet<Domain.LocalizationAggregate.Localization> Localization { get; set; }
    public DbSet<SupportedLocale> SupportedLocale { get; set; }
}
