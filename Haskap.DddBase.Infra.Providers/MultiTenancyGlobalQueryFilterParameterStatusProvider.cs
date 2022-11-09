using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class MultiTenancyGlobalQueryFilterParameterStatusProvider : IMultiTenancyGlobalQueryFilterParameterStatusProvider
{
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public bool IsEnabled => _currentTenantProvider.CurrentTenantId is not null;

    public MultiTenancyGlobalQueryFilterParameterStatusProvider(ICurrentTenantProvider currentTenantProvider)
    {
        _currentTenantProvider = currentTenantProvider;
    }

    public IDisposable DisableFilterParameter()
    {
        return _currentTenantProvider.ChangeCurrentTenant(null);
    }

    public IDisposable EnableFilterParameter()
    {
        throw new NotSupportedException();
    }





    public bool SoftDeleteFilterIsEnabled { get; private set; } = true;

    

    private IDisposable ChangeSoftDeleteFilterStatus(bool isEnabled)
    {
        var oldStatus = SoftDeleteFilterIsEnabled;
        SoftDeleteFilterIsEnabled = isEnabled;
        return new DisposeAction(() =>
        {
            SoftDeleteFilterIsEnabled = oldStatus;
        });
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
