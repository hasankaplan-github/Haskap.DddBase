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
    public Guid? PreviousUserUserId { get; set; } = null;
    public bool IsImpersonated {  get; set; }
    public string PreviousUserUsername { get; set; } = string.Empty;
    public string PreviousUserTenantName { get; set; } = string.Empty;
}
