namespace Haskap.DddBase.Domain.Providers;
public interface IGlobalQueryFilterProvider
{
    bool IsEnabled { get; }
    string Key { get; }

    IDisposable Disable();
    IDisposable Enable();
}

public interface IGlobalQueryFilterProvider<TFilter> : IGlobalQueryFilterProvider
{
    string IGlobalQueryFilterProvider.Key { get => typeof(TFilter).FullName ?? typeof(TFilter).Name; }
}
