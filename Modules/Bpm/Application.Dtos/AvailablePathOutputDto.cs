namespace Modules.Bpm.Application.Dtos;

public class AvailablePathOutputDto
{
    public Guid Id { get; set; }
    public Guid ProcessId { get; set; }
    public Guid FromStateId { get; set; }
    public Guid ToStateId { get; set; }
    public Guid CommandId { get; set; }
    public CommandOutputDto Command { get; set; }
    public string? ViewName { get; set; }

}
