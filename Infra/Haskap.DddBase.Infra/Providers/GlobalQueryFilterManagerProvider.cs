using Haskap.DddBase.Domain.Providers;
using System.Collections.Concurrent;

namespace Haskap.DddBase.Infra.Providers;
public class GlobalQueryFilterManagerProvider : IGlobalQueryFilterManagerProvider
{
    private ConcurrentDictionary<Type, IGlobalQueryFilterProvider> _dictionary = new();

    public void AddFilterProvider<TFilter>(IGlobalQueryFilterProvider provider)
    {
        AddFilterProvider(typeof(TFilter), provider);
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
        var provider = GetFilterProvider<TFilter>();

        return provider?.IsEnabled ?? false;
    }

    public IReadOnlyDictionary<Type, IGlobalQueryFilterProvider> GetAllProviders()
    {
        return _dictionary.AsReadOnly();
    }

    public void AddFilterProvider(Type filterType, IGlobalQueryFilterProvider provider)
    {
        _dictionary.TryAdd(filterType, provider);
    }

    public IGlobalQueryFilterProvider? GetFilterProvider(Type filterType)
    {
        if (_dictionary.TryGetValue(filterType, out IGlobalQueryFilterProvider? provider))
        {
            return provider;
        }

        return null;
    }

    public IGlobalQueryFilterProvider? GetFilterProvider<TFilter>()
    {
        return GetFilterProvider(typeof(TFilter));
    }
}
