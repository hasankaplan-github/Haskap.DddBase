using Haskap.DddBase.Application;
using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Modules.Localization.Application.Contracts;
using Modules.Localization.Application.Dtos;
using Modules.Localization.Domain;

namespace Modules.Localization.Application;

public class Localizer : UseCaseService, ILocalizer
{
    private readonly ILocalizationDbContext _localizationDbContext;

    public Localizer(ILocalizationDbContext localizationDbContext)
    {
        _localizationDbContext = localizationDbContext;
    }

    public string GetString(string key)
    {
        return GetLocalizedValueFromCache(key) ?? GetLocalizedValueFromDb(key) ?? GetDefaultLocalizedValueFromDb(key) ?? key;
    }

    private string? GetLocalizedValueFromCache(string key)
    {
        return null;
    }

    private string? GetLocalizedValueFromDb(string key)
    {
        return _localizationDbContext.Localization
            .Where(x => x.Locale.Value == Locale.CurrentUiLocale.Value && x.Key == key)
            .Select(x => x.Value)
            .FirstOrDefault();
    }

    private string? GetDefaultLocalizedValueFromDb(string key)
    {
        if (Locale.Default is null || Locale.Default == Locale.CurrentUiLocale)
            return null;

        return _localizationDbContext.Localization
            .Where(x => x.Locale.Value == Locale.Default.Value && x.Key == key)
            .Select(x => x.Value)
            .FirstOrDefault();
    }
}
