using Haskap.DddBase.Domain.Core;

namespace Haskap.DddBase.Domain.Providers;

public class CurrentUserIdProvider<TUserId>
{
    public TUserId CurrentUserId { get; set; }
}
