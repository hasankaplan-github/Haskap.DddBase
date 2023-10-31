using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Providers;
public interface IGlobalQueryFilterParameterStatusCollectionProvider
{
    IDisposable Disable<TFilter>();
    IDisposable Enable<TFilter>();
    bool IsEnabled<TFilter>();

    void AddFilterParameterStatusProvider<TFilter>(IGlobalQueryFilterParameterStatusProvider provider);
}
