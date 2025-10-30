namespace Modules.Tenants.Application.Dtos.Tenants;
public class TenantOutputDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public int TenantOrder { get; set; }
    public string? ConnectionString { get; set; }
}
