using Haskap.DddBase.Domain.TenantAggregate;
using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Providers;
public class CurrentTenantProvider : ICurrentTenantProvider
{
    public Guid? CurrentTenantId { get; private set; } = null;

    public bool MultiTenancyIsEnabled => CurrentTenantId is not null;

    public IDisposable ChangeCurrentTenant(Guid? newCurrentTenantId)
    {
        var tempTenantId = CurrentTenantId;
        CurrentTenantId = newCurrentTenantId;
        return new DisposeAction(() =>
        {
            CurrentTenantId = tempTenantId;
        });
    }

    public IDisposable DisableMultiTenancy()
    {
        return ChangeCurrentTenant(null);
    }
}
