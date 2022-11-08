using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class EfCoreGlobalQueryFilterParameterStatusProvider : IEfCoreGlobalQueryFilterParameterStatusProvider
{
    public bool MultiTenancyFilterIsEnabled => _currentTenantProvider.CurrentTenantId is not null;

    public bool SoftDeleteFilterIsEnabled { get; private set; } = true;

    private readonly ICurrentTenantProvider _currentTenantProvider;

    public EfCoreGlobalQueryFilterParameterStatusProvider(ICurrentTenantProvider currentTenantProvider)
    {
        _currentTenantProvider = currentTenantProvider;
    }

    private IDisposable ChangeSoftDeleteFilterStatus(bool isEnabled)
    {
        var oldStatus = SoftDeleteFilterIsEnabled;
        SoftDeleteFilterIsEnabled = isEnabled;
        return new DisposeAction(() =>
        {
            SoftDeleteFilterIsEnabled = oldStatus;
        });
    }

    public IDisposable DisableMultiTenancyFilter()
    {
        return _currentTenantProvider.ChangeCurrentTenant(null);
    }

    public IDisposable EnableSoftDeleteFilter()
    {
        return ChangeSoftDeleteFilterStatus(true);
    }

    public IDisposable DisableSoftDeleteFilter()
    {
        return ChangeSoftDeleteFilterStatus(false);
    }
}
