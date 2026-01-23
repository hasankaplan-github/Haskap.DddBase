namespace Modules.Bpm.Application.Dtos;

public class InitRequestInputDto
{
    public Guid ProcessId { get; set; }
    public Guid? DataId { get; set; }
}
