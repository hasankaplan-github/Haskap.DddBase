using Haskap.DddBase.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class GlobalQueryFilterParameterStatusCollectionProvider : IGlobalQueryFilterParameterStatusCollectionProvider
{
    private Dictionary<Type, IGlobalQueryFilterParameterStatusProvider> _dictionary = new();

    public void AddFilterParameterStatusProvider<TFilterParameter>(IGlobalQueryFilterParameterStatusProvider provider)
    {
        _dictionary.Add(typeof(TFilterParameter), provider);
    }

    public IDisposable Disable<TFilterParameter>()
    {
        return _dictionary[typeof(TFilterParameter)].DisableFilterParameter();
    }

    public IDisposable Enable<TFilterParameter>()
    {
        return _dictionary[typeof(TFilterParameter)].EnableFilterParameter();
    }

    public bool IsEnabled<TFilterParameter>()
    {
        IGlobalQueryFilterParameterStatusProvider provider;
        try
        {
            provider = _dictionary[typeof(TFilterParameter)];
        }
        catch (Exception)
        {
            return false;
        }
        
        return provider.IsEnabled;
    }
}
