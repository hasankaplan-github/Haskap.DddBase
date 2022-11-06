using Haskap.DddBase.Domain.Core;

namespace Haskap.DddBase.Domain.Providers;

public interface ICurrentUserIdProvider<TUserId>
{
    TUserId CurrentUserId { get; set; }
}
