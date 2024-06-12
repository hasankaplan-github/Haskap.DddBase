namespace Haskap.DddBase.Domain.Providers;

public interface ICurrentUserIdProvider
{
    Guid? CurrentUserId { get; set; }
    Guid? PreviousUserUserId { get; set; }
    bool IsImpersonated { get; set; }
    string PreviousUserUsername { get; set; }
    string PreviousUserTenantName { get; set; }
}
