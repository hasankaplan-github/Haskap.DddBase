using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Providers;
public interface IGlobalQueryFilterParameterStatusProvider
{
    bool IsEnabled { get; }

    IDisposable DisableFilterParameter();
    IDisposable EnableFilterParameter();
}
