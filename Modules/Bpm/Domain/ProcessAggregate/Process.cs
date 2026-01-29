using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Utilities.Guids;
using Modules.Bpm.Domain.Shared.Enums;

namespace Modules.Bpm.Domain.ProcessAggregate;

public class Process : AggregateRoot, IHasMultiTenant
{
    public string Name { get; private set; }
    public string? Description { get; set; }

    public IReadOnlyList<State> States => _states.AsReadOnly();
    private List<State> _states = new();

    public IReadOnlyList<Domain.ProcessAggregate.Path> Paths => _paths.AsReadOnly();
    private List<Domain.ProcessAggregate.Path> _paths = new();

    public IReadOnlyList<Request> Requests => _requests.AsReadOnly();
    private List<Request> _requests = new();
    

    public Guid? TenantId { get; set; }

    private Process()
    {
    }

    public Process(Guid id, string name, string? description)
        : base(id)
    {
        SetName(name);
        Description = description;
    }

    public void SetName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
    }

    public void AddPath(State fromState, State toState, Command command, string? viewName)
    {
        var path = new Path(GuidGenerator.CreateSimpleGuid(), fromState, toState, command, viewName);
        _paths.Add(path);
    }

    public IReadOnlyList<Path> GetAvailablePathsOfRequest(Guid? requestId, List<Guid> userRoleIds)
    {
        var currentStateId = _states
            .Where(x => x.StateType == StateType.StartState)
            .Select(x => x.Id)
            .First();

        if (requestId is not null)
        {
            currentStateId = _requests
                .Where(x => x.Id == requestId)
                .Select(x => x.CurrentStateId)
                .First();
        }

        var availablePaths = _paths
            .Where(x => x.FromStateId == currentStateId &&
                (x.Roles.Any(role => role.RoleId == null) ||
                x.Roles.Any(y => userRoleIds.Contains(y.RoleId!.Value))))
            .ToList()
            .AsReadOnly();

        return availablePaths;
    }

    public Progress MakeProgress(Guid? requestId, Guid commandId, List<Guid> userRoleIds, Guid? ownerUserId, Guid? requestDataId, Guid? progressDataId)
    {
        Request request;

        if (requestId is null)
        {
            request = InitRequest(ownerUserId, requestDataId);
        }
        else
        {
            request = _requests
                .Where(x => x.Id == requestId)
                .First();
        }

        var path = GetProgressPath(request.Id, commandId, userRoleIds);

        var progress = request.MakeProgress(path, ownerUserId, progressDataId);

        return progress;
    }

    public Path GetProgressPath(Guid? requestId, Guid commandId, List<Guid> userRoleIds)
    {
        var currentState = _states
            .Where(x => x.StateType == StateType.StartState)
            .First();

        if (requestId is not null)
        {
            var request = _requests
                .Where(x => x.Id == requestId)
                .First();

            currentState = request.CurrentState;
        }

        var path = _paths
            .Where(x => 
                x.FromStateId == currentState.Id &&
                x.CommandId == commandId &&
                (x.Roles.Any(role => role.RoleId == null) ||
                x.Roles.Any(y => userRoleIds.Contains(y.RoleId!.Value))))
            .First();

        return path;
    }

    public Request InitRequest(Guid? ownerUserId, Guid? dataId)
    {
        var startState = _states.First(x => x.StateType == StateType.StartState);

        var newRequest = new Request(GuidGenerator.CreateSimpleGuid(), ownerUserId, startState!, dataId);

        _requests.Add(newRequest);

        return newRequest;
    }
}