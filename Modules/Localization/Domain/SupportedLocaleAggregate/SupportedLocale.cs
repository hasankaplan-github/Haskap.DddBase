using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Common;

namespace Modules.Localization.Domain.SupportedLocaleAggregate;
public class SupportedLocale : AggregateRoot
{
    public Locale Locale { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDefault { get; private set; }

    private SupportedLocale()
    {}

    public SupportedLocale(Guid id, Locale locale, bool isActive, bool isDefault)
        : base(id)
    {
        Guard.Against.Null(locale, nameof(locale));

        Locale = locale;
        IsActive = isActive;
        IsDefault = isDefault;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void SetAsDefault()
    {
        IsDefault = true;
        Locale.Default = Locale;
    }

    public void UnsetAsDefault()
    {
        IsDefault = false;
    }
}
