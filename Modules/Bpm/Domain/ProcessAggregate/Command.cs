using Ardalis.GuardClauses;

namespace Modules.Bpm.Domain.ProcessAggregate;

public class Command : Entity
{
    public Guid ProcessId { get; private set; }
    public string DisplayName { get; private set; }

    private Command()
    {
    }

    public Command(Guid id, Guid processId, string displayName)
        : base(id)
    {
        ProcessId = processId;
        SetDisplayName(displayName);
    }

    public void SetDisplayName(string displayName)
    {
        Guard.Against.NullOrWhiteSpace(displayName);

        DisplayName = displayName;
    }
}
