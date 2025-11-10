using Ardalis.GuardClauses;
using Haskap.DddBase.Application.Contracts.Localization;
using Haskap.DddBase.Domain.Shared;
using Microsoft.Extensions.Localization;
using Modules.Localization.Application.Contracts;
using System.Globalization;

namespace Modules.Localization.Application;

public class CommonStringLocalizer : ICommonStringLocalizer
{
    private readonly IStringLocalizerFactory _stringLocalizerFactory;
    private readonly List<IStringLocalizer> _localizers = new();

    public CommonStringLocalizer(
        IStringLocalizerFactory stringLocalizerFactory,
        IDbStringLocalizer dbStringLocalizer)
    {
        _stringLocalizerFactory = stringLocalizerFactory;
        foreach (var resourceType in AppConfig.LocalizationResourceTypes)
        {
            var stringLocalizer = _stringLocalizerFactory.Create(resourceType);
            if (stringLocalizer is not null)
            {
                _localizers.Add(stringLocalizer);
            }
        }

        _localizers.Add(dbStringLocalizer);
    }

    public LocalizedString this[string name]
    {
        get
        {
            Guard.Against.Null(name);

            var value = GetStringSafely(name, out var searchedLocations);

            return new LocalizedString(name, value ?? name, resourceNotFound: value == null, searchedLocations);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            Guard.Against.Null(name);

            var format = GetStringSafely(name, out var searchedLocations);
            var value = string.Format(CultureInfo.CurrentCulture, format ?? name, arguments);

            return new LocalizedString(name, value, resourceNotFound: format == null, searchedLocations);
        }
    }

    protected string? GetStringSafely(string name, out string? searchedLocations)
    {
        searchedLocations = null;

        if (!_localizers.Any())
        {
            return null;
        }

        foreach (var localizer in _localizers)
        {           
            var localizedString = localizer[name];
            searchedLocations += (searchedLocations is null ? "" : ", ") + (localizedString.SearchedLocation ?? "Unknown");
            if (!localizedString.ResourceNotFound)
            {
                return localizedString.Value;
            }
        }

        return null;
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        if (!_localizers.Any())
        {
            return [];
        }

        var allStrings = new List<LocalizedString>();
        foreach (var localizer in _localizers)
        {
            allStrings.AddRange(localizer.GetAllStrings(includeParentCultures));
        }

        return allStrings;
    }
}
