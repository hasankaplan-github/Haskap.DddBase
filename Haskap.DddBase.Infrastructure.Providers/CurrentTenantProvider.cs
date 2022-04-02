using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infrastructure.Providers;

public class CurrentTenantProvider
{
    public bool CurrentTenantExists 
    { 
        get 
        { 
            return CurrentTenantId != null;
        } 
    }
    public Guid? CurrentTenantId { get; private set; }

    public IDisposable ChangeCurrentTenant(Guid? tenantId)
    {
        var tempTenantId = CurrentTenantId;
        CurrentTenantId = tenantId;
        return new DisposeAction(() =>
        {
            CurrentTenantId = tempTenantId;
        });
    }
}
