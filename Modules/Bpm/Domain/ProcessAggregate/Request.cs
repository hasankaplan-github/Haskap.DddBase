using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Utilities.Guids;

namespace Modules.Bpm.Domain.ProcessAggregate;

public class Request : Entity, IHasMultiTenant
{
    public Guid ProcessId { get; private set; }
    public Guid? OwnerUserId { get; private set; }
    public Guid CurrentStateId { get; private set; }
    public State CurrentState { get; private set; }

    private List<Progress> _progresses = new();
    public IReadOnlyList<Progress> Progresses => _progresses.AsReadOnly();
    public Guid? DataId { get; set; }
    public Guid? TenantId { get; set; }

    private Request()
    {
    }

    internal Request(
        Guid id,
        Guid? ownerUserId,
        State currentState,
        Guid? dataId)
        : base(id)
    {
        OwnerUserId = ownerUserId;
        SetCurrentState(currentState);
        DataId = dataId;
    }

    public Progress MakeProgress(Path path, Guid? ownerUserId, Guid? dataId)
    {
        SetCurrentState(path.ToState);

        var progress = AddProgress(path, ownerUserId, dataId);

        return progress;
    }

    private Progress AddProgress(Path path, Guid? ownerUserId, Guid? dataId)
    {
        var progress = new Progress(GuidGenerator.CreateSimpleGuid(), path, ownerUserId, dataId);
        _progresses.Add(progress);

        return progress;
    }

    public void SetCurrentState(State currentState)
    {
        Guard.Against.Null(currentState);

        CurrentState = currentState;
        CurrentStateId = currentState.Id;
    }
}