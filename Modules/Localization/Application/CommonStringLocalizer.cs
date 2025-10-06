using Ardalis.GuardClauses;
using Microsoft.Extensions.Localization;
using Modules.Localization.Application.Contracts;
using System.Globalization;

namespace Modules.Localization.Application;
public class CommonStringLocalizer : ICommonStringLocalizer
{
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

    public IList<IStringLocalizer>? Localizers { get; private set; } = null;

    protected string? GetStringSafely(string name, out string? searchedLocations)
    {
        searchedLocations = null;

        if (Localizers == null || !Localizers.Any())
        {
            return null;
        }

        foreach (var localizer in Localizers)
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
        if (Localizers == null || !Localizers.Any())
        {
            return [];
        }

        var allStrings = new List<LocalizedString>();
        foreach (var localizer in Localizers)
        {
            allStrings.AddRange(localizer.GetAllStrings(includeParentCultures));
        }

        return allStrings;
    }

    public void SetStringLocalizers(params IList<IStringLocalizer> localizers)
    {
        Guard.Against.Null(localizers);

        Localizers = localizers;
    }
}
