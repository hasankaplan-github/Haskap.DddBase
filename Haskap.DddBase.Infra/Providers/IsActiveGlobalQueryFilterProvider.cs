using Haskap.DddBase.Utilities;
using Haskap.DddBase.Domain.Providers;

namespace Haskap.DddBase.Infra.Providers;
public class IsActiveGlobalQueryFilterProvider : IIsActiveGlobalQueryFilterProvider
{
    public bool IsEnabled { get; private set; } = true;

    private IDisposable ChangeIsActiveFilterStatus(bool isEnabled)
    {
        var oldStatus = IsEnabled;
        IsEnabled = isEnabled;
        return new DisposeAction(() =>
        {
            IsEnabled = oldStatus;
        });
    }

    public IDisposable Disable()
    {
        return ChangeIsActiveFilterStatus(false);
    }

    public IDisposable Enable()
    {
        return ChangeIsActiveFilterStatus(true);
    }
}
