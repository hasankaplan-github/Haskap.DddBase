using Haskap.DddBase.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class CurrentUserIdProvider : ICurrentUserIdProvider
{
    public Guid? CurrentUserId { get; set; } = null;
    public bool IsImpersonated { get; set; }


    public Guid? ImpersonatorUserId { get; set; } = null;
    public string ImpersonatorUsername { get; set; } = string.Empty;
    public string ImpersonatorTenantName { get; set; } = string.Empty;
}
