namespace Modules.ModuleManagement.Application.Dtos;
public class UpdateEnabledModulesInputDto
{
    public Guid? TenantId { get; set; }
    public List<string>? CheckedModuleNames { get; set; }
    public List<string>? UncheckedModuleNames { get; set; }
}
