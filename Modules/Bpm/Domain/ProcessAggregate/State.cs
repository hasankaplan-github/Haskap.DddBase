using Ardalis.GuardClauses;
using Modules.Bpm.Domain.Shared.Enums;

namespace Modules.Bpm.Domain.ProcessAggregate;

public class State : Entity
{
    public Guid ProcessId { get; private set; }
    public string DisplayName { get; private set; }
    public StateType StateType { get; set; }

    private State()
    {
    }

    public State(Guid id, Guid processId, string displayName, StateType stateType)
        : base(id)
    {
        ProcessId = processId;
        SetDisplayName(displayName);
        StateType = stateType;
    }

    public void SetDisplayName(string displayName)
    {
        Guard.Against.NullOrWhiteSpace(displayName);

        DisplayName = displayName;
    }
}