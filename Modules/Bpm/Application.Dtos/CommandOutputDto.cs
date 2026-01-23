namespace Modules.Bpm.Application.Dtos;

public class CommandOutputDto
{
    public Guid Id { get; set; }
    public Guid ProcessId { get; set; }
    public string DisplayName { get; set; }
}