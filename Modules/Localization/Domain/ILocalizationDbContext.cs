using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Localization.Domain.SelectedCultureAggregate;

namespace Modules.Localization.Domain;
public interface ILocalizationDbContext : IUnitOfWork
{
    DbSet<SelectedCulture> SelectedCulture { get; set; }
}
