using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Providers;
public interface IEfCoreGlobalQueryFilterParameterStatusProvider
{
    bool MultiTenancyFilterIsEnabled { get; }
    bool SoftDeleteFilterIsEnabled { get; }

    IDisposable DisableMultiTenancyFilter();
    IDisposable EnableSoftDeleteFilter();
    IDisposable DisableSoftDeleteFilter();
}
