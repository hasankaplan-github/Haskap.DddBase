using Haskap.DddBase.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class GlobalQueryFilterGenericProvider : IGlobalQueryFilterGenericProvider
{
    private Dictionary<Type, IGlobalQueryFilterProvider> _dictionary = new();

    public void AddFilterProvider<TFilter>(IGlobalQueryFilterProvider provider)
    {
        _dictionary.Add(typeof(TFilter), provider);
    }

    public IDisposable Disable<TFilter>()
    {
        return _dictionary[typeof(TFilter)].Disable();
    }

    public IDisposable Enable<TFilter>()
    {
        return _dictionary[typeof(TFilter)].Enable();
    }

    public bool IsEnabled<TFilter>()
    {
        if (_dictionary.TryGetValue(typeof(TFilter), out IGlobalQueryFilterProvider provider))
        {
            return provider.IsEnabled;
        }

        return false;
    }
}
