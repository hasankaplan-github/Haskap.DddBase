using Haskap.DddBase.Domain.Core.TenantAggregate;
using Haskap.DddBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Haskap.DddBase.Domain.Providers;

public class CurrentTenantProvider
{
    // https://github.com/hikalkan/presentations/blob/master/2018-04-06-Multi-Tenancy/src/MultiTenancyDraft/Infrastructure/MultiTenancyMiddleware.cs
    public static Tenant? CurrentTenant 
    {
        get 
        {
            return _currentTenant.Value;
        } 
        private set
        {
            _currentTenant.Value = value;
        } 
    }
    private static readonly AsyncLocal<Tenant?> _currentTenant = new AsyncLocal<Tenant?>();

    public static IDisposable ChangeCurrentTenant(Tenant? tenant)
    {
        var tempTenant = CurrentTenant;
        CurrentTenant = tenant;
        return new DisposeAction(() =>
        {
            CurrentTenant = tempTenant;
        });
    }
}

//public interface ITenantService
//{
//    string Tenant { get; }

//    void SetTenant(string tenant);

//    string[] GetTenants();

//    event TenantChangedEventHandler OnTenantChanged;
//}
