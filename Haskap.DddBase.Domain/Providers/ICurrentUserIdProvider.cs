namespace Haskap.DddBase.Domain.Providers;

public interface ICurrentUserIdProvider
{
    Guid? CurrentUserId { get; set; }

    bool IsImpersonated { get; set; }
    Guid? ImpersonatorUserId { get; set; }
    string ImpersonatorUsername { get; set; }
    string ImpersonatorTenantName { get; set; }
}
