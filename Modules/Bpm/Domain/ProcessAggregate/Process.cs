using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Utilities.Guids;

namespace Modules.Bpm.Domain.ProcessAggregate;

public class Process : AggregateRoot, IHasMultiTenant
{
    public string Name { get; private set; }
    public string? Description { get; set; }


    private List<Domain.ProcessAggregate.Path> _paths = new();
    public IReadOnlyList<Domain.ProcessAggregate.Path> Paths => _paths.AsReadOnly();

    private List<Request> _requests = new();
    public IReadOnlyList<Request> Requests => _requests.AsReadOnly();

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

    public IReadOnlyList<Path> GetAvailablePathsOfRequest(Guid requestId, List<Guid> userRoleIds)
    {
        var request = _requests.Where(x => x.Id == requestId).First();

        var availablePaths = _paths
            .Where(x => x.FromStateId == request.CurrentStateId &&
                (x.Roles.Any(role => role.RoleId == null) ||
                x.Roles.Any(y => userRoleIds.Contains(y.RoleId!.Value))))
            .ToList()
            .AsReadOnly();

        return availablePaths;
    }

    public Guid MakeProgress(Guid requestId, Guid commandId, List<Guid> userRoleIds, Guid? ownerUserId, Guid? dataId, CancellationToken cancellationToken)
    {
        var request = _requests
            .Where(x => x.Id == requestId)
            .First();

        var path = _paths
            .Where(x => x.FromStateId == request.CurrentStateId &&
                x.CommandId == commandId &&
                (x.Roles.Any(role => role.RoleId == null) ||
                x.Roles.Any(y => userRoleIds.Contains(y.RoleId!.Value))))
            .First();

        var progressId = request.MakeProgress(path, ownerUserId, dataId);

        return progressId;
    }
}