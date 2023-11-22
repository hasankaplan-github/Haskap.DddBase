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
    public bool IsEnabled { get; private set; } = true;

    public MultiTenancyGlobalQueryFilterProvider()
    {
    }


    public IDisposable Disable()
    {
        var temp = IsEnabled;
        IsEnabled = false;
        return new DisposeAction(() =>
        {
            IsEnabled = temp;
        });
    }

    public IDisposable Enable()
    {
        var temp = IsEnabled;
        IsEnabled = true;
        return new DisposeAction(() =>
        {
            IsEnabled = temp;
        });
    }
}
