using Haskap.DddBase.Domain.Core;

namespace Haskap.DddBase.Domain.Providers;

public class CurrentUserProvider<TUserId>
{
    public TUserId CurrentUserId { get; set; }
}
