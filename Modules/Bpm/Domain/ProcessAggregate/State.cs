using Ardalis.GuardClauses;
using Modules.Bpm.Domain.Shared.Enums;
using Haskap.DddBase.Domain;

namespace Modules.Bpm.Domain.ProcessAggregate;

public class State : Entity
{
    public Guid ProcessId { get; private set; }
    public string DisplayName { get; private set; }
    public StateType StateType { get; set; }
    public string? ViewName { get; set; }

    private State()
    {
    }

    public State(Guid id, Guid processId, string displayName, StateType stateType, string? viewName)
        : base(id)
    {
        ProcessId = processId;
        SetDisplayName(displayName);
        StateType = stateType;
        ViewName = viewName;
    }

    public void SetDisplayName(string displayName)
    {
        Guard.Against.NullOrWhiteSpace(displayName);

        DisplayName = displayName;
    }
}