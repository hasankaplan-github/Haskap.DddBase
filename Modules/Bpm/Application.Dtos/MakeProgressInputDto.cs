namespace Modules.Bpm.Application.Dtos;

public class MakeProgressInputDto
{
    public Guid ProcessId { get; set; }
    public Guid RequestId { get; set; }
    public Guid CommandId { get; set; }
    public List<Guid> UserRoleIds { get; set; }
    public Guid? DataId { get; set; }
}
