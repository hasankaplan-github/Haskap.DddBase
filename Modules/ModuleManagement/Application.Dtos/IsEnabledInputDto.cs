namespace Modules.ModuleManagement.Application.Dtos;
public class IsEnabledInputDto
{
    public Guid? TenantId { get; set; }
    public string ModuleName { get; set; }
}
