using Haskap.DddBase.Domain;

namespace Modules.Bpm.Domain.ProcessAggregate;

public class PathRole : Entity
{
    private PathRole() { }

    public PathRole(Guid id)
        : base(id) { }

    public Guid PathId { get; set; }
    public Guid? RoleId { get; set; }
}