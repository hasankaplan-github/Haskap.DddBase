using Ardalis.GuardClauses;
using Haskap.DddBase.Utilities.Guids;

namespace Modules.Bpm.Domain.ProcessAggregate;

public class Request : Entity
{
    public Guid ProcessId { get; private set; }
    public Guid? OwnerUserId { get; private set; }
    public Guid CurrentStateId { get; private set; }
    public State CurrentState { get; private set; }

    private List<Progress> _progresses = new();
    public IReadOnlyList<Progress> Progresses => _progresses.AsReadOnly();
    public Guid? DataId { get; set; }


    private Request()
    {
    }

    public Request(
        Guid id,
        Guid processId,
        Guid? ownerUserId,
        State currentState,
        Guid? dataId)
        : base(id)
    {
        ProcessId = processId;
        OwnerUserId = ownerUserId;
        SetCurrentState(currentState);
        DataId = dataId;
    }

    public Guid MakeProgress(Path path, Guid? ownerUserId, Guid? dataId)
    {
        SetCurrentState(path.ToState);

        var progressId = AddProgress(path, ownerUserId, dataId);

        return progressId;
    }

    private Guid AddProgress(Path path, Guid? ownerUserId, Guid? dataId)
    {
        var progress = new Progress(GuidGenerator.CreateSimpleGuid(), path, ownerUserId, dataId);
        _progresses.Add(progress);

        return progress.Id;
    }

    private void SetCurrentState(State currentState)
    {
        Guard.Against.Null(currentState);

        CurrentState = currentState;
        CurrentStateId = currentState.Id;
    }
}