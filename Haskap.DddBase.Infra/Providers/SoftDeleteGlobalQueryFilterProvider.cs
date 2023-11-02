using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class SoftDeleteGlobalQueryFilterProvider : ISoftDeleteGlobalQueryFilterProvider
{
    public bool IsEnabled { get; private set; } = true;

    private IDisposable ChangeSoftDeleteFilterStatus(bool isEnabled)
    {
        var oldStatus = IsEnabled;
        IsEnabled = isEnabled;
        return new DisposeAction(() =>
        {
            IsEnabled = oldStatus;
        });
    }

    public IDisposable Disable()
    {
        return ChangeSoftDeleteFilterStatus(false);
    }

    public IDisposable Enable()
    {
        return ChangeSoftDeleteFilterStatus(true);
    }
}
