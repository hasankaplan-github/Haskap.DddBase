using Ardalis.GuardClauses;
using Haskap.DddBase.Application;
using Haskap.DddBase.Domain.Common;
using Microsoft.Extensions.Localization;
using Modules.Localization.Application.Contracts;
using Modules.Localization.Domain;
using Modules.Localization.Domain.LocalizationAggregate;
using System.Globalization;
using System.Linq;

namespace Modules.Localization.Application;

public class DbStringLocalizer : UseCaseService, IDbStringLocalizer
{
    private readonly ILocalizationDbContext _localizationDbContext;

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            Guard.Against.Null(name);

            var format = GetStringSafely(name, null);
            var value = string.Format(CultureInfo.CurrentCulture, format ?? name, arguments);

            return new LocalizedString(name, value, resourceNotFound: format == null);
        }
    }

    public LocalizedString this[string name]
    {
        get
        {
            Guard.Against.Null(name);

            var value = GetStringSafely(name, null);

            return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
        }
    }

    public DbStringLocalizer(ILocalizationDbContext localizationDbContext)
    {
        _localizationDbContext = localizationDbContext;
    }

    protected string? GetStringSafely(string name, Locale? locale)
    {
        Guard.Against.Null(name);

        var keyLocale = locale ?? Locale.CurrentUiLocale;

        return GetLocalizedValueFromCache(name, keyLocale) ??
            GetLocalizedValueFromDb(name, keyLocale) ??
            GetDefaultLocalizedValueFromDb(name, keyLocale);
    }

    private string? GetLocalizedValueFromCache(string key, Locale locale)
    {
        return null;
    }

    private string? GetLocalizedValueFromDb(string key, Locale locale)
    {
        return _localizationDbContext.Localization
            .Where(x => x.Locale.Value == locale.Value && x.Key == key)
            .Select(x => x.Value)
            .FirstOrDefault();
    }

    private string? GetDefaultLocalizedValueFromDb(string key, Locale locale)
    {
        if (Locale.Default is null || Locale.Default == locale)
            return null;

        return _localizationDbContext.Localization
            .Where(x => x.Locale.Value == Locale.Default.Value && x.Key == key)
            .Select(x => x.Value)
            .FirstOrDefault();
    }

    public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return GetAllStrings(includeParentCultures, Locale.CurrentUiLocale);
    }

    protected IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures, Locale locale)
    {
        Guard.Against.Null(locale);

        var locales = includeParentCultures
            ? GetLocaleHierarchy(locale)
            : [locale];

        var localeValues = locales.Select(l => l.Value).ToList();

        var localizations = (from localizationKey in _localizationDbContext.Localization.Select(x => x.Key).Distinct()
                             join localizationByKey in _localizationDbContext.Localization
                                 .Where(x => localeValues.Contains(x.Locale.Value))
                                 on localizationKey equals localizationByKey.Key into j

                             from localizationByKey in j.DefaultIfEmpty()

                             select new
                             {
                                 Key = localizationKey,
                                 Localization = localizationByKey
                             })
                 .ToList();

        foreach (var localizationByKey in localizations)
        {
            var localization = localizationByKey.Localization;
            yield return new LocalizedString(localizationByKey.Key, localization?.Value ?? localizationByKey.Key, resourceNotFound: localization is null);
        }
    }

    private IEnumerable<Locale> GetLocaleHierarchy(Locale locale)
    {
        var currentCulture = locale.CultureInfo;
        var locales = new HashSet<Locale>();

        while (true)
        {
            locales.Add(new Locale(currentCulture.Name));

            if (currentCulture == currentCulture.Parent)
            {
                // currentCulture begat currentCulture, probably time to leave
                break;
            }

            currentCulture = currentCulture.Parent;
        }

        return locales;
    }
}
