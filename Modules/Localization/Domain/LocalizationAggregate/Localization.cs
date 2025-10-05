using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Common;

namespace Modules.Localization.Domain.LocalizationAggregate;

public class Localization : AggregateRoot
{
    public string Key { get; private set; }
    public Locale Locale { get; private set; }
    public string Value { get; private set; }

    private Localization()
    { }

    public Localization(Guid id, string key, Locale locale, string value)
        : base(id)
    {
        Guard.Against.NullOrEmpty(key, nameof(key));
        Guard.Against.Null(locale, nameof(locale));

        Key = key;
        Locale = locale;
        Value = value;
    }

    public void UpdateValue(string value)
    {
        Value = value;
    }

    public void UpdateLocale(Locale locale)
    {
        Guard.Against.Null(locale, nameof(locale));

        Locale = locale;
    }
}
