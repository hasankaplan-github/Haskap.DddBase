using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Common;

namespace Modules.Localization.Domain.SelectedLocaleAggregate;

public class SelectedLocale : AggregateRoot
{
    public Locale Locale { get; private set; }
    public Guid UserId { get; private set; }

    private SelectedLocale()
    { }

    public SelectedLocale(Guid id, Locale locale, Guid userId)
        : base(id)
    {
        Guard.Against.Null(locale, nameof(locale));

        Locale = locale;
        UserId = userId;
    }
}
