namespace Haskap.DddBase.Domain;

public interface IHasMultiTenant
{
    Guid? TenantId { get; set; }
}
