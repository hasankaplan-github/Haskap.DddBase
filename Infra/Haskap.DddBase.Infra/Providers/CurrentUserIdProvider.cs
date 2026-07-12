using Haskap.DddBase.Domain.Providers;

namespace Haskap.DddBase.Infra.Providers;
public class CurrentUserIdProvider : ICurrentUserIdProvider
{
    public Guid? CurrentUserId { get; set; } = null;
    public bool IsImpersonated { get; set; }


    public Guid? ImpersonatorUserId { get; set; } = null;
    public string ImpersonatorUsername { get; set; } = string.Empty;
    public string ImpersonatorTenantName { get; set; } = string.Empty;


    public Guid? CurrentLoginId { get; set; } = null;
}
