using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class CurrentTenantProvider : ICurrentTenantProvider
{
    public Guid? CurrentTenantId { get; private set; } = null;

    public bool IsHost => CurrentTenantId is null;

    public IDisposable ChangeCurrentTenant(Guid? newCurrentTenantId)
    {
        var tempTenantId = CurrentTenantId;
        CurrentTenantId = newCurrentTenantId;
        return new DisposeAction(() =>
        {
            CurrentTenantId = tempTenantId;
        });
    }
}
