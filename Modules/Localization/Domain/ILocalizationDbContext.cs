using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Localization.Domain.SelectedLocaleAggregate;
using Modules.Localization.Domain.SupportedLocaleAggregate;

namespace Modules.Localization.Domain;

public interface ILocalizationDbContext : IUnitOfWork
{
    DbSet<SelectedLocale> SelectedLocale { get; set; }
    DbSet<LocalizationAggregate.Localization> Localization { get; set; }
    DbSet<SupportedLocale> SupportedLocale { get; set; }
}
