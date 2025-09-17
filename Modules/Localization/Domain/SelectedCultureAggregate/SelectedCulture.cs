using Ardalis.GuardClauses;

namespace Modules.Localization.Domain.SelectedCultureAggregate;

public class SelectedCulture : AggregateRoot
{
    public string Name { get; private set; }
    public Guid UserId { get; private set; }

    private SelectedCulture()
    { }

    public SelectedCulture(Guid id, string name, Guid userId)
        : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        Name = name;
        UserId = userId;
    }
}
