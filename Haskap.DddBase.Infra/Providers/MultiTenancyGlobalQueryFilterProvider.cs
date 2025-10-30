using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities;

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
