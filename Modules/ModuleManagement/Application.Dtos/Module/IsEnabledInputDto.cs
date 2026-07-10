namespace Modules.ModuleManagement.Application.Dtos.Module;
public class IsEnabledInputDto
{
    public Guid? TenantId { get; set; }
    public string ModuleName { get; set; }
}
