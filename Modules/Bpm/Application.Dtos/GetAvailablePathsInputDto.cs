namespace Modules.Bpm.Application.Dtos;

public class GetAvailablePathsInputDto
{
    public Guid ProcessId { get; set; }
    public Guid RequestId { get; set; }
    public List<Guid> UserRoleIds { get; set; } = null!;
}
