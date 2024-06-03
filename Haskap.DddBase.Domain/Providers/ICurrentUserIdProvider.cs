namespace Haskap.DddBase.Domain.Providers;

public interface ICurrentUserIdProvider
{
    Guid? CurrentUserId { get; set; }
}
