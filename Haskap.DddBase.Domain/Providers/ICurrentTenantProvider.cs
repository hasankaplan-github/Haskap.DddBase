using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Providers;
public interface ICurrentTenantProvider
{
    // https://github.com/hikalkan/presentations/blob/master/2018-04-06-Multi-Tenancy/src/MultiTenancyDraft/Infrastructure/MultiTenancyMiddleware.cs
    Guid? CurrentTenantId { get; }
    
    bool IsHost { get; }

    IDisposable ChangeCurrentTenant(Guid? newCurrentTenantId);
    
}
