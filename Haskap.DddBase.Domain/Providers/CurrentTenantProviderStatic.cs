using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Providers;
public class CurrentTenantProviderStatic
{
    // https://github.com/hikalkan/presentations/blob/master/2018-04-06-Multi-Tenancy/src/MultiTenancyDraft/Infrastructure/MultiTenancyMiddleware.cs
    public static Guid? CurrentTenantId
    {
        get
        {
            return _currentTenantId.Value;
        }
        private set
        {
            _currentTenantId.Value = value;
        }
    }
    private static readonly AsyncLocal<Guid?> _currentTenantId = new AsyncLocal<Guid?>();

    public static IDisposable ChangeCurrentTenant(Guid? newCurrentTenantId)
    {
        var tempTenantId = CurrentTenantId;
        CurrentTenantId = newCurrentTenantId;
        return new DisposeAction(() =>
        {
            CurrentTenantId = tempTenantId;
        });
    }
}
