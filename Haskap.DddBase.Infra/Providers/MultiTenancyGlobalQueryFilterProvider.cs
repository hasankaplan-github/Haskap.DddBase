using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class MultiTenancyGlobalQueryFilterProvider : IMultiTenancyGlobalQueryFilterProvider
{
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public bool IsEnabled => _currentTenantProvider?.CurrentTenantId is not null;

    public MultiTenancyGlobalQueryFilterProvider(ICurrentTenantProvider currentTenantProvider)
    {
        _currentTenantProvider = currentTenantProvider;
    }

    public IDisposable Disable()
    {
        return _currentTenantProvider.ChangeCurrentTenant(null);
    }

    public IDisposable Enable()
    {
        throw new NotSupportedException();
    }
}
