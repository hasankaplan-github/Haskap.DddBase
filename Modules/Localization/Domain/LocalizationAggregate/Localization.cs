using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Domain;

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
        Guard.Against.NullOrWhiteSpace(key, nameof(key));
        Guard.Against.Null(locale, nameof(locale));

        Key = key;
        Locale = locale;
        Value = value;
    }

    public void UpdateValue(string value)
    {
        Value = value;
    }

    public void UpdateKey(string key)
    {
        Guard.Against.NullOrWhiteSpace(key, nameof(key));

        Key = key;
    }
}
