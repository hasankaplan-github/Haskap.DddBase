using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities;

namespace Haskap.DddBase.Infra.Providers;
public class CurrentTenantProvider : ICurrentTenantProvider
{
    public Guid? CurrentTenantId { get; private set; } = null;

    public bool IsHost => CurrentTenantId is null;

    public IDisposable ChangeCurrentTenant(Guid? newCurrentTenantId)
    {
        var tempTenantId = CurrentTenantId;
        CurrentTenantId = newCurrentTenantId;
        return new DisposeAction(() =>
        {
            CurrentTenantId = tempTenantId;
        });
    }
}
