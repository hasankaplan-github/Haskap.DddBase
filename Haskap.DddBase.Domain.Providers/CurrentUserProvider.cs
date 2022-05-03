using Haskap.DddBase.Domain.Core;

namespace Haskap.DddBase.Domain.Providers;

public class CurrentUserProvider<TUser, TUserId>
    where TUser : class, IEntity<TUserId>
{
    public TUser CurrentUser { get; set; }
}
