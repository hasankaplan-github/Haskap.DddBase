namespace Modules.Tenants.Application.Dtos;

public class UpdateInputDto
{
    public Guid TenantId { get; set; }
    public string NewName { get; set; }
    public string? NewConnectionString { get; set; }
}