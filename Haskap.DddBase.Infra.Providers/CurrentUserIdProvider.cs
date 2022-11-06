using Haskap.DddBase.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class CurrentUserIdProvider<TUserId> : ICurrentUserIdProvider<TUserId>
{
    public TUserId CurrentUserId { get; set; } = default(TUserId);
}
