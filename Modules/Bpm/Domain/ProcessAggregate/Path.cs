using Ardalis.GuardClauses;
using Haskap.DddBase.Utilities.Guids;

namespace Modules.Bpm.Domain.ProcessAggregate;

public class Path : Entity
{
    public Guid ProcessId { get; private set; }

    public Guid FromStateId { get; private set; }
    public State FromState { get; private set; }

    public Guid ToStateId { get; private set; }
    public State ToState { get; private set; }

    public Guid CommandId { get; private set; }
    public Command Command { get; private set; }

    public string? ViewName { get; set; }


    private List<PathRole> _roles = new();
    public IReadOnlyList<PathRole> Roles => _roles.AsReadOnly();

    private Path()
    {
    }

    internal Path(Guid id, State fromState, State toState, Command command, string? viewName)
        : base(id)
    {
        SetCommand(command);
        SetFromState(fromState);
        SetToState(toState);
        ViewName = viewName;
    }

    public void AddRole(Guid? roleId)
    {
        _roles.Add(new PathRole(GuidGenerator.CreateSimpleGuid()) { RoleId = roleId });
    }

    public void RemoveRole(Guid? roleId)
    {
        _roles.RemoveAll(x => x.RoleId == roleId);
    }

    public void SetFromState(State fromState)
    {
        Guard.Against.Null(fromState);

        FromStateId = fromState.Id;
        FromState = fromState;
    }

    public void SetToState(State toState)
    {
        Guard.Against.Null(toState);

        ToStateId = toState.Id;
        ToState = toState;
    }

    public void SetCommand(Command command)
    {
        Guard.Against.Null(command);

        CommandId = command.Id;
        Command = command;
    }
}
