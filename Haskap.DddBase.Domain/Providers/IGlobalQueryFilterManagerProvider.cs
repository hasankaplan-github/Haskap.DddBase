namespace Haskap.DddBase.Domain.Providers;
public interface IGlobalQueryFilterManagerProvider
{
    IDisposable Disable<TFilter>();
    IDisposable Enable<TFilter>();
    bool IsEnabled<TFilter>();

    void AddFilterProvider<TFilter>(IGlobalQueryFilterProvider provider);
    void AddFilterProvider(Type filterType, IGlobalQueryFilterProvider provider);
    IReadOnlyDictionary<Type, IGlobalQueryFilterProvider> GetAllProviders();
    IGlobalQueryFilterProvider? GetFilterProvider(Type filterType);
    IGlobalQueryFilterProvider? GetFilterProvider<TFilter>();
}
