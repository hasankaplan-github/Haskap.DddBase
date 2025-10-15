using Ardalis.GuardClauses;
using Haskap.DddBase.Application;
using Haskap.DddBase.Domain.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Modules.Localization.Application.Contracts;
using Modules.Localization.Domain;
using System.Globalization;

namespace Modules.Localization.Application;

public class DbStringLocalizer : UseCaseService, IDbStringLocalizer
{
    private readonly ILocalizationDbContext _localizationDbContext;
    private readonly IMemoryCache _memoryCache;
    private readonly ILocalizationModule _localizationModule;

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            Guard.Against.Null(name);

            var localizedValue = GetLocalizedValueFromCache(name, Locale.CurrentUiLocale);

            if (localizedValue is not null)
            {
                var formattedValue = string.Format(CultureInfo.CurrentCulture, localizedValue.Value, arguments);
                return new LocalizedString(name, formattedValue, localizedValue.ResourceNotFound, searchedLocation: localizedValue.SearchedLocation);
            }

            var format = GetStringSafely(name, null);
            var value = string.Format(CultureInfo.CurrentCulture, format ?? name, arguments);

            return new LocalizedString(name, value, resourceNotFound: format == null, searchedLocation: "Database");
        }
    }

    public LocalizedString this[string name]
    {
        get
        {
            Guard.Against.Null(name);

            var localizedValue = GetLocalizedValueFromCache(name, Locale.CurrentUiLocale);

            if (localizedValue is not null)
            {
                return localizedValue;
            }

            var value = GetStringSafely(name, null);

            return new LocalizedString(name, value ?? name, resourceNotFound: value == null, searchedLocation: "Database");
        }
    }

    public DbStringLocalizer(
        ILocalizationDbContext localizationDbContext,
        IMemoryCache memoryCache,
        ILocalizationModule localizationModule)
    {
        _localizationDbContext = localizationDbContext;
        _memoryCache = memoryCache;
        _localizationModule = localizationModule;
    }

    protected string? GetStringSafely(string name, Locale? locale)
    {
        Guard.Against.Null(name);

        var keyLocale = locale ?? Locale.CurrentUiLocale;

        if (!_localizationModule.IsEnabledAsync().GetAwaiter().GetResult())
        {
            if (Locale.Default is null)
            {
                return null;
            }
            else
            {
                keyLocale = Locale.Default;
            }
        }

        return GetLocalizedValueFromDb(name, keyLocale) ??
            GetDefaultLocalizedValueFromDb(name, keyLocale);
    }

    private LocalizedString? GetLocalizedValueFromCache(string key, Locale locale)
    {
        if(!_memoryCache.TryGetValue(locale, out Dictionary<string, LocalizedString>? localizations))
        {
            return null;
        }

        if(!localizations!.TryGetValue(key, out var localizedString))
        {
            return null;
        }

        return localizedString;
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

        if (!_localizationModule.IsEnabledAsync().GetAwaiter().GetResult())
        {
            if (Locale.Default is null)
            {
                yield break;
            }
            else
            {
                locale = Locale.Default;
            }
        }

        var locales = includeParentCultures
            ? GetLocaleHierarchy(locale)
            : [locale];

        var localeValues = locales.Select(l => l.Value).ToList();

        var shouldIncludeDefaultLocale = Locale.Default != null && !localeValues.Contains(Locale.Default.Value);

        var localizations = (from localizationKey in _localizationDbContext.Localization.Select(x => x.Key).Distinct()
                             
                             join localizationByKey in _localizationDbContext.Localization
                                 .Where(x => localeValues.Contains(x.Locale.Value))
                                 on localizationKey equals localizationByKey.Key into j
                             from localizationByKey in j.DefaultIfEmpty()

                             join defaultLocalizationByKey in _localizationDbContext.Localization
                                 .Where(x => shouldIncludeDefaultLocale && x.Locale.Value == Locale.Default.Value)
                                 on localizationKey equals defaultLocalizationByKey.Key into dj
                             from defaultLocalizationByKey in dj.DefaultIfEmpty()

                             select new
                             {
                                 Key = localizationKey,
                                 Localization = localizationByKey ?? defaultLocalizationByKey
                             })
                 .ToList();

        foreach (var localizationByKey in localizations)
        {
            var localization = localizationByKey.Localization;
            yield return new LocalizedString(localizationByKey.Key, localization?.Value ?? localizationByKey.Key, resourceNotFound: localization is null, searchedLocation: "Database");
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
